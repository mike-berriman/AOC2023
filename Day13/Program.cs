namespace Day13
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\AOC2023\Day13\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AOC2023\Day13\InputData.txt";

        static void Main(string[] args)
        {
            Day13 day1 = new Day13();
            //day1.Execute1(fileName);
            //day1.Execute1(fileName2);
            day1.Execute1(@"D:\temp\advent\AOC2023\Day13\DebugData.txt");

            //day1.Execute2(fileName);
            //day1.Execute2(fileName2);

            day1.Execute2(@"D:\temp\advent\AOC2023\Day13\DebugData.txt");

            Console.ReadKey();
        }
    }
}