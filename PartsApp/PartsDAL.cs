using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using Excel = Microsoft.Office.Interop.Excel;
using PartsApp.SupportClasses;
using PartsApp.Models;

namespace PartsApp
{
    static class PartsDAL
    {
        private const string SparePartConfig = "SparePartConfig";

        #region ************Модификация данных в БД.****************************************************************************
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Модификация таблицы Avaliability.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Добавляет запись в таблицу Avaliability.
        /// </summary>
        /// <param name="avail">Запись добавляемая в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void AddSparePartAvaliability(Availability avail, SQLiteCommand cmd)
        {
            /*ERROR!!! лишние параметры */
            var query = "INSERT INTO Avaliability VALUES (@SparePartId, @OperationId, @Price, @Markup, @StorageAdress, @Count);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();
            
            cmd.Parameters.AddWithValue("@SparePartId",   avail.OperationDetails.SparePart.SparePartId);
            cmd.Parameters.AddWithValue("@OperationId",   avail.OperationDetails.Operation.OperationId);
            cmd.Parameters.AddWithValue("@Price",         avail.OperationDetails.Price);            
            cmd.Parameters.AddWithValue("@Markup",        avail.Markup);
            cmd.Parameters.AddWithValue("@StorageAdress", avail.StorageAddress);
            cmd.Parameters.AddWithValue("@Count",         avail.OperationDetails.Count);
            cmd.ExecuteNonQuery();    
        }//AddSparePartAvaliability 
       
        /// <summary>
        /// Обновляет количество в заданной записи таблицы Avaliability.
        /// </summary>
        /// <param name="sparePartId">Ид товара искомой записи</param>
        /// <param name="saleId">Ид прихода искомой записи</param>        
        /// <param name="newCount">Новое кол-во, которое будет записано в базу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        public static void UpdateSparePartСountAvaliability(int sparePartId, int purchaseId, double newCount, SQLiteCommand cmd)
        {
            string query = "UPDATE Avaliability SET Count = @Count WHERE SparePartId = @SparePartId AND OperationId = @OperationId;";
            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@OperationId", purchaseId);
            cmd.Parameters.AddWithValue("@Count", newCount);

            cmd.ExecuteNonQuery();            
        }//UpdateSparePartСountAvaliability
        /// <summary>
        /// Метод обновления значения Markup у записей с заданным SparePartId и PurchaseId.
        /// </summary>
        /// <param name="sparePartId">Id запчасти с изменяемой наценкой</param>
        /// <param name="saleId">Id прихода с изменяемой наценкой</param>
        /// <param name="markup">Значение наценки на которое стоит поменять текущее значение.</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        public static void UpdateSparePartMarkup(int sparePartId, int purchaseId, double markup, SQLiteCommand cmd)
        {
            const string query = "UPDATE Avaliability SET Markup = @Markup WHERE SparePartId = @SparePartId AND OperationId = @OperationId;";
            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@Markup", markup);
            cmd.Parameters.AddWithValue("@OperationId", purchaseId);

            cmd.ExecuteNonQuery();                                                              
        }//UpdateSparePartMarkup
        /// <summary>
        /// Изменяет наценку у записей с заданными SparePartId и PurchaseId на заданную Markup
        /// </summary>
        /// <param name="changeMarkupDict">Словарь типа (sparePartId, IDictionary(saleId, markup))</param>
        public static void UpdateSparePartMarkup(IDictionary<int, IDictionary<int, double>> changeMarkupDict)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            int sparePartId = 0, purchaseId = 0;
                            double markup = 0;
                            foreach (KeyValuePair<int, IDictionary<int, double>> spIdKeyValue in changeMarkupDict)
                            {
                                sparePartId = spIdKeyValue.Key;

                                foreach (KeyValuePair<int, double> purchIdKeyValue in spIdKeyValue.Value)
                                {
                                    purchaseId = purchIdKeyValue.Key;
                                    markup = purchIdKeyValue.Value;
                                    UpdateSparePartMarkup(sparePartId, purchaseId, markup, cmd);
                                }//foreach                    
                            }//foreach

                            trans.Commit();
                        }//try
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new System.Data.SQLite.SQLiteException(ex.Message);
                        }//catch
                    }//using cmd
                }//using transasction

                connection.Close();
            }//using 
        }//UpdateSparePartMarkup
        /// <summary>
        /// Удаляет заданную запись из таблицы Avaliability.
        /// </summary>
        /// <param name="sparePartId">Ид товара искомой записи</param>
        /// <param name="saleId">Ид прихода искомой записи</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        public static void DeleteSparePartAvaliability(int sparePartId, int purchaseId, SQLiteCommand cmd)
        {
            const string query = "DELETE FROM Avaliability WHERE SparePartId = @SparePartId AND OperationId = @OperationId;";
            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@OperationId", purchaseId);

            cmd.ExecuteNonQuery();
        }//DeleteSparePartAvaliability
        /// <summary>
        /// Уменьшает кол-во или удаляет запись из таблицы Avaliability.
        /// </summary>
        /// <param name="avail">уменьшаемый или удаляемый товар</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void SaleSparePartAvaliability(OperationDetails operDet, SQLiteCommand cmd)
        {
            //Узнаем количество данного товара в наличии.
            double spAvaliabilityCount = FindSparePartAvaliabilityCount(operDet.SparePart.SparePartId, operDet.Operation.OperationId);
            

            //Если кол-во продаваемого товара с данного прихода равно всему кол-во товара данной записи, удаляем из таблицы эту запись, иначе обновляем кол-во товара в базе.
            if (spAvaliabilityCount == operDet.Count)
                DeleteSparePartAvaliability(operDet.SparePart.SparePartId, operDet.Operation.OperationId, cmd);
            else
                UpdateSparePartСountAvaliability(operDet.SparePart.SparePartId, operDet.Operation.OperationId, spAvaliabilityCount - operDet.Count, cmd);

        }//SaleSparePartAvaliability






















        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы SpareParts.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void AddSparePart(SparePart sparePart)
        {
            /*ERROR добавить транзакцию.*/
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                //Вставляем запись в табл. "SparePart"
                const string query = "INSERT INTO SpareParts(Photo, Articul, Title, Description, ManufacturerId, MeasureUnit) " +
                                     "VALUES(@Photo, @Articul, @Title, @Description, @ManufacturerId, @MeasureUnit);";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@Photo", sparePart.Photo);
                cmd.Parameters.AddWithValue("@Articul", sparePart.Articul);
                cmd.Parameters.AddWithValue("@Title", sparePart.Title);
                cmd.Parameters.AddWithValue("@Description", sparePart.Description);                
                cmd.Parameters.AddWithValue("@MeasureUnit", sparePart.MeasureUnit);

                //Находим существующий manufacturerId в базе или добавляем новый объект если отсутствует.
                if (sparePart.Manufacturer == null)
                    cmd.Parameters.AddWithValue("@ManufacturerId", sparePart.Manufacturer);
                else
                {
                    IList<int> manufIds = FindManufacturersIdByName(sparePart.Manufacturer);
                    cmd.Parameters.AddWithValue("@ManufacturerId", (manufIds.Count == 0) ? AddManufacturer(sparePart.Manufacturer) : manufIds[0]);
                }//else

                cmd.ExecuteNonQuery();
                
                connection.Close();
            }//using
        }//AddSparePart
        /// <summary>
        /// Метод модификации записи с заданным Id.
        /// </summary>
        /// <param name="avail">Товар инф-ция о котором модифицируется.</param>
        public static void UpdateSparePart(SparePart sparePart)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                //Вставляем запись в табл. "SparePart"
                const string query = "UPDATE SpareParts SET Photo = @Photo, Articul = @Articul, Title = @Title, "
                                   + "Description = @Description, ManufacturerId = @ManufacturerId, MeasureUnit = @MeasureUnit " 
                                   + "WHERE SparePartId = @SparePartId;";
                 

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);
                cmd.Parameters.AddWithValue("@Photo", sparePart.Photo);
                cmd.Parameters.AddWithValue("@Articul", sparePart.Articul);
                cmd.Parameters.AddWithValue("@Title", sparePart.Title);
                cmd.Parameters.AddWithValue("@Description", sparePart.Description);
                cmd.Parameters.AddWithValue("@MeasureUnit", sparePart.MeasureUnit);

                //Находим существующий manufacturerId в базе или добавляем новый объект если отсутствует.
                if (sparePart.Manufacturer == null)
                    cmd.Parameters.AddWithValue("@ManufacturerId", sparePart.Manufacturer);
                else
                {
                    IList<int> manufIds = FindManufacturersIdByName(sparePart.Manufacturer);
                    cmd.Parameters.AddWithValue("@ManufacturerId", (manufIds.Count == 0) ? AddManufacturer(sparePart.Manufacturer) : manufIds[0]);
                }//else

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using
        
        }//UpdateSparePart



























