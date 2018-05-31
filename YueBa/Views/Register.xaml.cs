using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using YueBa.Global;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace YueBa
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
        }

        private StorageFile file;

        private void LogInClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Login), null);
        }

        private async void SignUpClick(object sender, RoutedEventArgs e)
        {
            Store store = Store.getInstance();
            JObject result = await Services.AuthServices.Register(username.Text, email.Text, password.Password, phone.Text, file);
            if (result != null)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["token"] = result["token"].ToString();
                composite["username"] = result["user"]["username"].ToString();
                composite["email"] = result["user"]["email"].ToString();
                composite["phone"] = result["user"]["phone"].ToString();
                composite["img"] = result["user"]["img"].ToString();
                ApplicationData.Current.LocalSettings.Values["store"] = composite;

                store.username = (String)result["user"]["username"];
                store.email = (String)result["user"]["email"];
                store.phone = (String)result["user"]["phone"];
                store.token = (String)result["token"];
                store.img = (String)result["user"]["img"];

                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(ControlBar), null);
            }
        }

        private async void ChoosePicture(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            //选择视图模式 
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            //openPicker.ViewMode = PickerViewMode.List; 
            //初始位置 
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //添加文件类型 
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var srcImage = new BitmapImage();
                    await srcImage.SetSourceAsync(stream);
                    userImg.ImageSource = srcImage;
                }
            }
        }
    }
}
