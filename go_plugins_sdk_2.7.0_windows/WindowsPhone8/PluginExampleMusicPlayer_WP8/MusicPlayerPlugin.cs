using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSDK;
using System.Windows.Controls;
using Microsoft.Phone.BackgroundAudio;

namespace PluginExample
{
  public class MusicPlayerPlugin : GOPlugin
  {
    #region Constructor
    public MusicPlayerPlugin(Microsoft.Phone.Controls.PhoneApplicationFrame frame)
      : base(frame)
    {
      this.Mp3Player = null;
    }
    #endregion

    #region Application lifecycle events
    public override void Application_Launching()
    {
    }
    public override void Application_Activated()
    {
    }
    public override void Application_Deactivated()
    {
    }
    public override void Application_Closing()
    {
    }
    #endregion

    //User variables
    public MediaElement Mp3Player { get; set; }
  }
}
