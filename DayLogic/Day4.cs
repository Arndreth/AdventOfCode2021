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

        public override void PartTwo()
        {
            //throw new System.NotImplementedException();
        }
    }
}