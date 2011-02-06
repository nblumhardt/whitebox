using System.Threading;
using Whitebox.Core.Connector;

namespace Whitebox.Console
{
    class Program
    {
        static void Main()
        {
            var queue = new NamedPipesReadQueue();

            System.Console.WriteLine("Waiting for the profiled application to connect...");
            queue.WaitForConnection();

            System.Console.WriteLine("Connected.");

            var counter = 0;
            do
            {
                Thread.Sleep(1000);
                object message;
                while (queue.TryDequeue(out message))
                    System.Console.WriteLine("{0}: {1}", ++counter, Formatter.Describe(message));
            } while (queue.IsConnected);

            System.Console.WriteLine("Done. Press any key...");
            System.Console.ReadKey(true);
        }
    }
}
