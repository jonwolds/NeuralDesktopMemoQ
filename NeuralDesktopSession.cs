using MemoQ.MTInterfaces;
using MemoQ.Addins.Common.DataStructures;
using System.Collections.Generic;

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

            string translatedSentence = rClient.GetTranslation(segm.PlainText, new List<string>(), "");
            sb.AppendString(translatedSentence);

            result.Translation = sb.ToSegment();



            return result;
        }


        public TranslationResult[] TranslateCorrectSegment(Segment[] segs, Segment[] tmSources, Segment[] tmTargets)
        {
            TranslationResult[] results = new TranslationResult[segs.Length];

            for (int i = 0; i < segs.Length; i++)
            {
                SegmentBuilder sb = new SegmentBuilder();


                results[i] = TranslateCorrectSegment(segs[i], tmSources[i], tmTargets[i]);
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
            //try
            //{
            //    MosesMTServiceHelper.StoreTranslation(options, transunit.Source.PlainText, transunit.Target.PlainText, this.srcLangCode, this.trgLangCode);
            //}
            //catch (Exception e)
            //{
            //    string localizedMessage = LocalizationHelper.Instance.GetResourceString("NetworkError");
            //    throw new MTException(string.Format(localizedMessage, e.Message), string.Format("A network error occured ({0}).", e.Message), e);
            //}
        }

        public int[] StoreTranslation(TranslationUnit[] transunits)
        {

            //try
            //{
            //    return MosesMTServiceHelper.BatchStoreTranslation(options,
            //                            transunits.Select(s => s.Source.PlainText).ToList(), transunits.Select(s => s.Target.PlainText).ToList(),
            //                            this.srcLangCode, this.trgLangCode);
            //}
            //catch (Exception e)
            //{
            //    string localizedMessage = LocalizationHelper.Instance.GetResourceString("NetworkError");
            //    throw new MTException(string.Format(localizedMessage, e.Message), string.Format("A network error occured ({0}).", e.Message), e);
            //}
            return null;
        }

        #endregion
    }
}
