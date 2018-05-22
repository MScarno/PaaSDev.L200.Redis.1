using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace PaaSDevL200Redis1
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RedisConnectionString"].ConnectionString;
            int requestCount = 150;
            int valueSize = 1000;
            var rand = new Random();

            byte[] file = new byte[valueSize];
            rand.NextBytes(file);
            
            try
            {
                Parallel.For(0, requestCount,
                (index) => { ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(connectionString);
                             IDatabase db = connection.GetDatabase();
                             db.StringSet(index.ToString(), value: file);
                             Console.WriteLine("Key {0} : Added", index.ToString());
                });

                Console.WriteLine(requestCount + " keys added to redis");
               
                Console.WriteLine("- All sets succeeded -\nPress a button to exit");
                Console.ReadLine();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("- {0} Exceptions occurred -\n",ae.InnerExceptions.Count.ToString());
                // Handle the  exception.
                foreach (var e in ae.InnerExceptions)
                    Console.WriteLine(e.Message+"\n");
                Console.WriteLine("\nPress a button to exit");
                Console.ReadLine();
            }
        }
    }
}
