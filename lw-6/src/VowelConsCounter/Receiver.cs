using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace VowelConsCounter
{
    public class Receiver
    {
        public Receiver()
        {
            ConnectionFactory factory = new ConnectionFactory();
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            channel.ExchangeDeclare("text-rank-tasks", ExchangeType.Direct);
            channel.QueueDeclare("count-task", false, false, false, null);
			channel.QueueBind("count-task", "text-rank-tasks", "text-rank-task");

		    channel.ExchangeDeclare("vowel-cons-counter", ExchangeType.Direct);


            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            Redis redis = new Redis();
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string message = Encoding.UTF8.GetString(body);
                var splitted = message.Split(':');

                if (splitted.Length == 2 && splitted[0] == "TextRankTask")
				{
                    string value = redis.Get(splitted[1]);
                    Counters<int, int> counters = VowelConsCounter.Get(value);

					channel.BasicPublish(
						exchange: "vowel-cons-counter",
						routingKey: "vowel-cons-task",
						basicProperties: null,
						body: Encoding.UTF8.GetBytes("VowelConsCounted:" + splitted[1] + ":" + counters.vowelsCount + ":" + counters.consonantsCount));
				}
            };

            channel.BasicConsume("count-task", true, consumer);

        }

    }
}
