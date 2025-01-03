using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;

namespace LISCareDataAccess.InventoryDbContext
{
    public class InventoryDbContext : DbContext
    {
        private readonly string _connectionString;

        public InventoryDbContext()
        {

        }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {

        }

        [Obsolete]
        public object FromSql(string v, SqlParameter[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
