using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2021.DayLogic.ExtraClasses
{
    public class BingoCard
    {
        private readonly List<int> m_bingoCard = new();
        private readonly List<int> m_activatedValues = new();

        private readonly bool[] m_scoreCard = new bool[25];

        (int row, int column) CoordinateOf(int index)
        {
            return (index / 5, index % 5);
        }

        public static implicit operator BingoCard(string[] input)
        {
            return new BingoCard(input);
        }
        
        BingoCard(string[] card)
        {
            // 5 lines in a bingo card.
            if (card.Length != 5)
            {
                throw new Exception($"[BingoClass] Trying to create a Bingo-Card with {card.Length} lines of numbers.");
            }

            foreach (var line in card)
            {
                // walk through the line. Can't split on spaces because of single digit numbers
                m_bingoCard.Add(int.Parse(line.Substring(0, 2)));
                m_bingoCard.Add(int.Parse(line.Substring(3, 2)));
                m_bingoCard.Add(int.Parse(line.Substring(6, 2)));
                m_bingoCard.Add(int.Parse(line.Substring(9, 2)));
                m_bingoCard.Add(int.Parse(line.Substring(12, 2)));
            }
        }

        public bool PlayNumber(int numberDrawn)
        {
            // Does the number exist in this card.
            var index = m_bingoCard.IndexOf(numberDrawn);
            if (index >= 0)
            {
                // we have a hit.
                m_activatedValues.Add(numberDrawn);
                m_scoreCard[index] = true;

                return CheckForCompletion(index);
            }

            return false;
        }

        bool CheckForCompletion(int index)
        {
            (int, int) position = CoordinateOf(index);
            // Check the row we're on, the column we're on;
            int rowStart = position.Item1*5; // fucking SCALE IT TO THE CORRECT ROW STARTER.
            int colStart = position.Item2;

            // Set initial flag
            bool rowComplete = true;
            for (int i = 0; i < 5; ++i)
            {
                // & flag check. Any false's will keep this perma-false.
                rowComplete &= m_scoreCard[rowStart + i];
            }

            if (rowComplete)
            {
                Console.WriteLine("Card: Row Completed! Marking as won");
                return true;
            }
            
            bool colComplete = true;
            for (int i = 0; i < 5; ++i)
            {
                colComplete &= m_scoreCard[colStart + (i * 5)];
            }

            if (colComplete)
            {
                Console.WriteLine("Card: Column Completed! Marking as won");
            }
            return colComplete;
        }

        public int CalculateCardScore(int lastNumberDrawn)
        {
            // find the sum of all unscored numbers
            var unscoredSum = (from p in m_bingoCard where !m_activatedValues.Contains(p) select p).Sum();

            Console.WriteLine($"Unscored Sum of numbers is {unscoredSum}");
            Console.WriteLine($"Last number drawn was {lastNumberDrawn}");
            
            

            return unscoredSum * lastNumberDrawn;
        }
    }
}