//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Suppliers и Customers.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        

        /// <summary>
        /// Добавляет переданный объект в БД.
        /// </summary>
        /// <param name="contragent">Контрагент.</param>
        public static void AddContragent(IContragent contragent)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //Вставляем запись в ContactInfo, если требуется.
                            if (contragent.ContactInfo != null)
                                contragent.ContactInfo.ContactInfoId = AddContactInfo(contragent.ContactInfo, cmd);

                            //Вставляем запись в Customers или Suppliers.
                            AddContragent(contragent, cmd);

                            trans.Commit();
                        }//try
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new Exception(ex.Message);
                        }//catch
                    }//using cmd
                }//using transaction

                connection.Close();
            }//using connection
        }//AddContragent

        /// <summary>
        /// Добавляет переданный объект в БД и возращает его Id.
        /// </summary>
        /// <param name="contragent">Контрагент.</param>
        /// <param name="cmd"></param>
        private static int AddContragent(IContragent contragent, SQLiteCommand cmd)
        {
            string tableName = (contragent is Supplier) ? "Suppliers" : "Customers";
            cmd.CommandText = "INSERT INTO " + tableName + " (ContragentName, Code, Entity, ContactInfoId, Description) "
                            + "VALUES (@ContragentName, @Code, @Entity, @ContactInfoId, @Description); "
                            + "SELECT last_insert_rowid();";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ContragentName", contragent.ContragentName);
            cmd.Parameters.AddWithValue("@Code",           contragent.Code);
            cmd.Parameters.AddWithValue("@Entity",         contragent.Entity);
            cmd.Parameters.AddWithValue("@ContactInfoId", (contragent.ContactInfo != null) ? contragent.ContactInfo.ContactInfoId : (int?)null);
            cmd.Parameters.AddWithValue("@Description",    contragent.Description);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }//AddContragent






























//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
      
        #region Модификация таблицы Manufacturers
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Добавляет нового производителя в БД и возвращает его Id.
        /// </summary>
        /// <param name="manufacturerName">Имя добавляемого производителя</param>
        /// <returns></returns>
        public static int AddManufacturer(string manufacturerName)
        {
            int id = 0;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                string query = String.Format("INSERT INTO Manufacturers(ManufacturerName) VALUES(@ManufacturerName); " +
                                             "SELECT ManufacturerId FROM Manufacturers WHERE rowid = last_insert_rowid();");

                //Вставляем запись в табл. "Manufacturer"
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerName);
                
                id = Convert.ToInt32(cmd.ExecuteScalar());    
                               
                connection.Close();
            }//using

            return id;
        }//AddManufacturer























//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы ContactInfo.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Метод добавляет новую запись в таблицу ContactInfo и возвращает Id вставленной записи.
        /// </summary>
        /// <param name="contactInfo">объект типа ContactInfo данные которого будут добавлены в базу</param>
        /// <returns></returns>
        public static int AddContactInfo(ContactInfo contactInfo, SQLiteCommand cmd)
        {
            cmd.CommandText = "INSERT INTO ContactInfo (Country, Region, City, Street, House, Room, Phone, ExtPhone, Website, Email) "
                            + "VALUES (@Country, @Region, @City, @Street, @House, @Room, @Phone, @ExtPhone, @Website, @Email); "
                            + "SELECT ContactInfoId FROM ContactInfo WHERE rowid = last_insert_rowid();";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Country", contactInfo.Country);
            cmd.Parameters.AddWithValue("@Region", contactInfo.Region);
            cmd.Parameters.AddWithValue("@City", contactInfo.City);
            cmd.Parameters.AddWithValue("@Street", contactInfo.Street);
            cmd.Parameters.AddWithValue("@House", contactInfo.House);
            cmd.Parameters.AddWithValue("@Room", contactInfo.Room);
            cmd.Parameters.AddWithValue("@Phone", contactInfo.Phone);
            cmd.Parameters.AddWithValue("@ExtPhone", contactInfo.ExtPhone);
            cmd.Parameters.AddWithValue("@Website", contactInfo.Website);
            cmd.Parameters.AddWithValue("@Email", contactInfo.Email);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }//AddContactInfo





















//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Operation.
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

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                                purchase.Contragent.ContragentId = AddContragent(purchase.Contragent, cmd);
                            //вставляем запись в таблицу Operation.
                            purchase.OperationId = AddPurchase(purchase, cmd);
                            //вставляем записи в PurchaseDetails и Avaliability.
                            foreach (Availability avail in availList)
                            {
                                AddPurchaseDetail(avail.OperationDetails, cmd);
                                AddSparePartAvaliability(avail, cmd);
                            }//foreach

                            trans.Commit();                        
                        }//try
                        catch(Exception ex)
                        {                             
                            trans.Rollback();
                            throw new Exception(ex.Message);
                        }//catch
                    }//using cmd
                }//using transaction

                connection.Close();
            }//using connection

            return purchase.OperationId;    
        }//AddPurchase

        /// <summary>
        /// Возвращает Id вставленной записи в таблицу Purchases.
        /// </summary>
        /// <param name="sale">Приход который нужно добавить в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        /// <returns></returns>
        private static int AddPurchase(Purchase purchase, SQLiteCommand cmd)
        {
            string query = "INSERT INTO Purchases (EmployeeID, ContragentId, ContragentEmployee, OperationDate, Description) "
                         + "VALUES (@EmployeeID, @ContragentId, @ContragentEmployee, strftime('%s', @OperationDate), @Description); "
                         + "SELECT OperationId FROM Purchases WHERE rowid = last_insert_rowid();";

            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EmployeeID",         purchase.Employee.EmployeeId);
            cmd.Parameters.AddWithValue("@ContragentId",       purchase.Contragent.ContragentId);
            cmd.Parameters.AddWithValue("@ContragentEmployee", purchase.ContragentEmployee);
            cmd.Parameters.AddWithValue("@Description",        purchase.Description);
            cmd.Parameters.AddWithValue("@OperationDate",      purchase.OperationDate);

            return Convert.ToInt32(cmd.ExecuteScalar());       
        }//AddPurchase

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
            cmd.Parameters.AddWithValue("@Price",       purchaseDetails.Price);

            cmd.ExecuteNonQuery();
        }//AddPurchaseDetail

















        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion


















