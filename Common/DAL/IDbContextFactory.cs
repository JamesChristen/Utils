using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Common.DAL
{
    internal interface IDbContextFactory
    {
        DbContext CreateNew();
        string ConnectionString { get; }

        int ExecSql(string sql);
        int ExecSproc(string sp, params SqlParameter[] sqlParams);
        int ExecSproc(SqlCommand cmd);
        IEnumerable<T> ExecSproc<T>(string sp, Func<IDataReader, T> reader, params SqlParameter[] sqlParams);
        IEnumerable<T> ExecFunction<T>(string cmd, Func<IDataReader, T> reader);
        T ExecScalar<T>(string cmd, Func<object, T> reader);
        void ExecuteTransaction(Action command);
    }
}
