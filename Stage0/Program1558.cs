namespace Stage0;

partial class Program
{
    static void Main(string[] args)
    {
        Welcome1558();
        Welcome3645();
        Console.ReadKey();
    }
    static partial void Welcome3645();

    private static void Welcome1558()
    {
        Console.WriteLine("Enter your name: ");
        string name = Console.ReadLine();
        Console.WriteLine(name + ", welcome to my first console application.");
    }
}
