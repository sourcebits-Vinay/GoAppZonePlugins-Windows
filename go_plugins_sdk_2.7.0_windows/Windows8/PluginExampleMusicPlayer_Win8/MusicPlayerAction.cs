using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using PluginSDK;

namespace PluginExample
{
  public class MusicPlayerAction : GOPluginAction
  {
    #region Constructor
    public MusicPlayerAction(MyToolkit.Paging.Page page, Windows.UI.Xaml.Controls.WebView webBrowser)
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
      {
        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        {
          var tmpTask = executeAction(action, parameters);
        }).AsTask();
      }
    }
    #endregion

    #region User Code
    private async System.Threading.Tasks.Task executeAction(string action, string parameters)
    {
      await HandleAction(action, parameters);
    }

    private async System.Threading.Tasks.Task HandleAction(string action, string parameters)
    {
      try
      {
        if (string.Compare(action, "PlayAction") == 0)
          await PlayAction(parameters);
        else if (string.Compare(action, "PauseAction") == 0)
          PauseAction(parameters);
        else if (string.Compare(action, "StopAction") == 0)
          StopAction(parameters);
      }
      catch (Exception ex)
      {
        ex.ToString();
        System.Diagnostics.Debugger.Break();
      }
    }

    public async System.Threading.Tasks.Task PlayAction(string parameters)
    {
      MediaElement _mediaElement = (this.Plugin as MusicPlayerPlugin).Mp3Player;
      if (_mediaElement == null)
      {
        Windows.Storage.Pickers.FileOpenPicker openPicker = new Windows.Storage.Pickers.FileOpenPicker();
        openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
        openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
        openPicker.FileTypeFilter.Add(".mp3");
        Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();
        if (file != null)
        {
          var myAudioStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
          _mediaElement = new MediaElement();
          _mediaElement.AudioCategory = AudioCategory.Alerts;
          _mediaElement.Volume = 1;
          _mediaElement.SetSource(myAudioStream, file.ContentType);
          _mediaElement.Play();
        }
      }
      else
      {
        if (_mediaElement.CurrentState != MediaElementState.Playing)
          _mediaElement.Play();
      }


      (this.Plugin as MusicPlayerPlugin).Mp3Player = _mediaElement;
    }
    public void PauseAction(string parameters)
    {
      MediaElement _mediaElement = (this.Plugin as MusicPlayerPlugin).Mp3Player;
      if (_mediaElement != null)
        if (_mediaElement.CanPause && _mediaElement.CurrentState != MediaElementState.Paused)
          _mediaElement.Pause();

      (this.Plugin as MusicPlayerPlugin).Mp3Player = _mediaElement;
    }
    public void StopAction(string parameters)
    {
      MediaElement _mediaElement = (this.Plugin as MusicPlayerPlugin).Mp3Player;
      if (_mediaElement != null)
      {
        if (_mediaElement.CurrentState != MediaElementState.Stopped)
          _mediaElement.Stop();
      }

      (this.Plugin as MusicPlayerPlugin).Mp3Player = _mediaElement;
    }
    #endregion
  }
}
