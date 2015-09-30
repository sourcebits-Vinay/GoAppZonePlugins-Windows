using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;

namespace PluginExample
{
  public partial class MusicPlayerPage : PhoneApplicationPage
  {
    public PluginSDK.GOPluginAction CurrentAction;
    public MusicPlayerPage()
    {
      InitializeComponent();
    }

    private void btnPlay_Click(object sender, RoutedEventArgs e)
    {
      if ((CurrentAction.Plugin as MusicPlayerPlugin).Mp3Player != null)
        Mp3Player = (CurrentAction.Plugin as MusicPlayerPlugin).Mp3Player;
      else
        Mp3Player.Source = new Uri("Audio/SoundEffects.mp3", UriKind.RelativeOrAbsolute);

      if (Mp3Player.CurrentState != System.Windows.Media.MediaElementState.Playing)
        Mp3Player.Play();
      (CurrentAction.Plugin as MusicPlayerPlugin).Mp3Player = Mp3Player;
    }
    private void btnPause_Click(object sender, RoutedEventArgs e)
    {
      Mp3Player = (CurrentAction.Plugin as MusicPlayerPlugin).Mp3Player;
      if (Mp3Player.CurrentState != System.Windows.Media.MediaElementState.Paused)
        Mp3Player.Pause();

      (CurrentAction.Plugin as MusicPlayerPlugin).Mp3Player = Mp3Player;
    }
    private void btnStop_Click(object sender, RoutedEventArgs e)
    {
      Mp3Player = (CurrentAction.Plugin as MusicPlayerPlugin).Mp3Player;
      if (Mp3Player.CurrentState != System.Windows.Media.MediaElementState.Stopped)
        Mp3Player.Stop();

      (CurrentAction.Plugin as MusicPlayerPlugin).Mp3Player = Mp3Player;
    }
    protected override void OnBackKeyPress(CancelEventArgs e)
    { 
      (CurrentAction.Plugin as MusicPlayerPlugin).Mp3Player = null;
    }
  }
}