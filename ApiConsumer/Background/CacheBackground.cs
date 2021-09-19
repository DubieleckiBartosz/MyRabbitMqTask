using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ApiConsumer.Enums;
using ApiConsumer.Interfaces;
using ApiConsumer.Models;
using ApiConsumer.Services.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApiConsumer.Background
{
    public class CacheBackground : BackgroundService
    {
        private CacheService _cacheService;
        private readonly ILogger<CacheBackground> _logger;
        private readonly IServiceProvider _serviceProvider;
        public CacheBackground(CacheService cacheService,
            ILogger<CacheBackground> logger,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _cacheService = cacheService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await SaveSlotInfo();
                _cacheService.Remove($"_key-{DateTime.Now.ToShortDateString()}");

                var result=DateTime.Now.Second % 60 == 0 ? 60 * 1000 : (60 - DateTime.Now.Second) * 1000;
                Console.WriteLine($"Data: {DateTime.Now} - Czas ponownego uruchomienia za [{result}] milisekund");
                await Task.Delay(result, stoppingToken);
            }
        }

        private async Task SaveSlotInfo()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ISlotService>();
            var messages = _cacheService.GetFromCache<List<CreateMessage>>($"_key-{DateTime.Now.ToShortDateString()}");
            if (messages != null)
            {
                var minutes = messages.Select(w => w.Date.Minute).Distinct().ToList();
             
                foreach (var min in minutes)
                {
                    var list = messages.Where(w => w.Date.Minute == min).ToList();
                    var listImportant=list.Where(w => w.Type == MessageType.Important.ToString()).ToList();

                    var slot = new CreateSlotInfo()
                    {
                        NumberMessages = list.Count,
                        MaxTimeBetweenContentChanges =
                        MaxTime(listImportant),
                        ChangedCharacters = ChangesInContent(list),
                        FirstAndLastCharacters = GetFirstAndLastCharacter(list)
                    };

                    var result = await service.CreateSlotAsync(slot);
                    if (result == false)
                    {
                        _logger.LogError($"Slot could not be saved");
                    }
                }
            }
        }

        private string MaxTime(List<CreateMessage> messages)
        {
            if (messages.Count > 1)
            {
                var maxSeconds = messages[1].Date.Second - messages[0].Date.Second;
                for (int i = 0; i < messages.Count; i++)
                {
                    if (i == messages.Count - 1)
                    {
                        break;
                    }
                    if ((messages[i + 1].Date.Second - messages[i].Date.Second) > maxSeconds)
                    {
                        maxSeconds = messages[i + 1].Date.Second - messages[i].Date.Second;
                    }
                }
                return $"Max seconds : [{maxSeconds}]  {messages[messages.Count-1].Date}";
            }
            return $"Only one element : [{messages[0].Date}]";
        }

        private string ChangesInContent(List<CreateMessage> messages)
        {
            var content = String.Empty;
            List<string> changes = new();
            for (int i = 0; i < messages.Count; i++)
            {
                for (int j = i + 1; j < messages.Count; j++)
                {
                    foreach (var item in messages[j].Content)
                    {
                        if (!messages[i].Content.Contains(item))
                        {
                            content += item;
                        }
                    }
                    changes.Add(content);
                    content = String.Empty;
                }
            }
            return string.Join(", ", changes.Distinct());
        }

        private string GetFirstAndLastCharacter(List<CreateMessage> messages)
        {
            var result=string.Join(", ", messages.Select(s => $"[{s.ElementId[0]}-{s.ElementId[s.ElementId.Length - 1]}]"));
            return result;
        }
    }
}
