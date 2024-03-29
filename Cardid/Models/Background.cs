﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace Cardid.Models
{
    public class Background
    {
        public List<string> AllPaths = new List<string> {
            "bg1.png",
            "bg2.png",
            "bg3.png",
            "bg4.png",
            "bg5.png",
            "bg6.png",
            "bg7.png",
            "bg8.png",
            "bg9.png"
        };

        public string Path
        {
            get
            {
                AllPaths.Shuffle();
                return AllPaths[0];
            }
            set { }
        }

    }


    public static class Randomize
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }

}