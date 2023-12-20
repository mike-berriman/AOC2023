namespace Day20
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\Day20\TestData2.txt";
        const string fileName2 = @"D:\temp\advent\Day20\InputData.txt";

        static void Main(string[] args)
        {
            Day20 day1 = new Day20();
            day1.Execute(fileName, false, 1);

            day1 = new Day20();
            day1.Execute(fileName2, false, 2);

            day1 = new Day20();
            day1.Execute(fileName2, true, 4);

            Console.ReadKey();
        }
    }
}