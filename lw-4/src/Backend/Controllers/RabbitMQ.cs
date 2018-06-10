using System;
using System.Text;
using RabbitMQ.Client;

namespace Backend.Controllers
{
    public class RabbitMQ
    {
        private IModel _channel;
        public RabbitMQ()
        {
            ConnectionFactory factory = new ConnectionFactory();
            IConnection conn = factory.CreateConnection();

            _channel = conn.CreateModel();
            //_channel.QueueDeclare("backend-api", false, false, false, null);
            _channel.ExchangeDeclare("backend-api", ExchangeType.Fanout);
        }

        public void Add(string message)
        {
            _channel.BasicPublish("backend-api", "", null, Encoding.UTF8.GetBytes(message));
        }
    }
}
