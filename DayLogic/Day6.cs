using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

            var totalFish = FishyBusiness(fishByAge, 80);

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

            var total = FishyBusiness(fishByAge, 256);
            var endTimestamp = DateTime.Now;

            // Give us a blank line, after the \r log's
            Console.WriteLine("");


            Log($"Processed in {(endTimestamp-startTimestamp).TotalMilliseconds}ms");

            Log($"Total Fish after 256 days of activity: {total}");
        }

        long FishyBusiness(long[] fishByAge, short totalDays)
        {
            short currentDay = 0;
            while (currentDay < totalDays)
            {
                // get our eggs.
                var eggs = fishByAge[0];

                for (int i = 1, j = 0; i < 9; ++i, ++j)
                {
                    // shuffle the fish count down
                    fishByAge[j] = fishByAge[i];
                }
                
                // add egg counts. Add onto age 6, set age 8 to new egg.
                fishByAge[6] += eggs;
                fishByAge[8] = eggs;
                
                // increment day
                currentDay++;
            }

            return fishByAge.Sum();
        }
    }
}