namespace Infrastructure
{
    public static class DbConnectionHelper
    {
        private static readonly string _providerName = "System.Data.SQLite.EF6";
        internal static readonly string ConnectionString = $"Data Source={FilesStorageHelper.DataDirectoryPath}\\Data\\TruckTirDB.db;Version=3;foreign keys=true;Pooling=True;Max Pool Size=100;";

        /// <summary>
        /// Коннект к базе данных.
        /// </summary>
        /// <param name="connectionString">Имя подключения</param>
        /// <returns></returns>
        public static System.Data.Common.DbConnection GetDatabaseConnection(string connectionString)
        {            
            var factory = System.Data.Common.DbProviderFactories.GetFactory(_providerName);
            var conn = factory.CreateConnection();
            conn.ConnectionString = connectionString;

            return conn;
        }
    }
}
