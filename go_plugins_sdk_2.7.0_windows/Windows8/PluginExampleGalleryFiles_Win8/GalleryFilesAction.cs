using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSDK;
using Newtonsoft.Json;

namespace PluginExample
{
  public class GalleryFilesAction : GOPluginAction
  {
    #region Constructor
    public GalleryFilesAction(MyToolkit.Paging.Page page, Windows.UI.Xaml.Controls.WebView webBrowser)
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
      {
        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        {
          getFiles(parameters);
        }).AsTask();
      }
    }
    private async void getFiles(String parameters)
    {
      try
      {
        Windows.Storage.Pickers.FileOpenPicker openPicker = new Windows.Storage.Pickers.FileOpenPicker();
        openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
        openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
        openPicker.FileTypeFilter.Add(".jpg");
        openPicker.FileTypeFilter.Add(".jpeg");
        openPicker.FileTypeFilter.Add(".png");

        IReadOnlyList<Windows.Storage.StorageFile> files = await openPicker.PickMultipleFilesAsync();
        if (files != null)
        {
          //serialize the files in json format
          string jsonPhotos = JsonConvert.SerializeObject(files.ToArray());

          //call the callback
          executeCallback(jsonPhotos);
        }
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
