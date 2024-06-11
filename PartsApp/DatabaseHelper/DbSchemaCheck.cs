using Infrastructure;
using System;
using System.Data.SQLite;

namespace PartsApp.DatabaseHelper
{
    internal static class DbSchemaCheck
    {
        internal static void EnsureNewColumnsExistsInDBTables()
        {
            using (var connection = DbConnectionHelper.GetDatabaseConnection() as SQLiteConnection)
            {
                connection.Open();
                EnsureStorageCellColumnExistsInSparePartsTable(connection);            
                connection.Close();
            }
        }

        private static void EnsureStorageCellColumnExistsInSparePartsTable(SQLiteConnection connection)
        {
            using (var cmd = new SQLiteCommand("", connection))
            {
                cmd.CommandText = $"SELECT COUNT(*) AS ColumnExists FROM sqlite_master WHERE type = 'table' AND name = 'SpareParts' AND sql LIKE '%StorageCell%';";

                bool columnExists = Convert.ToInt32(cmd.ExecuteScalar()) != 0;
                if (!columnExists)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = $"ALTER TABLE SpareParts ADD COLUMN StorageCell VARCHAR(100)";
                    cmd.ExecuteNonQuery();
                }
            }            
        }
    }
}
