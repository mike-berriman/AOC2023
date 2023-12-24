namespace Day23
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\AOC2023\Day23\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AOC2023\Day23\InputData.txt";

        static void Main(string[] args)
        {
            Day23 day1 = new Day23();
            //day1.Execute(fileName, false, 1);

            day1 = new Day23();
            //day1.Execute(fileName2, false, 2);

            day1 = new Day23();
            //day1.Execute(fileName, true, 3);

            day1 = new Day23();
            day1.FileName = fileName2;
            day1.Part2 = true;
            day1.Counter = 4;

            var stackSize = 10000000;
            Thread thread = new Thread(new ThreadStart(day1.Execute), stackSize);
            thread.Start();

            //day1.Execute();

            Console.ReadKey();
        }
    }
}