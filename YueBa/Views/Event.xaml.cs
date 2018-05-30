using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Event : Page
    {
        private Places places;

        public Event()
        {
            this.InitializeComponent();
            GetAllPlaces();
        }

        private StorageFile file;

        private async void GetAllPlaces()
        {
            places = await Services.PlaceServices.getAllPlaces();
            foreach (var place in places.places)
            {
                place.img = Config.api + place.img;
            }
        }

        private void TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            List<PlaceItem> temp = new List<PlaceItem>();
            for (int i = 0; i < places.places.Length; i++)
            {
                if (places.places[i].name.Contains(sender.Text))
                {
                    temp.Add(places.places[i]);
                }
            }
            place.ItemsSource = temp;
        }

        private void AddPlaceClick(object sender, RoutedEventArgs e)
        {
            ControlBar.Current.NavigateToPage("Place");
        }

        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            PlaceItem temp = args.SelectedItem as PlaceItem;
            place.Tag = temp.id;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && !(e.Parameter is String))
            {
                PlaceItem temp = (PlaceItem)e.Parameter;
                place.Text = temp.name;
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
                    image.Source = srcImage;
                }
            }
        }

        private async void CreateClick(object sender, RoutedEventArgs e)
        {
            await Services.EventServices.addEvent(Store.getInstance().getToken(), name.Text, detail.Text, startTime.Text, endTime.Text, place.Tag.ToString(), maxNum.Text, file);
            ControlBar.Current.NavigateToPage("Index");
        }
    }
}
