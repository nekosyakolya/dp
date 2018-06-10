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

            channel.ExchangeDeclare("backend-api", ExchangeType.Fanout);
			channel.ExchangeDeclare("text-rank-tasks", ExchangeType.Direct);



            string queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "backend-api", "");

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string message = Encoding.UTF8.GetString(body);

                var splitted = message.Split(':');


                if (splitted.Length == 1)
				{
                    channel.BasicPublish(
								exchange: "text-rank-tasks",
								routingKey: "text-rank-task",
								basicProperties: null,
								body: Encoding.UTF8.GetBytes("TextRankTask:" + message));
                }
            };

            channel.BasicConsume(queueName, true, consumer);

        }

    }
}
