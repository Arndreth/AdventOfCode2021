using System;
using System.IO;

namespace AoC2021.DayLogic
{
    public class Day2 : BaseDay, IDay
    {
        public void PartOne()
        {
            var input = GetInputFromFile();

            int depth = 0;
            int location = 0;

            foreach (var instruction in input)
            {
                string[] split = instruction.Split(' ');
                int delta = int.Parse(split[1]);
                switch (split[0])
                {
                    case "forward":
                        location += delta;
                        break;
                    case "up":
                        depth -= delta;
                        break;
                    case "down":
                        depth += delta;
                        break;
                }
            }

            Log($"Final Location is {location} at depth {depth}, final value is {location * depth}");
        }

        public void PartTwo()
        {
            var input = GetInputFromFile();

            int depth = 0;
            int location = 0;
            int aim = 0;

            foreach (var instruction in input)
            {
                string[] split = instruction.Split(' ');
                int delta = int.Parse(split[1]);
                switch (split[0])
                {
                    case "forward":
                        location += delta;
                        depth += delta * aim;
                        break;
                    case "up":
                        aim -= delta;
                        break;
                    case "down":
                        aim += delta;
                        break;
                }
            }

            Log($"Final Location is {location} at depth {depth}, final value is {location * depth}");
        }
    }
}