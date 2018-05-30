using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YueBa.Views;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace YueBa
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ControlBar : Page
    {
        public ControlBar()
        {
            this.InitializeComponent();
            this.ContentFrame.Navigate(typeof(Index));
        }

        private void Hamburgerbtn_Click(object sender, RoutedEventArgs e)
        {
            this.MainMenu.IsPaneOpen = !this.MainMenu.IsPaneOpen;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var x = (e.AddedItems[0] as ListViewItem).Name;

            NavigateToPage(x);
        }

        public void NavigateToPage(String page, String id = "")
        {
            switch (page)
            {
                case "Index":
                    this.ContentFrame.Navigate(typeof(Index));
                    break;
                case "Explore":
                    this.ContentFrame.Navigate(typeof(Explore));
                    break;
                case "AddPlace":
                    this.ContentFrame.Navigate(typeof(Place));
                    break;
                case "AddActivity":
                    this.ContentFrame.Navigate(typeof(Event));
                    break;
                case "EditEvent":
                    this.ContentFrame.Navigate(typeof(Event), id);
                    break;
                case "EditPlace":
                    this.ContentFrame.Navigate(typeof(Place), id);
                    break;
                case "Logout":
                    Global.Store.getInstance().Logout();
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(Login), "");
                    break;
            }
        }
    }
}
