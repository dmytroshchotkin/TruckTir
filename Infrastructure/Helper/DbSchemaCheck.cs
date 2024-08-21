using Infrastructure;
using System;
using System.Data.SQLite;

namespace Infrastructure.Helper
{
    public static class DbSchemaCheck
    {
        public static void EnsureNewColumnsExistsInDBTables()
        {
            using (var connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();
                EnsureStorageCellColumnExistsInSparePartsTable(connection);
                EnsureEnabledColumnExistsInCustomersAndSuppliersTables(connection);
                EnsurePaidCashColumnExistsInSalesTable(connection);
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

        private static void EnsureEnabledColumnExistsInCustomersAndSuppliersTables(SQLiteConnection connection)
        {
            using (var cmd = new SQLiteCommand("", connection))
            {
                cmd.CommandText = $"SELECT COUNT(*) AS ColumnExists FROM sqlite_master WHERE type = 'table' AND name = 'Customers' AND sql LIKE '%Enabled%';";
                bool columnExistsInCustomersTable = Convert.ToInt32(cmd.ExecuteScalar()) != 0;
                if (!columnExistsInCustomersTable)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = $"ALTER TABLE Customers ADD COLUMN Enabled INTEGER CHECK (Enabled in (1,0)) NOT NULL DEFAULT 1";
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = $"SELECT COUNT(*) AS ColumnExists FROM sqlite_master WHERE type = 'table' AND name = 'Suppliers' AND sql LIKE '%Enabled%';";
                bool columnExistsInSuppliersTable = Convert.ToInt32(cmd.ExecuteScalar()) != 0;
                if (!columnExistsInSuppliersTable)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = $"ALTER TABLE Suppliers ADD COLUMN Enabled INTEGER CHECK (Enabled in (1,0)) NOT NULL DEFAULT 1";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void EnsurePaidCashColumnExistsInSalesTable(SQLiteConnection connection)
        {
            using (var cmd = new SQLiteCommand("", connection))
            {
                cmd.CommandText = $"SELECT COUNT(*) AS ColumnExists FROM sqlite_master WHERE type = 'table' AND name = 'Sales' AND sql LIKE '%PaidCash%';";

                bool columnExists = Convert.ToInt32(cmd.ExecuteScalar()) != 0;
                if (!columnExists)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = $"ALTER TABLE Sales ADD COLUMN PaidCash INTEGER CHECK (PaidCash in (1,0)) NOT NULL DEFAULT 1";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
