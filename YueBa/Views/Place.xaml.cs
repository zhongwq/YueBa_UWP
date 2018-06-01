using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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

namespace YueBa.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Place : Page
    {
        private StorageFile file;

        public Place()
        {
            this.InitializeComponent();
        }

        private async void ChoosePicture(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

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
                    image.Source = srcImage;
                }
            }
        }

        private async void CreateClick(object sender, RoutedEventArgs e)
        {
            if (price.Text == "" ||
                name.Text == "" ||
                detail.Text == "" ||
                address.Text == "")
            {
                var show = new MessageDialog("各项输入不得为空").ShowAsync();
                return;
            }

            string pattern = @"^[0-9]*$";
            if (!Regex.IsMatch(price.Text, pattern))
            {
                var show = new MessageDialog("价格输入不合法").ShowAsync();
                return;
            }

            if (Double.Parse(price.Text) < 0)
            {
                var show = new MessageDialog("价格不得小于0").ShowAsync();
                return;
            }

            PlaceItem temp = new PlaceItem();
            temp.name = name.Text;
            temp.address = address.Text;
            temp.detail = detail.Text;
            temp.price = Double.Parse(price.Text);
            if ((string)create.Content == "Create")
            {
                string placeName = await Services.PlaceServices.addPlace(Store.getInstance().getToken(), temp.name, temp.address, temp.detail, temp.price, file);
                if (placeName != "")
                {
                    if (name.Tag == null)
                    {
                        ControlBar.Current.NavigateToPage("Index");
                    }
                    else
                    {
                        ControlBar.Current.NavigateToPage("AddActivity", "place" + placeName);
                    }

                }
            }
            else
            {
                if (await Services.PlaceServices.updatePlace(Store.getInstance().getToken(), name.Tag.ToString(), temp.name, temp.address, temp.detail, temp.price, file))
                {
                    ControlBar.Current.NavigateToPage("Index");
                }
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((string)e.Parameter != null)
            {
                if ((string)e.Parameter == "edit")
                {
                    name.Tag = "edit";
                }
                else
                {
                    PlaceItem place = await Services.PlaceServices.getSingleEvent((string)e.Parameter);
                    name.Text = place.name;
                    name.Tag = place.id.ToString();
                    image.Source = new BitmapImage(new Uri(Global.Config.api+place.img));
                    address.Text = place.address;
                    price.Text = place.price.ToString();
                    detail.Text = place.detail;
                    create.Content = "Update";
                }
            }
        }
    }
}
