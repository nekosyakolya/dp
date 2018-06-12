using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace textlistener
{
    public class TextListener
    {
        public TextListener()
        {
            Redis redis = new Redis();


            ConnectionFactory factory = new ConnectionFactory();
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            channel.ExchangeDeclare("backend-api", ExchangeType.Fanout);
            string queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "backend-api", "");

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string id = Encoding.UTF8.GetString(body);
                string text = redis.Get(id);

                Console.WriteLine(text);
            };

            channel.BasicConsume(queueName, true, consumer);

        }

    }
}
