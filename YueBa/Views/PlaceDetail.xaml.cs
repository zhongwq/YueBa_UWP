using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace YueBa.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PlaceDetail : Page
    {
        public PlaceDetail()
        {
            this.InitializeComponent();
            id = "1";
            getPlaceDetail();
        }

        private PlaceDetailClass place;
        private String id;
        
        private async void getPlaceDetail()
        {
            place = await Services.PlaceServices.getPlaceDetail(Global.Store.getInstance().token, id);
            PlaceImg.Source = new BitmapImage(new Uri(Global.Config.api + place.detail.img));
            PlaceName.Text = place.detail.name;
            ownerImg.ImageSource = new BitmapImage(new Uri(Global.Config.api + place.detail.owner.img));
            ownerName.Text = place.detail.owner.username;
            phone.Text = place.detail.owner.phone;
            detail.Text = place.detail.detail;
            price.Text = place.detail.price.ToString();

            if (place.detail.editFlag)
            {
                EditBtn.Visibility = Visibility.Visible;
                deleteBtn.Visibility = Visibility.Visible;
            }
        }

        private async void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            bool flag = await Services.PlaceServices.deletePlace(Global.Store.getInstance().token, id);
            if (flag)
            {
                new MessageDialog("删除成功").ShowAsync();
                ControlBar frame = (ControlBar)Window.Current.Content;
                frame.NavigateToPage("Index");
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlBar frame = (ControlBar)Window.Current.Content;
            frame.NavigateToPage("EditPlace", id);
        }
    }
}
