using System;
using System.IO;

namespace AoC2021.DayLogic
{
    public class Day1 : Day
    {
        
        public override void PartOne()
        {
            var input = GetInputFromFile();
        
            int increased = 0;
        
            for (int i = 1; i < input.Length; ++i)
            {
                int a = int.Parse(input[i]);
                int b = int.Parse(input[i - 1]);
        
                increased += (a > b) ? 1 : 0;
            }
        
            Log($"Depth Increases: {increased}");
        }
        
        public override void PartTwo()
        {
            var input = GetInputFromFile();
        
            int[] calculatedBuckets = new int[input.Length];
        
            int bucketCounter = 0;
        
            int[] bucketWindows = new int[4] {0, -1, -2, -3};
            int[] bucketTempCalcs = new int[4] {0, 0, 0, 0};
        
            for (int i = 0; i < input.Length; ++i)
            {
                for (int b = 0; b < 4; ++b)
                {
                    bucketWindows[b]++;
                    if (bucketWindows[b] == 3) // hit bucket capacity.
                    {
                        calculatedBuckets[bucketCounter] = bucketTempCalcs[b]; // store the sliding-bucket
                        bucketTempCalcs[b] = 0; // reset bucket
                        bucketWindows[b] = -1; // force to wait one reading
                        bucketCounter++; // next bucket calc goes in next slot etc.
                    }
                    if (bucketWindows[b] >= 0 && bucketWindows[b] < 3) // within reading range.
                    {
                        bucketTempCalcs[b] += int.Parse(input[i]);
                    }
                }
            }
            
            int increased = 0;
        
            for (int i = 1; i < bucketCounter; ++i)
            {
                int a = (calculatedBuckets[i]);
                int b = calculatedBuckets[i - 1];
        
                increased += (a > b) ? 1 : 0;
            }
            
            Log($"Depth Bucket Increases: {increased}");
        
        }
    }
}