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
            Dictionary<int, TranslationUnit> nonmaskedSegments = new Dictionary<int, TranslationUnit>();
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

            // Old
            //for (int i = 0; i < segs.Length; i++)
            //{
            //    SegmentBuilder sb = new SegmentBuilder();


            //    results[i] = TranslateCorrectSegment(segs[i], tmSources[i], tmTargets[i]);
            //}


            for (int x = 0; x < Math.Ceiling((decimal)nonmaskedSegments.Count / batchsize); x++)
            {
                var t = nonmaskedSegments.Skip(x * batchsize).Take(batchsize);
                Dictionary<int, NeuralDesktopProviderTagPlacer> tgPlacerdict = new Dictionary<int, NeuralDesktopProviderTagPlacer>();

                string for_batchtranslation = "";

                foreach (var tu in t)
                {



                    if (InterfaceConstants.TAGHANDLING_Auto)
                    {
                        NeuralDesktopProviderTagPlacer tp = new NeuralDesktopProviderTagPlacer(segm);
                        translatedSentence = rClient.GetTranslation(tp.PreparedSourceText, new List<string>(), "");
                        result.Translation = tp.GetTaggedSegment(translatedSentence);
                    }
                    else
                    {
                        translatedSentence = rClient.GetTranslation(segm.PlainText, new List<string>(), "");
                        sb.AppendString(translatedSentence);
                        result.Translation = sb.ToSegment();
                    }




                    if (tu.Value.SourceSegment.HasTags)
                    {
                        var tagPlacer = new NeuralDesktopProviderTagPlacer(tu.Value.SourceSegment);
                        tgPlacerdict.Add(tu.Key, tagPlacer);
                        for_batchtranslation = for_batchtranslation + tagPlacer.PreparedSourceText + "\n";
                    }
                    else
                    {
                        for_batchtranslation = for_batchtranslation + tu.Value.SourceSegment.ToPlain() + "\n";
                    }

                }

                translatedSentence = SearchInServer(for_batchtranslation.TrimEnd('\n'));
                string[] batchtranslation = translatedSentence.Split('\n');

                if (batchtranslation.Length != t.Count())
                {
                    int a = 0;
                }
                else
                {
                    int a = 0;

                    foreach (var tu in t)
                    {
                        translation = new Segment(_languageDirection.TargetCulture);


                        if (tu.Value.SourceSegment.HasTags)
                        {

                            translation = tgPlacerdict[tu.Key].GetTaggedSegment(batchtranslation[a].Replace(@"\n", "")).Duplicate();
                        }
                        else
                        {
                            translation.Add(batchtranslation[a].Replace(@"\n", ""));
                        }
                        result = CreateSearchResult(tu.Value.SourceSegment, translation, tu.Value.SourceSegment.Duplicate().ToPlain(), tu.Value.SourceSegment.HasTags);
                        srSegments.Add(tu.Key, result);
                        a++;
                    }

                }

                //{
                //    Segment translation = new Segment(_languageDirection.TargetCulture);

                //    if (results.SourceSegment.HasTags)
                //    {
                //        var tagPlacer = new NeuralDesktopProviderTagPlacer(results.SourceSegment);
                //        translatedSentence = SearchInServer(tagPlacer.PreparedSourceText);
                //        translation = tagPlacer.GetTaggedSegment(translatedSentence).Duplicate();
                //    }
                //    else
                //    {
                //        var sourcetext = results.SourceSegment.ToPlain();
                //        translatedSentence = SearchInServer(sourcetext);
                //        translation.Add(translatedSentence);
                //    }

                //    results.Add(CreateSearchResult(segment, translation, results.SourceSegment.ToPlain(), segment.HasTags));
                //}

                i = 0;

                //try
                //{
                foreach (var tu in translationUnits)
                {
                    if (mask == null || mask[i])
                    {
                        results_set = new SearchResults();
                        results_set.SourceSegment = srSegments[i].MemoryTranslationUnit.SourceSegment;
                        results_set.Add(srSegments[i]);
                        results.Add(results_set);
                    }
                    else
                    {
                        results.Add(null);
                    }
                    i++;
                }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}


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

    public class InterfaceConstants
    {


        public const bool TAGHANDLING_None = false;
        public const bool TAGHANDLING_Auto = true;
    }
}
