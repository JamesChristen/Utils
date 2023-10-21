using Common.Logging;
using Microsoft.Data.SqlClient;

namespace Common.DAL
{
    public interface IDatabaseValidator
    {
        /// <summary>
        /// Checks that the database is both accessible and follows the same schema as the entity framework code
        /// </summary>
        bool IsDatabaseValid(string connString, out string error);
    }

    public class DatabaseValidator : IDatabaseValidator
    {
        private static IDatabaseValidator _instance;
        public static IDatabaseValidator Instance => _instance ??= new DatabaseValidator();

        public bool IsDatabaseValid(string connString, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(connString))
            {
                error = $"{nameof(connString)} cannot be null or white space";
                return false;
            }

            using SqlConnection conn = new SqlConnection(connString);
            try
            {
                try
                {
                    conn.Open();
                }
                catch (SqlException ex) when (ex.Number == 53)
                {
                    error = $"Server invalid/unreachable: {conn.DataSource}";
                    return false;
                }
                catch (SqlException ex) when (ex.Number == 4060)
                {
                    error = $"Database invalid/not permissioned: {conn.Database}";
                    return false;
                }
                catch (SqlException ex)
                {
                    error = $"Unexpected {nameof(SqlException)} ({ex.Number}): {ex.Message}";
                    return false;
                }

                try
                {
                    ILog logger = new DummyLogger();
                    
                    // Create EntitySource instance, and run query. For example:
                    //EntitySource<A, B> source = new EntitySource<A, B>(logger, DataContextFactory.Instance);
                    //HashSet<long> ids = Enumerable.Range(1, 10).Select(x => (long)x).ToHashSet();
                    //source.LoadEntities(x => ids.Contains(x.Id)).ToList();
                }
                catch (SqlException)
                {
                    error = $"Error running test query, likely schema incompatibility";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = $"Unexpected error validating connection string: {ex.Message}";
                return false;
            }
        }
    }
}
