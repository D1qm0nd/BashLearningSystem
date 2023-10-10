using RemoteCommandHandler;

namespace ConsoleTestApp
{
    public class Program
    {
        public static void Main()
        {
            var commandHandler = new CommandHandler();
            while (true)
            {
                Thread.Sleep(10000);
                Console.WriteLine("Still working");
            }
        }
    }
}