using RagnaCustoms.App.Extensions;
using RagnaCustoms.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RagnaCustoms.Services
{
    public static class SongFolderNameFormatter
    {
        private static readonly Regex VariablePattern = new Regex(
            @"\$[a-z][a-z0-9]*(?:\.[a-z][a-z0-9]*)*",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly HashSet<char> InvalidFolderCharacters = new HashSet<char>(
            Path.GetInvalidFileNameChars()
                .Concat(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }));

        public static string Format(SongSearchModel song, string pattern, DateTime date)
        {
            if (song == null) throw new ArgumentNullException(nameof(song));

            var variables = BuildVariables(song, date);
            var template = string.IsNullOrWhiteSpace(pattern)
                ? Configuration.DefaultSongFolderPattern
                : pattern.Trim();

            var expanded = VariablePattern.Replace(template, match =>
            {
                string value;
                return variables.TryGetValue(match.Value.ToLowerInvariant(), out value) ? value : match.Value;
            });

            var folderName = Sanitize(expanded);
            return string.IsNullOrWhiteSpace(folderName) ? $"song-{song.Id.Slug()}" : folderName;
        }

        public static bool StartsWithNumericValue(SongSearchModel song, string pattern, DateTime date)
        {
            var folderName = Format(song, pattern, date);
            return folderName.Length > 0 && char.IsDigit(folderName[0]);
        }

        public static bool StartsWithNumericToken(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) return false;

            var value = pattern.TrimStart();
            if (value.Length > 0 && char.IsDigit(value[0])) return true;

            var firstVariable = VariablePattern.Match(value);
            if (!firstVariable.Success || firstVariable.Index != 0) return false;

            switch (firstVariable.Value.ToLowerInvariant())
            {
                case "$song.id":
                case "$date":
                case "$date.year":
                case "$date.month":
                case "$date.day":
                case "$time":
                    return true;
                default:
                    return false;
            }
        }

        private static Dictionary<string, string> BuildVariables(SongSearchModel song, DateTime date)
        {
            var songId = CleanValue(song.Id);
            var songName = song.Name.Slug();
            var author = song.Author.Slug();
            var mapper = song.Mapper.Slug();
            var level = CleanValue(song.Difficulties ?? string.Empty);
            var requester = CleanValue(string.Empty);

            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["$song.id"] = songId,
                ["$song.name"] = songName,
                ["$name"] = songName,
                ["$song.author"] = author,
                ["$author"] = author,
                ["$mapper.name"] = mapper,
                ["$mapper"] = mapper,
                ["$song.mapper"] = mapper,
                ["$song.level"] = level,
                ["$level"] = level,
                ["$song.requester"] = requester,
                ["$requester"] = requester,
                ["$date"] = date.ToString("yyyy-MM-dd"),
                ["$date.year"] = date.ToString("yyyy"),
                ["$date.month"] = date.ToString("MM"),
                ["$date.day"] = date.ToString("dd"),
                ["$time"] = date.ToString("HH-mm-ss")
            };
        }

        private static string CleanValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        private static string Sanitize(string value)
        {
            var characters = value.Select(character =>
                char.IsControl(character) || InvalidFolderCharacters.Contains(character) ? '-' : character);
            var sanitized = new string(characters.ToArray());
            sanitized = Regex.Replace(sanitized, @"\s+", " ").Trim().TrimEnd('.');
            return sanitized;
        }
    }
}
