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
    public class MessageService:IMessageService
    {
        private readonly AppDbContext _db;
        public MessageService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateNewMessageAsync(CreateMessage message)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@content",message.Content);
            parameters.Add("@type",message.Type);
            parameters.Add("@date",message.Date); 
            parameters.Add("@elementId",message.ElementId);
            
                using (var connection=_db.GetConnection())
                {
                var result=await connection.ExecuteAsync(
                    "spCreate_new_message",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return result > 0;
                }
        }

        public async Task<DateTime> GetLastDateMessageAsync()
        {
            var sql = "SELECT TOP(1) [Date] FROM [Messages] ORDER BY Id DESC";
            using (var connection = _db.GetConnection())
            {
                var result = await connection.QueryAsync<DateTime>(sql);
                return result.Any()?result.Single():default;
            }
        }

        public async Task<IReadOnlyList<Message>> GetMessagesByFiltersAsync(SearchMessages searchMessages)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@fromDate", searchMessages.FromDate);
            parameters.Add("@toDate",searchMessages.ToDate);
            parameters.Add("@pageNumber",searchMessages.PageNumber);
            parameters.Add("@pageSize",searchMessages.PageSize);
            parameters.Add("@content",searchMessages.Content);
            if (searchMessages.ContentLength != default)
            {
                parameters.Add("@contentLength", searchMessages.ContentLength);
            }

            using var connection = _db.GetConnection();
           
            var result = await connection.QueryAsync<Message>("spGet_messages_by_filters", parameters,commandType:CommandType.StoredProcedure);
         
            return result.ToList();
        }
    }
}
