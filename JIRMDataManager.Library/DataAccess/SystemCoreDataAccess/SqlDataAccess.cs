using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace JIRMDataManager.Library.DataAccess.SystemCoreDataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private bool disposed = false;
        private IConfiguration _config;
        private ILogger<SqlDataAccess> _logger;
        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger)
        {
            _config = config;
            _logger = logger;
        }
        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
            //return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AIRMData23050409BeforeCore;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
        public async Task<List<T>> LoadDataStoredProcedure<T, U>(string storedProcedure, U parameters, string pDbCode)
        {
            string connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public async Task<List<T>> LoadDataStoredProcedureAsync<T, U>(string storedProcedure, U parameters, string pDbCode)
        {
            string connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Use 'await' to asynchronously wait for the query result
                List<T> rows = (await connection.QueryAsync<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure)).ToList();

                return rows;
            }
        }
        public void SaveDataStoredProcedure<T>(string storedProcedure, T parameters, string pDbCode)
        {
            string connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<List<T>> LoadDataCodeProcedureAsync<T>(string sqlQuery, object parameters, string pDbCode)
        {
            string connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = (await connection.QueryAsync<T>(sqlQuery, parameters)).ToList();

                return rows;
            }
        }

        public List<T> LoadDataCodeProcedure<T>(string sqlQuery, object parameters, string pDbCode)
        {
            string connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlQuery, parameters).ToList();

                return rows;
            }
        }
        public async Task SaveDataCodeProcedureAsync<T>(string sqlQuery, T parameters, string pDbCode)
        {
            string connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sqlQuery, parameters);
            }
        }

        public void SaveDataCodeProcedure<T>(string sqlQuery, T parameters, string pDbCode)
        {
            string connectionString = GetConnectionString(pDbCode);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlQuery, parameters);
            }
        }

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTransaction(string pDbCode)
        {
            string connectionString = GetConnectionString(pDbCode);

            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();

            isClosed = false;
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            List<T> rows = _connection.Query<T>(storedProcedure, parameters,
                       commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

            return rows;

        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction);

        }

        private bool isClosed = false;

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();

            isClosed = true;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();

            isClosed = true;
        }

        public void Dispose()
        {
            if (!isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch (Exception ex)
                {
                    // TODO: Log this issue
                    _logger.LogError(ex, "Commit transaction failed in the dispose method.");
                }
            }

            _transaction = null;
            _connection = null;
        }
    }
}
