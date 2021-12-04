using System.Collections.Generic;
using AoC2021.DayLogic.ExtraClasses;

namespace AoC2021.DayLogic
{
    public class Day4 : Day
    {
        private List<BingoCard> m_bingoCards = new();
        public override void PartOne()
        {
            var input = GetInputFromFile();

            var numbersToCall = input[0].Split(',');

            GenerateCards(ref input);
            
            // Bingo cards generated.
            // Lets check the values.
            bool bWinningCard = false;
            for (int i = 0; i < numbersToCall.Length && !bWinningCard; ++i)
            {
                int draw = int.Parse(numbersToCall[i]);

                Log($"Playing Number -> {draw}");
                foreach (var card in m_bingoCards)
                {
                    if (card.PlayNumber(draw)) // we have a winner
                    {
                        int score = card.CalculateCardScore(draw);
                        bWinningCard = true;
                        Log($"Found winning card! Card index is {m_bingoCards.IndexOf(card)} Score calculated is {score}");
                    }
                }
            }
        }

        void GenerateCards(ref string[] input)
        {
            // from line 3, is when bingo card starts
            // read the input until the end.
            for (int i = 2; i < input.Length; i += 6)
            {
                string[] bingoInput = new[]
                {
                    input[i],
                    input[i + 1],
                    input[i + 2],
                    input[i + 3],
                    input[i + 4],
                };

                m_bingoCards.Add(bingoInput);
            }
        }

        public override void PartTwo()
        {
            var input = GetInputFromFile(false);

            m_bingoCards.Clear();
            var numbersToCall = input[0].Split(',');

            GenerateCards(ref input);

            List<BingoCard> toRemove = new();
            bool bWinningCard = false;
            int totalCards = m_bingoCards.Count;
            // We want to find the last card that will win this time.
            int callIndex = 0;
            int winningCards = 0;
            while (m_bingoCards.Count > 0 && callIndex < numbersToCall.Length)
            {
                int draw = int.Parse(numbersToCall[callIndex]);
                toRemove.Clear();

                Log($"Playing Number -> {draw}");
                foreach (var card in m_bingoCards)
                {
                    if (card.PlayNumber(draw)) // we have a winner
                    {
                        int score = card.CalculateCardScore(draw);
                        winningCards++;
                        toRemove.Add(card);

                        if (m_bingoCards.Count - toRemove.Count == 0)
                        {
                            Log($"Found the last winning card! Card index is {m_bingoCards.IndexOf(card)} Score calculated is {score}");

                        }
                    }
                }

                foreach (var card in toRemove)
                {
                    Log("Card has been completed, removing it from the pool");
                    m_bingoCards.Remove(card);
                }
                ++callIndex;
            }

        }
    }
}