using MemoQ.MTInterfaces;
using MemoQ.Addins.Common.DataStructures;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Lexorama.NeuralDesktopMemoQ
{
    public class NeuralDesktopSession : ISession, ISessionForStoringTranslations
    {
        /// <summary>
        /// The source language.
        /// </summary>
        private string srcLangCode;

        /// <summary>
        /// The target language.
        /// </summary>
        private string trgLangCode;

        /// <summary>
        /// Options of the plugin.
        /// </summary>
        private readonly NeuralDesktopOptions _options;

        public NeuralDesktopSession(string srcLangCode, string trgLangCode, NeuralDesktopOptions options)
        {
            this.srcLangCode = srcLangCode;
            this.trgLangCode = trgLangCode;
            this._options = options;
        }

        #region ISession Members

        /// <summary>
        /// Translates a single segment, possibly using a fuzzy TM hit for improvement
        /// </summary>
        public TranslationResult TranslateCorrectSegment(Segment segm, Segment tmSource, Segment tmTarget)
        {
            TranslationResult result = new TranslationResult();
            SegmentBuilder sb = new SegmentBuilder();
            string translatedSentence;

            string serverAddress = string.Empty;
            int port = 0;

            if (_options.GeneralSettings.framework != "local")
            {
                serverAddress = _options.GeneralSettings.serverAddress;
                port = int.Parse(_options.GeneralSettings.port);
            }


            RestClient rClient;
            if (_options.GeneralSettings.framework == "lua")
                rClient = new RestClientLua(serverAddress, port);
            else if (_options.GeneralSettings.framework == "wizard")
                rClient = new RestClient(serverAddress, port);
            else
                rClient = new SocketClient("LexService");


            

            if (InterfaceConstants.TAGHANDLING_Auto)
            {
                NeuralDesktopProviderTagPlacer tp = new NeuralDesktopProviderTagPlacer(segm);
                translatedSentence = rClient.GetTranslation(tp.PreparedSourceText, new List<string>(), "");
                 result.Translation = tp.GetTaggedSegment(translatedSentence);
            } else
            {
                translatedSentence = rClient.GetTranslation(segm.PlainText, new List<string>(), "");
                sb.AppendString(translatedSentence);
                result.Translation = sb.ToSegment();
            }




            return result;
        }


        public TranslationResult[] TranslateCorrectSegment(Segment[] segs, Segment[] tmSources, Segment[] tmTargets)
        {
            TranslationResult[] results = new TranslationResult[segs.Length];
            Dictionary<int, Segment> nonmaskedSegments = new Dictionary<int, Segment>();
            TranslationResult result = new TranslationResult();
            SegmentBuilder sb = new SegmentBuilder();
            string translatedSentence;

            string serverAddress = string.Empty;
            int port = 0;

            if (_options.GeneralSettings.framework != "local")
            {
                serverAddress = _options.GeneralSettings.serverAddress;
                port = int.Parse(_options.GeneralSettings.port);
            }


            RestClient rClient;
            if (_options.GeneralSettings.framework == "lua")
                rClient = new RestClientLua(serverAddress, port);
            else if (_options.GeneralSettings.framework == "wizard")
                rClient = new RestClient(serverAddress, port);
            else
                rClient = new SocketClient("LexService");

            int batchsize = segs.Count();


            #region Only1segmentinbatch
            if (batchsize == 1)
            {
                if (InterfaceConstants.TAGHANDLING_Auto)
                {
                    NeuralDesktopProviderTagPlacer tp = new NeuralDesktopProviderTagPlacer(segs[0]);
                    translatedSentence = rClient.GetTranslation(tp.PreparedSourceText, new List<string>(), "");
                    result.Translation = tp.GetTaggedSegment(translatedSentence);
                }
                else
                {
                    translatedSentence = rClient.GetTranslation(segs[0].PlainText, new List<string>(), "");
                    sb.AppendString(translatedSentence);
                    result.Translation = sb.ToSegment();
                }

                results[0] = result;
                return results;
                }
            #endregion

            int keepcount = 0;

            int i = 0;
            foreach (var tu in segs)
            {
                nonmaskedSegments.Add(i, tu);
                i++;
            }


            for (int x = 0; x < Math.Ceiling((decimal)nonmaskedSegments.Count() / batchsize); x++)
            {
                var t = nonmaskedSegments.Skip(x * batchsize).Take(batchsize);
                Dictionary<int, NeuralDesktopProviderTagPlacer> tgPlacerdict = new Dictionary<int, NeuralDesktopProviderTagPlacer>();

                string for_batchtranslation = "";

                foreach (var tu in t)
                {
                    NeuralDesktopProviderTagPlacer tagPlacer = new NeuralDesktopProviderTagPlacer(tu.Value);
                    tgPlacerdict.Add(tu.Key, tagPlacer);
                    for_batchtranslation = for_batchtranslation + tagPlacer.PreparedSourceText + "\n";

                }

                translatedSentence = rClient.GetTranslation(for_batchtranslation, new List<string>(), "");
                string[] batchtranslation = translatedSentence.TrimEnd('\n').Split('\n');

                if (batchtranslation.Length != t.Count())
                {
                    int a = 0;
                }
                else
                {
                    int a = 0;

                    foreach (var tu in t)
                    {
                        results[keepcount] = new TranslationResult();
                        results[keepcount].Translation = tgPlacerdict[tu.Key].GetTaggedSegment(batchtranslation[a]);
                        a++;
                        keepcount++;
                    }

                }




            }



            return results;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // dispose your resources if needed
        }

        #endregion
        #region ISessionForStoringTranslations
        public void StoreTranslation(TranslationUnit transunit)
        {

        }

        public int[] StoreTranslation(TranslationUnit[] transunits)
        {


            return null;
        }

        #endregion
    }

    public class InterfaceConstants
    {


        public const bool TAGHANDLING_None = false;
        public const bool TAGHANDLING_Auto = true;
    }
}
