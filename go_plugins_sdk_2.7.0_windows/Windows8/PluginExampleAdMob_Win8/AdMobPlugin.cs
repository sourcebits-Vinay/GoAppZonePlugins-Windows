using PluginSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginExample
{
    public class AdMobPlugin : GOPlugin
    {
        #region Constructor
        public AdMobPlugin(MyToolkit.Paging.Frame frame)
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
