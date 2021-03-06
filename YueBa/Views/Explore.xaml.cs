﻿using System;
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
using YueBa.Global;
using System.Collections.ObjectModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace YueBa.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Explore : Page
    {
        private Events MyEvents { get; set; }
        private Places MyPlaces { get; set; }
        private Store store;
        public Explore()
        {
            this.InitializeComponent();
        }

        private ObservableCollection<EventItem> events = new ObservableCollection<EventItem>();
        private ObservableCollection<PlaceItem> places = new ObservableCollection<PlaceItem>();

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            store = Store.getInstance();
            if (store != null)
            {
                MyEvents = await Services.EventServices.getAllEvents();
                foreach (var element in MyEvents.events)
                {
                    element.img = Config.api + element.img;
                    events.Add(element);
                }
                MyPlaces = await Services.PlaceServices.getAllPlaces();
                foreach (var element in MyPlaces.places)
                {
                    element.img = Config.api + element.img;
                    places.Add(element);
                }
            }
        }

        private void EventItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            EventItem eventItem = (EventItem)e.ClickedItem;
            ControlBar.Current.NavigateToPage("EventDetail", eventItem.id.ToString());
        }

        private void PlacesItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            PlaceItem placeItem = (PlaceItem)e.ClickedItem;
            ControlBar.Current.NavigateToPage("PlaceDetail", placeItem.id.ToString());
        }
    }
}

