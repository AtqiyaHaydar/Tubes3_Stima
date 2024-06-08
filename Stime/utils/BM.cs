using System;
using System.Collections.Generic;

namespace utils
{
    public class BoyerMoore
    {
        private int[] badCharTable;
        private int[] goodSuffixTable;
        private string pattern;
        private int patternLength;

        public BoyerMoore(string str1, string str2)
        {
            this.pattern = str1;
            this.patternLength = str1.Length;
            badCharTable = BuildBadCharTable(str1);
            goodSuffixTable = BuildGoodSuffixTable(str1);
        }

        // Membangun bad character table
        private int[] BuildBadCharTable(string pattern)
        {
            const int ASCII_SIZE = 256;
            int[] table = new int[ASCII_SIZE];

            // Inisialisasi semua kejadian sebagai -1
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = -1;
            }

            // Menetapkan nilai karakter sebagai kejadian terakhirnya dalam pola
            for (int i = 0; i < pattern.Length; i++)
            {
                table[(int)pattern[i]] = i;
            }

            return table;
        }

        // Membangun good sufix table
        private int[] BuildGoodSuffixTable(string pattern)
        {
            int patternLength = pattern.Length;
            int[] table = new int[patternLength];
            int[] suffixes = new int[patternLength];

            // Inisialisasi semua nilai ke nol
            for (int i = 0; i < patternLength; i++)
            {
                suffixes[i] = 0;
            }

            // Prapemrosesan pola untuk menemukan suffix
            suffixes[patternLength - 1] = patternLength;
            int g = patternLength - 1;
            int f = 0;

            for (int i = patternLength - 2; i >= 0; i--)
            {
                if (i > g && suffixes[i + patternLength - 1 - f] < i - g)
                {
                    suffixes[i] = suffixes[i + patternLength - 1 - f];
                }
                else
                {
                    if (i < g)
                    {
                        g = i;
                    }
                    f = i;
                    while (g >= 0 && pattern[g] == pattern[g + patternLength - 1 - f])
                    {
                        g--;
                    }
                    suffixes[i] = f - g;
                }
            }

            // Prapemrosesan untuk membangun good sufix table
            for (int i = 0; i < patternLength; i++)
            {
                table[i] = patternLength;
            }

            int j = 0;
            for (int i = patternLength - 1; i >= 0; i--)
            {
                if (suffixes[i] == i + 1)
                {
                    for (; j < patternLength - 1 - i; j++)
                    {
                        if (table[j] == patternLength)
                        {
                            table[j] = patternLength - 1 - i;
                        }
                    }
                }
            }

            for (int i = 0; i <= patternLength - 2; i++)
            {
                table[patternLength - 1 - suffixes[i]] = patternLength - 1 - i;
            }

            return table;
        }

        // Metode pencarian untuk memeriksa apakah pola ditemukan dalam teks
        public bool Search(string text)
        {
            int textLength = text.Length;
            int skip;

            for (int i = 0; i <= textLength - patternLength; i += skip)
            {
                skip = 0;
                for (int j = patternLength - 1; j >= 0; j--)
                {
                    if (pattern[j] != text[i + j])
                    {
                        skip = Math.Max(1, j - badCharTable[text[i + j]]);
                        skip = Math.Max(skip, goodSuffixTable[j]);
                        break;
                    }
                }
                if (skip == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static void Main(string[] args)
        {
            string text = "ANDHIKAFADILLAH";
            string pattern = "ANDHIKAFADILLAH";

            BoyerMoore bm = new BoyerMoore(pattern, text);
            if (bm.Search(text))
            {
                Console.WriteLine("Pola ditemukan dalam teks.");
            }
            else
            {
                Console.WriteLine("Pola tidak ditemukan dalam teks.");
            }
        }
    }
}
