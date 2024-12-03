using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Day7
{

    public class Hand: IComparable<Hand>
    {
        public List<char> OrderedCards = new List<char> { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' }
        ;
        public string HandString { get; set; }
        public string ModifiedString { get; set; }
        public long Bid { get; set; }
        public int Rank { get; set; }

        public Hand(string handString, int bid, bool JokerWild)
        {
            HandString = handString;
            Bid = bid;
            Rank = getRank(JokerWild);

            if (!JokerWild)
            {
                OrderedCards = new List<char> { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };
            }
            else
            {
                OrderedCards = new List<char> { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };
            }
}

        public void GetJokerHand()
        {
            List<char> distinctVals = HandString.ToCharArray().Distinct().ToList();

            ModifiedString = HandString;
            if (distinctVals.Contains('J'))
            {
                int[] possibilities = new int[5];

                for (int i = 0; i < distinctVals.Count; i++)
                {
                    if (distinctVals[i] == 'J')
                    {
                        possibilities[i] = 0;
                    }
                    else
                    {
                        possibilities[i] = HandString.Count(x => x == distinctVals[i]);
                    }
                }

                int maxCount = possibilities.Max();
                if (maxCount == 0)
                {
                    ModifiedString = ModifiedString.Replace('J', 'A');
                }
                else
                {
                    int max = 999;
                    int maxIndex = 0;
                    for (int i = 0; i < possibilities.Length; i++)
                    {
                        if (possibilities[i] == maxCount)
                        {
                            if (OrderedCards.IndexOf(distinctVals[i]) < max)
                            {
                                max = OrderedCards.IndexOf(distinctVals[i]);
                                maxIndex = i;
                            }

                        }
                    }
                    ModifiedString = ModifiedString.Replace('J', distinctVals[maxIndex]);
                }
            }
        }


        public int getRank(bool JokerWild)
        {
            string currentHand = HandString;
            if (JokerWild)
            {
                GetJokerHand();
                currentHand = ModifiedString;
            }

            char[] distinctVals = currentHand.ToCharArray().Distinct().ToArray();

            if (distinctVals.Length == 1)
            {
                return 0;
            }
            else if (distinctVals.Length == 5)
            {
                return 6;
            }
            else if (distinctVals.Length == 4)
            {
                return 5;
            }
            else if (distinctVals.Length == 3)
            {
                int[] possibilities = new int[3];
                // 2 pair or 3 of a kind
                possibilities[0]= currentHand.Count(x => x == distinctVals[0]);
                possibilities[1]= currentHand.Count(x => x == distinctVals[1]);
                possibilities[2]= currentHand.Count(x => x == distinctVals[2]);

                if (possibilities.Max() == 3)
                {
                    return 3;
                }
                else
                {
                    return 4;
                }
            }
            else if (distinctVals.Length == 2)
            {
                int[] possibilities = new int[2];
                // 4 of a kind, or full house
                possibilities[0] = currentHand.Count(x => x == distinctVals[0]);
                possibilities[1] = currentHand.Count(x => x == distinctVals[1]);

                if (possibilities.Max() == 4)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }

            return -1;
        }



        public int CompareTo(Hand? other)
        {
            if (Rank > other.Rank)
            {
                return 1;
            }
            else if (other.Rank > Rank)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (HandString[i] != other.HandString[i])
                    {
                        int idx1 = OrderedCards.IndexOf(HandString[i]);
                        int idx2 = OrderedCards.IndexOf(other.HandString[i]);
                        return idx1 > idx2 ? 1 : -1;
                    }
                }
            }

            return 0;
        }
    }

    internal class Day7
    {
        internal void Execute1(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            List<Hand> hands = new List<Hand>();
            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string[] splits = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    Hand hand = new Hand(splits[0], Convert.ToInt32(splits[1]), false);
                    hands.Add(hand);
                }
            }

            hands.Sort();

            int rank = 1;
            for (int i = hands.Count-1; i >= 0; i--)
            {
                total += rank * hands[i].Bid;
                rank++;
            }

            Console.WriteLine("1) Result is: " + total);
        }

        internal void Execute2(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            List<Hand> hands = new List<Hand>();
            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string[] splits = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    Hand hand = new Hand(splits[0], Convert.ToInt32(splits[1]), true);
                    hands.Add(hand);
                }
            }

            hands.Sort();

            int rank = 1;
            for (int i = hands.Count - 1; i >= 0; i--)
            {
                total += rank * hands[i].Bid;
                rank++;
            }

            Console.WriteLine("2) Result is: " + total);
        }

    }
}
