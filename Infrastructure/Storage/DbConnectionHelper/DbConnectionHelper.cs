namespace Infrastructure
{
    public static class DbConnectionHelper
    {
        internal const string SparePartConfig = "SparePartConfig";

        /// <summary>
        /// Коннект к базе данных.
        /// </summary>
        /// <param name="name">Имя подключения</param>
        /// <returns></returns>
        public static System.Data.Common.DbConnection GetDatabaseConnection(string name = SparePartConfig)
        {
            var settings = System.Configuration.ConfigurationManager.ConnectionStrings[name];
            var factory = System.Data.Common.DbProviderFactories.GetFactory(settings.ProviderName);

            var conn = factory.CreateConnection();
            conn.ConnectionString = settings.ConnectionString;

            return conn;
        }
    }
}
