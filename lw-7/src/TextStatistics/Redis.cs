using System;
using StackExchange.Redis;
using System.Collections.Generic;

namespace TextStatistics
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
            Console.WriteLine("Database: " + 0 + ", contextId: " + key);
            IDatabase database = _connection.GetDatabase();
            return database.StringGet(key);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            Console.WriteLine("Database: " + 0 + ", contextId: " + item.Key);

            IDatabase database = _connection.GetDatabase();
            database.StringSet(item.Key, item.Value);

        }
    }
}
