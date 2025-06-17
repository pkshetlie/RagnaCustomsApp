using Humanizer;
using Humanizer.Localisation.NumberToWords;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace RagnaCustoms.App.Extensions
{
    public static class StringExtension
    {

        public static string Slug(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "unrecognizedcharacters";

            // Translitération Unicode basique (ex : кириллица → kirillitsa)
            string latinized = TransliterateToAscii(input);

            // Supprimer les accents
            latinized = RemoveAccents(latinized);

            // Garder uniquement les lettres latines et chiffres
            string clean = Regex.Replace(latinized.ToLowerInvariant(), @"[^a-z0-9]", "");

            // Si tout a été supprimé → valeur par défaut
            return string.IsNullOrEmpty(clean) ? "unrecognized-characters" : clean;
        }

        private static string RemoveAccents(string input)
        {
            var normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string TransliterateToAscii(string input)
        {
            // Normalisation Unicode KD = compatibilité max (é → e, ™ → tm, etc.)
            return input.Normalize(NormalizationForm.FormKD);
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