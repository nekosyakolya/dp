using System;
using StackExchange.Redis;
using System.Collections.Generic;

namespace Backend.Controllers
{
    public class Redis
    {
        ConnectionMultiplexer _connection;

        public Redis()
        {
            _connection = ConnectionMultiplexer.Connect("localhost");
        }

        public string Get(string key)
        {
            int hash = CalculateDatabaseId.Get(key);
            Console.WriteLine("Database: " + hash + ", contextId: " + key);
            IDatabase database = _connection.GetDatabase(hash);
            return database.StringGet(key);
        }

        public string GetStatistics()
        {
            string key = "statistics";
            Console.WriteLine("Database: " + 0 + ", contextId: " + key);
            IDatabase database = _connection.GetDatabase();
            return database.StringGet(key);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            int hash = CalculateDatabaseId.Get(item.Key);

            Console.WriteLine("Database: " + hash + ", contextId: " + item.Key);

            IDatabase database = _connection.GetDatabase(hash);
            database.StringSet(item.Key, item.Value);

        }
    }
}
