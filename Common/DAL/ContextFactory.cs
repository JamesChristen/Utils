using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Common.DAL
{
    internal abstract class ContextFactory : IDbContextFactory
    {
        public abstract DbContext CreateNew();
        public abstract string ConnectionString { get; }

        public int ExecSql(
            string sql)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 600;
            return cmd.ExecuteNonQuery();
        }

        public int ExecSproc(
            string sp,
            params SqlParameter[] sqlParams)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = sp;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            if (sqlParams != null && sqlParams.Any())
            {
                foreach (SqlParameter param in sqlParams)
                {
                    cmd.Parameters.Add(param);
                }
            }

            return cmd.ExecuteNonQuery();
        }

        public int ExecSproc(
            SqlCommand cmd)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            cmd.Connection = conn;
            return cmd.ExecuteNonQuery();
        }

        public IEnumerable<T> ExecSproc<T>(
            string sp,
            Func<IDataReader, T> reader,
            params SqlParameter[] sqlParams)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = sp;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            if (sqlParams != null && sqlParams.Any())
            {
                foreach (SqlParameter param in sqlParams)
                {
                    cmd.Parameters.Add(param);
                }
            }

            List<T> result = new List<T>();
            IDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                T t = reader(dataReader);
                result.Add(t);
            }

            return result;
        }

        public IEnumerable<T> ExecFunction<T>(string cmdStr, Func<IDataReader, T> parser)
        {
            using SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = cmdStr;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 600;

            IDataReader reader = null;
            try
            {
                List<T> results = new List<T>();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    T item = parser(reader);
                    results.Add(item);
                }
                return results;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }

                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }

        public T ExecScalar<T>(string cmdStr, Func<object, T> parser)
        {
            using SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = cmdStr;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 600;

            try
            {
                object value = cmd.ExecuteScalar();
                return parser(value);
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd?.Dispose();
            }
        }

        public void ExecuteTransaction(Action command)
        {
            using var transaction = CreateNew().Database.BeginTransaction();

            try
            {
                command();

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }
}
