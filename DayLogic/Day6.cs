using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2021.DayLogic
{
    public class Day6 : Day
    {
        public override void PartOne()
        {
            var input = GetInputFromFile();
            var chunks = input[0].Split(',');
            long[] fishByAge = new long[9] {0, 0, 0, 0, 0, 0, 0, 0, 0};
            foreach (var inputAge in chunks)
            {
                var age = int.Parse(inputAge);
                fishByAge[age]++;
            }

            var totalFish = FishyBusiness(ref fishByAge, 80);

            Log($"There are now {totalFish} fish after 80 days\n");
            
        }
        public override void PartTwo()
        {
            // Fish sex for 256 days now.
            // time to break out the multi-threader.

            Log("Starting Day Two");
            
            var input = GetInputFromFile();
            var chunks = input[0].Split(',');
            long[] fishByAge = new long[9] {0, 0, 0, 0, 0, 0, 0, 0, 0};
            foreach (var t in chunks)
            {
                var age = int.Parse(t);
                fishByAge[age]++;
            }

            // Cache start for timing
            var startTimestamp = DateTime.Now;

            var total = FishyBusiness(ref fishByAge, 256);

            // Give us a blank line, after the \r log's
            Console.WriteLine("");

            var endTimestamp = DateTime.Now;

            Log($"Processed in {(endTimestamp-startTimestamp).TotalMilliseconds}ms");

            Log($"Total Fish after 256 days of activity: {total}");
        }

        long FishyBusiness(ref long[] fishByAge, int totalDays)
        {
            int currentDay = 0;
            while (currentDay < totalDays)
            {
                // bucketed fish by age, lets batch modify/shuffle.
                
                // get our eggs.
                long eggs = fishByAge[0];

                // LogFormat(
                //     $"\rFish Sexy Time Progress: Day {{0}}/{{1}} {{2:N1}}%",
                //     currentDay+1, 256,
                //     ((1+currentDay) / (float) 256) * 100.0f);

                for (var i = 1; i < fishByAge.Length; ++i)
                {
                    // shuffle the fish count down
                    fishByAge[i - 1] = fishByAge[i];
                }

                // Old fish die hard
                fishByAge[8] = 0;

                // add egg counts. Add onto age 6, set age 8 to new egg.
                fishByAge[6] += eggs;
                fishByAge[8] = eggs;
                
                // increment day
                ++currentDay;
            }

            // Count them babies
            long total = 0;
            foreach (var t in fishByAge)
            {
                total += t;
            }

            return total;
        }
    }
}