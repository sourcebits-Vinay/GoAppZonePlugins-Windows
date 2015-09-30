using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSDK;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Devices;

namespace PluginExample
{
  public class VibrateAction : GOPluginAction
  {
    #region Constructor
    public VibrateAction(Microsoft.Phone.Controls.PhoneApplicationPage page, Microsoft.Phone.Controls.WebBrowser webBrowser)
      : base(page, webBrowser)
    {
    }
    #endregion

    #region Execute Override
    public override void execute(string action, string parameters, string callback)
    {
      base.execute(action, parameters, callback);

      if (action != null)
        executeAction(action, parameters);
    }
    #endregion

    #region User Code
    private void executeAction(string action, string parameters)
    {
      if (string.Compare(action, "VibrateAction") == 0)
        Vibrate(parameters);
    }

    private void Vibrate(String parameters)
    {
      try
      {
        VibrateController testVibrateController = VibrateController.Default;
        testVibrateController.Start(TimeSpan.FromSeconds(1));
      }
      catch (Exception e)
      {
        e.ToString();
        System.Diagnostics.Debugger.Break();
      }
    }
    #endregion
  }
}
