using System;
using System.Net;
using System.IO;
using System.IO.Pipes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Lexorama.NeuralDesktopMemoQ
{
    public class RestClient
    {
        public RestClient() { }
        public RestClient(string serverAddress, int serverPort)
        {
            Uri = "http://" + serverAddress + ":" + Convert.ToString(serverPort) + "/translate";
        }

        protected enum HttpMethod
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        protected string Uri { get; set; }

        public List<string> PreparedFeatures(List<string> features)
        {
            var FeaturesList = new List<string>();
            foreach (var f in features)
            {
                var feat = "｟" + f + "｠";
                FeaturesList.Add(feat);
            }
            return FeaturesList;
        }

        public virtual string GetTranslation(string sourceString, List<string> features, string featurePosition)
        {
            var FeaturesWithChars = PreparedFeatures(features);
            string responseJson = string.Empty;
            string translation = string.Empty;

            TextField Source = new TextField
            {
                Text = sourceString //+ string.Join("", features.ToArray())
            };

            if (featurePosition == "start")
            {
                Source.Text = string.Join("", FeaturesWithChars.ToArray()) + Source.Text;
            }
            else if (featurePosition == "end")
            {
                Source.Text += string.Join("", FeaturesWithChars.ToArray());
            }

            Request SourceRequest = new Request
            {
                SourceText = new TextField[] { Source }
            };

            string serializedSourceString = JsonConvert.SerializeObject(SourceRequest);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Uri);
            request.Method = HttpMethod.POST.ToString();
            request.ContentType = "application/json";

            //We need to close and open the connection every time to avoid
            //errors with unfinished requests
            request.KeepAlive = false;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(serializedSourceString);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Error code: " + response.StatusCode.ToString());
                }
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            responseJson = reader.ReadToEnd();
                        }
                    }
                }
            }

            Response Target = JsonConvert.DeserializeObject<Response>(responseJson);

            translation = Target.TargetText[0][0].Text;

            return translation;
        }
    }

    public class RestClientLua : RestClient
    {
        public RestClientLua(string ServerAddress, int Port) : base(ServerAddress, Port)
        {
            Uri = "http://" + ServerAddress + ":" + Convert.ToString(Port) + "/translator/translate";
        }

        public override string GetTranslation(string sourceString, List<string> features, string featurePosition)
        {
            var FeaturesWithChars = PreparedFeatures(features);
            string responseJson = string.Empty;
            string translation = string.Empty;

            List<RequestLua> ListJson = new List<RequestLua>();
            RequestLua JsonTranslation = new RequestLua
            {
                SourceText = sourceString,
                Features = FeaturesWithChars
            };

            ListJson.Add(JsonTranslation);

            string serializedSourceString = JsonConvert.SerializeObject(ListJson);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Uri);
            request.Method = HttpMethod.POST.ToString();
            request.ContentType = "application/json";

            //We need to close and open the connection every time to avoid
            //errors with unfinished requests
            request.KeepAlive = false;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(serializedSourceString);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Error code: " + response.StatusCode.ToString());
                }
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            responseJson = reader.ReadToEnd();
                        }
                    }
                }
            }

            JArray jsonTranslation = JArray.Parse(responseJson);
            translation = jsonTranslation[0][0].SelectToken("tgt").ToString();

            return translation;
        }
    }


    public class TextField
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Request
    {
        [JsonProperty("src")]
        public TextField[] SourceText { get; set; }
    }

    public class Response
    {
        [JsonProperty("tgt")]
        public TextField[][] TargetText { get; set; }
    }

    public class RequestLua
    {
        [JsonProperty("src")]
        public string SourceText { get; set; }

        [JsonProperty("feats")]
        public List<string> Features { get; set; }
    }

    public class SocketClient : RestClient
    {

        private NamedPipeClientStream Client { get; set; }
        private string ServiceName { get; set; }
        public SocketClient(string LocalServiceName, int port = 0) : base()
        {
            ServiceName = LocalServiceName;
            Client = new NamedPipeClientStream(ServiceName);
        }

        public override string GetTranslation(string sourceString, List<string> features, string featurePosition)
        {
            var FeaturesWithChars = PreparedFeatures(features);
            var streamWriter = new StreamWriter(Client);
            var streamReader = new StreamReader(Client);

            Client.Connect(1000);
            //string translation = String.Empty;
            string featuredString = String.Empty;

            if (features.Count == 0)
            {
                featuredString = sourceString;
            }
            else
            {
                if (featurePosition == "start")
                {
                    featuredString = string.Join("", FeaturesWithChars.ToArray()) + sourceString;
                }
                else if (featurePosition == "end")
                {
                    featuredString = sourceString + string.Join("", FeaturesWithChars.ToArray());
                }
            }

            streamWriter.Write(featuredString);
            streamWriter.Flush();

            //Client.WaitForPipeDrain();

            var translation = streamReader.ReadToEnd();

            return translation;

        }


    }


}