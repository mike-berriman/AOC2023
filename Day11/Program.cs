namespace Day11
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\AOC2023\Day11\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AOC2023\Day11\InputData.txt";

        static void Main(string[] args)
        {
            Day11 day1 = new Day11();
            day1.Execute1(fileName);
            day1.Execute1(fileName2);

            day1.Execute2(fileName);
            day1.Execute2(fileName2);

            Console.ReadKey();
        }
    }
}