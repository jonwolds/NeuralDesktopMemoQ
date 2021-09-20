
using MemoQ.Addins.Common.DataStructures;
using MemoQ.Addins.Common.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;
using System.Text.RegularExpressions;

namespace Lexorama.NeuralDesktopMemoQ
{


    /// <summary>
    /// 
    /// 
    /// Holds data on a source segment and the tags it contains, which can be used to insert the tags in the target segment
    /// </summary>
    public class NeuralDesktopProviderTagPlacer
    {

        private string _preparedSourceText;
        private string _returnedText;
        private int _nbreturnedcodes;
        private Segment _sourceSegment;
        private Dictionary<string, MtTag> _tagsDictionary;
        private Dictionary<string, TagInfo> _tagsInfoDictionary;
        private Dictionary<string, int> _transi;
        private IList<InlineTag> _inlineTags;

        private string _regexcode = "(<b>|<i>|<u>|</b>|</i>|</u>|<span.*?</span>)+";



        public NeuralDesktopProviderTagPlacer(Segment sourceSegment)
        {
            _sourceSegment = sourceSegment;
            TagsInfo = new List<TagInfo>();
            _tagsDictionary = GetSourceTagsDict();
            _inlineTags = _sourceSegment.ITags;
        }

        /// <summary>
        /// Returns the source text with markup replacing the tags in the source segment
        /// </summary>
        public string PreparedSourceText => _preparedSourceText;

        public List<TagInfo> TagsInfo { get; set; }

        /// <summary>
        /// Returns a tagged segments from a target string containing markup, where the target string represents the translation of the class instance's source segment
        /// </summary>
        /// <param name="returnedText"></param>
        /// <returns></returns>
        public Segment GetTaggedSegment(string returnedText)
        {

            //Stack<string> sanityCheck = new Stack<string>(); // Make sure tags are ordered correctly in the segment returned
            //bool sanityCheckPassed = true;



            //var failed_sanity_test_segment = new Segment();

            //var targetElements =
            //        GetTargetElements();
            ////build our segment looping through elements
            ///


            string prevText = string.Empty;
            string nextText = string.Empty;
            string padleft = string.Empty;
            string padright = string.Empty;
            string mstring = returnedText;

            foreach (string panostag in _tagsDictionary.Keys)
            {

                MtTag tagtoreplace = _tagsDictionary[panostag];

                int startpos = mstring.IndexOf(panostag);

                if (startpos > -1)
                {
                    prevText = mstring.Substring(0, startpos);
                    nextText = mstring.Substring(startpos + panostag.Length).Replace(panostag, "");

                    padleft = _tagsDictionary[panostag].PadLeft;
                    padright = _tagsDictionary[panostag].PadRight;


                    mstring = prevText + padleft + tagtoreplace.memoqTag + padright + nextText;
                }



            }
            if (mstring.Contains("ph_"))
            {
                int g = 0;
            }


            //        if (_tagsInfoDictionary[text].TStatus == TagStatus.Start)
            //        {
            //            sanityCheck.Push(_tagsInfoDictionary[text].TagId);
            //        }
            //        else if (_tagsInfoDictionary[text].TStatus == TagStatus.End)
            //        {

            //            if (sanityCheck.Count > 0)
            //            {
            //                string starttagindex = Convert.ToString(sanityCheck.Pop());
            //                if (!starttagindex.Equals(_tagsInfoDictionary[text].TagId))
            //                {
            //                    sanityCheckPassed = false;
            //                }
            //                else
            //                {
            //                    sanityCheckPassed = false;
            //                }
            //            }


            //        }
            //        if (!failed_sanity_test_segment.ToString().EndsWith(" ") && failed_sanity_test_segment.ToString().Length > 0)
            //        {
            //            failed_sanity_test_segment.Add(" "); // Add an extra space instead of the missing code
            //        }
            //    }
            //    else
            //    {
            //        //if it is not in the list of tagtexts then the element is just the text
            //        if (text.Trim().Length > 0) //if the element is something other than whitespace, i.e. some text in addition
            //        {
            //            text = text.Trim(); //trim out extra spaces, since they are dealt with by associating them with the tags
            //            segment.Add(text); //add to the segment
            //            failed_sanity_test_segment.Add(text);
            //        }
            //    }
            //}
            //if ((sanityCheck.Count > 0) || (sanityCheckPassed == false))
            //{
            //    segment = failed_sanity_test_segment;
            //}

            var segment = SegmentHtmlConverter.ConvertHtml2Segment(mstring, _inlineTags); //our segment to return
                                                                                              //get our array of elements..it will be array of tagtexts and text in the order received from google

            return segment; //this will return a tagged segment
        }



