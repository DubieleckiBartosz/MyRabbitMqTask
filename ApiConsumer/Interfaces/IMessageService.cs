using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiConsumer.Models;
using ApiConsumer.Models.Requests;

namespace ApiConsumer.Interfaces
{
    public interface IMessageService
    {
        Task<IReadOnlyList<Message>> GetMessagesByFiltersAsync(SearchMessages searchMessages);
        Task<bool> CreateNewMessageAsync(CreateMessage message);
        Task<DateTime> GetLastDateMessageAsync();
    }
}
