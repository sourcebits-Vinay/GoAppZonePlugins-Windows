using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSDK;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PluginExample
{
  public class ContactsAction : GOPluginAction
  {
    #region Constructor
    public ContactsAction(Microsoft.Phone.Controls.PhoneApplicationPage page, Microsoft.Phone.Controls.WebBrowser webBrowser)
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
      if (string.Compare(action, "getContacts") == 0)
        getContacts(parameters);
    }
    private void getContacts(String parameters)
    {
      try
      {
        //Get All Device Contacts
        Microsoft.Phone.UserData.Contacts cons = new Microsoft.Phone.UserData.Contacts();

        cons.SearchCompleted += (sender, e) =>
        {
          //serialize the contacts in json format
          string jsonContacts = JsonConvert.SerializeObject(e.Results);

          //call the callback
          executeCallback(jsonContacts);
        };

        //Search the phone for contacts
        cons.SearchAsync(String.Empty, Microsoft.Phone.UserData.FilterKind.None, "#1");
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
