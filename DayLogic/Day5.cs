using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace AoC2021.DayLogic
{
    public class Day5 : Day
    {
        private const int GRID_WIDTH = 1000;

        readonly struct Pipe
        {
            public readonly Point Origin;
            public readonly Point Destination;

            private readonly string _originData;

            public Pipe(string data)
            {
                var chunks = data.Split("->", StringSplitOptions.None);
                var left = chunks[0].Trim().Split(',');
                var right = chunks[1].Trim().Split(',');

                Origin = new Point(int.Parse(left[0]), int.Parse(left[1]));
                Destination = new Point(int.Parse(right[0]), int.Parse(right[1]));

                _originData = data;
            }

            public override string ToString()
            {
                return _originData;
            }

            public bool IsCardinal => !IsHorizontal && !IsVertical;

            // Pipe Directions.
            bool IsHorizontal => Origin.Y == Destination.Y;
            bool IsVertical => Origin.X == Destination.X;

            // Used to figure out which way we traverse.
            int HorizontalDirection => Origin.X > Destination.X ? -1 : 1;
            int VerticalDirection => Origin.Y > Destination.Y ? -1 : 1;
            
            /// <summary>
            /// Used to get the pipe direction for traversal.
            /// </summary>
            public Point TraverseDirection 
            {
                get
                {
                    var direction = Point.Empty;
                    if (IsCardinal)
                    {
                        direction.X = HorizontalDirection;
                        direction.Y = VerticalDirection;
                    }

                    if (IsHorizontal)
                    {
                        direction.X = HorizontalDirection;
                    }

                    if (IsVertical)
                    {
                        direction.Y = VerticalDirection;
                    }

                    return direction;
                }
            }
        }
        
        /// <summary>
        /// Converts a point into a 1-Dimensional array index point.
        /// </summary>
        /// <param name="point">Point we're converting</param>
        /// <returns>1D Array Indexer</returns>
        int ToIndexer(Point point)
        {
            return (point.X * GRID_WIDTH) + point.Y;
        }
        
        public override void PartOne()
        {
            var input = GetInputFromFile();
            // line input is
            // 5,5 -> 8,2

            // Generate Pipe data.
            List<Pipe> pipeData = new ();
            foreach (var line in input)
            {
                pipeData.Add(new Pipe(line));
            }
            
            // Make our seabed
            int[] seabed = new int[GRID_WIDTH*GRID_WIDTH];
            
            // Iterate through our pipes.
            foreach (var pipe in pipeData)
            {
                // No longer needed, as part-two requires cardinal pipes.
                // if (pipe.IsCardinal) continue;

                Point currentPipeMarker = pipe.Origin;
                Point traverseDirection = pipe.TraverseDirection;
                
                // Mark the start of the pipe.
                seabed[ToIndexer(currentPipeMarker)]++;

                Log($"Processing Pipe Instructions: {pipe}");
                
                // traverse the pipe, marking along the way
                while (currentPipeMarker != pipe.Destination)
                {
                    currentPipeMarker += (Size) traverseDirection;
                    seabed[ToIndexer(currentPipeMarker)]++;
                }
            }

            // prints a representation of the map. Uncomment to generate.
            //PrintMap(ref seabed);
            
            int overlapping = (from p in seabed where p >= 2 select p).Count();

            Log($"Overlapping Pipes Points: {overlapping}");
        }

        void PrintMap(ref int[] seabed)
        {
            string output = "";
            for (int y = 0; y < GRID_WIDTH; y++)
            {
                output = string.Empty;
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    output += seabed[x * GRID_WIDTH + y] + " ";
                }

                output = output.Replace('0', '.');
                Console.WriteLine(output);
            }
        }

        public override void PartTwo()
        {
            Log("Part Two integrated within Part one");
        }
    }
}