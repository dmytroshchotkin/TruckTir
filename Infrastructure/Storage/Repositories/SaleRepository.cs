using Infrastructure.Storage.PropertiesHandlers;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Infrastructure.Storage.Repositories
{
    public class SaleRepository
    {
        #region Модификация таблицы Sales.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Осуществляет полный цикл продажи товара, вставляя записи в таблицы Sales, Avaliability и SaleDetails.
        /// Возвращает Id вставленной записи в табл. Sale.
        /// </summary>
        /// <param name="availabilityList">Список продаваемого товара.</param>
        /// <param name="purchase">Информация о продаже.</param>
        /// <returns></returns>
        public static int AddSale(Sale sale, List<OperationDetails> operDetList)
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
                            //Добавляем контрагента, если такого нет в базе, иначе обновляем его баланс.
                            if (sale.Contragent.ContragentId == 0)
                            {
                                if (sale.Contragent is Supplier)
                                {
                                    sale.Contragent.ContragentId = SupplierRepository.AddSupplier(sale.Contragent as Supplier, cmd);
                                }
                                else if (sale.Contragent is Customer)
                                {
                                    sale.Contragent.ContragentId = CustomerRepository.AddCustomer(sale.Contragent as Customer, cmd);
                                }
                            }

                            //вставляем запись в таблицу Sales.
                            sale.OperationId = AddSale(sale, cmd);
                            //вставляем записи в SaleDetails.
                            foreach (OperationDetails operDet in operDetList)
                            {
                                AvailabilityDatabaseHandler.SaleSparePartAvaliability(operDet, cmd);
                            }
                            // и модифицируем Avaliability.
                            foreach (OperationDetails operDet in sale.OperationDetailsList)
                            {
                                AddSaleDetail(sale.OperationId, operDet, cmd);
                            }

                            trans.Commit(); //Фиксируем изменения.
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback(); //Отменяем изменения.
                            throw new Exception(ex.Message);
                        }
                    }
                }

                connection.Close();
            }

            return sale.OperationId;
        }

        //Модификация таблицы Sales
        /// <summary>
        /// Возвращает Id вставленной записи в таблицу Sales.
        /// </summary>
        /// <param name="purchase">Продажа которую нужно добавить в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        /// <returns></returns>
        public static int AddSale(Sale sale, SQLiteCommand cmd)
        {
            int saleId;

            string query = String.Format("INSERT INTO Sales (EmployeeID, ContragentId, ContragentEmployee, OperationDate, Description)"
                                       + "VALUES (@EmployeeID, @ContragentId, @ContragentEmployee, strftime('%s', @OperationDate), @Description);"
                                       + "SELECT OperationId FROM Sales WHERE rowid = last_insert_rowid();");

            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EmployeeID", sale.Employee.EmployeeId);
            cmd.Parameters.AddWithValue("@ContragentId", sale.Contragent.ContragentId);
            cmd.Parameters.AddWithValue("@ContragentEmployee", sale.ContragentEmployee);
            cmd.Parameters.AddWithValue("@Description", sale.Description);
            cmd.Parameters.AddWithValue("@OperationDate", sale.OperationDate);

            saleId = Convert.ToInt32(cmd.ExecuteScalar());

            return saleId;
        }


        /// <summary>
        /// Обновляет запись в БД, данными из переданного объекта.
        /// </summary>
        /// <param name="saleId">Ид обновляемой записи в базе.</param>
        /// <param name="description">новое описание</param>
        public static void UpdateSale(int saleId, string description)
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
                            string query = "UPDATE Sales SET Description = @Description "
                                         + "WHERE OperationId = @OperationId;";

                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@Description", description);
                            cmd.Parameters.AddWithValue("@OperationId", saleId);

                            cmd.ExecuteNonQuery();

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
        #endregion

        #region Модификация таблицы SaleDetails.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Добавляет запись в таблицу SaleDetails.
        /// </summary>
        /// <param name="saleId">Ид продажи</param>
        /// <param name="sparePartId">Ид товара</param>
        /// <param name="price">Отпускная цена товара</param>
        /// <param name="quantity">Кол-во товара</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void AddSaleDetail(int saleId, OperationDetails operDet, SQLiteCommand cmd)
        {
            string query = "INSERT INTO SaleDetails VALUES (@OperationId, @SparePartId, @Quantity, @SellingPrice);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@OperationId", saleId);
            cmd.Parameters.AddWithValue("@SparePartId", operDet.SparePart.SparePartId);
            cmd.Parameters.AddWithValue("@Quantity", operDet.Count);
            cmd.Parameters.AddWithValue("@SellingPrice", operDet.Price);

            cmd.ExecuteNonQuery();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion       

        #region Поиск по таблице Sales
        /// <summary>
        /// Возвращает объект типа Sale, найденный по заданному Id.
        /// </summary>
        /// <param name="saleId">Id продажи</param>
        /// <returns></returns>
        public static Sale FindSale(int saleId)
        {
            Sale sale = null;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Sales WHERE OperationId = @SaleId;";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SaleId", saleId);

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            sale = CreateSale(dataReader);
                        }
                    }
                }

                connection.Close();
            }

            return sale;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cust">STUB</param>
        /// <returns></returns>
        public static List<Sale> FindSales(int customerId, Customer cust)
        {
           var salesList = new List<Sale>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Sales "
                                   + "WHERE ContragentId = @ContragentId "
                                   + "ORDER BY OperationDate desc;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@ContragentId", customerId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    salesList.Add(CreateSale(dataReader));
                }

                connection.Close();
            }

            return salesList;
        }

        public static List<Sale> FindSales(SparePart sparePart)
        {
            List<Sale> salesList = new List<Sale>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Sales "
                                   + "WHERE OperationId IN (SELECT OperationId FROM SaleDetails "
                                                          + "WHERE SparePartId = @SparePartId);";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        salesList.Add(CreateSale(dataReader));
                    }
                }

                connection.Close();
            }

            return salesList;
        }

        /// <summary>
        /// Возвращает список операций продажи осуществленных данным сотрудником.
        /// </summary>
        /// <param name="emp">Сотрудник по которому выдаются данные.</param>
        /// <param name="startDate">Минимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <param name="endDate">Максимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <returns></returns>
        public static List<Sale> FindSales(Employee emp, DateTime? startDate, DateTime? endDate)
        {
            List<Sale> salesList = new List<Sale>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Sales as s "
                                   + "WHERE s.EmployeeId = @EmployeeId "
                                        + "and s.OperationDate BETWEEN strftime('%s', @startDate) AND strftime('%s', @endDate);";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                cmd.Parameters.AddWithValue("@startDate", startDate != null ? startDate : new DateTime(1970, 1, 1)); //Если стартовая дата не задана, ищем по минимально возможному значению.
                cmd.Parameters.AddWithValue("@endDate", endDate != null ? endDate : new DateTime(2038, 1, 19)); //Если конечная дата не задана, ищем по максимально возможному значению.

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        salesList.Add(CreateSale(dataReader));
                    }
                }

                connection.Close();
            }

            return salesList;
        }

        /// <summary>
        /// Возвращает список операций продажи осуществленных данным сотрудником.
        /// </summary>
        /// <param name="emp">Сотрудник по которому выдаются данные.</param>
        /// <param name="startDate">Минимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <param name="endDate">Максимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <returns></returns>
        public static List<Sale> FindSales(DateTime? startDate, DateTime? endDate)
        {
            List<Sale> salesList = new List<Sale>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Sales as s "
                                   + "WHERE s.OperationDate BETWEEN strftime('%s', @startDate) AND strftime('%s', @endDate);";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@startDate", startDate != null ? startDate : new DateTime(1970, 1, 1)); //Если стартовая дата не задана, ищем по минимально возможному значению.
                cmd.Parameters.AddWithValue("@endDate", endDate != null ? endDate : new DateTime(2038, 1, 19)); //Если конечная дата не задана, ищем по максимально возможному значению.

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        salesList.Add(CreateSale(dataReader));
                    }
                }

                connection.Close();
            }

            return salesList;
        }

        private static Sale CreateSale(SQLiteDataReader dataReader)
        {
            var result = new Sale
            (
                operationId: Convert.ToInt32(dataReader["OperationId"]),
                employee: (dataReader["EmployeeId"] != DBNull.Value) ? EmployeeRepository.FindEmployees(Convert.ToInt32(dataReader["EmployeeId"])) : null,
                contragent: CustomerRepository.FindCustomer(Convert.ToInt32(dataReader["ContragentId"])),
                contragentEmployee: dataReader["ContragentEmployee"] as string,
                operationDate: Convert.ToDateTime(dataReader["OD"]),
                description: dataReader["Description"] as string
            );

            result.TrySetOperationDetails(new Lazy<List<OperationDetails>>(() => FindSaleDetails(result)));
            return result;
        }

        /// <summary>
        /// Возвращает детали операции для заданного расхода.
        /// </summary>
        /// <param name="purchase">Приход.</param>
        /// <returns></returns>
        private static List<OperationDetails> FindSaleDetails(Sale sale)
        {
            List<OperationDetails> operDetList = new List<OperationDetails>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.ConnectionString) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT * FROM SaleDetails "
                                   + "WHERE OperationId = @OperationId;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@OperationId", sale.OperationId);

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            operDetList.Add(CreateOperationDetails(dataReader, sale));
                        }
                    }
                }

                connection.Close();
            }

            return operDetList;
        }

        private static OperationDetails CreateOperationDetails(SQLiteDataReader dataReader, Sale operat)
        {
            return new OperationDetails
            (
                sparePart: SparePartRepository.FindSparePart(Convert.ToInt32(dataReader["SparePartId"])),
                operation: operat,
                count: Convert.ToSingle(dataReader["Count"]),
                price: Convert.ToSingle(dataReader["Price"])
            );
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
