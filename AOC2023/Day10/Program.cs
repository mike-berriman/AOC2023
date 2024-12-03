namespace Day10
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\AOC2023\Day10\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AOC2023\Day10\InputData.txt";

        static void Main(string[] args)
        {
            Day10 day1 = new Day10();
            day1.Execute1(fileName);
            day1.Execute1(fileName2);

            day1.Execute2(@"D:\temp\advent\AOC2023\Day10\TestData2.txt");
            day1.Execute2(fileName2);

            Console.ReadKey();
        }
    }
}