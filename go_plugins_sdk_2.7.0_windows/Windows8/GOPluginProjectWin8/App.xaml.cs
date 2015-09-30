using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace GOPluginProject
{
  /// <summary>
  /// Provides application-specific behavior to supplement the default Application class.
  /// </summary>
  sealed partial class App : Application
  {
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
      this.InitializeComponent();
      this.Suspending += OnSuspending;
      this.Resuming += App_Resuming;

    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
#if DEBUG
      if (System.Diagnostics.Debugger.IsAttached)
      {
        this.DebugSettings.EnableFrameRateCounter = false;
      }
#endif

      MyToolkit.Paging.Frame rootFrame = Window.Current.Content as MyToolkit.Paging.Frame;

      // Do not repeat app initialization when the Window already has content,
      // just ensure that the window is active
      if (rootFrame == null)
      {
        // Create a Frame to act as the navigation context and navigate to the first page
        rootFrame = new MyToolkit.Paging.Frame();
        // Set the default language
        rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

        if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
        {
          //TODO: Load state from previously suspended application
        }

        // Place the frame in the current Window
        Window.Current.Content = rootFrame;
      }

      if (rootFrame.Content == null)
      {
        // When the navigation stack isn't restored navigate to the first page,
        // configuring the new page by passing required information as a navigation
        // parameter
        rootFrame.NavigateAsync(typeof(MainPage), e.Arguments);
      }
      // Ensure the current window is active
      Window.Current.Activate();

      PluginSDK.GOPluginManager.Init(rootFrame);
      PluginSDK.GOPluginManager.Application_Launching();
    }

    /// <summary>
    /// Invoked when application execution is being suspended.  Application state is saved
    /// without knowing whether the application will be terminated or resumed with the contents
    /// of memory still intact.
    /// </summary>
    /// <param name="sender">The source of the suspend request.</param>
    /// <param name="e">Details about the suspend request.</param>
    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
      PluginSDK.GOPluginManager.Application_Deactivated();
    }
    private void App_Resuming(object sender, object e)
    {
      PluginSDK.GOPluginManager.Application_Activated();
    }
  }
}
