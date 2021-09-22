﻿using System.Windows.Forms;
using System.Reflection;
using MemoQ.MTInterfaces;
using MemoQ.Addins.Common.Framework;
using System.Drawing;

namespace Lexorama.NeuralDesktopMemoQ
{
    /// <summary>
    /// The main class of the Moses MT plugin.
    /// </summary>
    public class NeuralDesktopPluginDirector : PluginDirectorBase, IModule
    {
        /// <summary>
        /// The identifier of the plugin.
        /// </summary>
        public const string PluginId = "NeuralDesktop";

        /// <summary>
        /// The memoQ's application environment; e.g., to provide UI language settings etc. to the plugin.
        /// </summary>
        private IEnvironment environment;

        #region IModule Members

        public void Cleanup()
        {
            // write your cleanup code here
        }

        public void Initialize(IModuleEnvironment env)
        {
            // write your initialization code here
        }

        public bool IsActivated
        {
            get { return true; }
        }

        #endregion


        #region IPluginDirector Members

        /// <summary>
        /// Indicates whether storing translations is supported.
        /// </summary>
        public override bool StoringTranslationSupported
        {
            get { return false; }
        }


        /// <summary>
        /// Indicates whether interactive lookup (in the translation grid) is supported or not.
        /// </summary>
        public override bool InteractiveSupported
        {
            get { return true; }
        }

        /// <summary>
        /// Indicates whether batch lookup is supported or not.
        /// </summary>
        public override bool BatchSupported
        {
            get { return true; }
        }

        /// <summary>
        /// The plugin's non-localized name.
        /// </summary>
        public override string PluginID
        {
            get { return PluginId; }
        }

        /// <summary>
        /// Returns the friendly name to show in memoQ's Tools / Options.
        /// </summary>
        public override string FriendlyName
        {
            get { return "NeuralDesktop Plugin"; }
        }

        /// <summary>
        /// Return the copyright text to show in memoQ's Tools / Options.
        /// </summary>
        public override string CopyrightText
        {
            get { return "(C) Lexorama 2021"; }
        }

        /// <summary>
        /// Return a 48x48 display icon to show in MemoQ's Tools / Options. Black is the transparent color.
        /// </summary>
        public override Image DisplayIcon
        {
            get { return Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Lexorama.NeuralDesktopMemoQ.lexo.png")); }
        }

        /// <summary>
        /// The memoQ's application environment; e.g., to provide UI language settings etc. to the plugin.
        /// </summary>
        public override IEnvironment Environment
        {
            set
            {
                this.environment = value;

                // initialize the localization helper
                LocalizationHelper.Instance.SetEnvironment(value);
            }
        }

        /// <summary>
        /// Tells memoQ if the plugin supports the provided language combination. The strings provided are memoQ language codes.
        /// </summary>
        public override bool IsLanguagePairSupported(LanguagePairSupportedParams args) => true;



        /// <summary>
        /// Creates an MT engine for the supplied language pair.
        /// </summary>
        public override IEngine2 CreateEngine(CreateEngineParams args)
        {
            return new NeuralDesktopEngine(args.SourceLangCode, args.TargetLangCode, new NeuralDesktopOptions(args.PluginSettings));
        }
        /// <summary>
        /// Shows the plugin's options form.
        /// </summary>

        public override PluginSettings EditOptions(IWin32Window parentForm, PluginSettings settings)
        {
            using (var form = new NeuralDesktopOptionsForm() { Options = new NeuralDesktopOptions(settings) })
            {
                if (form.ShowDialog(parentForm) == DialogResult.OK)
                {
                    environment.PluginAvailabilityChanged();
                }
                return form.Options.GetSerializedSettings();
            }
        }


        /// <summary>
        /// Shows the plugin's options form.
        /// </summary>
        public void ShowOptionsForm(Form parentForm)
        {
            using (NeuralDesktopOptionsForm form = new NeuralDesktopOptionsForm())
            {
                if (form.ShowDialog(parentForm) == DialogResult.OK)
                {
                    environment.PluginAvailabilityChanged();
                }
            }
        }

        #endregion
    }
}
