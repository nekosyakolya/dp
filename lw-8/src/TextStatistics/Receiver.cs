using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace TextStatistics
{
    public class Receiver
    {
        public Receiver()
        {
            Statistics statistics = new Statistics();


            ConnectionFactory factory = new ConnectionFactory();
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            channel.ExchangeDeclare("text-success-marker", ExchangeType.Fanout);
            channel.QueueDeclare("text-success-marker", false, false, false, null);
            
            channel.QueueBind("text-success-marker", "text-success-marker", "");

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string message = Encoding.UTF8.GetString(body);

                string[] items = message.Split(":");
				if (items.Length == 3 && items[0] == "TextRankCalculated")
				{
					statistics.Update(float.Parse(items[2]));
				}
            };

            channel.BasicConsume("text-success-marker", true, consumer);

        }

    }
}
