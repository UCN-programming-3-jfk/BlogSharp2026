using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSharp2026.DataAccess.SqlClient;

abstract public class BaseDao
{
    public string ConnectionString { get; set; }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}
