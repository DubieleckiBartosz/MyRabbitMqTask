using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiConsumer.Models;
using ApiConsumer.Models.Requests;
using ApiConsumer.Services;

namespace ApiConsumer.Interfaces
{
    public interface ISlotService
    {
        Task<bool> CreateSlotAsync(CreateSlotInfo slot);
        Task<IEnumerable<ResponseSlotInfo>> GetSlotsInfoAsync(SearchSlots query);
    }
}
