using GoogleAds;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PluginSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginExample
{
    public class AdMobAction : GOPluginAction
    {
        #region Constructor

        public AdMobAction(MyToolkit.Paging.Page page, Windows.UI.Xaml.Controls.WebView webBrowser)
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
            if (string.Compare(action, "ShowAds") == 0)
                ShowAds(parameters);

            if (string.Compare(action, "CloseAds") == 0)
                CloseAds(parameters);
        }

        /// <summary>
        /// Displays Ads according to input from the JSON object
        /// </summary>
        /// <param name="parameters"></param>
        private void ShowAds(String parameters)
        {
            try
            {
                verticalposition = System.Windows.VerticalAlignment.Top;
                horizontalposition = System.Windows.HorizontalAlignment.Left;
                AdFormats adformat = AdFormats.Banner;

                JObject optionsJson = JObject.Parse(parameters);
                if (optionsJson != null)
                {
                    JToken argsJson = null;
                    argsJson = optionsJson["args"];
                    if (argsJson != null)
                    {
                        string adUnitId = string.Empty;
                        string adSize = string.Empty;
                        string adPosition = string.Empty;
                        string callback = string.Empty;
                        string leftmagin = string.Empty;
                        string rightmagin = string.Empty;
                        string topmagin = string.Empty;
                        string bottommagin = string.Empty;

                        JToken adUnitIdJToken = null;
                        adUnitIdJToken = argsJson["adUnitId"];
                        if (adUnitIdJToken != null)
                            adUnitId = (adUnitIdJToken as JValue).Value.ToString();

                        JToken adSizeJToken = null;
                        adSizeJToken = argsJson["adSize"];
                        if (adSizeJToken != null)
                        {
                            adSize = (adSizeJToken as JValue).Value.ToString().ToUpper();

                            if (adSize != "INTERSTITIAL")
                            {
                                switch (adSize)
                                {
                                    case "BANNER": adformat = AdFormats.Banner;
                                        break;
                                    case "SMART_BANNER": adformat = AdFormats.SmartBanner;
                                        break;
                                }
                            }
                        }

                        JToken adPositionJToken = null;
                        adPositionJToken = argsJson["adPosition"];
                        if (adPositionJToken != null)
                        {
                            adPosition = (adPositionJToken as JValue).Value.ToString();
                            char[] delimiterChars = { '_' };
                            string[] Positions = adPosition.Split(delimiterChars);

                            switch (Positions[0].ToLower())
                            {
                                case "top": verticalposition = System.Windows.VerticalAlignment.Top;
                                    break;
                                case "bottom": verticalposition = System.Windows.VerticalAlignment.Bottom;
                                    break;
                                case "center": verticalposition = System.Windows.VerticalAlignment.Center;
                                    break;
                                case "stretch": verticalposition = System.Windows.VerticalAlignment.Stretch;
                                    break;
                                default: verticalposition = System.Windows.VerticalAlignment.Top;
                                    break;
                            }


                            switch (Positions[1].ToLower())
                            {
                                case "left": horizontalposition = System.Windows.HorizontalAlignment.Left;
                                    break;
                                case "right": horizontalposition = System.Windows.HorizontalAlignment.Right;
                                    break;
                                case "center": horizontalposition = System.Windows.HorizontalAlignment.Center;
                                    break;
                                case "stretch": horizontalposition = System.Windows.HorizontalAlignment.Stretch;
                                    break;
                                default: horizontalposition = System.Windows.HorizontalAlignment.Left;
                                    break;
                            }

                        }

                        JToken admarginsJToken = null;
                        admarginsJToken = argsJson["margins"];
                        if (admarginsJToken != null)
                        {
                            JToken leftJToken = null;
                            leftJToken = admarginsJToken["left"];
                            if (leftJToken != null)
                                leftmagin = (leftJToken as JValue).Value.ToString();

                            JToken rightJToken = null;
                            rightJToken = admarginsJToken["right"];
                            if (rightJToken != null)
                                rightmagin = (rightJToken as JValue).Value.ToString();

                            JToken topJToken = null;
                            topJToken = admarginsJToken["top"];
                            if (topJToken != null)
                                topmagin = (topJToken as JValue).Value.ToString();

                            JToken bottomJToken = null;
                            bottomJToken = admarginsJToken["bottom"];
                            if (leftJToken != null)
                                bottommagin = (leftJToken as JValue).Value.ToString();
                        }

                        JToken callbackJToken = null;
                        callbackJToken = argsJson["callback"];
                        if (callbackJToken != null)
                            callback = (callbackJToken as JValue).Value.ToString();

                        // NOTE: Edit "MY_AD_UNIT_ID" with your ad unit id.
                        AdView bannerAd = new AdView
                        {
                            Format = adformat,
                            AdUnitID = adUnitId,
                            VerticalAlignment = verticalposition,
                            HorizontalAlignment = horizontalposition
                        };
                        bannerAd.ReceivedAd += OnAdReceived;
                        bannerAd.FailedToReceiveAd += OnFailedToReceiveAd;
                        //LayoutRoot.Children.Add(bannerAd);
                        AdRequest adRequest = new AdRequest();
                        adRequest.ForceTesting = true;
                        bannerAd.LoadAd(adRequest);

                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
                System.Diagnostics.Debugger.Break();
            }
        }

        private void OnFailedToReceiveAd(object sender, AdErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Failed to receive ad with error " + e.ErrorCode);
        }

        private void OnAdReceived(object sender, AdEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Received ad successfully");
        }

        private void CloseAds(String parameters)
        {
            try
            {
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
