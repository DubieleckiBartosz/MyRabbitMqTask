using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PublisherConsoleApp
{
    public static class ExchangePublisher
    {
        private const string routingKey = "msg.prd";
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange:"message_production",
                type:ExchangeType.Topic,
                durable:true
                );
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            var content = String.Empty;
            var typeMsg = String.Empty;
            var _cnt = new Random().Next(1, 5);
            var _cntLoop=0;
            while (true)
            {
                if (string.IsNullOrEmpty(content) || (_cntLoop==_cnt))
                {
                    _cnt = new Random().Next(3, 4);
                    _cntLoop = 0;
                    typeMsg = GetTypeMessage();
                    content = GetNumber();
                }
                var msg = new Message(content,typeMsg);

                var bytes = msg.Serialize<Message>();
                channel.BasicPublish(exchange: "message_production",
                    routingKey: routingKey,
                    basicProperties: properties,
                    body: bytes);

                Console.WriteLine("Sent '{0}':'{1}'\nDate: {2}, Status : {3}\nElementID: {4}\n", 
                    routingKey, msg.Content, msg.Date, msg.Type,msg.ElementId);
                _cntLoop++;
                Thread.Sleep(1000);
            }
        }
        
        private static string GetNumber()
        {
            var myString = "";
            var _cnt = 0;
            var _rnd = new Random().Next(5, 8);
            while (_cnt<_rnd)
            {
                myString += new Random().Next(0, 10);
                _cnt++;
            }
            return myString;
        }

        private static string GetTypeMessage()
        {
         string[] MsgTypes = { "NotImportant", "VeryImportant", "Important" };
         var _rnd = new Random().Next(0,3);
         return MsgTypes[_rnd];
        }
    }
}
