﻿using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Infrastructure.Storage
{
    public class CustomerRepository
    {
        private const string TableName = "Customers";

        #region Модификация таблицы Customers.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        

        /// <summary>
        /// Добавляет переданный объект в БД.
        /// </summary>
        /// <param name="customer">Контрагент.</param>
        public static void AddCustomer(Customer customer)
        {
            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //Вставляем запись в ContactInfo, если требуется.
                            if (customer.ContactInfo != null)
                            {
                                customer.ContactInfo.ContactInfoId = ContactInfoDatabaseHandler.AddContactInfo(customer.ContactInfo, cmd);
                            }                                

                            //Вставляем запись в Customers или Suppliers.
                            AddCustomer(customer, cmd);

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
        /// <param name="customer">Контрагент.</param>
        /// <param name="cmd"></param>
        internal static int AddCustomer(Customer customer, SQLiteCommand cmd)
        {
            cmd.CommandText = "INSERT INTO " + TableName + " (ContragentName, Code, Entity, ContactInfoId, Description) "
                            + "VALUES (@ContragentName, @Code, @Entity, @ContactInfoId, @Description); "
                            + "SELECT last_insert_rowid();";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ContragentName", customer.ContragentName);
            cmd.Parameters.AddWithValue("@Code", customer.Code);
            cmd.Parameters.AddWithValue("@Entity", customer.Entity);
            cmd.Parameters.AddWithValue("@ContactInfoId", (customer.ContactInfo != null) ? customer.ContactInfo.ContactInfoId : (int?)null);
            cmd.Parameters.AddWithValue("@Description", customer.Description);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Обновляет контрагента в таблице.
        /// </summary>
        /// <param name="customer">Обновляемый контрагент</param>
        public static void UpdateCustomer(Customer customer)
        {
            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //Вставляем запись в ContactInfo, если требуется.
                            ContactInfo contactInfo = FindContactInfo(customer);
                            if (customer.ContactInfo != null)
                            {
                                //Если есть у объекта, но нет в базе -- добавляем запись в таблицу. Если есть в базе -- обновляем запись.
                                if (contactInfo != null)
                                {
                                    customer.ContactInfo.ContactInfoId = contactInfo.ContactInfoId;
                                    ContactInfoDatabaseHandler.UpdateContactInfo(customer.ContactInfo, cmd);
                                }
                                else
                                {
                                    customer.ContactInfo.ContactInfoId = ContactInfoDatabaseHandler.AddContactInfo(customer.ContactInfo, cmd);
                                }                                    
                            }

                            //Вставляем запись в Customers или Suppliers.
                            UpdateCustomer(customer, cmd);

                            //Если есть в базе, но нет у объекта -- удаляем запись с базы
                            if (contactInfo != null && customer.ContactInfo == null)
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

        /// <summary>
        /// Обновляет контрагента в таблице.
        /// </summary>
        /// <param name="customer">Обновляемый контрагент</param>
        /// <param name="cmd"></param>
        internal static void UpdateCustomer(Customer customer, SQLiteCommand cmd)
        {
            cmd.CommandText = "UPDATE " + TableName
                            + " SET ContragentName = @ContragentName, Code = @Code, Entity = @Entity, "
                            + "ContactInfoId = @ContactInfoId, Description = @Description, Balance = @Balance "
                            + "WHERE ContragentId = @ContragentId;";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ContragentId", customer.ContragentId);
            cmd.Parameters.AddWithValue("@ContragentName", customer.ContragentName);
            cmd.Parameters.AddWithValue("@Code", customer.Code);
            cmd.Parameters.AddWithValue("@Entity", customer.Entity);
            cmd.Parameters.AddWithValue("@ContactInfoId", (customer.ContactInfo != null) ? customer.ContactInfo.ContactInfoId : (int?)null);
            cmd.Parameters.AddWithValue("@Description", customer.Description);
            cmd.Parameters.AddWithValue("@Balance", customer.Balance);

            cmd.ExecuteNonQuery();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблицe Customers.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает коллекцию из всех Customer.
        /// </summary>
        /// <returns></returns>
        public static IList<IContragent> FindCustomers()
        {
            IList<IContragent> customers = new List<IContragent>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM Customers;";
                var cmd = new SQLiteCommand(query, connection);

                var dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    customers.Add(CreateCustomers(dataReader));
                }                

                connection.Close();
            }

            return customers;
        }

        /// <summary>
        /// Возвращает объект Customer найденный по заданному customerName, или null если такого объекта не найдено.
        /// </summary>
        /// <param name="customerName">имя Customer-а, которого надо найти.</param>
        /// <returns></returns>
        public static Customer FindCustomers(string customerName)
        {
            Customer customer = null;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM Customers WHERE ToLower(ContragentName) LIKE @ContragentName;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ContragentName", customerName.ToLower());

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        customer = CreateCustomers(dataReader);
                    }                        
                }

                connection.Close();
            }

            return customer;
        }

        /// <summary>
        /// Возвращает объект типа Customer найденный по заданному Id.
        /// </summary>
        /// <param name="customerId">Id клиента, которого надо найти.</param>
        /// <returns></returns>
        public static IContragent FindCustomers(int customerId)
        {
            Customer customer = new Customer();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM Customers WHERE ContragentId = @ContragentId;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ContragentId", customerId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    customer.ContragentId = Convert.ToInt32(dataReader["ContragentId"]);
                    customer.ContragentName = dataReader["ContragentName"] as string;
                    customer.Code = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    customer.Entity = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    customer.ContactInfo = (dataReader["ContactInfoId"] != DBNull.Value) ? ContactInfoDatabaseHandler.FindContactInfo(Convert.ToInt32(dataReader["ContactInfoId"])) : null;
                    customer.Description = (dataReader["Description"] == DBNull.Value) ? null : dataReader["Description"] as string;
                }

                connection.Close();
            }

            return customer;
        }
        private static Customer CreateCustomers(SQLiteDataReader dataReader)
        {
            return new Customer
            (
                contragentId: Convert.ToInt32(dataReader["ContragentId"]),
                contragentName: dataReader["ContragentName"] as string,
                code: dataReader["Code"] as string,
                entity: dataReader["Entity"] as string,
                contactInfo: (dataReader["ContactInfoId"] != DBNull.Value) ? ContactInfoDatabaseHandler.FindContactInfo(Convert.ToInt32(dataReader["ContactInfoId"])) : null,
                description: dataReader["Description"] as string,
                balance: (double)dataReader["Balance"]
            );
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск объекта Customer в таблице ContactInfo
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает объект типа ContactInfo, найденный по заданному Id контрагента, или null если ничего не найдено.
        /// </summary>
        /// <returns></returns>
        private static ContactInfo FindContactInfo(Customer customer)
        {
            ContactInfo contactInfo = null;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                string query = "SELECT ci.* FROM " + TableName + " as c "
                             + "JOIN ContactInfo as ci "
                             + "ON c.ContactInfoId = ci.ContactInfoId "
                             + "WHERE ContragentId = @ContragentId;";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@ContragentId", customer.ContragentId);

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
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
