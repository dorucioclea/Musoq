﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Musoq.Plugins.Attributes;

namespace Musoq.Plugins
{
    public abstract partial class LibraryBase
    {
        private readonly Soundex _soundex = new Soundex();

        [BindableMethod]
        public string Substring(string value, int index, int length)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (length < 1)
                return string.Empty;

            var valueLastIndex = value.Length - 1;
            var computedLastIndex = index + (length - 1);

            if (valueLastIndex < computedLastIndex)
                length = ((value.Length - 1) - index) + 1;

            return value.Substring(index, length);
        }

        [BindableMethod]
        public string Substring(string value, int length)
        {
            return Substring(value, 0, length);
        }

        [BindableMethod]
        public string Concat(params string[] strings)
        {
            var concatedStrings = new StringBuilder();

            foreach (var value in strings)
                concatedStrings.Append(value);

            return concatedStrings.ToString();
        }

        [BindableMethod]
        public string Concat(params char[] strings)
        {
            var concatedStrings = new StringBuilder();

            foreach (var value in strings)
                concatedStrings.Append(value);

            return concatedStrings.ToString();
        }

        [BindableMethod]
        public string Concat(string firstString, params char[] chars)
        {
            var concatedStrings = new StringBuilder();

            concatedStrings.Append(firstString);

            foreach (var value in chars)
                concatedStrings.Append(value);

            return concatedStrings.ToString();
        }

        public string Concat(char firstChar, params string[] strings)
        {
            var concatedStrings = new StringBuilder();

            concatedStrings.Append(firstChar);

            foreach (var value in strings)
                concatedStrings.Append(value);

            return concatedStrings.ToString();
        }

        [BindableMethod]
        public string Concat(params object[] objects)
        {
            var concatedStrings = new StringBuilder();

            foreach (var value in objects)
                concatedStrings.Append(value);

            return concatedStrings.ToString();
        }

        [BindableMethod]
        public bool Contains(string content, string what)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(what))
                return false;

            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(content, what, CompareOptions.IgnoreCase) >= 0;
        }

        [BindableMethod]
        public int IndexOf(string value, string text)
        {
            return value.IndexOf(text, StringComparison.OrdinalIgnoreCase);
        }

        [BindableMethod]
        public string Soundex(string value)
        {
            return _soundex.For(value);
        }

        [BindableMethod]
        public bool HasWordThatSoundLike(string text, string word, string separator = " ")
        {
            var soundexedWord = _soundex.For(word);

            foreach (var tokenizedWord in text.Split(separator[0]))
            {
                if (soundexedWord == _soundex.For(tokenizedWord))
                    return true;
            }

            return false;
        }

        [BindableMethod]
        public bool HasTextThatSoundLikeSentence(string text, string sentence, string separator = " ")
        {
            var words = sentence.Split(separator[0]);
            var tokens = text.Split(separator[0]);
            var wordsMatchTable = new bool[words.Length];

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                var soundexedWord = _soundex.For(word);

                foreach (var token in tokens)
                {
                    if (soundexedWord == _soundex.For(token))
                    {
                        wordsMatchTable[i] = true;
                        break;
                    }
                }
            }

            return wordsMatchTable.All(entry => entry);
        }

        [BindableMethod]
        public string ToUpperInvariant(string value)
        {
            return value.ToUpperInvariant();
        }

        [BindableMethod]
        public string ToLowerInvariant(string value)
        {
            return value.ToLowerInvariant();
        }

        [BindableMethod]
        public string PadLeft(string value, string character, int totalWidth)
        {
            return value.PadLeft(totalWidth, character[0]);
        }

        [BindableMethod]
        public string PadRight(string value, string character, int totalWidth)
        {
            return value.PadRight(totalWidth, character[0]);
        }

        [BindableMethod]
        public string Head(string value, int length = 10) => value.Substring(0, length);

        [BindableMethod]
        public string Tail(string value, int length = 10) => value.Substring(value.Length - length, length);


        [BindableMethod]
        public int? LevenshteinDistance(string firstValue, string secondValue)
        {
            if (firstValue == null || secondValue == null)
                return null;

            return Fastenshtein.Levenshtein.Distance(firstValue, secondValue);
        }

        [BindableMethod]
        public char? GetCharacterOf(string value, int index)
        {
            if (value.Length <= index || index < 0)
                return null;

            return value[index];
        }

        [BindableMethod]
        public string Reverse(string value)
        {
            if (value == null)
                return null;

            if (value == string.Empty)
                return value;

            if (value.Length == 1)
                return value;

            return string.Concat(value.Reverse());
        }

        [BindableMethod]
        public string[] Split(string value, params string[] separators) => value.Split(separators, StringSplitOptions.None);

        [BindableMethod]
        public string LongestCommonSubstring(string source, string pattern)
            => new string(LongestCommonSequence(source, pattern).ToArray());

        [BindableMethod]
        public string Replicate(string value, int integer)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < integer; ++i)
                builder.Append(value);

            return builder.ToString();
        }

        [BindableMethod]
        public string Translate(string value, string characters, string translations)
        {
            if (value == null)
                return null;

            if (characters == null || translations == null)
                return null;

            if (characters.Length != translations.Length)
                return null;

            var builder = new StringBuilder();

            for(int i = 0; i < value.Length; ++i)
            {
                var index = characters.IndexOf(value[i]);

                if (index == -1)
                    builder.Append(value[i]);
                else
                    builder.Append(translations[index]);
            }

            return builder.ToString();
        }

        [BindableMethod]
        public string CapitalizeFirstLetterOfWords(string value)
        {
            if (value == null)
                return null;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
        }
    }
}
