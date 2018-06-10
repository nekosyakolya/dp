using System;
using System.Collections.Generic;

namespace VowelConsCounter
{
    public class VowelConsCounter
    {

        private static readonly HashSet<char> VOWELS = new HashSet<char>
        { 'a', 'e', 'i', 'o', 'u', 'y' 
        };
        private static readonly HashSet<char> CONSONANTS = new HashSet<char>
        { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n',
            'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z' 
        };

        static public Counters<int, int> Get(string text)
        {
            Counters<int, int> counters = new Counters<int, int>();
            counters.vowelsCount = 0;
            counters.consonantsCount = 0;

            foreach (char ch in text)
            {
                if (VOWELS.Contains(Char.ToLower(ch)))
                {
                    ++counters.vowelsCount;
                }
                if (CONSONANTS.Contains(Char.ToLower(ch)))
                {
                    ++counters.consonantsCount;
                }
            }
            return counters;
        }
    }
}
