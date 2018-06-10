using System;
using System.Collections.Generic;

namespace TextRankCalc
{
    public class TextRankCalculator
    {

        private static readonly HashSet<char> VOWELS = new HashSet<char>
        { 'a', 'e', 'i', 'o', 'u', 'y' 
        };
        private static readonly HashSet<char> CONSONSNTS = new HashSet<char>
        { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n',
            'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z' 
        };

        static public float Get(string text)
        {
            int vowelsCount = 0;
            int consonantsCount = 0;

            foreach (char ch in text)
            {
                if (VOWELS.Contains(Char.ToLower(ch)))
                {
                    ++vowelsCount;
                }
                if (CONSONSNTS.Contains(Char.ToLower(ch)))
                {
                    ++consonantsCount;
                }
            }
            return (consonantsCount == 0) ? (vowelsCount) : ((float)vowelsCount / consonantsCount);
        }
    }
}
