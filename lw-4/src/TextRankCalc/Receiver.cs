using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;


namespace TextRankCalc
{
    public class Receiver
    {
        public Receiver()
        {
            Redis redis = new Redis();


            ConnectionFactory factory = new ConnectionFactory();
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            //channel.QueueDeclare("backend-api", false, false, false, null);

channel.ExchangeDeclare("backend-api", ExchangeType.Fanout);
string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queueName, "backend-api", "");

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string id = Encoding.UTF8.GetString(body);
                string text = redis.Get(id);
                float rank = TextRankCalculator.Get(text);
               
                redis.Add(new KeyValuePair<string, string>("rank:" + id, rank.ToString("0.00")));
            };

            channel.BasicConsume(queueName, true, consumer);

        }

    }
}
