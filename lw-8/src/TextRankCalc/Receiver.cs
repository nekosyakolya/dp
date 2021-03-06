using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace TextRankCalc
{
    public class Receiver
    {
        public Receiver()
        {
            ConnectionFactory factory = new ConnectionFactory();
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            channel.ExchangeDeclare("processing-limiter", ExchangeType.Fanout);
			channel.ExchangeDeclare("text-rank-tasks", ExchangeType.Direct);



            string queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "processing-limiter", "");

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string message = Encoding.UTF8.GetString(body);

                string[] items = message.Split(':');


                if (items.Length == 3 && items[0] == "ProcessingAccepted" && items[2] == "true")
				{
                    channel.BasicPublish(
								exchange: "text-rank-tasks",
								routingKey: "text-rank-task",
								basicProperties: null,
								body: Encoding.UTF8.GetBytes("TextRankTask:" + items[1]));
                }
            };

            channel.BasicConsume(queueName, true, consumer);

        }

    }
}