//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        
        #region Модификация таблицы Sales.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Осуществляет полный цикл продажи товара, вставляя записи в таблицы Sales, Avaliability и SaleDetails.
        /// Возвращает Id вставленной записи в табл. Sale.
        /// </summary>
        /// <param name="availabilityList">Список продаваемого товара.</param>
        /// <param name="sale">Информация о продаже.</param>
        /// <returns></returns>
        public static int AddSale(Sale sale, List<OperationDetails> operDetList)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //Добавляем контрагента, если такого нет в базе.
                            if (sale.Contragent.ContragentId == 0)
                                sale.Contragent.ContragentId =  AddContragent(sale.Contragent, cmd);
                            //вставляем запись в таблицу Sales.
                            sale.OperationId = AddSale(sale, cmd);
                            //вставляем записи в SaleDetails.
                            foreach (OperationDetails operDet in operDetList)
                                SaleSparePartAvaliability(operDet, cmd);
                            // и модифицируем Avaliability.
                            foreach (OperationDetails operDet in sale.OperationDetailsList)
                                AddSaleDetail(sale.OperationId, operDet, cmd);

                            trans.Commit(); //Фиксируем изменения.
                        }//try
                        catch (Exception ex)
                        {                            
                            trans.Rollback(); //Отменяем изменения.
                            throw new Exception(ex.Message);
                        }//catch
                    }//using cmd
                }//using transaction

                connection.Close();
            }//using connection

            return sale.OperationId;
        }//AddSale

        //Модификация таблицы Sales
        /// <summary>
        /// Возвращает Id вставленной записи в таблицу Sales.
        /// </summary>
        /// <param name="sale">Продажа которую нужно добавить в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        /// <returns></returns>
        private static int AddSale(Sale sale, SQLiteCommand cmd)
        {
            int saleId = 0;


            string query = String.Format("INSERT INTO Sales (EmployeeID, ContragentId, ContragentEmployee, OperationDate, Description)"
                                       + "VALUES (@EmployeeID, @ContragentId, @ContragentEmployee, strftime('%s', @OperationDate), @Description);"
                                       + "SELECT OperationId FROM Sales WHERE rowid = last_insert_rowid();");

            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EmployeeID",          sale.Employee.EmployeeId);
            cmd.Parameters.AddWithValue("@ContragentId",        sale.Contragent.ContragentId);
            cmd.Parameters.AddWithValue("@ContragentEmployee",  sale.ContragentEmployee);
            cmd.Parameters.AddWithValue("@Description",         sale.Description);
            cmd.Parameters.AddWithValue("@OperationDate",       sale.OperationDate);

            saleId = Convert.ToInt32(cmd.ExecuteScalar());

            return saleId;
        }//AddSale

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
        }//AddSaleDetail




















//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion












        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Employees.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void AddEmployee(Employee employee)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //Вставляем запись в таблицу ContactInfo, если требуется.
                            if (employee.ContactInfo != null)
                                employee.ContactInfo.ContactInfoId = AddContactInfo(employee.ContactInfo, cmd);
                            //Вставляем записm в табл. Employees.
                            AddEmployee(employee, cmd);
                            

                            trans.Commit();
                        }//try
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new Exception(ex.Message);
                        }//catch
                    }//using cmd
                }//using transaction

                connection.Close();
            }//using connection
        }//AddEmployee

        /// <summary>
        /// Добавляет объект типа Employee в таблицу Employees.
        /// </summary>
        /// <param name="employee">объект типа Employee добавляемый в БД.</param>
        private static void AddEmployee(Employee employee, SQLiteCommand cmd)
        {

            cmd.CommandText = "INSERT INTO Employees (LastName, FirstName, MiddleName, BirthDate, HireDate, DismissalDate, "
                            + "ContactInfoId, Photo, Note, PassportNum, Title, AccessLayer, Login, Password) "
                            + "VALUES (@LastName, @FirstName, @MiddleName, @BirthDate, strftime('%s', @HireDate), "
                            + "strftime('%s', @DismissalDate), @ContactInfoId, @Photo, @Note, @PassportNum, @Title, @AccessLayer, @Login, @Password);";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@LastName",      employee.LastName);
            cmd.Parameters.AddWithValue("@FirstName",     employee.FirstName);
            cmd.Parameters.AddWithValue("@MiddleName",    employee.MiddleName);
            cmd.Parameters.AddWithValue("@BirthDate",     (employee.BirthDate     != null) ? ((DateTime)employee.BirthDate).ToShortDateString() : null);
            cmd.Parameters.AddWithValue("@HireDate",      (employee.HireDate      != null) ? employee.HireDate : null);
            cmd.Parameters.AddWithValue("@DismissalDate", (employee.DismissalDate != null) ? employee.DismissalDate : null);
            cmd.Parameters.AddWithValue("@ContactInfoId", (employee.ContactInfo   != null) ? employee.ContactInfo.ContactInfoId : (int?)null);
            cmd.Parameters.AddWithValue("@Photo",         employee.Photo);
            cmd.Parameters.AddWithValue("@Note",          employee.Note);
            cmd.Parameters.AddWithValue("@PassportNum",   employee.PassportNum);
            cmd.Parameters.AddWithValue("@Title",         employee.Title);
            cmd.Parameters.AddWithValue("@AccessLayer",   employee.AccessLayer);
            cmd.Parameters.AddWithValue("@Login",         employee.Login);
            cmd.Parameters.AddWithValue("@Password",      employee.Password);

            cmd.ExecuteNonQuery();
        }//AddEmployee

        /// <summary>
        /// Метод обновляющий значения заданного сотрудника.
        /// </summary>
        /// <param name="employee">Сотрудник, значения которого необходимо обновить в базе.</param>
        public static void UpdateEmployee(Employee employee)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "UPDATE Employees SET LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName, "
                                   + "BirthDate = @BirthDate, HireDate = strftime('%s', @HireDate), ContactInfoId = @ContactInfoId, "
                                   + "Photo = @Photo, Note = @Note, PassportNum = @PassportNum, Title = @Title, AccessLayer = @AccessLayer, "
                                   + "Login = @Login, Password = @Password, DismissalDate = strftime('%s', @DismissalDate) "
                                   + "WHERE EmployeeId = @EmployeeId;";


                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@EmployeeId",    employee.EmployeeId);
                cmd.Parameters.AddWithValue("@LastName",      employee.LastName);
                cmd.Parameters.AddWithValue("@FirstName",     employee.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName",    employee.MiddleName);
                cmd.Parameters.AddWithValue("@BirthDate",     (employee.BirthDate != null) ? ((DateTime)employee.BirthDate).ToShortDateString() : null);
                cmd.Parameters.AddWithValue("@HireDate",      (employee.HireDate != null) ? employee.HireDate : null);
                cmd.Parameters.AddWithValue("@DismissalDate", (employee.DismissalDate != null) ? employee.DismissalDate : null);
                cmd.Parameters.AddWithValue("@ContactInfoId", (employee.ContactInfo != null) ? employee.ContactInfo.ContactInfoId : (int?)null);
                cmd.Parameters.AddWithValue("@Photo",         employee.Photo);
                cmd.Parameters.AddWithValue("@Note",          employee.Note);
                cmd.Parameters.AddWithValue("@PassportNum",   employee.PassportNum);
                cmd.Parameters.AddWithValue("@Title",         employee.Title);
                cmd.Parameters.AddWithValue("@AccessLayer",   employee.AccessLayer);
                cmd.Parameters.AddWithValue("@Login",         employee.Login);
                cmd.Parameters.AddWithValue("@Password",      employee.Password);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using
        }//UpdateEmployee
        /// <summary>
        /// Метод обновляющий значения заданного сотрудника, без обновления его пароля.
        /// </summary>
        /// <param name="employee">Сотрудник, значения которого необходимо обновить в базе.</param>
        public static void UpdateEmployeeWithoutPassword(Employee employee)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "UPDATE Employees SET LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName, "
                                   + "BirthDate = @BirthDate, HireDate = strftime('%s', @HireDate), ContactInfoId = @ContactInfoId, "
                                   + "Photo = @Photo, Note = @Note, PassportNum = @PassportNum, Title = @Title, AccessLayer = @AccessLayer, "
                                   + "Login = @Login, DismissalDate = strftime('%s', @DismissalDate) "
                                   + "WHERE EmployeeId = @EmployeeId;";


                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                cmd.Parameters.AddWithValue("@BirthDate", (employee.BirthDate != null) ? ((DateTime)employee.BirthDate).ToShortDateString() : null);
                cmd.Parameters.AddWithValue("@HireDate", (employee.HireDate != null) ? employee.HireDate : null);
                cmd.Parameters.AddWithValue("@DismissalDate", (employee.DismissalDate != null) ? employee.DismissalDate : null);
                cmd.Parameters.AddWithValue("@ContactInfoId", (employee.ContactInfo != null) ? employee.ContactInfo.ContactInfoId : (int?)null);
                cmd.Parameters.AddWithValue("@Photo", employee.Photo);
                cmd.Parameters.AddWithValue("@Note", employee.Note);
                cmd.Parameters.AddWithValue("@PassportNum", employee.PassportNum);
                cmd.Parameters.AddWithValue("@Title", employee.Title);
                cmd.Parameters.AddWithValue("@AccessLayer", employee.AccessLayer);
                cmd.Parameters.AddWithValue("@Login", employee.Login);


                cmd.ExecuteNonQuery();

                connection.Close();
            }//using
        }//UpdateEmployeeWithoutPassword















        






