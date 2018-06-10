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

            channel.QueueDeclare("backend-api", false, false, false, null);


            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string id = Encoding.UTF8.GetString(body);
                string key = id;
                string text = redis.Get(key);

                Console.WriteLine(text);
            };

            channel.BasicConsume("backend-api", true, consumer);

        }

    }
}
