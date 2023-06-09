﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace nightClub.Helpers
{
    public class LoginHelper
    {
        public static string HashGen(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var originalBytes = Encoding.Default.GetBytes(password + "UTM2023_echipa2");
            var encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", "").ToLower();
        }
    }
}
