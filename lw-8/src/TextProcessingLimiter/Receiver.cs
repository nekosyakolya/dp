using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;


namespace TextProcessingLimiter
{
    public class Receiver
    {
        private int _availableCount;
        private Redis _redis = new Redis();

        public Receiver()
        {
            _availableCount = 3;

            ConnectionFactory factory = new ConnectionFactory();
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            channel.ExchangeDeclare("backend-api", ExchangeType.Fanout);
            channel.ExchangeDeclare("processing-limiter", ExchangeType.Fanout);
            channel.ExchangeDeclare("text-success-marker", ExchangeType.Fanout);



            string queueName = channel.QueueDeclare(
				queue: "",
				exclusive: false,
				autoDelete: false,
				arguments: null).QueueName;
            channel.QueueBind(queueName, "backend-api", "");

            string successMarkerQueue = channel.QueueDeclare(
				queue: "",
				exclusive: false,
				autoDelete: false,
				arguments: null).QueueName;
            channel.QueueBind(successMarkerQueue, "text-success-marker", "");

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string message = Encoding.UTF8.GetString(body);

                string[] items = message.Split(':');

                if (items.Length == 1)
				{
                    string successCode = ":true";
                    if (_availableCount != 0)
					{
						--_availableCount;
						Console.WriteLine("Available text count: " + _availableCount);
					}
					else
					{
						Console.WriteLine("Limit reached");
                        successCode = ":false";
						_redis.Add(new KeyValuePair<string, string>("status:" + items[0], "canNotBeProcessed"));
					}

                    channel.BasicPublish(
                        exchange: "processing-limiter",
                        routingKey: "",
                        basicProperties: null,
                        body: Encoding.UTF8.GetBytes("ProcessingAccepted:" + items[0] + successCode));
                }

                if (items.Length == 3 && items[0] == "TextSuccessMarked")
				{
					if (items[2] == "true")
					{
						Console.WriteLine("Text marked as successful: " + items[1]);
					}
					else if (items[2] == "false")
					{
						Console.WriteLine("Text is unsuccessfull, rollback: " + items[1]);
						++_availableCount;
					}
				}


            };

            channel.BasicConsume(queueName, true, consumer);
            channel.BasicConsume(successMarkerQueue, true, consumer);


        }

    }
}
