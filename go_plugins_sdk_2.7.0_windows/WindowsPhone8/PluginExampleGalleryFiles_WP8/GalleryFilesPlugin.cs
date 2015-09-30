using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSDK;

namespace PluginExample
{
  public class GalleryFilesPlugin : GOPlugin
  {
    #region Constructor
    public GalleryFilesPlugin(Microsoft.Phone.Controls.PhoneApplicationFrame frame)
      : base(frame)
    {
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
  }
}
