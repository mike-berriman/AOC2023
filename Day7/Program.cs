﻿namespace Day7
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\AOC2023\Day7\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AOC2023\Day7\InputData.txt";

        static void Main(string[] args)
        {
            Day7 day1 = new Day7();
            day1.Execute1(fileName);
            day1.Execute1(fileName2);

            day1.Execute2(fileName);
            day1.Execute2(fileName2);

            Console.ReadKey();
        }
    }
}