        private Dictionary<string, MtTag> GetSourceTagsDict()
        {
            _tagsDictionary = new Dictionary<string, MtTag>();
            _tagsInfoDictionary = new Dictionary<string, TagInfo>(); /// Used to keep info we need to handle missing tags
            _transi = new Dictionary<string, int>();

            
            string retv = SegmentHtmlConverter.ConvertSegment2Html(_sourceSegment, true);

            string mstring = retv;

            string tagText = string.Empty;
            string prevText = string.Empty;
            string nextText = string.Empty;
            int whitespace;

            MatchCollection matchList = Regex.Matches(mstring, _regexcode);
                var trans = matchList.Cast<Match>().Select(match => match.Value).ToList();


                if (trans.Count > 1)
                {
                int c = 1;

                    for (int i = 0; i < trans.Count; i++)
                    {
                        if ("</b></i></u>".Contains(trans[i]))
                    {
                        tagText = $"｟ph_{c.ToString()}_end｠";
                    } else
                    {
                        tagText = $"｟ph_{c.ToString()}_start｠";
                    }

                    int startpos = mstring.IndexOf(trans[i]);
                    prevText = mstring.Substring(0, startpos);
                    nextText = mstring.Substring(startpos + trans[i].Length);

                    mstring =  prevText + tagText + nextText;

                    var theTag = new MtTag(trans[i]);

                    //var tagInfo = new TagInfo
                    //{
                    //    TagType = theTag.SdlTag.Type,
                    //    Index = i,
                    //    TStatus = TagStatus.Individual,
                    //    TagId = theTag.SdlTag.TagID,
                    //    prevTag = pTag
                    //};

                    if (!prevText.Trim().Equals(""))//and not just whitespace
                    {
                        //get number of trailing spaces for that segment
                         whitespace = prevText.Length - prevText.TrimEnd().Length;
                        //add that trailing space to our tag as leading space
                        theTag.PadLeft = prevText.Substring(prevText.Length - whitespace);
                    }
                    whitespace = nextText.Length - nextText.TrimStart().Length;
                    //add that trailing space to our tag as leading space
                    theTag.PadRight = nextText.Substring(0, whitespace);

                    //add our new tag code to the dict with the corresponding tag if it's not already there
                    if (!_tagsDictionary.ContainsKey(tagText))
                    {
                        _tagsDictionary.Add(tagText, theTag);
                    }

                    c++;
                    }
                }


            ////build dict
            //for (var i = 0; i < _sourceSegment.Elements.Count; i++)
            //{
            //    var elType = _sourceSegment.Elements[i].GetType();

            //    if (elType.ToString() == "Sdl.LanguagePlatform.Core.Tag") //if tag, add to dictionary
            //    {
            //        if (true) /// This is where we could limit the number of tags dealt with
            //        {
            //            var theTag = new MtTag((Tag)_sourceSegment.Elements[i].Duplicate());
            //            var tagText = string.Empty;

            //            var tagInfo = new TagInfo
            //            {
            //                TagType = theTag.SdlTag.Type,
            //                Index = i,
            //                TStatus = TagStatus.Individual,
            //                TagId = theTag.SdlTag.TagID,
            //                prevTag = pTag
            //            };


            //            if (theTag.SdlTag.Type == TagType.Start)
            //            {
            //                tagText = $"｟ph_{c.ToString()}_start｠";
            //                pTag = tagText;
            //                tagInfo.TStatus = TagStatus.Start;
            //                _transi.Add(tagInfo.TagId, c);
            //                c += 1;
            //                _tagsInfoDictionary.Add(tagText, tagInfo);
            //            }
            //            if (theTag.SdlTag.Type == TagType.End)
            //            {
            //                tagInfo.TStatus = TagStatus.End;
            //                tagText = $"｟ph_{_transi[tagInfo.TagId].ToString()}_end｠";
            //                pTag = tagText;
            //                _tagsInfoDictionary.Add(tagText, tagInfo);

            //            }
            //            if (theTag.SdlTag.Type == TagType.Standalone || theTag.SdlTag.Type == TagType.TextPlaceholder || theTag.SdlTag.Type == TagType.LockedContent)
            //            {
            //                tagText = $"｟ph_{c.ToString()}_start｠";
            //                pTag = tagText;
            //                c += 1;
            //                _tagsInfoDictionary.Add(tagText, tagInfo);

            //            }

            //            _preparedSourceText += tagText;


            //            //now we have to figure out whether this tag is preceded and/or followed by whitespace
            //            if (i > 0 && !_sourceSegment.Elements[i - 1].GetType().ToString().Equals("Sdl.LanguagePlatform.Core.Tag"))
            //            {
            //                var prevText = _sourceSegment.Elements[i - 1].ToString();
            //                if (!prevText.Trim().Equals(""))//and not just whitespace
            //                {
            //                    //get number of trailing spaces for that segment
            //                    var whitespace = prevText.Length - prevText.TrimEnd().Length;
            //                    //add that trailing space to our tag as leading space
            //                    theTag.PadLeft = prevText.Substring(prevText.Length - whitespace);
            //                }
            //            }
            //            if (i < _sourceSegment.Elements.Count - 1 && !_sourceSegment.Elements[i + 1].GetType().ToString().Equals("Sdl.LanguagePlatform.Core.Tag"))
            //            {
            //                //here we don't care whether it is only whitespace
            //                //get number of leading spaces for that segment
            //                var nextText = _sourceSegment.Elements[i + 1].ToString();
            //                var whitespace = nextText.Length - nextText.TrimStart().Length;
            //                //add that trailing space to our tag as leading space
            //                theTag.PadRight = nextText.Substring(0, whitespace);
            //            }

            //            //add our new tag code to the dict with the corresponding tag if it's not already there
            //            if (!_tagsDictionary.ContainsKey(tagText))
            //            {
            //                _tagsDictionary.Add(tagText, theTag);
            //            }
            //        }
            //    }
            //    else
            //    {//if not a tag
            //        var str = _sourceSegment.Elements[i].ToString(); //HtmlEncode our plain text to be better processed by google and add to string
            //        _preparedSourceText += _sourceSegment.Elements[i].ToString();
            //    }
            //}

            TagsInfo.Clear();
            _preparedSourceText = mstring;

            return _tagsDictionary;
        }
 
