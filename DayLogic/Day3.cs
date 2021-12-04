using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AoC2021.DayLogic
{
    public class Day3 : Day
    {
        public override void PartOne()
        {
            var input = GetInputFromFile();
            int bitCount = input[0].Trim().Length;
            int[] oneCount = new int[bitCount];
            
            foreach (var line in input)
            {
                // check each bit.
                for (int i = 0; i < bitCount; ++i)
                {
                    oneCount[i] += line[i].Equals('1') ? 1 : 0;
                }
            }

            string gammaRaw = "";
            string epsilonRaw = "";

            for (int i = 0; i < bitCount; ++i)
            {
                string bit = oneCount[i] > input.Length / 2 ? "1" : "0";
                gammaRaw += bit;
                epsilonRaw += bit.Equals("0") ? "1" : "0";
            }

            int gamma = Convert.ToInt32(gammaRaw, 2);
            int epsilon = Convert.ToInt32(epsilonRaw, 2);

            Log($"Output Gamma {gammaRaw} [{gamma}]");
            Log($"Output Epsilon {epsilonRaw} [{epsilon}]");
            Log($"Final Power Output = {gamma * epsilon}");
        }

        public override void PartTwo()
        {
            var input = GetInputFromFile();
            
            var oxygenRating = FindRating(ref input, 1);
            var co2Rating = FindRating(ref input, 0);

            int oxygen = Convert.ToInt32(oxygenRating, 2);
            int co2 = Convert.ToInt32(co2Rating, 2);

            Log("Calculating Oxygen * C02");
            Log($"Oxygen is {oxygenRating} as {oxygen}");
            Log($"C02 is {co2Rating} as {co2}");
            
            Log($"Final Rating is {co2*oxygen}");
        }


        string FindRating(ref string[] input, int superiorBit)
        {
            // because i don't trust myself again with the wrong bitcount-length.
            int bitCount = input[0].Length;

            List<string> remainingSet = new(input);
            List<string> oneList = new();
            List<string> zeroList = new();
            // most common

            int bitIndex = 0;
            while (remainingSet.Count > 1 && bitIndex < bitCount)
            {
                Log($"Processing Bit index {bitIndex}...");
                oneList.Clear();
                zeroList.Clear();
                foreach (var item in remainingSet)
                {
                    switch (item[bitIndex])
                    {
                        case '0':
                            zeroList.Add(item);
                            break;
                        case '1':
                            oneList.Add(item);
                            break;
                    }
                }

                List<string> toRemove;
                
                if (superiorBit == 0)
                {
                    toRemove = zeroList.Count <= oneList.Count ? oneList : zeroList;
                }
                else
                {
                     toRemove = oneList.Count >= zeroList.Count ? zeroList : oneList;
                }

                foreach (var item in toRemove)
                {
                    remainingSet.Remove(item);
                }

                Log($"There are now {remainingSet.Count} entries left to check.");

                bitIndex++;
            }

            Log($"[Day3] Last entry found for superior bit {superiorBit} is {remainingSet[0]}");
            return remainingSet[0];
        }
    }
}

