using Common.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Common.DAL
{
    internal static class ConnectionInjector
    {
        public static void InjectConnectionString(DbContextOptionsBuilder optionsBuilder, string connString)
        {
#pragma warning disable EF1001 // Internal EF Core API usage.
            SqlServerOptionsExtension sqlServerOptionsExtension = optionsBuilder.Options.FindExtension<SqlServerOptionsExtension>();
#pragma warning restore EF1001 // Internal EF Core API usage.
            if (sqlServerOptionsExtension == null || string.IsNullOrWhiteSpace(sqlServerOptionsExtension.ConnectionString))
            {
                optionsBuilder.UseSqlServer(connString, builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }

        public static string GetConnectionStringSetting(string name)
        {
            try
            {
                return ConfigurationProvider.GetConfigValue(name);
            }
            catch (FileNotFoundException fnfEx)
            {
                throw new Exception($"Could not find config file", fnfEx);
            }
            catch (ArgumentException argEx)
            {
                throw new Exception($"Connection string not found in config file", argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error loading connection string", ex);
            }
        }
    }
}