//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        

















////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region ************Точный поиск по БД.*********************************************************************************
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       

        #region *****************Поиск по таблицам Avaliablility********************************************************************
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       

        /// <summary>
        /// Возвращает количество в наличии заданной единицы товара.
        /// </summary>
        /// <param name="sparePartId">Ид товара искомой записи</param>
        /// <param name="saleId">Ид прихода искомой записи</param>
        /// <returns></returns>
        public static double FindSparePartAvaliabilityCount(int sparePartId, int purchaseId)
        {
            double count = 0;         
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                string query = "SELECT Count FROM Avaliability WHERE SparePartId = @SparePartId AND OperationId = @OperationId;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
                cmd.Parameters.AddWithValue("@OperationId", purchaseId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    count = Convert.ToDouble(dataReader["Count"]);
                }//while
                connection.Close();
            }//using
            return count;        
        }//FindSparePartAvaliabilityCount
        
       
       





        public static List<Availability> FindAvailability(SparePart sparePart)
        {
            List<Availability> availabilityList = new List<Availability>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM Avaliability "
                                   + "WHERE SparePartId = @SparePartId;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                            availabilityList.Add(CreateAvailability(dataReader, sparePart));
                    }//using dataReader
                }//using cmd

                connection.Close();
            }//using

            return availabilityList;
        }//FindAvailability




        private static Availability CreateAvailability(SQLiteDataReader dataReader, SparePart sparePart)
        {
            return new Availability
            (
                operationDetails : CreateOperationDetails(dataReader, sparePart),
                storageAddress   : dataReader["StorageAdress"] as string,
                markup           : Convert.ToSingle(dataReader["Markup"])
            );
        }//CreateAvailability




        









////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region *****************Поиск по таблицам SpareParts. *********************************************************************
//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        /// <summary>
        /// Возвращает объект типа SparePart, найденный по заданному Id, или null если такого объекта не найдено.
        /// </summary>
        /// <param name="sparePartId">Ид товара</param>
        /// <returns></returns>
        public static SparePart FindSparePart(int sparePartId)
        {
            SparePart sparePart = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                sparePart = FindSparePart(sparePartId, connection);
                
                connection.Close();
            }//using

            return sparePart;
        }//FindSparePart

        /// <summary>
        /// Возвращает объект типа SparePart, найденный по заданному Id, или null если такого объекта не найдено.
        /// </summary>
        /// <param name="sparePartId">Ид товара</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        /// <returns></returns>
        private static SparePart FindSparePart(int sparePartId, SQLiteConnection openConnection)
        {
            SparePart sparePart = null;

            var cmd = new SQLiteCommand("SELECT * FROM SpareParts WHERE SparePartId = @SparePartId;", openConnection);
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

            using (SQLiteDataReader dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                    sparePart = CreateSparePart(dataReader);
            }//using dataReader

            return sparePart;
        }//FindSparePart



        /// <summary>
        /// Возвращает список запчастей с заданным артикулом, заполненных только полями таблицы SpareParts, остальные поля не заполнены. 
        /// </summary>
        /// <param name="sparePartArticul">Артикул искомых запчастей.</param>
        /// <returns></returns>
        public static IList<SparePart> FindSparePartsByArticul(string sparePartArticul)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                IList<int> sparePartsId = FindSparePartsIdByArticul(sparePartArticul, connection);
                foreach (var sparePartId in sparePartsId)
                    spareParts.Add(FindSparePart(sparePartId, connection));

                connection.Close();
            }//using
            return spareParts;
        }//FindSparePartsIdByArticul
        
        public static IList<int> FindSparePartsIdByArticul(string sparePartArticul, SQLiteConnection openConnection)
        {
            IList<int> sparePartsId = new List<int>();

            var cmd = new SQLiteCommand("SELECT SparePartId FROM SpareParts WHERE Articul LIKE @Articul;", openConnection);

            cmd.Parameters.AddWithValue("@Articul", sparePartArticul);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                sparePartsId.Add(Convert.ToInt32(dataReader["SparePartId"]));
            }//while    

            return sparePartsId;
        }//FindSparePartsIdByArticul


















//|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion
        #region *****************Поиск по полям остальных таблиц.*******************************************************************
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Поиск по таблицe Manufacturers.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                
/*!!!*/ public static string[] FindAllManufacturersName()
        {
            IList<string> manufacturers = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT ManufacturerName FROM Manufacturers;", connection);
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    manufacturers.Add(dataReader["ManufacturerName"] as string);
                }//while
                connection.Close();
            }//using

            //создаём массив string.
            string[] manuf = new string[manufacturers.Count];
            for (int i = 0; i < manuf.Length; ++i)
                manuf[i] = manufacturers[i];

            return manuf;
        }//FindAllManufacturersName

/*!!!*/ public static string FindManufacturerNameById(int? manufacturerId)
        {
            string manufacturer = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT ManufacturerName FROM Manufacturers WHERE ManufacturerId = @ManufacturerId;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

                var dataReader = cmd.ExecuteReader();
                dataReader.Read();
                manufacturer = dataReader["ManufacturerName"] as string;

                connection.Close();
            }//using
            return manufacturer;
        }//FindManufacturerNameById        
        /// <summary>
        /// Возвращает список Id-ков производителей с заданным именем.
        /// </summary>
        /// <param name="manufacturerName">Имя искомых производителей.</param>
        /// <returns></returns>
        public static IList<int> FindManufacturersIdByName(string manufacturerName)
        {
            IList<int> manufacturersId = new List<int>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                var cmd = new SQLiteCommand("SELECT ManufacturerId FROM Manufacturers WHERE ManufacturerName = @ManufacturerName;", connection);

                cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerName);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    manufacturersId.Add(Convert.ToInt32(dataReader["ManufacturerId"]));
                }//while

                connection.Close();
            }//using

            return manufacturersId;       
        }//FindManufacturersIdByName
        




























