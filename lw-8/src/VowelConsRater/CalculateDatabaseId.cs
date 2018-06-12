using System;
using System.Text;
using RabbitMQ.Client;

namespace VowelConsRater
{
    public class CalculateDatabaseId
    {
        public static int Get(string value)
		{
			int hash = 0;
			foreach (char ch in value)
			{
				if (Char.IsDigit(ch))
				{
					hash += ch;
				}
				else 
				{
					hash += 1;
				}
			}
			return hash % 16;
		}
       
    }
}
