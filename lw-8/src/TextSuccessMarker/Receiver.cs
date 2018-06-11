using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace TextSuccessMarker
{
    public class Receiver
    {
        public Receiver()
        {
            float min = 0.5f;

            ConnectionFactory factory = new ConnectionFactory();
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            channel.ExchangeDeclare("text-rank-calc", ExchangeType.Fanout);
			channel.ExchangeDeclare("text-success-marker", ExchangeType.Fanout);

            string queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "text-rank-calc", "");

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string message = Encoding.UTF8.GetString(body);

                string[] items = message.Split(':');


                if (items.Length == 3 && items[0] == "TextRankCalculated")
				{
                    float rank = float.Parse(items[2]);
                    string successCode = ":true";
                    if (rank >= min)
					{
						Console.WriteLine("Success mark: " + items[1]);
						
					}
					else
					{
						Console.WriteLine("Unsuccess mark: " + items[1]);
						successCode = ":false";
					}

                    channel.BasicPublish(
								exchange: "text-success-marker",
								routingKey: "",
								basicProperties: null,
								body: Encoding.UTF8.GetBytes("TextSuccessMarked:" + items[1] + successCode));
                }
            };

            channel.BasicConsume(queueName, true, consumer);

        }

    }
}