        /// <summary>
        /// puts returned string into an array of elements
        /// </summary>
        /// <returns></returns>
        private string[] GetTargetElements()
        {
            //first create a regex to put our array separators around the tags
            var str = _returnedText;

            const string aplhanumericPattern = @"｟ph_[1-7]_(start|end)｠";



            var alphaRgx = new Regex(aplhanumericPattern, RegexOptions.IgnoreCase);
            var alphaMatches = alphaRgx.Matches(str);
            _nbreturnedcodes = alphaMatches.Count;

            if (_nbreturnedcodes != _tagsDictionary.Count)
            {
                if ((_tagsDictionary.Count - _nbreturnedcodes) == 1)
                {
                    /// First we need to find which code is missing
                    ///
                    var sTags = new Dictionary<string, TagInfo>(_tagsInfoDictionary);
                    foreach (Match match in alphaMatches)
                    {
                        sTags.Remove(match.Value);
                    }
                    string tagToAdd = sTags.Keys.First();
                    str = str.Replace(sTags[tagToAdd].prevTag, sTags[tagToAdd].prevTag + tagToAdd);
                    alphaMatches = alphaRgx.Matches(str);
                    _nbreturnedcodes = alphaMatches.Count;

                }
                else
                {
                    str = Regex.Replace(str, aplhanumericPattern, "");
                    _nbreturnedcodes = 0;
                }



            }


            //if (_nbreturnedcodes > 0)
            //{
            //    str = AddSeparators(str, alphaMatches);
            //}

            var stringSeparators = new[] { "```" };
            var strAr = str.Split(stringSeparators, StringSplitOptions.None);
            return strAr;
        }
    }
    public class TagInfoOld
    {
        public string TagId { get; set; }
        public int Index { get; set; }
        // public TagType TagType { get; set; }
        public TagStatus TStatus { get; set; }
        public string prevTag { get; set; }
    }
    public class TagInfo
    {
        public string TagId { get; set; }
        public int Index { get; set; }
        public TagStatus TStatus { get; set; }
        public string prevTag { get; set; }
    }
    internal class MtTag
    {
        internal MtTag(string tag)
        {
            this.memoqTag = tag;
            PadLeft = string.Empty;
            PadRight = string.Empty;
        }

        internal string PadLeft { get; set; }

        internal string PadRight { get; set; }

        internal string memoqTag { get; set;  }
    }

    public enum TagStatus
    {
        Start,
        End,
        Individual
    }
}