////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблице Suppliers.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      
        /// <summary>
        /// Возвращает коллекцию из всех Supplier-ов.
        /// </summary>
        /// <returns></returns>
        public static IList<IContragent> FindSuppliers()
        {
            IList<IContragent> suppliers = new List<IContragent>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Suppliers;", connection);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Supplier supplier = new Supplier();
                    supplier.ContragentId = Convert.ToInt32(dataReader["ContragentId"]);
                    supplier.ContragentName = dataReader["ContragentName"] as string;
                    supplier.Code        = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    supplier.Entity      = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    supplier.ContactInfo = (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfoById(Convert.ToInt32(dataReader["ContactInfoId"])) : null;
                    supplier.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                    suppliers.Add(supplier);
                }//while

                connection.Close();
            }//using
            return suppliers;
        }//FindAllSuppliers

        /// <summary>
        /// Возвращает объект типа Contragent по заданному Id.
        /// </summary>
        /// <param name="supplierId">Id поставщика, которого надо найти.</param>
        /// <returns></returns>
        public static Supplier FindSuppliers(int supplierId)
        {
            Supplier supplier = new Supplier();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM  Suppliers WHERE ContragentId = @ContragentId;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ContragentId", supplierId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    supplier.ContragentId = Convert.ToInt32(dataReader["ContragentId"]);
                    supplier.ContragentName = dataReader["ContragentName"] as string;
                    supplier.Code = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    supplier.Entity = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    supplier.ContactInfo = (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfoById(Convert.ToInt32(dataReader["ContactInfoId"])) : null;
                    supplier.Description = (dataReader["Description"] == DBNull.Value) ? null : dataReader["Description"] as string;
                }//while

                connection.Close();
            }//using

            return supplier;
        }//FindSuppliers
        /// <summary>
        /// Возвращает объект Contragent, заполненный данными с таблицы Suppliers по заданному Id поставки. 
        /// </summary>
        /// <param name="saleId">Id поставки, по которой находятся данные о поставщике.</param>
        /// <returns></returns>
        public static Supplier FindSupplierByPurchaseId(int purchaseId)
        {
            Supplier supplier = new Supplier();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM Purchases AS p JOIN Suppliers AS s ON p.ContragentId = s.ContragentId "
                                   + "WHERE p.OperationId = @OperationId;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@OperationId", purchaseId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    supplier.ContragentId = Convert.ToInt32(dataReader["ContragentId"]);
                    supplier.ContragentName = dataReader["ContragentName"] as string;
                    supplier.Code = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    supplier.Entity = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    supplier.ContactInfo = (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfoById(Convert.ToInt32(dataReader["ContactInfoId"])) : null;
                    supplier.Description = (dataReader["Description"] == DBNull.Value) ? null : dataReader["Description"] as string;
                }//while

                connection.Close();
            }//using

            return supplier;
        }//FindSupplierByPurchaseId

        /// <summary>
        /// Возвращает объект Supplier найденный по заданному SupplierName, или null если такого объекта не найдено.
        /// </summary>
        /// <param name="SupplierName">имя Supplier-а, которого надо найти.</param>
        /// <returns></returns>
        public static IContragent FindSuppliers(string supplierName)
        {
            Supplier supplier = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM Suppliers WHERE ContragentName LIKE @ContragentName;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ContragentName", supplierName);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    supplier = new Supplier();
                    supplier.ContragentId = Convert.ToInt32(dataReader["ContragentId"]);
                    supplier.ContragentName = dataReader["ContragentName"] as string;
                    supplier.Code = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    supplier.Entity = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    supplier.ContactInfo = (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfoById(Convert.ToInt32(dataReader["ContactInfoId"])) : null;
                    supplier.Description = (dataReader["Description"] == DBNull.Value) ? null : dataReader["Description"] as string;
                }//while

                connection.Close();
            }//using

            return supplier;

        }//FindSuppliers

        /// <summary>
        /// Возвращает true если такой code уже есть в таблице Suppliers, иначе false.
        /// </summary>
        /// <param name="code">code наличие которого нужно проверить.</param>
        /// <returns></returns>
        public static bool IsSupplierCodeExist(string code)
        {
            bool isCodeExist = false;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Suppliers WHERE Code LIKE @Code;", connection);

                cmd.Parameters.AddWithValue("@Code", code);

                if (cmd.ExecuteScalar() != null)
                    isCodeExist = true;
                
                connection.Close();
            }//using
            return isCodeExist;
        }//IsSupplierCodeExist



























////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM Customers;";
                var cmd = new SQLiteCommand(query, connection);

                var dataReader = cmd.ExecuteReader();

                
                while (dataReader.Read())
                {
                    Customer customer = new Customer();
                    customer.ContragentId   = Convert.ToInt32(dataReader["ContragentId"]);
                    customer.ContragentName = dataReader["ContragentName"] as string;
                    customer.Code           = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    customer.Entity      = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    customer.ContactInfo = (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfoById(Convert.ToInt32(dataReader["ContactInfoId"])) : null;
                    customer.Description = (dataReader["Description"] == DBNull.Value) ? null : dataReader["Description"] as string;

                    customers.Add(customer);
                }//while

                connection.Close();
            }//using

            return customers;        
        }//FindCustomers
        /// <summary>
        /// Возвращает объект Customer найденный по заданному customerName, или null если такого объекта не найдено.
        /// </summary>
        /// <param name="customerName">имя Customer-а, которого надо найти.</param>
        /// <returns></returns>
        public static Customer FindCustomers(string customerName)
        {
            Customer customer = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM Customers WHERE ContragentName LIKE @ContragentName;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ContragentName", customerName);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    customer = new Customer();
                    customer.ContragentId = Convert.ToInt32(dataReader["ContragentId"]);
                    customer.ContragentName = dataReader["ContragentName"] as string;
                    customer.Code        = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    customer.Entity      = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    customer.ContactInfo = (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfoById(Convert.ToInt32(dataReader["ContactInfoId"])) : null;
                    customer.Description = (dataReader["Description"] == DBNull.Value) ? null : dataReader["Description"] as string;
                }//while

                connection.Close();
            }//using

            return customer; 

        }//FindCustomers

        /// <summary>
        /// Возвращает объект типа Customer найденный по заданному Id.
        /// </summary>
        /// <param name="customerId">Id клиента, которого надо найти.</param>
        /// <returns></returns>
        public static IContragent FindCustomers(int customerId)
        {
            Customer customer = new Customer();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                    customer.Code   = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    customer.Entity = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    customer.ContactInfo = (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfoById(Convert.ToInt32(dataReader["ContactInfoId"])) : null;
                    customer.Description = (dataReader["Description"] == DBNull.Value) ? null : dataReader["Description"] as string;
                }//while

                connection.Close();
            }//using

            return customer;        
        }//FindCustomerByName



















