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
  public class GalleryFilesAction : GOPluginAction
  {
    #region Constructor
    public GalleryFilesAction(Microsoft.Phone.Controls.PhoneApplicationPage page, Microsoft.Phone.Controls.WebBrowser webBrowser)
      : base(page, webBrowser)
    {
    }
    #endregion

    #region Execute Override
    //called from plugin sdk through background thread
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
      if (string.Compare(action, "GalleryFilesAction") == 0)
        getFiles(parameters);
    }

    private void getFiles(String parameters)
    {
      try
      {
        //Get All Device Contacts
        Microsoft.Phone.Tasks.PhotoChooserTask task = new Microsoft.Phone.Tasks.PhotoChooserTask();

        task.Completed += (sender, e) =>
        {
          //serialize the contacts in json format
          string jsonPhoto = JsonConvert.SerializeObject(e.OriginalFileName);

          //call the callback
          executeCallback(jsonPhoto);
        };

        //Search the phone for contacts
        task.Show();
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
