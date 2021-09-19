using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiConsumer.Enums;
using ApiConsumer.Helpers;
using ApiConsumer.Interfaces;
using ApiConsumer.Models;
using ApiConsumer.Services.Cache;
using ApiConsumer.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ApiConsumer.Background
{
    public class RabbitBackground : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitBackground> _logger;
        private readonly RabbitSettings _rabbitSettings;
        private readonly CacheService _cache;
        private IModel _channel;
        private IConnection _connection;
        public RabbitBackground(ILogger<RabbitBackground> logger,
            IServiceProvider serviceProvider, CacheService cache,
            IOptions<RabbitSettings> rabbitSettings)
        {
            _cache = cache;
            _logger = logger;
            _rabbitSettings = rabbitSettings.Value;
            _serviceProvider = serviceProvider;
            GetRabbitMQ();
        }

        private void GetRabbitMQ()
        { 
             var factory = new ConnectionFactory
             {
                 HostName = _rabbitSettings.Host, 
                 DispatchConsumersAsync = true
             };
               _connection = factory.CreateConnection();
               _channel = _connection.CreateModel();

               _channel.QueueDeclare(
                 queue:"Recipient_message", 
                 durable: true,
                 exclusive: false,
                 autoDelete:false,
                 arguments: null);
               _channel.QueueBind(
                queue:"Recipient_message",
                exchange: "message_production",
                routingKey:"msg.*",
                null);
               _channel.BasicQos(0, 0, false);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
           
            stoppingToken.ThrowIfCancellationRequested();

            List<CreateMessage> messages = new();

            var consumer = new AsyncEventingBasicConsumer(_channel);
         consumer.Received += async (ch, ea) =>
         {
             using var scope = _serviceProvider.CreateScope(); 
             var service = scope.ServiceProvider.GetRequiredService<IMessageService>();
             
             var content = ea.Body.ToArray().Deserialize<CreateMessage>();
             AddToCache(content);
             var result = MessageValidation(content);
             if (result)
             {
                 if (messages.Count != 0)
                 {
                     var msg = messages[messages.Count - 1];
                     if (msg.Date.Minute != content.Date.Minute)
                     {
                         await service.CreateNewMessageAsync(messages[messages.Count - 1]);
                         messages.Clear();
                         await service.CreateNewMessageAsync(content);
                     }
                     else
                     {
                         messages.Add(content);
                     }
                 }
                 else
                 {
                     var date = await service.GetLastDateMessageAsync();
                
                     if (!(date.Month == content.Date.Month && date.Day == content.Date.Day &&
                         date.Hour == content.Date.Hour && date.Minute == content.Date.Minute) || date == default)
                     {
                         await service.CreateNewMessageAsync(content);
                     }
                     else
                     {
                         messages.Add(content);
                     }
                 }

             }
             _channel.BasicAck(ea.DeliveryTag, true);
         };

         _channel.BasicConsume("Recipient_message", false, consumer);
          return Task.CompletedTask;
        }

        private bool MessageValidation(CreateMessage message)
        {
            if (message == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(message.Content))
            {
                return false;
            }
            if (message.Type != MessageType.Important.ToString())
            {
                return false;
            }
            return true;
        }

        private void AddToCache(CreateMessage message)
        {
            if (message == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(message.Content))
            {
                return;
            }
            List<CreateMessage> result = new();
            var fromcache = _cache.GetFromCache<List<CreateMessage>>($"_key-{DateTime.Now.ToShortDateString()}");
            if (fromcache != null)
            {
                fromcache.Add(message);
                result.AddRange(fromcache);
            }
            else
            {
                result.Add(message);
            }
            _cache.SaveToCache(result, $"_key-{DateTime.Now.ToShortDateString()}");
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
