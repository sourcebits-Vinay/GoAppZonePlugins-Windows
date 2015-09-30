using System;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Controls;

namespace GOPluginProject
{
  public sealed partial class MainPage : MyToolkit.Paging.Page
  {
    private static readonly Uri HomeUri = new Uri("ms-appx-web:///Html/index.html");

    public MainPage()
    {
      this.InitializeComponent();

      this.WebViewControl.ScriptNotify += WebViewControl_ScriptNotify;
    }

    public override void OnNavigatedTo(MyToolkit.Paging.NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      WebViewControl.Navigate(HomeUri);
    }

    private async void WebViewControl_ScriptNotify(object sender, NotifyEventArgs e)
    {
      if (!string.IsNullOrEmpty(e.Value))
      {
        if (e.Value.ToLower().StartsWith("alert:"))
        {
          string cmdParams = e.Value.Substring(e.Value.IndexOf(":") + 1);
          var msg = new Windows.UI.Popups.MessageDialog(cmdParams, "Alert");
          await msg.ShowAsync();
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
