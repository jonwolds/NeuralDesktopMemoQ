using MemoQ.MTInterfaces;

namespace Lexorama.NeuralDesktopMemoQ
{
        /// <summary>
        /// Singleton helper class to be able to localize the plugin's textual information.
        /// 
        /// This does nothing but is ready to be used if localization is required later.
        /// 
        /// </summary>
        internal class LocalizationHelper
        {
            /// <summary>
            /// The singleton instance of the localization helper.
            /// </summary>
            private static LocalizationHelper instance = new LocalizationHelper();

            /// <summary>
            /// Private constructor to avoid multiple instances.
            /// </summary>
            private LocalizationHelper()
            { }

            /// <summary>
            /// The environment to be used to get localized texts from memoQ.
            /// </summary>
            private IEnvironment environment;

            /// <summary>
            /// The singleton instance of the localization helper.
            /// </summary>
            public static LocalizationHelper Instance
            {
                get { return instance; }
            }

            /// <summary>
            /// Sets the environment to be able to get localized texts.
            /// </summary>
            /// <param name="environment"></param>
            public void SetEnvironment(IEnvironment environment)
            {
                this.environment = environment;
            }

            /// <summary>
            /// Gets the localized text belonging to the specified key.
            /// </summary>
            public string GetResourceString(string key)
            {


                return key;
            }
        }
    }
