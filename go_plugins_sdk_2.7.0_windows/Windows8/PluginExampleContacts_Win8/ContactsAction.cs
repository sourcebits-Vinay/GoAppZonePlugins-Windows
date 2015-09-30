using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSDK;

namespace PluginExample
{
  public class ContactsAction : GOPluginAction
  {
    #region Constructor
    public ContactsAction(MyToolkit.Paging.Page page, Windows.UI.Xaml.Controls.WebView webBrowser)
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
      {
        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        {
          var tmpTask = getContacts(parameters);
        }).AsTask();
      }
        
    }
    private async Task getContacts(String parameters)
    {
      try
      {
        //Get All Device Contacts
        var contactPicker = new Windows.ApplicationModel.Contacts.ContactPicker();
        contactPicker.SelectionMode = Windows.ApplicationModel.Contacts.ContactSelectionMode.Contacts;
        IReadOnlyList<Windows.ApplicationModel.Contacts.ContactInformation> contacts = await contactPicker.PickMultipleContactsAsync();

        //serialize the contacts in json format
        string jsonContacts = Newtonsoft.Json.JsonConvert.SerializeObject(contacts.ToArray());
        
        //call the callback
        executeCallback(jsonContacts);
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
