﻿namespace Boilerplate
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\AOC2023\Boilerplate\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AOC2023\Boilerplate\InputData.txt";

        static void Main(string[] args)
        {
            Boilerplate day1 = new Boilerplate();
            day1.Execute1(fileName);
            day1.Execute1(fileName2);

            day1.Execute2(fileName);
            day1.Execute2(fileName2);

            Console.ReadKey();
        }
    }
}