////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
             
        #region Поиск по таблице Purchases и Sales.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static List<IOperation> FindPurchases(int supplierId)
        {
            List<IOperation> purchases = new List<IOperation>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
            }//using

            return purchases;
        }//FindPurchases
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cust">STUB</param>
        /// <returns></returns>
        public static List<IOperation> FindSales(int customerId, Customer cust)
        {
            List<IOperation> salesList = new List<IOperation>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                    salesList.Add(CreateSale(dataReader));

                connection.Close();
            }//using

            return salesList;
        }//FindSales


        /// <summary>
        /// Возвращает объект класса Operation, найденный по заданному Id. 
        /// </summary>
        /// <param name="saleId">Id прихода информацию о котором нужно вернуть.</param>
        /// <returns></returns>
        public static Purchase FindPurchase(int purchaseId)
        {
            Purchase purchase = new Purchase();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Purchases WHERE OperationId = @OperationId;", connection);

                cmd.Parameters.AddWithValue("@OperationId", purchaseId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    purchase.OperationId = Convert.ToInt32(dataReader["OperationId"]);
                    purchase.Employee = (dataReader["EmployeeId"] != DBNull.Value) ? FindEmployees(Convert.ToInt32(dataReader["EmployeeId"])) : null;
                    purchase.Contragent = FindSuppliers(Convert.ToInt32(dataReader["ContragentId"]));
                    purchase.ContragentEmployee = dataReader["ContragentEmployee"] as string;
                    //Переводим кол-во секунд Utc в DateTime.
                    TimeSpan ts = TimeSpan.FromSeconds(Convert.ToInt32(dataReader["OperationDate"]));
                    DateTime purchaseDate = new DateTime(1970, 1, 1);
                    purchaseDate += ts;
                    purchase.OperationDate = purchaseDate;

                }//while
                connection.Close();
            }//using

            return purchase;
        }//FindPurchase


        /// <summary>
        /// Возвращает список всех операций производимых с заданным товаром.
        /// </summary>
        /// <param name="sparePartId">Ид искомого товара.</param>
        /// <returns></returns>
        public static List<IOperation> FindOperations(int sparePartId)
        {
            List<IOperation> operations = new List<IOperation>();

            List<Purchase> purchList = FindPurchases(sparePartId, null);
            foreach (Purchase purch in purchList)
                operations.Add(purch); 
                
            List<Sale> salesList = FindSales(sparePartId);
            foreach (Sale sale in salesList)
                operations.Add(sale); 
            
                        
            return operations;
        }//FindOperations

        public static List<Purchase> FindPurchases(int sparePartId, SparePart spr)
        {
            /*ERROR!!! лишний пар-р SparePart.*/
            List<Purchase> purchases = new List<Purchase>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Purchases "
                                   + "WHERE OperationId IN (SELECT OperationId FROM PurchaseDetails "
                                   + "WHERE SparePartId = @SparePartId);";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        purchases.Add(CreatePurchase(dataReader));
                }//using dataReader

                connection.Close();
            }//using
            
            return purchases;
        }//FindPurchases
        public static List<Sale> FindSales(int sparePartId)
        {
            List<Sale> salesList = new List<Sale>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(OperationDate, 'unixepoch') as OD "
                                   + "FROM Sales "
                                   + "WHERE OperationId IN (SELECT OperationId FROM SaleDetails "
                                   + "WHERE SparePartId = @SparePartId);";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        salesList.Add(CreateSale(dataReader));                    
                }//using dataReader

                connection.Close();
            }//using

            return salesList;
        }//FindSales



        /// <summary>
        /// Возвращает объект типа IOperation созданный из переданного SQLiteDataReader.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private static Purchase CreatePurchase(SQLiteDataReader dataReader)
        {
            return new Purchase
            (
                operationId        : Convert.ToInt32(dataReader["OperationId"]),
                employee           : (dataReader["EmployeeId"] != DBNull.Value) ? FindEmployees(Convert.ToInt32(dataReader["EmployeeId"])) : null,
                contragent         : FindSuppliers(Convert.ToInt32(dataReader["ContragentId"])),
                contragentEmployee : dataReader["ContragentEmployee"] as string,
                operationDate      : Convert.ToDateTime(dataReader["OD"]),
                description        : dataReader["Description"] as string
            );
        }//CreatePurchase

        private static Sale CreateSale(SQLiteDataReader dataReader)
        {
            return new Sale
            (
                operationId        : Convert.ToInt32(dataReader["OperationId"]),
                employee           : (dataReader["EmployeeId"] != DBNull.Value) ? FindEmployees(Convert.ToInt32(dataReader["EmployeeId"])) : null,
                contragent         : FindSuppliers(Convert.ToInt32(dataReader["ContragentId"])),
                contragentEmployee : dataReader["ContragentEmployee"] as string,
                operationDate      : Convert.ToDateTime(dataReader["OD"]),
                description        : dataReader["Description"] as string
            );
        }//CreateSale




        /// <summary>
        /// Возвращает детали операции для заданного прихода.
        /// </summary>
        /// <param name="sale">Приход.</param>
        /// <returns></returns>
        public static List<OperationDetails> FindPurchaseDetails(Purchase purchase)
        {
            List<OperationDetails> operDetList = new List<OperationDetails>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                    }//using dataReader
                }//using cmd

                connection.Close();
            }//using

            return operDetList;
        }//FindPurchaseDetails

        /// <summary>
        /// Возвращает детали операции для заданного расхода.
        /// </summary>
        /// <param name="sale">Приход.</param>
        /// <returns></returns>
        public static List<OperationDetails> FindSaleDetails(Sale sale)
        {
            List<OperationDetails> operDetList = new List<OperationDetails>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                            operDetList.Add(CreateOperationDetails(dataReader, sale));
                    }//using dataReader
                }//using cmd

                connection.Close();
            }//using

            return operDetList;
        }//FindSaleDetails



        private static OperationDetails CreateOperationDetails(SQLiteDataReader dataReader, SparePart sparePart)
        {
            return new OperationDetails
            (
                sparePart : sparePart,
                operation : FindPurchase(Convert.ToInt32(dataReader["OperationId"])),
                count     : Convert.ToSingle(dataReader["Count"]),
                price     : Convert.ToSingle(dataReader["Price"])
            );
        }//CreateOperationDetails

        private static OperationDetails CreateOperationDetails(SQLiteDataReader dataReader, IOperation operat)
        {
            return new OperationDetails
            (
                sparePart   : FindSparePart(Convert.ToInt32(dataReader["SparePartId"])),
                operation   : operat,
                count       : Convert.ToSingle(dataReader["Count"]),
                price       : Convert.ToSingle(dataReader["Price"])
            );
        }//CreateOperationDetails

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблице Employees.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает список из объектов типа Employee, состоящий из всех сотрудников.
        /// </summary>
        /// <returns></returns>
        public static List<Employee> FindEmployees()
        {
            List<Employee> employeesList = new List<Employee>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                
                const string query = "SELECT date(HireDate, \"Unixepoch\") AS 'HD', date(DismissalDate, \"Unixepoch\") AS 'DD', * "
                                   + "FROM Employees;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        employeesList.Add(CreateEmployee(dataReader));
                }//using dataReader

                connection.Close();
            }//using

            return employeesList;
        }//FindAllEmployees
        /// <summary>
        /// Возвращает объект типа Employee, найденный по заданному Id.
        /// </summary>
        /// <param name="employeeId">Ид сотрудника, которого надо найти.</param>
        /// <returns></returns>
        public static Employee FindEmployees(int employeeId)
        {
            Employee employee = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT date(HireDate, \"Unixepoch\") AS 'HD', date(DismissalDate, \"Unixepoch\") AS 'DD', * "
                                   + "FROM Employees WHERE EmployeeId = @EmployeeId;";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        employee = CreateEmployee(dataReader);
                }//using dataReader

                connection.Close();
            }//using

            return employee;
        }//FindEmployeeById

        public static IList<Employee> FindEmployees(string lastName, string firstName = null)
        {
            IList<Employee> employees = new List<Employee>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT date(HireDate, \"Unixepoch\") AS 'HD', date(DismissalDate, \"Unixepoch\") AS 'DD', * "
                                   + "FROM Employees WHERE LastName LIKE @LastName AND FirstName ;";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@LastName", lastName);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        employees.Add(CreateEmployee(dataReader));
                }//using dataReader

                connection.Close();
            }//using

            return employees;       
        }//FindEmployees






        /// <summary>
        /// Возвращает объект типа Employee созданный из данныз переданного SQLiteDataReader.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private static Employee CreateEmployee(SQLiteDataReader dataReader)
        { 
            return new Employee
            (
                employeeId     : Convert.ToInt32(dataReader["EmployeeId"]),
                lastName       : dataReader["LastName"] as string,
                firstName      : dataReader["FirstName"] as string,
                middleName     : dataReader["MiddleName"] as string,
                birthDate      : (dataReader["BirthDate"] != DBNull.Value)     ? Convert.ToDateTime(dataReader["BirthDate"]) : (DateTime?)null,
                hireDate       : (dataReader["HireDate"] != DBNull.Value) ? Convert.ToDateTime(dataReader["HD"]) : (DateTime?)null,
                dismissalDate  : (dataReader["DismissalDate"] != DBNull.Value) ? Convert.ToDateTime(dataReader["DD"]) : (DateTime?)null,                
                photo          : dataReader["Photo"] as string,
                note           : dataReader["Note"] as string,
                passportNum    : dataReader["PassportNum"] as string,
                title          : dataReader["Title"] as string,
                accessLayer    : dataReader["AccessLayer"] as string,
                login          : dataReader["Login"] as string,
                password       : dataReader["Password"] as string               
            );
        }//CreateEmployee



