using Infrastructure.Storage.PropertiesHandlers;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Infrastructure.Storage
{
    public class PurchaseRepository
    {
        #region Модификация таблицы Operation данными Purchase.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Осуществляет полный цикл приходования товара, вставляя записи в таблицы Purchases, Avaliability и PurchaseDetails.
        /// Возвращает Id вставленной записи в табл. Operation.
        /// </summary>
        /// <param name="availList">Список приходуемого товара.</param>
        /// <returns></returns>
        public static int AddPurchase(List<Availability> availList)
        {
            Purchase purchase = availList[0].OperationDetails.Operation as Purchase;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //Если такого контрагента нет в базе, то добавляем.
                            if (purchase.Contragent.ContragentId == 0)
                                if (purchase.Contragent.ContragentId == 0)
                                {
                                    if (purchase.Contragent is Supplier)
                                    {
                                        purchase.Contragent.ContragentId = SupplierRepository.AddSupplier(purchase.Contragent as Supplier, cmd);
                                    }

                                    else if (purchase.Contragent is Customer)
                                    {
                                        purchase.Contragent.ContragentId = CustomerRepository.AddCustomer(purchase.Contragent as Customer, cmd);
                                    }
                                }

                                else
                                {
                                    if (purchase.Contragent is Supplier)
                                    {
                                        SupplierRepository.UpdateSupplier(purchase.Contragent as Supplier, cmd);
                                    }

                                    else if (purchase.Contragent is Customer)
                                    {
                                        CustomerRepository.UpdateCustomer(purchase.Contragent as Customer, cmd);
                                    }
                                }
                            //вставляем запись в таблицу Operation.
                            purchase.OperationId = AddPurchase(purchase, cmd);
                            //вставляем записи в PurchaseDetails и Avaliability.
                            foreach (Availability avail in availList)
                            {
                                AddPurchaseDetail(avail.OperationDetails, cmd);
                                AvailabilityHandler.AddSparePartAvaliability(avail, cmd);
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

            return purchase.OperationId;
        }

        /// <summary>
        /// Возвращает Id вставленной записи в таблицу Purchases.
        /// </summary>
        /// <param name="purchase">Приход который нужно добавить в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        /// <returns></returns>
        public static int AddPurchase(Purchase purchase, SQLiteCommand cmd)
        {
            string query = "INSERT INTO Purchases (EmployeeID, ContragentId, ContragentEmployee, OperationDate, Description) "
                         + "VALUES (@EmployeeID, @ContragentId, @ContragentEmployee, strftime('%s', @OperationDate), @Description); "
                         + "SELECT OperationId FROM Purchases WHERE rowid = last_insert_rowid();";

            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EmployeeID", purchase.Employee.EmployeeId);
            cmd.Parameters.AddWithValue("@ContragentId", purchase.Contragent.ContragentId);
            cmd.Parameters.AddWithValue("@ContragentEmployee", purchase.ContragentEmployee);
            cmd.Parameters.AddWithValue("@Description", purchase.Description);
            cmd.Parameters.AddWithValue("@OperationDate", purchase.OperationDate);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Обновляет запись в БД, данными из переданного объекта.
        /// </summary>
        /// <param name="purchase">Объект. данными которого будет обновлена запись в БД</param>
        public static void UpdatePurchase(int purchaseId, string description)
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
                            string query = "UPDATE Purchases SET Description = @Description "
                                         + "WHERE OperationId = @OperationId;";
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@Description", description);
                            cmd.Parameters.AddWithValue("@OperationId", purchaseId);

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

        #region Модификация таблицы PurchaseDetails
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Добавляет запись в таблицу PurchaseDetails.
        /// </summary>
        /// <param name="purchaseDetails">Запись добавляемая в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void AddPurchaseDetail(OperationDetails purchaseDetails, SQLiteCommand cmd)
        {
            string query = "INSERT INTO PurchaseDetails VALUES (@OperationId, @SparePartId, @Count, @Price);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@OperationId", purchaseDetails.Operation.OperationId);
            cmd.Parameters.AddWithValue("@SparePartId", purchaseDetails.SparePart.SparePartId);
            cmd.Parameters.AddWithValue("@Count", purchaseDetails.Count);
            cmd.Parameters.AddWithValue("@Price", purchaseDetails.Price);

            cmd.ExecuteNonQuery();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Returns.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Осуществляет возврат товара.
        /// </summary>
        /// <param name="operDetList">Список возвращаемого товара</param>
        /// <param name="note">Заметка по возврату</param>
        public static void AddReturn(Purchase purchase, string note)
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
                            //Вставляем запись в таблицу Purchases.
                            purchase.OperationId = AddPurchase(purchase, cmd);

                            //Добавляем запись в таблицу Returns
                            AddReturn(purchase, note, cmd);

                            purchase.OperationDetailsList[0].Operation.OperationId = purchase.OperationId; //Меняем на Id нового прихода
                            foreach (OperationDetails operDet in purchase.OperationDetailsList)
                            {
                                //Присваиваем мин. цену прихода для данного товара.
                                operDet.Price = FindMinSparePartPurchasePrice(operDet.SparePart.SparePartId);

                                //Вставляем записи в PurchaseDetails и Avaliability.
                                AddPurchaseDetail(operDet, cmd);
                                AvailabilityHandler.AddSparePartAvaliability(new Availability(operDet, null, (float)Markup.Types.Retail), cmd);
                            }

                            trans.Commit();  //фиксируем изменения.
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
        }

        /// <summary>
        /// Добавляет запись в таблицу Returns.
        /// </summary>
        /// <param name="purchase">Новый приход, возвращенного товара</param>
        /// <param name="note">Заметка по возврату</param>
        /// <param name="cmd"></param>
        private static void AddReturn(Purchase purchase, string note, SQLiteCommand cmd)
        {
            string query = "INSERT INTO Returns (PurchaseId, SaleId, Note) "
                         + "VALUES (@PurchaseId, @SaleId, @Note);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@PurchaseId", purchase.OperationId);
            cmd.Parameters.AddWithValue("@SaleId", purchase.OperationDetailsList[0].Operation.OperationId);
            cmd.Parameters.AddWithValue("@Note", note);

            cmd.ExecuteNonQuery();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблице Purchases
        /// <summary>
        /// Возвращает объект типа Purchase, найденный по заданному Id.
        /// </summary>
        /// <param name="saleId">Id поставки</param>
        /// <returns></returns>
        public static Purchase FindPurchase(int purchaseId)
        {
            Purchase purchase = null;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Purchases WHERE OperationId = @PurchaseId;";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                            purchase = CreatePurchase(dataReader);
                    }
                }

                connection.Close();
            }

            return purchase;
        }

        public static List<IOperation> FindPurchases(int supplierId, SparePart spr)
        {
            List<IOperation> purchases = new List<IOperation>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Purchases "
                                   + "WHERE ContragentId = @ContragentId "
                                   + "ORDER BY OperationDate desc;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@ContragentId", supplierId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                    purchases.Add(CreatePurchase(dataReader));

                connection.Close();
            }

            return purchases;
        }

        public static List<Purchase> FindPurchases(SparePart sparePart)
        {
            List<Purchase> purchases = new List<Purchase>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Purchases "
                                   + "WHERE OperationId IN (SELECT OperationId FROM PurchaseDetails "
                                                         + "WHERE SparePartId = @SparePartId);";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        purchases.Add(CreatePurchase(dataReader));
                }

                connection.Close();
            }

            return purchases;
        }

        /// <summary>
        /// Возвращает список операций приходования осуществленных данным сотрудником.
        /// </summary>
        /// <param name="emp">Сотрудник по которому выдаются данные.</param>
        /// <param name="startDate">Минимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <param name="endDate">Максимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <returns></returns>
        public static List<Purchase> FindPurchases(Employee emp, DateTime? startDate, DateTime? endDate)
        {
            List<Purchase> purchases = new List<Purchase>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Purchases as p "
                                   + "WHERE p.EmployeeId = @EmployeeId "
                                        + "and p.OperationDate BETWEEN strftime('%s', @startDate) AND strftime('%s', @endDate);";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                cmd.Parameters.AddWithValue("@startDate", startDate != null ? startDate : new DateTime(1970, 1, 1)); //Если стартовая дата не задана, ищем по минимально возможному значению.
                cmd.Parameters.AddWithValue("@endDate", endDate != null ? endDate : new DateTime(2038, 1, 19)); //Если конечная дата не задана, ищем по максимально возможному значению.

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        purchases.Add(CreatePurchase(dataReader));
                }

                connection.Close();
            }

            return purchases;
        }

        /// <summary>
        /// Возвращает список операций приходования осуществленных данным сотрудником.
        /// </summary>
        /// <param name="emp">Сотрудник по которому выдаются данные.</param>
        /// <param name="startDate">Минимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <param name="endDate">Максимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <returns></returns>
        public static List<Purchase> FindPurchases(DateTime? startDate, DateTime? endDate)
        {
            List<Purchase> purchases = new List<Purchase>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Purchases as p "
                                   + "WHERE p.OperationDate BETWEEN strftime('%s', @startDate) AND strftime('%s', @endDate);";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@startDate", startDate != null ? startDate : new DateTime(1970, 1, 1)); //Если стартовая дата не задана, ищем по минимально возможному значению.
                cmd.Parameters.AddWithValue("@endDate", endDate != null ? endDate : new DateTime(2038, 1, 19)); //Если конечная дата не задана, ищем по максимально возможному значению.

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        purchases.Add(CreatePurchase(dataReader));
                }

                connection.Close();
            }

            return purchases;
        }

        /// <summary>
        /// Возвращает объект типа IOperation созданный из переданного SQLiteDataReader.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private static Purchase CreatePurchase(SQLiteDataReader dataReader)
        {
            var result = new Purchase
            (
                operationId: Convert.ToInt32(dataReader["OperationId"]),
                employee: (dataReader["EmployeeId"] != DBNull.Value) ? EmployeeRepository.FindEmployees(Convert.ToInt32(dataReader["EmployeeId"])) : null,
                contragent: SupplierRepository.FindSuppliers(Convert.ToInt32(dataReader["ContragentId"])),
                contragentEmployee: dataReader["ContragentEmployee"] as string,
                operationDate: Convert.ToDateTime(dataReader["OD"]),
                description: dataReader["Description"] as string
            );

            result.TrySetOperationDetails(new Lazy<IList<OperationDetails>>(() => FindPurchaseDetails(result)));
            return result;
        }

        /// <summary>
        /// Возвращает детали операции для заданного прихода.
        /// </summary>
        /// <param name="purchase">Приход.</param>
        /// <returns></returns>
        private static List<OperationDetails> FindPurchaseDetails(Purchase purchase)
        {
            List<OperationDetails> operDetList = new List<OperationDetails>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT * FROM PurchaseDetails "
                                   + "WHERE OperationId = @OperationId;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@OperationId", purchase.OperationId);

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                            operDetList.Add(CreateOperationDetails(dataReader, purchase));
                    }
                }

                connection.Close();
            }

            return operDetList;
        }

        /// <summary>
        /// Находит список возвращенного товара по заданному Id продажи.
        /// </summary>
        /// <param name="saleId">Id продажи</param>
        /// <returns></returns>
        public static List<OperationDetails> FindReturnDetails(int saleId)
        {
            List<OperationDetails> operDetList = new List<OperationDetails>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT * FROM Returns as r JOIN PurchaseDetails as pd "
                                   + "ON r.PurchaseId = pd.OperationId "
                                   + "WHERE r.SaleId = @SaleId;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SaleId", saleId);

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                            operDetList.Add(CreateOperationDetails(dataReader, (Purchase)null));
                    }
                }

                connection.Close();
            }

            return operDetList;
        }

        /// <summary>
        /// Возвращает минимальную закупочную цену для переданного товара.
        /// </summary>
        /// <param name="sparePartId">Ид товара для которого находится мин. закупочная цена</param>
        /// <returns></returns>
        private static float FindMinSparePartPurchasePrice(int sparePartId)
        {
            float minPrice = 0;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT MIN(Price) FROM PurchaseDetails "
                                   + "WHERE SparePartId = @SparePartId;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                    minPrice = Convert.ToSingle(cmd.ExecuteScalar());
                }

                connection.Close();
            }

            return minPrice;
        }

        private static OperationDetails CreateOperationDetails(SQLiteDataReader dataReader, Purchase operat)
        {
            return new OperationDetails
            (
                sparePart: SparePartRepository.FindSparePart(Convert.ToInt32(dataReader["SparePartId"])),
                operation: operat,
                count: Convert.ToSingle(dataReader["Count"]),
                price: Convert.ToSingle(dataReader["Price"])
            );
        }
        #endregion
    }
}
