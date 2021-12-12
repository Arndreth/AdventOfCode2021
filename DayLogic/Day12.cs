using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AoC2021.DayLogic
{
    class Cave
    {
        
        // stats
        public static int RouteCount;
        public static List<List<Cave>> Routes = new();
        
        private string m_caveName;
        private bool m_smallCave;

        public string CaveName => m_caveName;
        public bool SmallCave => m_smallCave;

        public bool IsStartCave => m_caveName.Equals("start");
        public bool IsEndCave => m_caveName.Equals("end");

        private List<Cave> m_caveConnections = new();
        public List<Cave> Connections => m_caveConnections;

        public Cave(string caveName)
        {
            m_caveName = caveName;
            m_smallCave = IsLower(caveName);
        }

        public void AddConnection(Cave connection)
        {
            m_caveConnections.Add(connection);
        }

        bool IsLower(string cave)
        {
            return Char.IsLower(cave[0]);
            
        }

        public static void FindRoutes(Cave cave, List<Cave> route, Dictionary<Cave, int> visitedCount)
        {
            if (cave.IsStartCave)
            {
                foreach (var connection in cave.Connections)
                {
                    FindRoutes(connection, new(), new());
                }

                return;
            }

            if (cave.IsEndCave)
            {
                RouteCount++;
                Routes.Add(new(route){cave});
                return;
            }
            
            // part two.

            route.Add(cave);

            if (cave.SmallCave)
            {
                visitedCount.TryAdd(cave, 0);
                visitedCount[cave]++;
            }

            foreach (var connection in cave.Connections)
            {
                if (connection.IsStartCave) continue;
                
                if (!connection.IsEndCave && connection.SmallCave)
                {
                    // have we visited any cave twice? Yes? All rest must be one visit.
                    bool twice = visitedCount.Any(x => x.Value == 2);
                    
                    // If we've visited this connection before, and visited another cave twice, skip.
                    if (route.Contains(connection) && twice) continue;
                    FindRoutes(connection,new( route), new(visitedCount));
                    continue;

                }
                FindRoutes(connection,new( route), new(visitedCount));
            }
        }
    }
    
    public class Day12 : Day
    {
        private List<Cave> m_Caves = new();

        public override void PartOne()
        {
            var input = GetInputFromFile(true);
            foreach (var line in input)
            {
                Log(line);
                var chunks = line.Split('-');
                Cave? from = m_Caves.Find(x => x.CaveName == chunks[0]);
                if (from == null)
                {
                    from = new Cave(chunks[0]);
                    m_Caves.Add(from);
                }

                Cave? to = m_Caves.Find(x => x.CaveName.Equals(chunks[1]));
                if (to == null)
                {
                    to = new Cave(chunks[1]);
                    m_Caves.Add(to);
                }
                from.AddConnection(to);
                to.AddConnection(from);
            }

            // Find our start cave.
            Cave? startCave = m_Caves.Find(x => x.IsStartCave);

            // from start, find all possible routes to end.
            int routeCount = 0;

            List<Cave> route = new() {startCave};
            List<List<Cave>> routes = new();

            Cave.FindRoutes(startCave,new(), new());
            Log($"Final Routes: {Cave.RouteCount}");
            
        }

        public override void PartTwo()
        {
            //throw new System.NotImplementedException();
        }
    }
}