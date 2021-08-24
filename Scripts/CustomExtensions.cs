using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomExtensions
{
    public static class StringExtension
    {
        public static void FancyDebug(this string str)
        {
            Debug.LogFormat("This string cocntains {0} characters.", str.Length);
        }
    }
}

