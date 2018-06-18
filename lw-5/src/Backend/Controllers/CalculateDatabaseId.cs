using System;
using System.Text;
using RabbitMQ.Client;

namespace Backend.Controllers
{
    public class CalculateDatabaseId
    {
        public static int Get(string value)
		{
			int hash = 0;
			foreach (char ch in value)
			{
				hash += ch;
			}
			return hash % 16;
		}
       
    }
}
