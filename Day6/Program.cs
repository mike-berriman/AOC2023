﻿namespace Day6
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\AdventOfCoding\Day6\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AdventOfCoding\Day6\InputData.txt";

        static void Main(string[] args)
        {
            Day6 day1 = new Day6();
            day1.Execute1(fileName);
            day1.Execute1(fileName2);

            day1.Execute2(fileName);
            day1.Execute2(fileName2);

            Console.ReadKey();
        }
    }
}