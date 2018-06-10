using System;
using StackExchange.Redis;
using System.Collections.Generic;

namespace textlistener
{
    public class Redis
    {
        private IDatabase _database;
        public Redis()
        {
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("localhost");
            _database = connection.GetDatabase();
        }

        public string Get(string key)
        {
            return _database.StringGet(key);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _database.StringSet(item.Key, item.Value);

        }
    }
}
