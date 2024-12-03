using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{

    public class Card
    {
        public int CardNum { get; set; }
        public List<int> WinningNumbers { get; set; } = new List<int>();
        public List<int> Numbers { get; set; } = new List<int>();
        public int Instances { get; set; } = 1;

        Dictionary<int, Card> CardList = null;

        public Card(string line, Dictionary<int, Card> cardList)
        {
            CardList = cardList;
            ReadLine(line);
        }

        public void ReadLine(string line)
        {
            int colonINdex = line.IndexOf(':');
            CardNum = Convert.ToInt32(line.Substring(5, colonINdex - 5).Trim());

            line = line.Substring(colonINdex + 1);
            string[] splits = line.Split('|');

            WinningNumbers.AddRange(splits[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt32(x)));
            Numbers.AddRange(splits[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt32(x)));
        }

        public void ProcessCards()
        {
            int score = WinningNumbers.Intersect(Numbers).Count();

            for (int i = 0; i < score; i++)
            {
                CardList[i + CardNum +1].Instances += Instances;
            }

        }

        public int CalculateWin()
        {
            int score = 0;
            int count = WinningNumbers.Intersect(Numbers).Count();

            if (count > 0)
            {
                score = (int)(Math.Pow(2, count-1));
            }

            return score;
        }
    }

    internal class Day4
    {
        //string fileName = @"D:\temp\advent\AdventOfCoding\Day4\TestData1.txt";
        string fileName = @"D:\temp\advent\AdventOfCoding\Day4\InputData.txt";

        internal void Execute1()
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    Card card = new Card(line, CardList);
                    total += card.CalculateWin();

                }
            }

            Console.WriteLine("1) Result is: " + total);
        }

        public Dictionary<int, Card> CardList = new Dictionary<int, Card>();
        internal void Execute2()
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    Card card = new Card(line, CardList);
                    CardList.Add(card.CardNum, card);
                }
            }

            foreach (var card in CardList.Keys)
            {
                CardList[card].ProcessCards();
            }

            foreach (Card card in CardList.Values)
            {
                total += card.Instances;
            }


            Console.WriteLine("2) Result is: " + total);
        }

    }
}
