using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;


namespace VowelConsRater
{
    public class Receiver
    {
        public Receiver()
        {
            ConnectionFactory factory = new ConnectionFactory();
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            channel.ExchangeDeclare("vowel-cons-counter", ExchangeType.Direct);
            channel.ExchangeDeclare("text-rank-calc", ExchangeType.Fanout);


            channel.QueueDeclare(
                queue: "rank-task",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(
                queue: "rank-task",
                exchange: "vowel-cons-counter",
                routingKey: "vowel-cons-task");
    
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            
            Redis redis = new Redis();
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string message = Encoding.UTF8.GetString(body);
                string[] items = message.Split(':');
                if (items.Length == 4 && items[0] == "VowelConsCounted")
				{
				    int vowels = Int32.Parse(items[2]);
					int consonants = Int32.Parse(items[3]);

					float rank = (consonants == 0) ? (vowels) : ((float)vowels / consonants);
                    redis.Add(new KeyValuePair<string, string>("rank:" + items[1], rank.ToString("0.00")));

                    channel.BasicPublish(
                            exchange: "text-rank-calc",
                            routingKey: "",
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes("TextRankCalculated:" + items[1] + ":" + rank));
                            }
            };

            channel.BasicConsume("rank-task", true, consumer);

        }

    }
}
