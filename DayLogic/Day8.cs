using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace AoC2021.DayLogic
{
    public class Day8 : Day
    {
        private readonly int[] m_segmentCount = new[]
        {
            6,
            2,
            5,
            5,
            4,
            5,
            6,
            3,
            7,
            6,
        };

        Hashtable valueMapping = new Hashtable();

        private Dictionary<int, List<string>> segmentValidators = new Dictionary<int, List<string>>()
        {
            {2, new List<string>() {"cf"}},
            {3, new List<string>() {"acf"}},
            {4, new List<string>() {"bcdf"}},
            {5, new List<string>() {"acdeg", "acdfg", "abdfg"}},
            {6, new List<string>() {"abcefg", "abdefg", "abcdfg"}},
            {7, new List<string>() {"abcdefg"}}
        };

        public override void PartOne()
        {
            var input = GetInputFromFile();

            int[] numbersToCheck = new[]
            {
                m_segmentCount[1],
                m_segmentCount[4],
                m_segmentCount[7],
                m_segmentCount[8]
            };
            int totalWords = 0;
            foreach (var line in input)
            {
                ProcessLine(line, numbersToCheck, out int sum);
                totalWords += sum;
            }

            Log($"Total Found which match: {totalWords}");
        }

        void ProcessLine(string input, int[] toCheck, out int sum)
        {
            string pre = input.Split('|')[1].Trim();
            string[] chunks = pre.Split(' ');
            sum = (from p in chunks let l = p.Length where toCheck.Contains(l) select l).Count();
        }

        public override void PartTwo()
        {
            var input = GetInputFromFile();

            // For determining whcih is which later, store as string[]
            valueMapping.Add(5, new string[] {"2", "3", "5"});
            valueMapping.Add(6, new string[] {"0", "6", "9"});

            // Unique values, store as string
            valueMapping.Add(2, "1");
            valueMapping.Add(3, "7");
            valueMapping.Add(4, "4");
            valueMapping.Add(7, "8");

            int sum = 0;
            foreach (var line in input)
            {
                DecodeLine(line, out int tally);
                sum += tally;
            }

            Log($"Final Sum: {sum}");

        }

        /// <summary>
        /// Generates a new Codex based on which generation of iteration we're trying.
        /// </summary>
        /// <param name="codex"></param>
        /// <param name="generation"></param>
        void GenerateCodex(ref Dictionary<string, string> codex, int generation)
        {
            string segments = "abcdefg";
            string remaining = "abcdefg";
            int index = 0;

            Random random = new Random(generation);

            while (remaining.Length > 0)
            {
                string selected = remaining[random.Next(0, remaining.Length)].ToString();
                codex[segments[index].ToString()] = selected;

                remaining = remaining.Replace(selected, String.Empty);
                ++index;
            }

        }


        /// <summary>
        /// Checks a given cipher (value input) against the current codex iteration
        /// </summary>
        /// <param name="codex"></param>
        /// <param name="toCheck"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        bool CheckCipherAgainstCodex(ref Dictionary<string, string> codex, string toCheck, string comparison)
        {
            bool cipherValidated = true;
            for (int i = 0; i < comparison.Length; ++i)
            {
                // get the key for the value.
                string key = codex[comparison[i].ToString()];
                cipherValidated &= toCheck.Contains(key);
            }

            return cipherValidated;
        }

        /// <summary>
        /// Can check a cipher against all modelled validators.
        /// </summary>
        /// <param name="codex"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        bool ValidateCipher(ref Dictionary<string, string> codex, string toCheck)
        {
            var validators = segmentValidators[toCheck.Length];
            bool anyValidated = false;
            foreach (var comparison in validators)
            {
                anyValidated |= CheckCipherAgainstCodex(ref codex, toCheck, comparison);
            }

            return anyValidated;
        }

        void DecodeLine(string input, out int outputValue)
        {
            Log($"Decoding: {input}");

            Dictionary<int, List<string>> mappedValues = new();
            string[] chunks = input.Split('|')[0].Trim().Split(' ');

            string[] toDecode = input.Split('|')[1].Trim().Split(' ');

            foreach (var item in chunks)
            {
                int key = item.Length;
                if (!mappedValues.ContainsKey(key))
                {
                    mappedValues.Add(key, new List<string>());
                }

                mappedValues[key].Add(item);
            }

            Dictionary<string, string> codex = new()
            {
                {"a", "d"},
                {"b", "e"},
                {"c", "a"},
                {"d", "f"},
                {"e", "g"},
                {"f", "b"},
                {"g", "c"}
            };


            bool bCompleted = false;
            int cipherGeneration = 0;

            int maxGenerations = 850000; // there are 823k vairations of letters in a 7-seg display. (7^7)
            while (bCompleted == false && cipherGeneration < maxGenerations)
            {
                ++cipherGeneration;

                bCompleted = true;

                // do all the values make sense and pass validation?
                foreach (var check in chunks)
                {
                    bCompleted &= ValidateCipher(ref codex, check);
                }

                // Only generate a new Cipher if we have failed.
                if (!bCompleted)
                {
                    GenerateCodex(ref codex, cipherGeneration);
                }

                // if bCompleted remains true, then it's a valid cipher.
            }

            if (cipherGeneration >= maxGenerations)
            {
                throw new Exception($"Tried {maxGenerations} generations of cipher and no dice");
            }

            string?[] decoded = new string?[toDecode.Length];
            // Decode the cipher into a 4-digit number.

            for (int i = 0; i < decoded.Length; ++i)
            {
                if (valueMapping[toDecode[i].Length] is string)
                {
                    decoded[i] = valueMapping[toDecode[i].Length] as string;
                    continue;
                }

                // Determine our cipher
                var validators = segmentValidators[toDecode[i].Length];
                int whichValidator = -1;
                for (int c = 0; c < validators.Count && whichValidator == -1; ++c)
                {
                    if (CheckCipherAgainstCodex(ref codex, toDecode[i], validators[c]))
                    {
                        whichValidator = c;
                    }
                }

                decoded[i] = ((string[]) valueMapping[toDecode[i].Length]!)[whichValidator];
            }

            string preBuild = String.Join("", decoded);
            if (!int.TryParse(preBuild, out var outValue))
            {
                throw new Exception($"Tried to turn [{String.Join('-', decoded)}] into a numeric value and failed");
            }

            Log($"Decoding Value: {outValue}");

            outputValue = outValue;
        }
    }
}