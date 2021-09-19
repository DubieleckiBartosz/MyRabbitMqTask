using System;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace PublisherConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            ExchangePublisher.Publish(channel);
        }
    }
}
