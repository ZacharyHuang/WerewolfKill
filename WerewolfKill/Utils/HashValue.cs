using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace WerewolfKill.Utils
{
    public class HashValue
    {
        public static string md5(string input)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", string.Empty);
        }
    }
}