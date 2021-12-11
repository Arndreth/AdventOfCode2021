using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021.DayLogic
{
    public class Day10 : Day
    {
        private string openers = "[{(<";

        private Dictionary<char, char> closers = new()
        {
            {'{', '}'},
            {'(', ')'},
            {'<', '>'},
            {'[', ']'},
        };

        public override void PartOne()
        {
            var inputs = GetInputFromFile();
            
            List<Stack<char>> incompleteLines = new();
            IDictionary<char, int> corruptedBits = new Dictionary<char, int>();
            
            // find invalid chunks
            foreach (var line in inputs)
            {
                bool bCorrupted = false;
                Stack<char> buckets = new();
                foreach (char c in line)
                {
                    if (openers.Contains(c))
                    {
                        // Opening a chunk.
                        buckets.Push(c);
                        continue;
                    }
                    
                    // Get the most recent chunk opener.
                    if (buckets.TryPop(out var top))
                    {
                        // If it doesn't match, we're corrupt
                        if (c != closers[top])
                        {
                            corruptedBits.TryAdd(c, 0); // fails if it already exists.
                            corruptedBits[c]++;
                            bCorrupted = true; // flag so we don't add it to incomplete.
                            break;
                        }
                    }
                }
                
                // Flag an incomplete line if we're not corrupted, and we have chunks left open.
                if (!bCorrupted && buckets.Count > 0)
                {
                    incompleteLines.Add(buckets);
                }
            }
            Log("------");
            string closingScoreSheet = ")]}>";

            int score = 0;
            foreach (var kvp in corruptedBits)
            {
                var idx = closingScoreSheet.IndexOf(kvp.Key);
                // THE POWER OF MATH
                score += kvp.Value * (idx == 0 ? 3 : 57 * ((int) MathF.Pow(21, idx - 1)));
            }
            Log($"Corruption Level: {score}");
            
            /* PART TWO */
            
            // Closing incomplete lines.
            List<long> finalScores = new();
            string scoreSheet = "([{<";
            foreach (var stack in incompleteLines)
            {
                long closingScore = 0;
                
                // We know how to complete this from the unfinished chunk openers we detected
                // while checking for corruption.
                while (stack.TryPop(out var c))
                {
                    closingScore *= 5;
                    closingScore += scoreSheet.IndexOf(c) + 1;
                }

                finalScores.Add(closingScore);
            }

            // Sort the scores, find the mid-point.
            // Always be an odd number, so len/2 is safe.
            finalScores = finalScores.OrderByDescending(x => x).ToList();
            var result = finalScores[(finalScores.Count / 2)];
                
            Log($"Middle result: {result}");
            
        }

        public override void PartTwo()
        {
            
            // Rolled into part-one _again_
        }
    }
}