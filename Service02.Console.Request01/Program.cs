using Flurl.Http;

namespace Service02.Console.Request01
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var urlTemplate = "http://localhost:5109/api/event/{userId}/{ipAddress}";

            IFlurlResponse state;
            var tasks = new List<Task>();
            var rnd = new Random();

            foreach (var i in Enumerable.Range(1, 100000))
            {
                try
                {
                    var url = urlTemplate
                        .Replace("{userId}", $"{i}")
                        .Replace("{ipAddress}", $"127.{rnd.Next(0,254)}.0.{rnd.Next(0, 254)}");

                    var task = url
                        .WithTimeout(TimeSpan.FromMinutes(2))
                        .GetAsync();

                    System.Console.Write(".");

                    tasks.Add(task);
                }
                catch (FlurlHttpException ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }

            System.Console.WriteLine("Generated request");

            Task.WaitAll(tasks);

            System.Console.WriteLine("Press any key...");
            System.Console.ReadLine();
        }
    }
}
