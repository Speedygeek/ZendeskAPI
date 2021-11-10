// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text;

namespace Speedygeek.ZendeskAPI.Utilities
{
    internal enum SeparatedCaseState
    {
        Start,
        Lower,
        Upper,
        NewWord,
    }

    /// <summary>
    /// String helpers
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// converts string to lower Snake Case
        /// </summary>
        /// <param name="value"> string to convert</param>
        /// <returns>string in snake case</returns>
        public static string ToSnakeCase(this string value) => ToSeparatedCase(value, '_');

        private static string ToSeparatedCase(string s, char separator)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            StringBuilder sb = new();
            var state = SeparatedCaseState.Start;

            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                {
                    if (state != SeparatedCaseState.Start)
                    {
                        state = SeparatedCaseState.NewWord;
                    }
                }
                else if (char.IsUpper(s[i]))
                {
                    switch (state)
                    {
                        case SeparatedCaseState.Upper:
                            var hasNext = i + 1 < s.Length;
                            if (i > 0 && hasNext)
                            {
                                var nextChar = s[i + 1];
                                if (!char.IsUpper(nextChar) && nextChar != separator)
                                {
                                    sb.Append(separator);
                                }
                            }

                            break;
                        case SeparatedCaseState.Lower:
                        case SeparatedCaseState.NewWord:
                            sb.Append(separator);
                            break;
                    }

                    char c;
                    c = char.ToLowerInvariant(s[i]);
                    sb.Append(c);

                    state = SeparatedCaseState.Upper;
                }
                else if (s[i] == separator)
                {
                    sb.Append(separator);
                    state = SeparatedCaseState.Start;
                }
                else
                {
                    if (state == SeparatedCaseState.NewWord)
                    {
                        sb.Append(separator);
                    }

                    sb.Append(s[i]);
                    state = SeparatedCaseState.Lower;
                }
            }

            return sb.ToString();
        }
    }
}
