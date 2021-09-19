using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ApiConsumer.DapperContext
{
    public class AppDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connection;
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection GetConnection() =>
            new SqlConnection(_connection);
    }
}
