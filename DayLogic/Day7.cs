using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AoC2021.DayLogic
{
    public class Day7 : Day
    {
        public override void PartOne()
        {

            List<int> crabPositions = new();
            Dictionary<int,int> fuelAmounts = new();
            var input = GetInputFromFile();
            var chunks = input[0].Split(',');

            foreach (var s in chunks)
            {
                crabPositions.Add(int.Parse(s));
            }
            
            var min = crabPositions.Min();
            var max = crabPositions.Max();

            for (var i = min; i < max; ++i)
            {
                fuelAmounts.Add(i, (from c in crabPositions let d = GenerateBurn(c-i) select d).Sum());
            }
            
            // Find smallest fuel
            var minFuel = fuelAmounts.Values.Min();
            Console.WriteLine($"Minimum Fuel {minFuel}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GenerateBurn(int delta)
        {
            delta = (int)MathF.Abs(delta);
            int fuel = 0;
            for (int i = 1; i <= delta; ++i)
            {
                fuel += i;
            }

            return fuel;
        }
        public override void PartTwo()
        {
            // Rolled into part one. Only change was the let d = MathF.Abs(c-i) into GenerateBurn(c-i)
        }
    }
}