using GoogleAds;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PluginSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Windows.Devices.Geolocation;

namespace PluginExample
{
    public class AdMobAction : GOPluginAction
    {
        private InterstitialAd interstitialAd;
        Grid AdGrid;
        JToken admargins = null;
        string parameters = string.Empty;
        double leftmagin = 0;
        double rightmagin = 0;
        double topmagin = 0;
        double bottommagin = 0;
        double screenwidth = 0;
        double screenheight = 0;
        private object OnReceivedstandartAd;

        #region Constructor
        public AdMobAction(Microsoft.Phone.Controls.PhoneApplicationPage page, Microsoft.Phone.Controls.WebBrowser webBrowser)
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
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    AdmobAction(parameters);
                });
            }
        }

        /// <summary>
        /// Displays Ads according to input from the JSON object
        /// </summary>
        /// <param name="parameters"></param>
        private async void AdmobAction(String parameters)
        {
            try
            {
                if (parameters != null)
                {
                    System.Windows.VerticalAlignment verticalposition = System.Windows.VerticalAlignment.Top;
                    System.Windows.HorizontalAlignment horizontalposition = System.Windows.HorizontalAlignment.Left;
                    AdFormats adformat = AdFormats.Banner;

                    screenwidth = Application.Current.Host.Content.ActualWidth;
                    screenheight = Application.Current.Host.Content.ActualHeight;

                    JObject optionsJson = JObject.Parse(parameters);
                    if (optionsJson != null)
                    {
                        JToken argsJson = optionsJson["args"];
                        if (argsJson != null)
                        {
                            string adUnitId = string.Empty;
                            string adSize = string.Empty;
                            string adPosition = string.Empty;
                            string callback = string.Empty;
                            string gender = string.Empty;

                            JToken adUnitIdJToken = null;
                            adUnitIdJToken = argsJson["adUnitId"];
                            if (adUnitIdJToken != null)
                                adUnitId = (adUnitIdJToken as JValue).Value.ToString();

                            JToken admarginsJToken = argsJson["margins"];
                            admargins = admarginsJToken;

                            JToken adSizeJToken = null;
                            adSizeJToken = argsJson["adSize"];
                            if (adSizeJToken != null)
                            {
                                adSize = (adSizeJToken as JValue).Value.ToString().ToUpper();

                                if (adSize != "INTERSTITIAL")
                                {
                                    #region Standard banner and Smart banner
                                    switch (adSize)
                                    {
                                        case "BANNER": adformat = AdFormats.Banner;
                                            validationofStandardBannerMargin();
                                            break;
                                        case "SMART_BANNER": adformat = AdFormats.SmartBanner;
                                            validationofSmartBannerMargin();
                                            break;
                                    }

                                    JToken adPositionJToken = argsJson["adPosition"];
                                    if (adPositionJToken != null)
                                    {
                                        adPosition = (adPositionJToken as JValue).Value.ToString();
                                        char[] delimiterChars = { '_' };
                                        string[] Positions = adPosition.Split(delimiterChars);

                                        if (Positions.Count() > 0)
                                        {
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
                                        }

                                        if (Positions.Count() > 1)
                                        {
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
                                    }

                                    JToken callbackJToken = argsJson["callback"];
                                    if (callbackJToken != null)
                                        callback = (callbackJToken as JValue).Value.ToString();

                                    AdGrid = new Grid();

                                    AdView bannerAd = new AdView
                                    {
                                        Format = adformat,
                                        AdUnitID = adUnitId,
                                        VerticalAlignment = verticalposition,
                                        HorizontalAlignment = horizontalposition,
                                        Margin = new Thickness(leftmagin, topmagin, rightmagin, bottommagin),
                                    };

                                    AdRequest adRequest = new AdRequest();
                                    JToken targetingJToken = argsJson["targeting"];
                                    if (targetingJToken != null)
                                    {
                                        JToken locationJToken = targetingJToken["location"];
                                        JToken xJToken = locationJToken["x"];
                                        string x = (xJToken as JValue).Value.ToString();
                                        JToken yJToken = locationJToken["y"];
                                        string y = (yJToken as JValue).Value.ToString();


                                        Geolocator geolocator = new Geolocator();
                                        geolocator.DesiredAccuracyInMeters = 1;

                                        Geoposition geoposition = await geolocator.GetGeopositionAsync(
                                                TimeSpan.FromMinutes(5),
                                                TimeSpan.FromSeconds(10));

                                        adRequest.Location = geoposition.Coordinate;

                                        JToken genderJToken = targetingJToken["gender"];
                                        if (genderJToken != null)
                                            gender = (genderJToken as JValue).Value.ToString();
                                        if (gender.ToLower() == "male")
                                            adRequest.Gender = UserGender.Male;
                                        else if (gender.ToLower() == "female")
                                            adRequest.Gender = UserGender.Female;


                                        JToken birthdayJToken = targetingJToken["birthday"];
                                        string birthday = (birthdayJToken as JValue).Value.ToString();
                                        double unixTimeStamp = Convert.ToDouble(birthday);
                                        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                        adRequest.Birthday = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

                                    }
                                    adRequest.ForceTesting = true;     // Enable test ads
                                    AdGrid.Children.Add(bannerAd);
                                    bannerAd.LoadAd(adRequest);
                                    this.ApplicationFrame.Content = AdGrid;
                                    #endregion
                                }

                                else
                                {
                                    #region Interstitial Banner
                                    JToken callbackJToken = argsJson["callback"];
                                    if (callbackJToken != null)
                                        callback = (callbackJToken as JValue).Value.ToString();

                                    interstitialAd = new InterstitialAd(adUnitId);
                                    AdRequest interstitialadRequest = new AdRequest();
                                    JToken targetingJToken = argsJson["targeting"];
                                    if (targetingJToken != null)
                                    {
                                        JToken locationJToken = targetingJToken["location"];
                                        JToken xJToken = locationJToken["x"];
                                        string x = (xJToken as JValue).Value.ToString();
                                        JToken yJToken = locationJToken["y"];
                                        string y = (yJToken as JValue).Value.ToString();

                                        Geolocator geolocator = new Geolocator();

                                        JToken accuracyJToken = targetingJToken["accuracy"];
                                        string accuracy = (accuracyJToken as JValue).Value.ToString();
                                        if (string.IsNullOrEmpty(accuracy))
                                            geolocator.DesiredAccuracyInMeters = 1;
                                        else
                                            geolocator.DesiredAccuracyInMeters = Convert.ToUInt32(accuracy, 16);

                                        Geoposition geoposition = await geolocator.GetGeopositionAsync(
                                                TimeSpan.FromMinutes(5),
                                                TimeSpan.FromSeconds(10));

                                        interstitialadRequest.Location = geoposition.Coordinate;

                                        JToken genderJToken = targetingJToken["gender"];
                                        if (genderJToken != null)
                                            gender = (genderJToken as JValue).Value.ToString();
                                        if (gender.ToLower() == "male")
                                            interstitialadRequest.Gender = UserGender.Male;
                                        else if (gender.ToLower() == "female")
                                            interstitialadRequest.Gender = UserGender.Female;


                                        JToken birthdayJToken = targetingJToken["birthday"];
                                        string birthday = (birthdayJToken as JValue).Value.ToString();
                                        double unixTimeStamp = Convert.ToDouble(birthday);
                                        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                        interstitialadRequest.Birthday = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

                                    }

                                    interstitialadRequest.ForceTesting = true;// Enable test ads
                                    interstitialAd.ReceivedAd += OnAdReceived;
                                    interstitialAd.FailedToReceiveAd += OnFailedToReceiveAd;
                                    interstitialAd.LoadAd(interstitialadRequest);
                                    #endregion
                                }
                            }
                        }
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
            executeCallback(e.ToString());
        }

        private void OnAdReceived(object sender, AdEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Ad received successfully");
            interstitialAd.ShowAd();
        }

        private void validationofStandardBannerMargin()
        {
            if (admargins != null)
            {
                JToken leftJToken = admargins["left"];
                if (leftJToken != null)
                {
                    leftmagin = Convert.ToDouble((leftJToken as JValue).Value.ToString());
                    if (screenwidth < (leftmagin + 320))
                        leftmagin = 0;
                }

                JToken rightJToken = admargins["right"];
                if (rightJToken != null)
                {
                    rightmagin = Convert.ToDouble((rightJToken as JValue).Value.ToString());
                    if (screenwidth < (rightmagin + 320))
                        rightmagin = 0;
                }

                JToken topJToken = admargins["top"];
                if (topJToken != null)
                {
                    topmagin = Convert.ToDouble((topJToken as JValue).Value.ToString());
                    if (screenheight < (topmagin + 50))
                        topmagin = 0;
                }

                JToken bottomJToken = admargins["bottom"];
                if (bottomJToken != null)
                {
                    bottommagin = Convert.ToDouble((bottomJToken as JValue).Value.ToString());
                    if (screenheight < (bottommagin + 50))
                        bottommagin = 0;
                }
            }
        }

        private void validationofSmartBannerMargin()
        {
            if (admargins != null)
            {
                JToken leftJToken = admargins["left"];
                if (leftJToken != null)
                {
                    leftmagin = Convert.ToDouble((leftJToken as JValue).Value.ToString());
                    if (leftmagin != 0)
                        leftmagin = 0;
                }

                JToken rightJToken = admargins["right"];
                if (rightJToken != null)
                {
                    rightmagin = Convert.ToDouble((rightJToken as JValue).Value.ToString());
                    if (rightmagin != 0)
                        rightmagin = 0;
                }

                JToken topJToken = admargins["top"];
                JToken bottomJToken = admargins["bottom"];

                if (screenheight < 400)
                {
                    if (topJToken != null)
                    {
                        topmagin = Convert.ToDouble((topJToken as JValue).Value.ToString());
                        if (screenheight < (topmagin + 32))
                            topmagin = 0;
                    }

                    if (bottomJToken != null)
                    {
                        bottommagin = Convert.ToDouble((leftJToken as JValue).Value.ToString());
                        if (screenheight < (bottommagin + 32))
                            bottommagin = 0;
                    }
                }
                else if (screenheight > 400 || screenheight < 720)
                {
                    if (topJToken != null)
                    {
                        topmagin = Convert.ToDouble((topJToken as JValue).Value.ToString());
                        if (screenheight < (topmagin + 50))
                            topmagin = 0;
                    }

                    if (bottomJToken != null)
                    {
                        bottommagin = Convert.ToDouble((leftJToken as JValue).Value.ToString());
                        if (screenheight < (bottommagin + 50))
                            bottommagin = 0;
                    }
                }
                else if (screenheight > 720)
                {
                    if (topJToken != null)
                    {
                        topmagin = Convert.ToDouble((topJToken as JValue).Value.ToString());
                        if (screenheight < (topmagin + 90))
                            topmagin = 0;
                    }

                    if (bottomJToken != null)
                    {
                        bottommagin = Convert.ToDouble((leftJToken as JValue).Value.ToString());
                        if (screenheight < (bottommagin + 90))
                            bottommagin = 0;
                    }
                }
            }
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
