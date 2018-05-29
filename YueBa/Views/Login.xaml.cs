using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YueBa.Global;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace YueBa
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Login : Page
    {
        private Store store;

        public Login()
        {
            this.InitializeComponent();
            store = Store.getInstance();
        }

        private async void LogInClick(object sender, RoutedEventArgs e)
        {
            JObject result = await Services.AuthServices.Login(username.Text, password.Password);
            if (result != null)
            {
                if ((bool)remember.IsChecked)
                {
                    ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                    composite["token"] = result["token"].ToString();
                    composite["username"] = result["user"]["username"].ToString();
                    composite["email"] = result["user"]["email"].ToString();
                    composite["phone"] = result["user"]["phone"].ToString();
                    ApplicationData.Current.LocalSettings.Values["store"] = composite;
                }
                store.username = (String)result["user"]["username"];
                store.email = (String)result["user"]["email"];
                store.phone = (String)result["user"]["phone"];
                store.token = (String)result["token"];

                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(ControlBar), null);
            }
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Register), null);
        }
    }
}
