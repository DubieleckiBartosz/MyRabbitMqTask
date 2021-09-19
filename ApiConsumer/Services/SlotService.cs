using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ApiConsumer.DapperContext;
using ApiConsumer.Interfaces;
using ApiConsumer.Models;
using ApiConsumer.Models.Requests;
using Dapper;

namespace ApiConsumer.Services
{
    public class SlotService: ISlotService
    {
        private readonly AppDbContext _db;
        public SlotService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateSlotAsync(CreateSlotInfo slot)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@numberMessages",slot.NumberMessages);
            parameters.Add("@firstAndLastCharacters",slot.FirstAndLastCharacters);
            parameters.Add("@ChangedCharacters",slot.ChangedCharacters);
            parameters.Add("@maxTimeBetweenContentChanges",slot.MaxTimeBetweenContentChanges);
        
            using var connection = _db.GetConnection();
            var result = await connection.ExecuteAsync("spCreate_new_Slot_Info", parameters,
                commandType: CommandType.StoredProcedure);
            return result > 0;
        }

        public async Task<IEnumerable<ResponseSlotInfo>> GetSlotsInfoAsync(SearchSlots query)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@pageNumber",query.PageNumber);
            parameters.Add("@pageSize",query.PageSize);
            parameters.Add("@fromId",query.FromId);

            using var connection = _db.GetConnection();
            var result = await connection.QueryAsync<SlotInfo>("spGetSlots_by_filters",
                parameters,commandType:CommandType.StoredProcedure);

            return ArrangeResult(result.ToList());
        }
        
        private IEnumerable<ResponseSlotInfo> ArrangeResult(IEnumerable<SlotInfo> slots)
        {
            List<ResponseSlotInfo> response = new();
            foreach (var item in slots)
            {
                ResponseSlotInfo info = new()
                {
                    Id=item.Id,
                    NumberMessages = item.NumberMessages,
                    Characters = item.FirstAndLastCharacters.Split(", "),
                    ChangedCharacters = item.ChangedCharacters.Split(", "),
                    MaxTimeBetweenContentChanges = item.MaxTimeBetweenContentChanges
                };
                response.Add(info);
            }

            return response;
        }
    }

    public class ResponseSlotInfo
    {
        public int Id { get; set; }
        public int NumberMessages { get; set; }
        public string[] Characters { get; set; }
        public string[] ChangedCharacters { get; set; }
        public string MaxTimeBetweenContentChanges { get; set; }
    }
}