////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблице ContactInfo
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает объект типа ContactInfo заполненный по заданному Id.
        /// </summary>
        /// <param name="contactInfoId">Id по которому находится информация.</param>
        /// <returns></returns>
        public static ContactInfo FindContactInfoById(int contactInfoId)
        {
            ContactInfo contactInfo = new ContactInfo();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM ContactInfo WHERE ContactInfoId = @ContactInfoId;";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@ContactInfoId", contactInfoId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        contactInfo = CreateContactInfo(dataReader);
                }//using dataReader

                connection.Close();
            }//using

            return contactInfo;
        }//FindContactInfoById

        /// <summary>
        /// Возвращает объект типа ContactInfo, найденный по заданному Id сотрудника, или null если ничего не найдено.
        /// </summary>
        /// <param name="employeeId">Id сотрудника.</param>
        /// <returns></returns>
        public static ContactInfo FindContactInfo(int employeeId)
        {
            ContactInfo contactInfo = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT ci.* FROM Employees as e "
                                   + "JOIN ContactInfo as ci "
                                   + "ON e.ContactInfoId = ci.ContactInfoId "
                                   + "WHERE EmployeeId = @EmployeeId;";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        contactInfo = CreateContactInfo(dataReader);
                    }//while 
                }//using dataReader

                connection.Close();
            }//using

            return contactInfo;

        }//FindContactInfo











        /// <summary>
        /// Возвращает объект типа ContactInfo, заполненный инф-цией из переданного SQLiteDataReader.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private static ContactInfo CreateContactInfo(SQLiteDataReader dataReader)
        {
            return new ContactInfo
            (
                contactInfoId : Convert.ToInt32(dataReader["ContactInfoId"]),
                country       : dataReader["Country"]  as string,
                region        : dataReader["Region"]   as string,
                city          : dataReader["City"]     as string,
                street        : dataReader["Street"]   as string,
                house         : dataReader["House"]    as string,
                room          : dataReader["Room"]     as string,
                phone         : dataReader["Phone"]    as string,
                extPhone      : dataReader["ExtPhone"] as string,
                email         : dataReader["Email"]    as string,
                website       : dataReader["Website"]  as string                                
            );
        }//CreateContactInfo


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion


















