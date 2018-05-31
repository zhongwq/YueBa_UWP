using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
using YueBa.Utils;

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

        private StorageFile file = null;

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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((string)e.Parameter != "")
            {
                var temp = (string)e.Parameter;
                if (temp.Contains("place"))
                {
                    place.Text = temp.Substring(5);
                }
                else
                {
                    EventItem activity = await Services.EventServices.getSingleEvent(temp);
                    name.Tag = temp;
                    place.Tag = activity.place.id;
                    place.Text = activity.place.name;
                    name.Text = activity.name;
                    startDate.Date = activity.startTime;
                    StringBuilder sTime = new StringBuilder();
                    sTime.AppendFormat("{0}:{1}:00", activity.startTime.Hour, activity.startTime.Minute);
                    startTime.Time = TimeSpan.Parse(sTime.ToString());
                    endDate.Date = activity.endTime;
                    StringBuilder eTime = new StringBuilder();
                    eTime.AppendFormat("{0}:{1}:00", activity.endTime.Hour, activity.endTime.Minute);
                    endTime.Time = TimeSpan.Parse(eTime.ToString());
                    detail.Text = activity.detail;
                    maxNum.Text = activity.maxNum.ToString();
                    create.Content = "Update";
                }   
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
            StringBuilder start = new StringBuilder();
            StringBuilder end = new StringBuilder();
            start.AppendFormat("{0}/{1}/{2} {3}:{4}:00", startDate.Date.Year, startDate.Date.Month, startDate.Date.Day, startTime.Time.Hours, startTime.Time.Minutes);
            end.AppendFormat("{0}/{1}/{2} {3}:{4}:00", endDate.Date.Year, endDate.Date.Month, endDate.Date.Day, endTime.Time.Hours, endTime.Time.Minutes);

            if (place.Text == ""  ||
                name.Text == ""   ||
                detail.Text == "" ||
                maxNum.Text == "")
            {
                var show = new MessageDialog("各项输入不得为空").ShowAsync();
                return;
            }

            string pattern = @"^[0-9]*$";
            if(!Regex.IsMatch(maxNum.Text, pattern))
            {
                var show = new MessageDialog("最大人数不合法").ShowAsync();
                return;
            }

            if(int.Parse(maxNum.Text)<=0)
            {
                var show = new MessageDialog("最大参与人数要大于0").ShowAsync();
                return;
            }

            if (DateTime.Parse(start.ToString()).CompareTo(DateTime.Now) == -1 ||
               DateTime.Parse(end.ToString()).CompareTo(DateTime.Parse(start.ToString())) == -1)
            {
                var show = new MessageDialog("起始时间要大于当前日期且结束时间不得小于起始时间").ShowAsync();
                return;
            }

            if (place.Tag == null)
            {
                var show = new MessageDialog("当前地址不存在").ShowAsync();
                return;
            }

            if ((string)create.Content == "Create")
            {
                await Services.EventServices.addEvent(Store.getInstance().getToken(), name.Text, detail.Text, start.ToString(), end.ToString(), place.Tag.ToString(), maxNum.Text, file);
            }
            else
            {
                await Services.EventServices.updateEvent(Store.getInstance().getToken(), name.Tag.ToString(), name.Text, detail.Text, start.ToString(), end.ToString(), place.Tag.ToString(), maxNum.Text, file);
            }
            ControlBar.Current.NavigateToPage("Index");
            TileService.UpdateTiles();
        }
    }
}
