using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AoC2021.DayLogic
{

    class Octopus
    {
        public Point GridIndex;

        private int m_energyLevel;
        private bool bHasFlashed = false;

        public int EnergyLevel
        {
            get => m_energyLevel;
            set
            {
                m_energyLevel = value;
                if (WantsToFlash && !bHasFlashed)
                {
                    FlashNeighbours();
                }

                if (value == 0)
                {
                    bHasFlashed = false;
                }
            }
        }

        public bool WantsToFlash => EnergyLevel > 9;
        List<Octopus> _neighbours;

        public Octopus(int rawIndex, int energyLevel, int width)
        {
            GridIndex = new Point(rawIndex % width, rawIndex / width);
            _neighbours = new();
            m_energyLevel = energyLevel;
        }

        void FlashNeighbours()
        {
            bHasFlashed = true;
            _neighbours.ForEach(x => x.EnergyLevel++);
        }

        public void FindNeighbours(ref List<Octopus> grid)
        {
            _neighbours = (from o in grid let d = (o.GridIndex - (Size) GridIndex) where o != this && MathF.Abs(d.X) <= 1 && MathF.Abs(d.Y) <= 1 select o)
                .ToList();

            Debug.Assert(_neighbours.Count > 0 && _neighbours.Count < 9,
                $"Gridspace {GridIndex} has {_neighbours.Count} neighbours, which is invalid");
        }
    }
    public class Day11 : Day
    {
        private int m_gridWidth = 0;

        private List<Octopus> m_Grid = new();
        public override void PartOne()
        {
            var input = GetInputFromFile();
            m_gridWidth = input[0].Length;
            int index = 0;
            for (int i = 0; i < input.Length; ++i)
            {
                for (int j = 0; j < input[i].Length; ++j)
                {
                    m_Grid.Add(new Octopus(index, int.Parse(input[i][j].ToString()), m_gridWidth));
                    ++index;
                }
            }

            m_Grid.ForEach(x => x.FindNeighbours(ref m_Grid));

            // Part 1 - 100 iterations
            //

            int flashDance = 0;
            int day = 0;
            bool bSyncFlash = false;
            while (!bSyncFlash)
            {
                ++day;

                m_Grid.ForEach(x => x.EnergyLevel++);
                // Finally, find all the octopus that want to, or have flashed.
                var flashCount = m_Grid.Count(x => x.WantsToFlash);
                
                flashDance += flashCount;
                
                // part two.
                bSyncFlash = flashCount == m_Grid.Count;
                // Reset flash level to 0
                m_Grid.ForEach(x =>
                {
                    if (x.WantsToFlash) x.EnergyLevel = 0;
                });

            }

            Log($"total flashdance after {day} days: {flashDance}");
            Log($"Sync at stage: {day}");

        }


        public override void PartTwo()
        {
            // throw new System.NotImplementedException();
        }
    }
}