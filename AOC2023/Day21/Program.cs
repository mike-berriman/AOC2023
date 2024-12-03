namespace Day21
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\AOC2023\Day21\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AOC2023\Day21\InputData.txt";

        static void Main(string[] args)
        {
            Day21 day1 = new Day21();
            //day1.Execute(fileName, false, 1);

            day1 = new Day21();
            day1.Execute(fileName2, false, 2);

            day1 = new Day21();
            //day1.Execute(fileName, true, 3);

            day1 = new Day21();
            day1.Execute(fileName2, true, 4);

            Console.ReadKey();
        }
    }
}