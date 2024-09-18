using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RagnaCustoms.App.Extensions
{
    public static class StringExtension
    {
        public static string Slug(this string text)
        {
            Dictionary<char, string> cyrToLat = new Dictionary<char, string>
        {
            {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"}, {'е', "e"}, {'ё', "io"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"}, {'й', "y"},
            {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"}, {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"}, {'у', "u"}, {'ф', "f"},
            {'х', "h"}, {'ц', "ts"}, {'ч', "ch"}, {'ш', "sh"}, {'щ', "sht"}, {'ъ', "a"}, {'ы', "i"}, {'ь', "y"}, {'э', "e"}, {'ю', "yu"}, {'я', "ya"},
            {'А', "A"}, {'Б', "B"}, {'В', "V"}, {'Г', "G"}, {'Д', "D"}, {'Е', "E"}, {'Ё', "Io"}, {'Ж', "Zh"}, {'З', "Z"}, {'И', "I"}, {'Й', "Y"},
            {'К', "K"}, {'Л', "L"}, {'М', "M"}, {'Н', "N"}, {'О', "O"}, {'П', "P"}, {'Р', "R"}, {'С', "S"}, {'Т', "T"}, {'У', "U"}, {'Ф', "F"},
            {'Х', "H"}, {'Ц', "Ts"}, {'Ч', "Ch"}, {'Ш', "Sh"}, {'Щ', "Sht"}, {'Ъ', "A"}, {'Ы', "Y"}, {'Ь', "Y"}, {'Э', "E"}, {'Ю', "Yu"}, {'Я', "Ya"}
        };

            text = CyrillicToLatin(text, cyrToLat);
            var rgx = new Regex("[^a-zA-Z]");
            text = rgx.Replace(text, "");

            return text.ToLower();
        }
    

    static string CyrillicToLatin(string textcyr, Dictionary<char, string> cyrToLat)
    {
        string textlat = "";
        foreach (char c in textcyr)
        {
            if (cyrToLat.ContainsKey(c))
            {
                textlat += cyrToLat[c];
            }
            else
            {
                textlat += c;
            }
        }
        return textlat;
    }
    // Calculate Levenshtein Distance
    public static int LevenshteinDistance(this string source1, string source2)
        {
            var source1Length = source1.Length;
            var source2Length = source2.Length;
            var matrix = new int[source1Length + 1, source2Length + 1];
            // First calculation, if one entry is empty return full length
            if (source1Length == 0)
                return source2Length;
            if (source2Length == 0)
                return source1Length;
            // Initialization of matrix with row size source1Length and columns size source2Length
            for (var i = 0; i <= source1Length; matrix[i, 0] = i++)
            {
            }

            for (var j = 0; j <= source2Length; matrix[0, j] = j++)
            {
            }

            // Calculate rows and collumns distances
            for (var i = 1; i <= source1Length; i++)
            for (var j = 1; j <= source2Length; j++)
            {
                var cost = source2[j - 1] == source1[i - 1] ? 0 : 1;

                matrix[i, j] = Math.Min(
                    Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost);
            }

            // return result
            return matrix[source1Length, source2Length];
        }
    }
}