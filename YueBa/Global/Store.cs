﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace YueBa.Global
{
    class Store
    {
        public String username;
        public String email;
        public String phone;
        public String token = null;
        public String img;
        private static Store instance;

        private Store() { }

        public static Store getInstance()
        {
            if (instance == null)
            {
                instance = new Store();
            }
            return instance;
        }

        public void Initialize()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("store"))
            {
                var composite = ApplicationData.Current.LocalSettings.Values["store"] as ApplicationDataCompositeValue;
                username = (String)composite["username"];
                email = (String)composite["email"];
                phone = (String)composite["phone"];
                token = (String)composite["token"];
                img = (String)composite["img"];
            } else
            {
                username = email = phone = token = null;
            }
        }

        public bool hasToken()
        {
            if (token != null)
                return true;
            return false;
        }

        public String getToken()
        {
            return token;
        }

        public void Logout()
        {
            ApplicationData.Current.LocalSettings.Values.Remove("store");
            Initialize();
        }
    }
}
