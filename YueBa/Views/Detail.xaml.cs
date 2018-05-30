using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Composition;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using YueBa.Services;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace YueBa.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Detail : Page
    {
        public Detail()
        {
            this.InitializeComponent();
            this.getDetailEvent("1");
            id = "1"; // 到时从页面传参获取id
        }

        DataTransferManager dtm;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dtm = DataTransferManager.GetForCurrentView();
            //创建event handler
            dtm.DataRequested += dtm_DataRequested;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            dtm.DataRequested -= dtm_DataRequested;
        }

        private async void dtm_DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var deferral = e.Request.GetDeferral();
            DataPackage dp = e.Request.Data;
            dp.Properties.Title = "共享文本";
            dp.Properties.Description = "共享文本详情描述";
            string str = "Name: " + detail.detail.name + "\n" + "Detail: " + detail.detail.detail + "\n" + detail.detail.startTime.ToString() + " - " + detail.detail.endTime.ToString() + '\n' + "地点: " + detail.detail.place.name + " 地址: " + detail.detail.place.address + '\n';
            dp.SetText(str);

            /*共享图片*/

            dp.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(detail.detail.img));
            dp.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(detail.detail.img)));

            e.Request.Data = dp;
            deferral.Complete();
        }

        private EventDetail detail;
        private String id;
        private ObservableCollection<Participator> participatorList = new ObservableCollection<Participator>();
        
        private async void getDetailEvent(String id)
        {
            detail = await EventServices.getEventDetail(Global.Store.getInstance().token, id);
            detail.detail.img = Global.Config.api + detail.detail.img;
            detail.detail.organizer.img = Global.Config.api + detail.detail.organizer.img;
            detail.detail.place.img = Global.Config.api + detail.detail.place.img;
            for (int i = 0; i < detail.detail.participators.Length; i++)
            {
                detail.detail.participators[i].img = Global.Config.api + detail.detail.participators[i].img;
                participatorList.Add(detail.detail.participators[i]);
            }

            EventImg.Source = new BitmapImage(new Uri(detail.detail.img));
            EventName.Text = detail.detail.name;
            organizerImg.ImageSource = new BitmapImage(new Uri(detail.detail.organizer.img));
            organizerName.Text = detail.detail.organizer.username;
            phone.Text = detail.detail.organizer.phone;
            placeImg.Source = new BitmapImage(new Uri(detail.detail.place.img));
            placeDetail.Text = detail.detail.place.detail;
            placeName.Text = detail.detail.place.name;
            placeAddress.Text = detail.detail.place.address;
            EventDetail.Text = detail.detail.detail;
            Status.Text = detail.detail.participators.Length + " / " + detail.detail.maxNum;

            if (detail.detail.editFlag)
            {
                EditBtn.Visibility = Visibility.Visible;
                participateBtn.Content = "删除";
            } else if (detail.detail.flag)
            {
                participateBtn.Content = "退出";
            } else
            {
                participateBtn.Content = "参加";
            }
        }

        private async void participateBtn_Click(object sender, RoutedEventArgs e)
        {
            String token = Global.Store.getInstance().token;
            bool flag = false;
            switch(participateBtn.Content)
            {
                case "参加":
                    flag = await EventServices.participateEvent(token, id);
                    break;
                case "退出":
                    flag = await EventServices.exitEvent(token, id);
                    break;
                case "删除":
                    flag = await EventServices.deleteEvent(token, id);
                    break;
            }
            if (flag)
            {
                new MessageDialog(participateBtn.Content + "事件成功!").ShowAsync();
            }
            while(participatorList.Count > 0)
            {
                participatorList.RemoveAt(0);
            }
            getDetailEvent(id);
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            //gotoEdit
        }

        private void shareBtn_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }
    }
}
