using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoBlog.Classes
{
    public class Utils
    {
        public static string CutText(string text, int textLength = 100)
        {
            if (text == null || text.Length <= textLength) {
                return text;
            } else {
                var shortText = text.Substring (0, textLength) + "...";
                return shortText;
            }
        }
    }
}