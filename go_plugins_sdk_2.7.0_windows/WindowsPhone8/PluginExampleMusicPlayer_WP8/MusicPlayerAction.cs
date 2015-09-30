using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using System.Windows;
using Microsoft.Phone.BackgroundAudio;
using System.IO;
using System.Reflection;
using Microsoft.Phone.Shell;
using PluginSDK;

namespace PluginExample
{
    public class MusicPlayerAction : GOPluginAction
    {
        #region Constructor
        public MusicPlayerAction(Microsoft.Phone.Controls.PhoneApplicationPage page, Microsoft.Phone.Controls.WebBrowser webBrowser)
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
            HandleAction(action, parameters);
        }
        private void HandleAction(string action, string parameters)
        {
            try
            {
                if (string.Compare(action, "Mp3PlayerAction") == 0)
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Mp3PlayerAction(parameters);
                    });
                }
            }
            catch (Exception e)
            {
                e.ToString();
                System.Diagnostics.Debugger.Break();
            }
        }

        public void Mp3PlayerAction(string parameters)
        {
            //Hook the navigation events
            this.ApplicationPage.NavigationService.Navigated += NavigationService_Navigated;
            this.ApplicationPage.NavigationService.NavigationFailed += NavigationService_NavigationFailed;
            this.ApplicationFrame.Navigate(new Uri("/PluginExampleMusicPlayerWP8;component/MusicPlayerPage.xaml", UriKind.Relative));
        }
        private void NavigationService_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            //UnHook the navigation events
            this.ApplicationPage.NavigationService.Navigated -= NavigationService_Navigated;
            this.ApplicationPage.NavigationService.NavigationFailed -= NavigationService_NavigationFailed;
        }
        private void NavigationService_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //UnHook the navigation events
            this.ApplicationPage.NavigationService.Navigated -= NavigationService_Navigated;
            this.ApplicationPage.NavigationService.NavigationFailed -= NavigationService_NavigationFailed;

            //Set the action to the UnHook the RemoteLoginPage
            if (e.Content is MusicPlayerPage)
                (e.Content as MusicPlayerPage).CurrentAction = this;
        }
        #endregion
    }
}
