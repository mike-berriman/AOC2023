namespace Boilerplate
{
    internal class Program
    {
        const string fileName = @"D:\temp\advent\AOC2023\Boilerplate\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AOC2023\Boilerplate\InputData.txt";

        static void Main(string[] args)
        {
            Boilerplate day1 = new Boilerplate();
            day1.Execute(fileName, false, 1);

            day1 = new Boilerplate();
            day1.Execute(fileName2, false, 2);

            day1 = new Boilerplate();
            day1.Execute(fileName, true, 3);

            day1 = new Boilerplate();
            day1.Execute(fileName2, true, 4);

            Console.ReadKey();
        }
    }
}