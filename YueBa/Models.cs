﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YueBa
{
    public class Places
    {
        public PlaceItem[] places { get; set; }
    }

    public class PlaceItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string detail { get; set; }
        public float price { get; set; }
        public bool available { get; set; }
        public string img { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int ownerId { get; set; }
        public Owner owner { get; set; }
    }

    public class Owner
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Events
    {
        public EventItem[] events { get; set; }
    }

    public class EventItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int maxNum { get; set; }
        public string img { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int organizerId { get; set; }
        public int placeId { get; set; }
        public Organizer organizer { get; set; }
        public PlaceItem place { get; set; }
        public int participantsNum { get; set; }
    }

    public class Organizer
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }


    public class ErrorItem
    {
        public string error { get; set; }
    }
}
