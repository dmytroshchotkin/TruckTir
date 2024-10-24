﻿using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Infrastructure.Storage.Repositories
{
    public class SupplierRepository
    {
        private const string TableName = "Suppliers";

        #region Поиск по таблице Suppliers.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает коллекцию из всех Supplier-ов.
        /// </summary>
        /// <returns></returns>
        public static List<Supplier> FindSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Suppliers;", connection);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Supplier supplier = CreateSupplier(dataReader);

                    suppliers.Add(supplier);
                }

                connection.Close();
            }
            return suppliers;
        }

        /// <summary>
        /// Возвращает объект типа Contragent по заданному Id.
        /// </summary>
        /// <param name="supplierId">Id поставщика, которого надо найти.</param>
        /// <returns></returns>
        public static Supplier FindSuppliers(int supplierId)
        {
            Supplier supplier = null;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM  Suppliers WHERE ContragentId = @ContragentId;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ContragentId", supplierId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    supplier = CreateSupplier(dataReader);
                }
                connection.Close();
            }

            return supplier;
        }

        /// <summary>
        /// Возвращает объект Supplier найденный по заданному SupplierName, или null если такого объекта не найдено.
        /// </summary>
        /// <param name="SupplierName">имя Supplier-а, которого надо найти.</param>
        /// <returns></returns>
        public static Supplier FindSupplier(string supplierName)
        {
            Supplier supplier = null;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM Suppliers WHERE ToLower(ContragentName) LIKE @ContragentName;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ContragentName", supplierName.ToLower());

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    supplier = CreateSupplier(dataReader);
                }

                connection.Close();
            }

            return supplier;
        }

        /// <summary>
        /// Возвращает true если такой code уже есть в таблице Suppliers, иначе false.
        /// </summary>
        /// <param name="code">code наличие которого нужно проверить.</param>
        /// <returns></returns>
        public static bool IsSupplierCodeExist(string code)
        {
            bool isCodeExist = false;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Suppliers WHERE Code LIKE @Code;", connection);

                cmd.Parameters.AddWithValue("@Code", code);

                if (cmd.ExecuteScalar() != null)
                {
                    isCodeExist = true;
                }

                connection.Close();
            }
            return isCodeExist;
        }

        private static Supplier CreateSupplier(SQLiteDataReader dataReader)
        {
            return new Supplier
            (
                contragentId: Convert.ToInt32(dataReader["ContragentId"]),
                contragentName: dataReader["ContragentName"] as string,
                code: dataReader["Code"] as string,
                entity: dataReader["Entity"] as string,
                contactInfo: (dataReader["ContactInfoId"] != DBNull.Value) ? ContactInfoDatabaseHandler.FindContactInfo(Convert.ToInt32(dataReader["ContactInfoId"])) : null,
                description: dataReader["Description"] as string,
                enabled: Convert.ToBoolean(dataReader["Enabled"])
            );
        }
        #endregion

        #region Модификация таблицы Suppliers
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        

        /// <summary>
        /// Добавляет переданный объект в БД.
        /// </summary>
        /// <param name="supplier">Контрагент.</param>
        public static void AddSupplier(Supplier supplier)
        {
            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //Вставляем запись в ContactInfo, если требуется.
                            if (supplier.ContactInfo != null)
                            {
                                supplier.ContactInfo.ContactInfoId = ContactInfoDatabaseHandler.AddContactInfo(supplier.ContactInfo, cmd);
                            }

                            //Вставляем запись в Customers или Suppliers.
                            AddSupplier(supplier, cmd);

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }

                connection.Close();
            }
        }

        /// <summary>
        /// Добавляет переданный объект в БД и возращает его Id.
        /// </summary>
        /// <param name="supplier">Контрагент.</param>
        /// <param name="cmd"></param>
        public static int AddSupplier(Supplier supplier, SQLiteCommand cmd)
        {
            cmd.CommandText = "INSERT INTO " + TableName + " (ContragentName, Code, Entity, ContactInfoId, Description, Enabled) "
                            + "VALUES (@ContragentName, @Code, @Entity, @ContactInfoId, @Description, @Enabled); "
                            + "SELECT last_insert_rowid();";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ContragentName", supplier.ContragentName);
            cmd.Parameters.AddWithValue("@Code", supplier.Code);
            cmd.Parameters.AddWithValue("@Entity", supplier.Entity);
            cmd.Parameters.AddWithValue("@ContactInfoId", (supplier.ContactInfo != null) ? supplier.ContactInfo.ContactInfoId : (int?)null);
            cmd.Parameters.AddWithValue("@Description", supplier.Description);
            cmd.Parameters.AddWithValue("@Enabled", supplier.Enabled);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Обновляет контрагента в таблице.
        /// </summary>
        /// <param name="supplier">Обновляемый контрагент</param>
        public static void UpdateSupplier(Supplier supplier)
        {
            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //Вставляем запись в ContactInfo, если требуется.
                            ContactInfo contactInfo = FindContactInfo(supplier);
                            if (supplier.ContactInfo != null)
                            {
                                //Если есть у объекта, но нет в базе -- добавляем запись в таблицу. Если есть в базе -- обновляем запись.
                                if (contactInfo != null)
                                {
                                    supplier.ContactInfo.ContactInfoId = contactInfo.ContactInfoId;
                                    ContactInfoDatabaseHandler.UpdateContactInfo(supplier.ContactInfo, cmd);
                                }
                                else
                                {
                                    supplier.ContactInfo.ContactInfoId = ContactInfoDatabaseHandler.AddContactInfo(supplier.ContactInfo, cmd);
                                }
                            }

                            //Вставляем запись в Customers или Suppliers.
                            UpdateSupplier(supplier, cmd);

                            //Если есть в базе, но нет у объекта -- удаляем запись с базы
                            if (contactInfo != null && supplier.ContactInfo == null)
                            {
                                ContactInfoDatabaseHandler.DeleteContactInfo(contactInfo.ContactInfoId, cmd);
                            }

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }

                connection.Close();
            }
        }

        internal static void DisableSupplier(int supplierId)
        {
            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();
                UpdateSupplierEnability(supplierId, true, connection);
                connection.Close();
            }
        }

        internal static void EnableSupplier(int supplierId)
        {
            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();
                UpdateSupplierEnability(supplierId, false, connection);
                connection.Close();
            }
        }

        private static void UpdateSupplierEnability(int supplierId, bool disable, SQLiteConnection connection)
        {
            int enabilityValue = disable ? 0 : 1;
            using (var cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = $"UPDATE {TableName} SET Enabled = @Enabled WHERE ContragentId = @ContragentId;";
                cmd.Parameters.AddWithValue("@ContragentId", supplierId);
                cmd.Parameters.AddWithValue("@Enabled", enabilityValue);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Обновляет контрагента в таблице.
        /// </summary>
        /// <param name="supplier">Обновляемый контрагент</param>
        /// <param name="cmd"></param>
        public static void UpdateSupplier(Supplier supplier, SQLiteCommand cmd)
        {
            cmd.CommandText = "UPDATE " + TableName
                            + " SET ContragentName = @ContragentName, Code = @Code, Entity = @Entity, "
                            + "ContactInfoId = @ContactInfoId, Description = @Description, Balance = @Balance, Enabled = @Enabled "
                            + "WHERE ContragentId = @ContragentId;";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ContragentId", supplier.ContragentId);
            cmd.Parameters.AddWithValue("@ContragentName", supplier.ContragentName);
            cmd.Parameters.AddWithValue("@Code", supplier.Code);
            cmd.Parameters.AddWithValue("@Entity", supplier.Entity);
            cmd.Parameters.AddWithValue("@ContactInfoId", (supplier.ContactInfo != null) ? supplier.ContactInfo.ContactInfoId : (int?)null);
            cmd.Parameters.AddWithValue("@Description", supplier.Description);
            cmd.Parameters.AddWithValue("@Balance", supplier.Balance);
            cmd.Parameters.AddWithValue("@Enabled", supplier.Enabled);

            cmd.ExecuteNonQuery();
        }
        #endregion

        #region Поиск объекта Supplier в таблице ContactInfo
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает объект типа ContactInfo, найденный по заданному Id контрагента, или null если ничего не найдено.
        /// </summary>
        /// <param name="employeeId">Id сотрудника.</param>
        /// <returns></returns>
        private static ContactInfo FindContactInfo(Supplier supplier)
        {
            ContactInfo contactInfo = null;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                string query = "SELECT ci.* FROM " + TableName + " as c "
                             + "JOIN ContactInfo as ci "
                             + "ON c.ContactInfoId = ci.ContactInfoId "
                             + "WHERE ContragentId = @ContragentId;";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@ContragentId", supplier.ContragentId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        contactInfo = ContactInfoDatabaseHandler.CreateContactInfo(dataReader);
                    }
                }

                connection.Close();
            }

            return contactInfo;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
