using AOCShared;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    public class Instruction
    {
        private static long GetValue(long[] registers, long operand)
        {
            switch (operand)
            {
                case 0:
                    {
                        return 0;
                    }
                case 1:
                    {
                        return 1;
                    }
                case 2:
                    {
                        return 2;
                    }
                case 3:
                    {
                        return 3;
                    }
                case 4:
                    {
                        return registers[0];
                    }
                case 5:
                    {
                        return registers[1];
                    }
                case 6:
                    {
                        return registers[2];
                    }
                default:
                case 7:
                    {
                        return 1;
                    }
            }
        }

        public static long Calculate(long Opcode, long Operand, long[] registers, ref string outputString)
        {
            long jumpVal = -1;
            switch (Opcode)
            {
                case 0:
                case 6:
                case 7:
                    {
                        // adv, bdv, cdv
                        double numerator = registers[0];
                        double denominator = Math.Pow(2.0, GetValue(registers, Operand));

                        long result = (long)(numerator / denominator);


                        if (Opcode == 0)
                        {
                            registers[0] = result;
                        }
                        else if (Opcode == 6)
                        {
                            registers[1] = result;
                        }
                        else
                        {
                            registers[2] = result;
                        }
                        break;
                    }
                case 1:
                    {
                        // bxl
                        long part1 = registers[1];

                        registers[1] = part1 ^ Operand;
                        break;
                    }
                case 2:
                    {
                        // bst
                        long part2 = GetValue(registers, Operand);

                        registers[1] = part2 & 0x7;
                        break;
                    }
                case 3:
                    {
                        // jnz
                        long part1 = registers[0];

                        if (part1 != 0)
                        {
                            jumpVal = Operand;
                        }
                        break;
                    }
                case 4:
                    {
                        // bxc
                        registers[1] = registers[1] ^ registers[2];

                        break;
                    }
                case 5:
                    {
                        // out
                        long value = GetValue(registers, Operand);
                        if (outputString.Length != 0)
                        {
                            outputString += ',';
                        }
                        outputString += value & 0x7;

                        break;
                    }
            }

            return jumpVal;
        }
    }


    internal class Day17
    {
        private bool m_part2 = false;

        public long[] Registers { get; set; } = null;
        public List<long> Instructions { get; set; } = new List<long>();

        public Day17(bool part2)
        {
            m_part2 = part2;

            Registers = new long[3] { 0, 0, 0 };
        }

        public string Calculate(bool breakOnJump)
        {
            string output = string.Empty;
            for (int i = 0; i < Instructions.Count; i += 2)
            {
                long jumpVal = Instruction.Calculate(Instructions[i], Instructions[i + 1], Registers, ref output);

                if ((!breakOnJump) && (jumpVal >= 0))
                {
                    i = (int)(jumpVal - 2);
                }
            }
            return output;
        }

        public long Calculate1()
        {
            long total = 0;
            string output = string.Empty;
            long[] registersCopy = Registers.ToArray();

            output = Calculate(false);

            //Instructions = StringLibraries.GetListOfInts(output, ',');
            //Registers = registersCopy.ToArray();

            //output = Calculate();

            return total;
        }

        public long Calculate2()
        {
            long total = 0;
            long[] registersCopy = Registers.ToArray();

            List<string> resultStrings = new List<string>();
            //resultStrings.Add("2");
            //resultStrings.Add("2,4");
            //resultStrings.Add("2,4,1");
            //resultStrings.Add("2,4,1,1");

            resultStrings.Add("0");
            resultStrings.Add("3,0");
            resultStrings.Add("5,3,0");
            resultStrings.Add("5,5,3,0");
            resultStrings.Add("3,5,5,3,0");
            resultStrings.Add("0,3,5,5,3,0");
            resultStrings.Add("5,0,3,5,5,3,0");
            resultStrings.Add("4,5,0,3,5,5,3,0");
            resultStrings.Add("5,4,5,0,3,5,5,3,0");
            resultStrings.Add("1,5,4,5,0,3,5,5,3,0");
            resultStrings.Add("5,1,5,4,5,0,3,5,5,3,0");
            resultStrings.Add("7,5,1,5,4,5,0,3,5,5,3,0");
            resultStrings.Add("1,7,5,1,5,4,5,0,3,5,5,3,0");
            resultStrings.Add("1,1,7,5,1,5,4,5,0,3,5,5,3,0");
            resultStrings.Add("4,1,1,7,5,1,5,4,5,0,3,5,5,3,0");
            resultStrings.Add("2,4,1,1,7,5,1,5,4,5,0,3,5,5,3,0");

            long count = 0;
            for ( ;; )
            {
                string output = string.Empty;
                Registers[0] = count;
                Registers[1] = 0;
                Registers[2] = 0;

                output = Calculate(false);
                if (resultStrings.First().Equals(output))
                {
                    resultStrings.RemoveAt(0);

                    string preCount = Convert.ToString(count, 8);

                    Console.WriteLine(resultStrings.First() + ": " + preCount);

                    count = count * 8;
                    string postCount = Convert.ToString(count, 8);

                    if (resultStrings.Count == 0)
                    {
                        break;
                    }
                }
                else
                {
                    count += 1;
                }

            }


            return total;
        }


        internal void ProcessSingleInput(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.StartsWith("Register A:"))
                    {
                        Registers[0] = Convert.ToInt64(line.Substring(12));
                    }
                    else if (line.StartsWith("Register A:"))
                    {
                        Registers[1] = Convert.ToInt64(line.Substring(12));
                    }
                    else if (line.StartsWith("Register A:"))
                    {
                        Registers[2] = Convert.ToInt64(line.Substring(12));
                    }
                    else if (line.StartsWith("Program:"))
                    {
                        Instructions = StringLibraries.GetListOfInts(line.Substring(9), ',');
                    }
                }
            }
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
