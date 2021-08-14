using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RagnaCustoms.App.Extensions
{
    public static class StringExtension
    {

        public static string Slug(this string text)
        {
            Regex rgx = new Regex("[^a-zA-Z]");
            text = rgx.Replace(text, "");

            return text.ToLower();
        }
        
    }
}
