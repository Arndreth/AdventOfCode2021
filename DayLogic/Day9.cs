using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021.DayLogic
{
    public class Day9 : Day
    {
        private static readonly List<int> _seaBed = new();
        private static int _width, _height;
        readonly List<int> m_lowPoints = new();
        readonly List<int> m_lowPointIndexes = new();

        class Basin
        {
            private List<int> m_IncludedPoints = new();

            public int Size => m_IncludedPoints.Count;

            public List<int> Points => m_IncludedPoints;

            public Basin(int origin)
            {
            }

            void AddPoint(int index)
            {
                if (!m_IncludedPoints.Contains(index))
                {
                    m_IncludedPoints.Add(index);
                }
            }

            public void AddPoints(List<int> points)
            {
                m_IncludedPoints.AddRange(points);
            }
            

            public void Traverse(int currentIndex)
            {
                // check all surrounding points
            
                // then if it's higher, we want to set a traverse from that index.
                int test = _seaBed[currentIndex];

                if (test == 9) return;
                (int y, int x) = ToCoordinate(currentIndex);

                AddPoint(currentIndex);
            
                var upIndex = ToIndexer(x, y - 1);
                var downIndex = ToIndexer(x, y + 1);
                var leftIndex = ToIndexer(x - 1, y);
                var rightIndex = ToIndexer(x + 1, y);

                int up = y - 1 >= 0 && upIndex >= 0 && upIndex < _seaBed.Count ? _seaBed[upIndex] : -1;
                int down =  y + 1 < _height && downIndex >= 0  && downIndex < _seaBed.Count ? _seaBed[downIndex] : -1;
                int left = x - 1 >= 0 && leftIndex >= 0 && leftIndex < _seaBed.Count  ? _seaBed[leftIndex] : -1;
                int right = x + 1 < _width && rightIndex >= 0 && rightIndex < _seaBed.Count  ? _seaBed[rightIndex] : -1;

                if (up > test) Traverse(upIndex);
                if (down > test) Traverse(downIndex);
                if (left > test) Traverse(leftIndex);
                if (right > test) Traverse(rightIndex);

            }

            public void PrintBasin()
            {
                string line = string.Empty;
                for (int y = 0; y < _height; ++y)
                {
                    line = string.Empty;
                    for (int x = 0; x < _width; ++x)
                    {
                        int index = ToIndexer(x, y);
                        line += m_IncludedPoints.Contains(index) ? _seaBed[index] : ".";
                        line += " ";
                    }

                    Console.WriteLine(line);
                }
            }
        }
        static int ToIndexer(int x, int y)
        {
            return y *  _width + x;
        }

        static (int, int) ToCoordinate(int index)
        {
            return (index / _width, index % _width);
        }

        public override void PartOne()
        {
            var input = GetInputFromFile();

            _width = input[0].Length; 
            _height = input.Length;

            // create the seabed
            foreach (var line in input)
            {
                foreach (var depth in line)
                {
                    _seaBed.Add(byte.Parse(depth.ToString()));
                }
            }

            for (int y = 0; y < _height; ++y)
            {
                for (int x = 0; x < _width; ++x)
                {
                    var index = ToIndexer(x, y);
                    var upIndex = ToIndexer(x, y - 1);
                    var downIndex = ToIndexer(x, y + 1);
                    var leftIndex = ToIndexer(x - 1, y);
                    var rightIndex = ToIndexer(x + 1, y);

                    int up = upIndex >= 0 && upIndex < _seaBed.Count ? _seaBed[upIndex] : 100;
                    int down = downIndex >= 0  && downIndex < _seaBed.Count ? _seaBed[downIndex] : 100;
                    int left = leftIndex >= 0 && leftIndex < _seaBed.Count  ? _seaBed[leftIndex] : 100;
                    int right = rightIndex >= 0 && rightIndex < _seaBed.Count  ? _seaBed[rightIndex] : 100;
                    
                    int depth = _seaBed[index];
                    
                    // Check around us.
                    if (depth < up && depth < down && depth < left && depth < right)
                    {
                        m_lowPoints.Add(depth+1);
                        m_lowPointIndexes.Add(index);
                    }
                }
            }
            
            Log($"Sum of depth points: {m_lowPoints.Sum()}");
            
            
        }

        public override void PartTwo()
        {
            List<int> basinSizes = new();
            Basin megaBasin = new(0);
            // Time to go basin hunting.
            
            // from each of our low-points, expand back 'up'
            foreach (var index in m_lowPointIndexes)
            {
                Basin basin = new(index);
                
                // Traverse out from each point finding our basin size.
                // Recursion time BAYBEEE
                basin.Traverse(index);

                megaBasin.AddPoints(basin.Points); // So we can print a big map.

                basinSizes.Add(basin.Size);
            }
            
            megaBasin.PrintBasin();

            var topThree = basinSizes.OrderByDescending(x => x).Take(3).ToArray();
            var final = topThree[0] * topThree[1] * topThree[2];

            Log($"Final Summation of Basins: {final}");
        }
    }

    
}