////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion


































        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region ************Поиск совпадений по БД.*****************************************************************************
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает список размера не более limit, состоящий из запчастей в наличии, чьи Articul имеют совпадение с параметром articul.
        /// </summary>
        /// <param name="articul">Артикул по которому ищутся совпадения.</param>
        /// <param name="limit">Максимально возможное кол-во эл-тов.</param>
        /// <param name="withoutIDs">Список Id товара который не должен входить в результирующий список.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSparePartsAvaliablityByArticul(string articul, int limit, IList<int> withoutIDs)
        {
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(connection);

                StringBuilder notIn = new StringBuilder();
                if (withoutIDs.Count != 0)
                {
                    for (int i = 0; i < withoutIDs.Count; ++i)
                    {
                        notIn.Append("@NotIn" + i + ", ");
                        cmd.Parameters.AddWithValue("@NotIn" + i, withoutIDs[i]);
                    }
                    notIn.Remove(notIn.Length - 2, 2);
                }//if

                var query = "SELECT av.SparePartId, * FROM Avaliability AS av JOIN SpareParts AS sp "
                          + "ON av.SparePartId = sp.SparePartId AND ToLower(sp.Articul) LIKE @Articul AND av.SparePartId NOT IN(" + notIn + ")" 
                          + "GROUP BY av.SparePartId LIMIT @Limit;";

                cmd.Parameters.AddWithValue("@Articul", articul.ToLower() + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);

                cmd.CommandText = query;

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = CreateSparePart(dataReader);

                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;

        }//SearchSparePartsByArticul       

        /// <summary>
        /// Возвращает список размера не более limit, состоящий из запчастей в Наличии, чьи Title имеют совпадение с параметром title. 
        /// </summary>
        /// <param name="title">Строка по которой ищутся совпадения.</param>
        /// <param name="limit">Максимально возможное кол-во эл-тов.</param>
        /// <param name="withoutIDs">Список Id товара который не должен входить в результирующий список.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSparePartsAvaliablityByTitle(string title, int limit, IList<int> withoutIDs)
        {
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(connection);

                StringBuilder notIn = new StringBuilder();
                //Формируем строку с Id товара который пропускается при поиске.
                if (withoutIDs.Count != 0)
                {
                    for (int i = 0; i < withoutIDs.Count; ++i)
                    {
                        notIn.Append("@NotIn" + i + ", ");
                        cmd.Parameters.AddWithValue("@NotIn" + i, withoutIDs[i]);
                    }
                    notIn.Remove(notIn.Length - 2, 2); //убираем последний добавленный пробел и запятую ", ".
                }//if

                var query = "SELECT av.SparePartId, * FROM Avaliability AS av JOIN SpareParts AS sp "
                          + "ON av.SparePartId = sp.SparePartId AND ToLower(sp.Title) LIKE @Title AND av.SparePartId NOT IN(" + notIn + ")"
                          + "GROUP BY av.SparePartId LIMIT @Limit;";

                cmd.Parameters.AddWithValue("@Title", title.ToLower() + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);

                cmd.CommandText = query;

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = CreateSparePart(dataReader);
                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;
        }//SearchSparePartsAvaliablityByTitle        
                        

        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из заданного кол-ва эл-тов найдейнных по заданному параметру. 
        /// </summary>
        /// <param name="titleOrArticulOrManuf">Title или Articul или Manufacturer совпадение с которым нужно искать.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSpByTitleOrArticulOrManufacturerToDisplay(string titleOrArticulOrManuf)
        {
            return SearchSpByTitleOrArticulOrManufacturerToDisplay(titleOrArticulOrManuf, -1);
        }//SearchByTitleOrArticul

        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из заданного кол-ва эл-тов найдейнных по заданному параметру. 
        /// </summary>
        /// <param name="titleOrArticulOrManuf">Title или Articul или Manufacturer совпадение с которым нужно искать.</param>
        /// <param name="limit">Ограничение по максимальному кол-ву эл-тов.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSpByTitleOrArticulOrManufacturerToDisplay(string titleOrArticulOrManuf, int limit)
        {
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT SparePartId FROM SpareParts AS sp LEFT JOIN Manufacturers AS m "
                                   + "ON sp.ManufacturerId = m.ManufacturerId "                              
                                   + "WHERE ToLower(sp.Articul) LIKE @TitleOrArticul OR ToLower(sp.Title) LIKE @TitleOrArticul "
                                   + "OR ToLower(m.ManufacturerName) LIKE @TitleOrArticul LIMIT @limit;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@TitleOrArticul", "%" + titleOrArticulOrManuf.ToLower() + "%");
                cmd.Parameters.AddWithValue("@limit", limit);


                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    spareParts.Add(FindSparePart(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;
        }//SearchByTitleOrArticul
        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из заданного кол-ва эл-тов в Наличии, найдейнных по заданному параметру.  
        /// </summary>
        /// <param name="titleOrArticulOrManuf">Title или Articul или Manufacturer совпадение с которым нужно искать.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSpAvaliabilityByTitleOrArticulOrManufacturerToDisplay(string titleOrArticulOrManuf)
        {
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query =
                   "SELECT DISTINCT sp.SparePartId FROM SpareParts AS sp JOIN Avaliability AS a ON sp.SparePartId = a.SparePartId "
                 + "LEFT JOIN Manufacturers AS m ON m.ManufacturerId = sp.ManufacturerId "
                 + "WHERE ToLower(sp.Articul) LIKE @TitleOrArticul OR ToLower(sp.Title) LIKE @TitleOrArticul "
                 + "OR ToLower(m.ManufacturerName) LIKE @TitleOrArticul;";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@TitleOrArticul", "%" + titleOrArticulOrManuf.ToLower() + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    spareParts.Add(FindSparePart(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;

        }//SearchSpAvaliabilityByTitleOrArticulOrManufacturerToDispla
        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из заданного кол-ва эл-тов в Наличии, найдейнных по заданному параметру.  
        /// </summary>
        /// <param name="titleOrArticulOrManuf">Title или Articul или Manufacturer совпадение с которым нужно искать.</param>
        /// <param name="limit">Ограничение по максимальному кол-ву эл-тов.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSpAvaliabilityByTitleOrArticulOrManufacturerToDisplay(string titleOrArticulOrManuf, int limit)
        {
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query =
                   "SELECT DISTINCT sp.SparePartId FROM SpareParts AS sp JOIN Avaliability AS a ON sp.SparePartId = a.SparePartId "
                 + "LEFT JOIN Manufacturers AS m ON m.ManufacturerId = sp.ManufacturerId "
                 + "WHERE ToLower(sp.Articul) LIKE @TitleOrArticul OR ToLower(sp.Title) LIKE @TitleOrArticul "
                 + "OR ToLower(m.ManufacturerName) LIKE @TitleOrArticul LIMIT @Limit;";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@TitleOrArticul", "%" + titleOrArticulOrManuf.ToLower() + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);


                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    spareParts.Add(FindSparePart(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;

        }//SearchSpAvaliabilityByTitleOrArticulOrManufacturerToDispla
                        
        
        /// <summary>
        /// Возвращает список размера не более limit, состоящий из запчастей чьи Title имеют совпадение с параметром title. 
        /// </summary>
        /// <param name="title">Строка по которой ищутся совпадения.</param>
        /// <param name="limit">Максимально возможное кол-во эл-тов.</param>
        /// <param name="withoutIDs">Список Id товара который не должен входить в результирующий список.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSparePartsByTitle(string title, int limit, IList<int> withoutIDs)
        {
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(connection);

                StringBuilder notIn = new StringBuilder();
                if (withoutIDs.Count != 0)
                {
                    for (int i = 0; i < withoutIDs.Count; ++i)
                    {
                        notIn.Append("@NotIn" + i + ", ");
                        cmd.Parameters.AddWithValue("@NotIn" + i, withoutIDs[i]);
                    }
                    notIn.Remove(notIn.Length - 2, 2);
                }

                var query = "SELECT * FROM SpareParts WHERE ToLower(Title) LIKE @Title AND SparePartId NOT IN(" + notIn + ") LIMIT @Limit;";
                
                cmd.Parameters.AddWithValue("@Title", title.ToLower() + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);

                cmd.CommandText = query;
                    
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = CreateSparePart(dataReader);                    
                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;
        
        }//SearchSparePartsByTitle
                
        /// <summary>
        /// Возвращает список всех SparePart размером не более limit, найденных по совпадению с заданным артикулом.
        /// </summary>
        /// <param name="articul">Артикул по которому ищутся совпадения.</param>
        /// <param name="limit">Максимально возможное кол-во эл-тов.</param>
        /// <param name="withoutIDs">Список Id товара который не должен входить в результирующий список.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSparePartsByArticul(string articul, int limit, IList<int> withoutIDs)
        {
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(connection);

                StringBuilder notIn = new StringBuilder();
                if (withoutIDs.Count != 0)
                {
                    for (int i = 0; i < withoutIDs.Count; ++i)
                    {
                        notIn.Append("@NotIn" + i + ", ");
                        cmd.Parameters.AddWithValue("@NotIn" + i, withoutIDs[i]);
                    }
                    notIn.Remove(notIn.Length - 2, 2);
                }

                var query = "SELECT * FROM  SpareParts WHERE ToLower(Articul) LIKE @Articul AND SparePartId NOT IN(" + notIn + ") LIMIT @Limit;";

                cmd.Parameters.AddWithValue("@Articul", articul.ToLower() + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);

                cmd.CommandText = query;

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = CreateSparePart(dataReader);

                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;

        }//SearchSparePartsByArticul           








        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion





        #region Вспомогательные методы.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Метод регистрирующий в базе User-Defined Functions.
        /// </summary>
        public static void RegistrateUDFs()
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                //SQLiteCommand cmd = new SQLiteCommand("PRAGMA integrity_check", connection);
                //cmd.ExecuteNonQuery();  

                SQLiteFunction.RegisterFunction(typeof(LowerRegisterConverter));

                connection.Close();
            }//using
        }//RegistrateUDFs
        /// <summary>
        /// Возвращает объект SparePart созданный из переданного dataReader.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private static SparePart CreateSparePart(SQLiteDataReader dataReader)
        {
            return new SparePart
            (
                sparePartId    : Convert.ToInt32(dataReader["SparePartId"]),
                photo          : dataReader["Photo"] as string,
                articul        : dataReader["Articul"] as string,
                title          : dataReader["Title"] as string,
                description    : (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string,
                manufacturer   : (dataReader["ManufacturerId"] == DBNull.Value) ? null : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])), 
                measureUnit    : dataReader["MeasureUnit"] as string             
            );     
        }//CreateSparePart


        /// <summary>
        /// Коннект к базе данных.
        /// </summary>
        /// <param name="name">Имя подключения</param>
        /// <returns></returns>
        static private System.Data.Common.DbConnection GetDatabaseConnection(string name)
        {
            var settings = System.Configuration.ConfigurationManager.ConnectionStrings[name];
            var factory = System.Data.Common.DbProviderFactories.GetFactory(settings.ProviderName);

            var conn = factory.CreateConnection();
            conn.ConnectionString = settings.ConnectionString;

            return conn;
        }//GetDatabaseConnection





        //public static IList<Operation> FindPurchasesByParameters(Operation sale)
        //{
        //    IList<Operation> operDetList = new List<Operation>();

        //    using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
        //    {
        //        connection.Open();

        //        const string query = "SELECT * FROM Purchases WHERE PurchaseId = @PurchaseId AND ";
        //        SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId AND sp.Articul LIKE @Articul", connection);

        //        cmd.Parameters.AddWithValue("@Articul", articul + "%");

        //        var dataReader = cmd.ExecuteReader();
        //        while (dataReader.Read())
        //        {
        //            Operation sale = new Operation();


        //            operDetList.Add(sale);
        //        }//while
        //        connection.Close();
        //    }//using

        //    return operDetList;
        //}//FindPurchasesByParameters























/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

    }//PartsDAL

    [SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "ToLower")]
    class LowerRegisterConverter : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            string initialString = (args[0] as string);
            return (initialString != null) ? initialString.ToLower() : null;
        }//Invoke
    }//LowerRegisterConverter

    

}//namespace

/*Задачи*/