using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Newtonsoft.Json.Linq;
using GoogleAds;

namespace GOPluginProject
{
    public partial class MainPage : PhoneApplicationPage
    {
        private static readonly Uri HomeUri = new Uri("Html/index.html", UriKind.Relative);

        public MainPage()
        {
            InitializeComponent();

            this.WebViewControl.ScriptNotify += WebViewControl_ScriptNotify;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            WebViewControl.IsScriptEnabled = true;
            WebViewControl.Navigate(HomeUri);
        }

        void WebViewControl_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                if (e.Value.ToLower().StartsWith("alert:"))
                {
                    string cmdParams = e.Value.Substring(e.Value.IndexOf(":") + 1);
                    MessageBox.Show(cmdParams, "Alert", MessageBoxButton.OK);
                }
                else
                {
                    JObject optionsJson = JObject.Parse(e.Value);

                    if (optionsJson != null)
                    {
                        string plugin = "";
                        string action = "";
                        string args = "";
                        string callback = "";

                        JToken pluginJToken = null;
                        pluginJToken = optionsJson["plugin"];
                        if (pluginJToken != null)
                            plugin = (pluginJToken as JValue).Value.ToString();

                        JToken actionJToken = null;
                        actionJToken = optionsJson["action"];
                        if (actionJToken != null)
                            action = (actionJToken as JValue).Value.ToString();

                        JToken argsJToken = null;
                        argsJToken = optionsJson["args"];
                        if (argsJToken != null)
                            args = (argsJToken as JValue).Value.ToString();

                        JToken callbackJToken = null;
                        callbackJToken = optionsJson["callback"];
                        if (callbackJToken != null)
                            callback = (callbackJToken as JValue).Value.ToString();

                        PluginSDK.GOPluginManager.Execute(this, this.WebViewControl, plugin, action, args, callback);
                    }
                }
            }
        }
    }
}