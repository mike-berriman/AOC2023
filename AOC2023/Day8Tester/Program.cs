namespace Day8Tester
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\AOC2023\Day8Tester\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AOC2023\Day8Tester\InputData.txt";

        static void Main(string[] args)
        {
            Day8Tester day1 = new Day8Tester();
            day1.Execute1(fileName);
            day1.Execute1(fileName2);

            day1.Execute2(@"D:\temp\advent\AOC2023\Day8\TestData2.txt");
            day1.Execute2(fileName2);

            Console.ReadKey();
        }
    }
}