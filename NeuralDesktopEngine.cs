using System;
using System.Reflection;
using System.Drawing;
using MemoQ.MTInterfaces;

namespace Lexorama.NeuralDesktopMemoQ
{

    public class NeuralDesktopEngine : EngineBase
    {
        /// <summary>
        /// The source language.
        /// </summary>
        private readonly string srcLangCode;

        /// <summary>
        /// The target language.
        /// </summary>
        private readonly string trgLangCode;

        /// <summary>
        /// Plugin options
        /// </summary>
        private readonly NeuralDesktopOptions options;

        public NeuralDesktopEngine(string srcLangCode, string trgLangCode, NeuralDesktopOptions options)
        {
            this.srcLangCode = srcLangCode;
            this.trgLangCode = trgLangCode;
            this.options = options;

        }

        #region IEngine Members

        /// <summary>
        /// Creates a session for translating segments. Session will not be used in a multi-threaded way.
        /// </summary>
        public override ISession CreateLookupSession()
        {
            return new NeuralDesktopSession(srcLangCode, trgLangCode, options);
        }

        /// <summary>
        /// Creates a session for translating segments. Session will not be used in a multi-threaded way.
        /// </summary>
        public ISession CreateSession()
        {
            return new NeuralDesktopSession(srcLangCode, trgLangCode, options);
        }

        /// <summary>
        /// Set an engine-specific custom property, e.g., subject matter area.
        /// </summary>
        public override void SetProperty(string name, string value)
        {
            // not needed
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a small icon to be displayed under translation results when an MT hit is selected from this plugin.
        /// </summary>
        public override Image SmallIcon
        {
            get { return Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Lexorama.NeuralDesktopMemoQ.Icon.bmp")); }
        }

        /// <summary>
        /// Indicates whether the engine supports the adjustment of fuzzy TM hits through machine translation.
        /// </summary>
        public override bool SupportsFuzzyCorrection
        {
            get { return false; }
        }
        /// <summary>
        /// Creates a session for translating segments. Session will not be used in a multi-threaded way.
        /// </summary>
        public override ISessionForStoringTranslations CreateStoreTranslationSession()
        {
            return new NeuralDesktopSession(srcLangCode, trgLangCode, options);
        }
        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            // dispose your resources if needed
        }

        #endregion
    }
}
