using System;
using MemoQ.MTInterfaces;

namespace Lexorama.NeuralDesktopMemoQ
{
    public class NeuralDesktopOptions : MemoQ.MTInterfaces.PluginSettingsObject<NeuralDesktopGeneralSettings, NeuralDesktopSecureSetting>, IPluginSettingsMigrator
    {



        public NeuralDesktopOptions(MemoQ.MTInterfaces.PluginSettings serializedSettings) : base(serializedSettings)
        {
        }

        /// <summary>
        /// Create instance by providing the settings objects.
        /// </summary>
        public NeuralDesktopOptions(NeuralDesktopGeneralSettings generalSettings, NeuralDesktopSecureSetting secureSettings) : base(generalSettings, secureSettings)
        {
        }

        public PluginSettings ReadSettingsFromFile(string pluginSettingsDirectory)
        {
            throw new NotImplementedException();
        }
    }
    public class NeuralDesktopGeneralSettings
    {
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        public string[] SupportedLanguages = new string[0];
        /// <summary>
        /// Indicates whether the plugin is enabled or not.
        /// </summary>
        /// 

        public string framework;

        public string client;
        public string port;
        public string serverAddress;


        public bool PluginEnabled = false;
    }
    /// <summary>
    /// Secure settings, content not preserved when settings leave the machine.
    /// </summary>
    public class NeuralDesktopSecureSetting
    {
        /// <summary>
        /// The user name used to be able to use the MT service.
        /// </summary>
        public string UserName = string.Empty;
        /// <summary>
        /// The password used to be able to use the MT service.
        /// </summary>
        public string Password = string.Empty;
    }
}