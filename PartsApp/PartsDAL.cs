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
        /// <param name="sparePart">Запись добавляемая в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void AddSparePartAvaliability(SparePart sparePart, SQLiteCommand cmd)
        {
            var query = "INSERT INTO Avaliability VALUES (@SparePartId, @PurchaseId, @Price, @Markup, @StorageAdress, @Count);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();
            
            cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);
            cmd.Parameters.AddWithValue("@PurchaseId", sparePart.PurchaseId);
            cmd.Parameters.AddWithValue("@Price", sparePart.Price);
            cmd.Parameters.AddWithValue("@Markup", sparePart.Markup);
            cmd.Parameters.AddWithValue("@StorageAdress", sparePart.StorageAdress);
            cmd.Parameters.AddWithValue("@Count", (sparePart.Count == 0) ? sparePart.VirtCount : sparePart.Count);
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
            string query = "UPDATE Avaliability SET Count = @Count WHERE SparePartId = @SparePartId AND PurchaseId = @PurchaseId;";
            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);
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
            const string query = "UPDATE Avaliability SET Markup = @Markup WHERE SparePartId = @SparePartId AND PurchaseId = @PurchaseId;";
            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@Markup", markup);
            cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

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
            const string query = "DELETE FROM Avaliability WHERE SparePartId = @SparePartId AND PurchaseId = @PurchaseId;";
            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

            cmd.ExecuteNonQuery();
        }//DeleteSparePartAvaliability
        /// <summary>
        /// Уменьшает кол-во или удаляет запись из таблицы Avaliability.
        /// </summary>
        /// <param name="sparePart">уменьшаемый или удаляемый товар</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void SaleSparePartAvaliability(SparePart sparePart, SQLiteCommand cmd)
        {
            //Узнаем количество данного товара в наличии.
            double spAvaliabilityCount = FindSparePartAvaliabilityCount(sparePart.SparePartId, sparePart.PurchaseId);//FindSparePartAvaliability(sparePart.SparePartId, sparePart.PurchaseId, cmd);
            //В зависимости от того на осн. или вирт. складе находится товар, узнаем его количестов. 
            double saleSpCount = (sparePart.VirtCount == 0) ? sparePart.Count : sparePart.VirtCount;

            //Если кол-во продаваемого товара с данного прихода равно всему кол-во товара данной записи, удаляем из таблицы эту запись, иначе обновляем кол-во товара в базе.
            if (spAvaliabilityCount == saleSpCount) 
                DeleteSparePartAvaliability(sparePart.SparePartId, sparePart.PurchaseId, cmd);
            else
                UpdateSparePartСountAvaliability(sparePart.SparePartId, sparePart.PurchaseId, spAvaliabilityCount - saleSpCount, cmd);

        }//SaleSparePartAvaliability






















        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы SpareParts.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void AddSparePart(SparePart sparePart)
        {
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
                cmd.Parameters.AddWithValue("@ManufacturerId", sparePart.ManufacturerId);
                cmd.Parameters.AddWithValue("@MeasureUnit", sparePart.MeasureUnit);

                cmd.ExecuteNonQuery();
                
                connection.Close();
            }//using
        }//AddSparePart
        /// <summary>
        /// Метод модификации записи с заданным Id.
        /// </summary>
        /// <param name="sparePart">Товар инф-ция о котором модифицируется.</param>
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
                cmd.Parameters.AddWithValue("@ManufacturerId", sparePart.ManufacturerId);
                cmd.Parameters.AddWithValue("@MeasureUnit", sparePart.MeasureUnit);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using
        
        }//UpdateSparePart



























//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Suppliers.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void AddSupplier(Supplier supplier)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                //Вставляем запись в табл. "Supplier"
                var cmd = new SQLiteCommand("INSERT INTO Suppliers(SupplierName, Code, Entity, ContactInfoId, Description) VALUES (@SupplierName, @Code, @Entity, @ContactInfoId, @Description);", connection);

                cmd.Parameters.AddWithValue("@SupplierName",  supplier.ContragentName);
                cmd.Parameters.AddWithValue("@Code",          supplier.Code);
                cmd.Parameters.AddWithValue("@Entity",        supplier.Entity);
                cmd.Parameters.AddWithValue("@ContactInfoId", (supplier.ContactInfo != null) ? supplier.ContactInfo.ContactInfoId : (int?)null);
                cmd.Parameters.AddWithValue("@Description",   supplier.Description);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using

        }//AddSupplier

































//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Customers.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void AddCustomer(Customer customer)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "INSERT INTO Customers(CustomerName, Code, Entity, ContactInfoId, Description) " 
                                   + "VALUES (@CustomerName, @Code, @Entity, @ContactInfoId, @Description);";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@CustomerName", customer.ContragentName);
                cmd.Parameters.AddWithValue("@Code", customer.Code);
                cmd.Parameters.AddWithValue("@Entity", customer.Entity);
                cmd.Parameters.AddWithValue("@ContactInfoId", (customer.ContactInfo != null) ? customer.ContactInfo.ContactInfoId : (int?)null);
                cmd.Parameters.AddWithValue("@Description", customer.Description);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using

        }//AddCustomer



























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
        public static int AddContactInfo(ContactInfo contactInfo)
        {
            int id = 0;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                string query = String.Format("INSERT INTO ContactInfo (Country, Region, City, Street, House, Room, Phone, ExtPhone, Website, Email)"
                                           + "VALUES (@Country, @Region, @City, @Street, @House, @Room, @Phone, @ExtPhone, @Website, @Email);"
                                           + "SELECT ContactInfoId FROM ContactInfo WHERE rowid = last_insert_rowid();");
                //Вставляем запись в табл. "Manufacturer"
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@Country",   contactInfo.Country);
                cmd.Parameters.AddWithValue("@Region",    contactInfo.Region);
                cmd.Parameters.AddWithValue("@City",      contactInfo.City);
                cmd.Parameters.AddWithValue("@Street",    contactInfo.Street);
                cmd.Parameters.AddWithValue("@House",     contactInfo.House);
                cmd.Parameters.AddWithValue("@Room",      contactInfo.Room);
                cmd.Parameters.AddWithValue("@Phone",     contactInfo.Phone);
                cmd.Parameters.AddWithValue("@ExtPhone",  contactInfo.ExtPhone);
                cmd.Parameters.AddWithValue("@Website",   contactInfo.Website);
                cmd.Parameters.AddWithValue("@Email",     contactInfo.Email);

                id = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }//using
            return id;
        }//AddContactInfo





















//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Purchase.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Осуществляет полный цикл приходования товара, вставляя записи в таблицы Purchases, Avaliability и PurchaseDetails.
        /// Возвращает Id вставленной записи в табл. Purchase.
        /// </summary>
        /// <param name="sale">Информация о приходе.</param>
        /// <returns></returns>
        public static int AddPurchase(Purchase purchase)
        {
            int purchaseId = 0;
            string message = null;
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                
                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        { 
                            //вставляем запись в таблицу Purchase.
                            purchaseId = AddPurchase(purchase, cmd);
                            //вставляем записи в PurchaseDetails и Avaliability.
                            foreach (SparePart sp in purchase.OperationDetails)
                            {
                                sp.PurchaseId = purchaseId;                                
                                AddPurchaseDetail(sp, cmd);
                                AddSparePartAvaliability(sp, cmd);
                            }//foreach

                            trans.Commit();                        
                        }//try
                        catch(Exception ex)
                        {
                            message = ex.Message;
                            trans.Rollback();                        
                        }//catch
                    }//using cmd
                }//using transaction

                connection.Close();
            }//using connectio
            if (message != null) throw new Exception(message);

            return purchaseId;    
        }//AddPurchase

        /// <summary>
        /// Возвращает Id вставленной записи в таблицу Purchases.
        /// </summary>
        /// <param name="sale">Приход который нужно добавить в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        /// <returns></returns>
        private static int AddPurchase(Purchase purchase, SQLiteCommand cmd)
        {
            int purchaseId = 0;
            
            string query = String.Format("INSERT INTO Purchases (EmployeeID, SupplierId, SupplierEmployee, PurchaseDate, Description)"
                                       + "VALUES (@EmployeeID, @SupplierId, @SupplierEmployee, strftime('%s', @PurchaseDate), @Description);"
                                       + "SELECT PurchaseId FROM Purchases WHERE rowid = last_insert_rowid();");

            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@EmployeeID", purchase.Employee.EmployeeId);
            cmd.Parameters.AddWithValue("@SupplierId", purchase.Contragent.ContragentId);
            cmd.Parameters.AddWithValue("@SupplierEmployee", purchase.ContragentEmployee);

            cmd.Parameters.AddWithValue("@Description", purchase.Description);
            cmd.Parameters.AddWithValue("@PurchaseDate", purchase.OperationDate);

            purchaseId = Convert.ToInt32(cmd.ExecuteScalar());     
                   
            return purchaseId;
        }//AddPurchase

        #region Модификация таблицы PurchaseDetails
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Добавляет запись в таблицу PurchaseDetails.
        /// </summary>
        /// <param name="purchaseDetail">Запись добавляемая в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void AddPurchaseDetail(SparePart purchaseDetail, SQLiteCommand cmd)
        {
            string query = "INSERT INTO PurchaseDetails VALUES (@PurchaseId, @SparePartId, @Price, @Quantity);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@PurchaseId", purchaseDetail.PurchaseId);
            cmd.Parameters.AddWithValue("@SparePartId", purchaseDetail.SparePartId);
            cmd.Parameters.AddWithValue("@Price", purchaseDetail.Price);
            cmd.Parameters.AddWithValue("@Quantity", purchaseDetail.Count);

            cmd.ExecuteNonQuery();
        }//AddPurchaseDetail

















        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion


















//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        
        #region Модификация таблицы Sales.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*!!! Возможно можно убрать параметр IList<SparePart> передавая его св-вом объекта класса Purchase, PurchaseDetails. Надобность класса PurchaseDetail вообще под вопросом, ведь его можно спокойно заменить объектом уже созданного класса SparePart либо же класс SparePart нужно модифицировать в сторону разбиения на большее кол-во классов!*/
        /// <summary>
        /// Осуществляет полный цикл продажи товара, вставляя записи в таблицы Sales, Avaliability и SaleDetails.
        /// Возвращает Id вставленной записи в табл. Sale.
        /// </summary>
        /// <param name="spareParts">Список продаваемого товара.</param>
        /// <param name="sale">Информация о продаже.</param>
        /// <returns></returns>
        public static int AddSale(IList<SparePart> spareParts, IList<SparePart> extSpareParts, Sale sale)
        {
            int saleId = 0;
            string message = null;
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //вставляем запись в таблицу Sales.
                            saleId = AddSale(sale, cmd);
                            //вставляем записи в SaleDetails.
                            foreach (SparePart sp in extSpareParts)
                                SaleSparePartAvaliability(sp, cmd);
                            // и модифицируем Avaliability.
                            foreach (SparePart sp in spareParts)
                                AddSaleDetail(saleId, sp.SparePartId, (double)sp.Price, sp.Count, cmd);

                            trans.Commit();
                        }//try
                        catch (Exception ex)
                        {
                            message = ex.Message;
                            trans.Rollback();
                        }//catch
                    }//using cmd
                }//using transaction

                connection.Close();
            }//using connection
            if (message != null) throw new Exception(message);

            return saleId;
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

            var query = String.Format("INSERT INTO Sales (EmployeeID, CustomerId, CustomerEmployee, SaleDate, Description) "
                                    + "VALUES (@EmployeeID, @CustomerId, @CustomerEmployee, strftime('%s', @SaleDate), @Description); "
                                    + "SELECT SaleId FROM Sales WHERE rowid = last_insert_rowid();");

            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@EmployeeID", sale.Employee.EmployeeId);
            cmd.Parameters.AddWithValue("@CustomerId", sale.Contragent.ContragentId);
            cmd.Parameters.AddWithValue("@CustomerEmployee", sale.ContragentEmployee);
            cmd.Parameters.AddWithValue("@Description", sale.Description);           

            cmd.Parameters.AddWithValue("@SaleDate", sale.OperationDate);

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
        /// <param name="sellingPrice">Отпускная цена товара</param>
        /// <param name="quantity">Кол-во товара</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void AddSaleDetail(int saleId, int sparePartId, double sellingPrice, double quantity, SQLiteCommand cmd)
        {
            string query = "INSERT INTO SaleDetails VALUES (@SaleId, @SparePartId, @Quantity, @SellingPrice);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@SaleId", saleId);
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
            
            cmd.ExecuteNonQuery();
        }//AddSaleDetail




















//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion












        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Employees.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Добавляет объект типа Employee в таблицу Employees.
        /// </summary>
        /// <param name="employee">объект типа Employee добавляемый в БД.</param>
        public static void AddEmployee(Employee employee)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "INSERT INTO Employees (LastName, FirstName, MiddleName, BirthDate, HireDate, "
                                   + "ContactInfoId, Photo, Note, PassportNum, Title, AccessLayer, Login, Password) "
                                   + "VALUES (@LastName, @FirstName, @MiddleName, @BirthDate, @HireDate, @ContactInfoId, "
                                   + "@Photo, @Note, @PassportNum, @Title, @AccessLayer, @Login, @Password);";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                cmd.Parameters.AddWithValue("@BirthDate", (employee.BirthDate != null) ? ((DateTime)employee.BirthDate).ToShortDateString() : null);
                cmd.Parameters.AddWithValue("@ContactInfoId", employee.ContactInfoId);
                cmd.Parameters.AddWithValue("@Photo", employee.Photo);
                cmd.Parameters.AddWithValue("@Note", employee.Note);
                cmd.Parameters.AddWithValue("@PassportNum", employee.PassportNum);
                cmd.Parameters.AddWithValue("@Title", employee.Title);
                cmd.Parameters.AddWithValue("@AccessLayer", employee.AccessLayer);
                cmd.Parameters.AddWithValue("@Login", employee.Login);
                cmd.Parameters.AddWithValue("@Password", employee.Password);

                if (employee.HireDate != null)
                {
                    Int32 unixTimestamp = (Int32)(((DateTime)employee.HireDate).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    cmd.Parameters.AddWithValue("@HireDate", unixTimestamp);
                }
                else cmd.Parameters.AddWithValue("@HireDate", null);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using 



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
                                   + "BirthDate = @BirthDate, ContactInfoId = @ContactInfoId, Photo = @Photo, Note = @Note, "
                                   + "PassportNum = @PassportNum, Title = @Title, AccessLayer = @AccessLayer, Login = @Login, "
                                   + "Password = @Password "
                                   + "WHERE EmployeeId = @EmployeeId;";


                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                cmd.Parameters.AddWithValue("@BirthDate", (employee.BirthDate != null) ? ((DateTime)employee.BirthDate).ToShortDateString() : null);
                cmd.Parameters.AddWithValue("@ContactInfoId", employee.ContactInfoId);
                cmd.Parameters.AddWithValue("@Photo", employee.Photo);
                cmd.Parameters.AddWithValue("@Note", employee.Note);
                cmd.Parameters.AddWithValue("@PassportNum", employee.PassportNum);
                cmd.Parameters.AddWithValue("@Title", employee.Title);
                cmd.Parameters.AddWithValue("@AccessLayer", employee.AccessLayer);
                cmd.Parameters.AddWithValue("@Login", employee.Login);
                cmd.Parameters.AddWithValue("@Password", employee.Password);

                if (employee.HireDate != null)
                {
                    Int32 unixTimestamp = (Int32)(((DateTime)employee.HireDate).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    cmd.Parameters.AddWithValue("@HireDate", unixTimestamp);
                }
                else cmd.Parameters.AddWithValue("@HireDate", null);

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
                                   + "BirthDate = @BirthDate, ContactInfoId = @ContactInfoId, Photo = @Photo, Note = @Note, "
                                   + "PassportNum = @PassportNum, Title = @Title, AccessLayer = @AccessLayer, Login = @Login "
                                   + "WHERE EmployeeId = @EmployeeId;";


                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                cmd.Parameters.AddWithValue("@BirthDate", (employee.BirthDate != null) ? ((DateTime)employee.BirthDate).ToShortDateString() : null);
                cmd.Parameters.AddWithValue("@ContactInfoId", employee.ContactInfoId);
                cmd.Parameters.AddWithValue("@Photo", employee.Photo);
                cmd.Parameters.AddWithValue("@Note", employee.Note);
                cmd.Parameters.AddWithValue("@PassportNum", employee.PassportNum);
                cmd.Parameters.AddWithValue("@Title", employee.Title);
                cmd.Parameters.AddWithValue("@AccessLayer", employee.AccessLayer);
                cmd.Parameters.AddWithValue("@Login", employee.Login);

                if (employee.HireDate != null)
                {
                    Int32 unixTimestamp = (Int32)(((DateTime)employee.HireDate).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    cmd.Parameters.AddWithValue("@HireDate", unixTimestamp);
                }
                else cmd.Parameters.AddWithValue("@HireDate", null);

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
        /// Возвращает единицу товара найденную по заданным параметрам.
        /// </summary>
        /// <param name="sparePartId">Ид товара искомой записи</param>
        /// <param name="saleId">Ид прихода искомой записи</param>
        /// <returns></returns>
        public static SparePart FindSparePartAvaliability(int sparePartId, int purchaseId)
        {
            SparePart sparePart = new SparePart();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();               

                const string query = "SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId "
                                   + "AND av.SparePartId = @SparePartId AND PurchaseId = @PurchaseId;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
                cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    sparePart = CreateFullSparePart(dataReader);
                }//while
                connection.Close();
            }//using
            return sparePart;
        }//FindSparePartAvaliability
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

                string query = "SELECT Count FROM Avaliability WHERE SparePartId = @SparePartId AND PurchaseId = @PurchaseId;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
                cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    count = Convert.ToDouble(dataReader["Count"]);
                }//while
                connection.Close();
            }//using
            return count;        
        }//FindSparePartAvaliabilityCount

        public static IList<SparePart> FindAllUniqueSparePartsAvaliability(SQLiteConnection openConnection)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            const string query = "SELECT * FROM Avaliability as av "
                               + "JOIN SpareParts as sp "
                               + "ON av.SparePartId = sp.SparePartId "
                               + "GROUP BY av.SparePartId;";
            SQLiteCommand cmd = new SQLiteCommand(query, openConnection);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                SparePart sparePart = new SparePart();

                sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                sparePart.Articul = dataReader["Articul"] as string;
                sparePart.Title = dataReader["Title"] as string;

                sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                sparePart.MeasureUnit = dataReader["MeasureUnit"] as string;

                spareParts.Add(sparePart);
            }//while     

            return spareParts;
        }//FindAllUniqueSparePartsAvaliability
        
        //Нахождение кол-ва SparePart на осн. и вирт. складах отдельно.
        public static IList<SparePart> FindAvaliabilityBySparePartId(int sparePartId)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId AND av.SparePartId = @SparePartId;", connection);
                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = CreateFullSparePart(dataReader);
                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using
            return spareParts;
        }//FindAvaliabilityBySparePartId
        public static IList<SparePart> FindAvaliabilityBySparePartId(int sparePartId, SQLiteConnection openConnection)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            const string query = "SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId AND av.SparePartId = @SparePartId;";
            SQLiteCommand cmd = new SQLiteCommand(query, openConnection);
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                SparePart sparePart = CreateFullSparePart(dataReader);
                spareParts.Add(sparePart);
            }//while    

            return spareParts;
        }//FindAvaliabilityBySparePartId
        //Возвращает разделенный по приходам список всех товаров в Наличии из ИД переданных SpareParts
        /// <summary>
        /// Возвращает разделенный по приходам список всех товаров в Наличии из ИД переданных SpareParts
        /// </summary>
        /// <param name="sparePartsId">Список Ид наличие которых надо найти.</param>
        /// <returns></returns>
        public static IList<SparePart> FindAvaliabilityBySparePartId(IList<SparePart> sparePartsId)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM Avaliability AS av JOIN SpareParts AS sp " +
                                     "ON av.SparePartId = sp.SparePartId AND av.SparePartId = @SparePartId;";
                var cmd = new SQLiteCommand(query, connection);

                var param = new SQLiteParameter();
                param.ParameterName = "@SparePartId";
                cmd.Parameters.Add(param);

                for (int i = 0; i < sparePartsId.Count; ++i )
                {
                    param.Value = sparePartsId[i].SparePartId;

                    var dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        SparePart sparePart = CreateFullSparePart(dataReader);

                        spareParts.Add(sparePart);
                    }//while

                    dataReader.Dispose();
                }//for

                connection.Close();
            }//using
            return spareParts;
        }//FindAvaliabilityBySparePartId

        //Добавляет в передаваемый SparePart общее значение Count из таблицы Avaliability.
        /// <summary>
        /// Добавляет в передаваемый SparePart общее значение Count из таблицы Avaliability.
        /// </summary>
        /// <param name="sparePart">Модифицируемый SparePart</param>
        /// <returns></returns>
        public static SparePart FindUniqueSparePartsAvaliabilityCount(SparePart sparePart)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT SUM(Count), StorageAdress "
                                   + "FROM Avaliability "
                                   + "WHERE SparePartId = @SparePartId "
                                   + "GROUP BY StorageAdress;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    if ((dataReader["StorageAdress"] as string) == null) 
                        sparePart.Count = Convert.ToDouble(dataReader["SUM(Count)"]);
                    else 
                        sparePart.VirtCount = Convert.ToDouble(dataReader["SUM(Count)"]);
                }//while

                connection.Close();
            }//using
            return sparePart;
        }//FindAllUniqueSparePartAvaliability

        /// <summary>
        /// Добавляет в передаваемый SparePart общее значение Count из таблицы Avaliability.
        /// </summary>
        /// <param name="sparePart">Модифицируемый SparePart</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        /// <returns></returns>
        public static SparePart FindUniqueSparePartsAvaliabilityCount(SparePart sparePart, SQLiteConnection openConnection)
        {
            const string query = "SELECT SUM(Count), StorageAdress "
                                   + "FROM Avaliability "
                                   + "WHERE SparePartId = @SparePartId "
                                   + "GROUP BY StorageAdress;";
            SQLiteCommand cmd = new SQLiteCommand(query, openConnection);

            cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {            
                if ((dataReader["StorageAdress"] as string) == null) 
                    sparePart.Count = Convert.ToDouble(dataReader["SUM(Count)"]);
                else 
                    sparePart.VirtCount = Convert.ToDouble(dataReader["SUM(Count)"]);
            }//while    

            return sparePart;
        }//FindAllUniqueSparePartAvaliability

        /// <summary>
        /// Возвращает кол-во записей данной SparePart (со скольких приходов данная запчасть сейчас в наличии, 0 -- запчасти нет в наличии.) 
        /// </summary>
        /// <param name="sparePartId">Ид искомой SP</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        /// <returns></returns>
        public static int FindCountOfEntrySparePartInAvaliability(int sparePartId, SQLiteConnection openConnection)
        {
            var cmd = new SQLiteCommand("SELECT COUNT() FROM SpareParts AS sp JOIN Avaliability AS av ON sp.SparePartId = av.SparePartId WHERE sp.SparePartId = @SparePartId;", openConnection);
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

            var dataReader = cmd.ExecuteReader();
            dataReader.Read();

            return Convert.ToInt32(dataReader["COUNT()"]);
        }//FindCountOfEntrySparePart
        /// <summary>
        /// Возвращает полный список готовый к выводу всех запчастей кот. сейчас в наличии.
        /// </summary>
        /// <returns></returns>
        public static IList<SparePart> FindAllSparePartsAvaliableToDisplay()
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                var cmd = new SQLiteCommand("SELECT DISTINCT SparePartId FROM Avaliability;", connection);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    int sparePartId = Convert.ToInt32(dataReader["SparePartId"]);

                    spareParts.Add(FindSparePartByIdToDisplay(sparePartId, connection));
                }//while

                connection.Close();
            }//using
            return spareParts;
        }//FindAllSparePartsAvaliableToDisplay















////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region *****************Поиск по таблицам SpareParts. *********************************************************************

        /// <summary>
        /// Возвращает SparePart заполненный только полями из таблицы SpareParts, остальные поля не заполняются.
        /// </summary>
        /// <param name="sparePartId">Ид заполняемой SP</param>
        /// <returns></returns>
        public static SparePart FindSparePartById(int sparePartId)
        {
            SparePart sparePart = new SparePart();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                var cmd = new SQLiteCommand("SELECT * FROM SpareParts WHERE SparePartId = @SparePartId;", connection);
                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    sparePart = CreateSparePart(dataReader);
                }//while

                connection.Close();
            }//using
            return sparePart;

        }//FindSparePartById
        /// <summary>
        /// /// Возвращает SparePart заполненный только полями из таблицы SpareParts, остальные поля не заполняются.
        /// </summary>
        /// <param name="sparePartId">Ид заполняемой SP</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        /// <returns>Заполненный SparePart</returns>
        public static SparePart FindSparePartById(int sparePartId, SQLiteConnection openConnection)
        {
            SparePart sparePart = new SparePart();

            var cmd = new SQLiteCommand("SELECT * FROM SpareParts WHERE SparePartId = @SparePartId;", openConnection);
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                sparePart = CreateSparePart(dataReader);
            }//while

            return sparePart;
        }//FindSparePartById
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
                    spareParts.Add(FindSparePartById(sparePartId, connection));

                connection.Close();
            }//using
            return spareParts;
        }//FindSparePartsIdByArticul
        
        public static IList<int> FindSparePartsIdByArticul(string sparePartArticul)
        {
            IList<int> sparePartsId = new List<int>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                var cmd = new SQLiteCommand("SELECT SparePartId FROM SpareParts WHERE Articul LIKE @Articul;", connection);

                cmd.Parameters.AddWithValue("@Articul", sparePartArticul);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    sparePartsId.Add(Convert.ToInt32(dataReader["SparePartId"]));
                }//while

                connection.Close();
            }//using
            return sparePartsId;
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
        /// <summary>
        /// Возвращает SparePart полостью готовый для отображения в общей таблице.
        /// </summary>
        /// <param name="sparePartId">ИД искомого SparePart</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        /// <returns></returns>
        public static SparePart FindSparePartByIdToDisplay(int sparePartId, SQLiteConnection openConnection)
        {
            SparePart sparePart = new SparePart();
            //Находим кол-во записей с данной запчастью.
            int countOfEntry = FindCountOfEntrySparePartInAvaliability(sparePartId, openConnection);
            //Если нет в наличии.
            if (countOfEntry == 0)
                sparePart = FindSparePartById(sparePartId, openConnection);
            //Если товар в наличии только с одного прихода
            else if (countOfEntry == 1)
                sparePart = FindAvaliabilityBySparePartId(sparePartId, openConnection)[0];
            //Если товар в наличии с многочисленных приходов.                
            else if (countOfEntry > 1)
            {
                IList<SparePart> spareParts = FindAvaliabilityBySparePartId(sparePartId, openConnection);
                //Выводим в общ. таблицу значения товара с наибольшей ценой продажи.
                sparePart = spareParts.OrderByDescending(sp => sp.SellingPrice).First();

                FindUniqueSparePartsAvaliabilityCount(sparePart, openConnection);

                sparePart.PurchaseId = -1; //Помечаем что у данной строки имеется подтаблица(т.е. болеее одного поставщика).
            }//if    

            return sparePart;
        }//FindSparePartById
        /// <summary>
        /// Возвращает полностью готовый к выводу список всех запчастей в БД с общим кол-вом.
        /// </summary>
        /// <returns></returns>
        public static IList<SparePart> FindAllSparePartsToDisplay()
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT SparePartId FROM SpareParts;", connection);
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    int sparePartId = Convert.ToInt32(dataReader["SparePartId"]);

                    spareParts.Add(FindSparePartByIdToDisplay(sparePartId, connection));
                }//while
                connection.Close();
            }//using
            return spareParts;
        }//FindAllSparePartsToDisplay


















        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                    supplier.ContragentId   = Convert.ToInt32(dataReader["SupplierId"]);
                    supplier.ContragentName = dataReader["SupplierName"] as string;
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
        /// Возвращает массив строк состоящий из всех имен поставщиков. 
        /// </summary>
        /// <returns></returns>
        public static string[] FindAllSuppliersName()
        {
            IList<string> suppliersNameList = new List<string>();
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT SupplierName FROM Suppliers;", connection);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    suppliersNameList.Add(dataReader["SupplierName"] as string);                    
                }//while

                connection.Close();
            }//using
            string[] suppliersName = new string[suppliersNameList.Count];
            for (int i = 0; i < suppliersName.Length; ++i)
                suppliersName[i] = suppliersNameList[i];

            return suppliersName;
        }//FindAllSuppliersName
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

                const string query = "SELECT * FROM  Suppliers WHERE SupplierId = @SupplierId;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@SupplierId", supplierId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    supplier.ContragentId = Convert.ToInt32(dataReader["SupplierId"]);
                    supplier.ContragentName = dataReader["SupplierName"] as string;
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

                const string query = "SELECT * FROM Purchases AS p JOIN Suppliers AS s ON p.SupplierId = s.SupplierId "
                                   + "WHERE p.PurchaseId = @PurchaseId;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    supplier.ContragentId = Convert.ToInt32(dataReader["SupplierId"]);
                    supplier.ContragentName = dataReader["SupplierName"] as string;
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

                const string query = "SELECT * FROM Suppliers WHERE SupplierName LIKE @SupplierName;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@SupplierName", supplierName);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    supplier = new Supplier();
                    supplier.ContragentId = Convert.ToInt32(dataReader["SupplierId"]);
                    supplier.ContragentName = dataReader["SupplierName"] as string;
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
                    customer.ContragentId   = Convert.ToInt32(dataReader["CustomerId"]);
                    customer.ContragentName = dataReader["CustomerName"] as string;
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

                const string query = "SELECT * FROM Customers WHERE CustomerName LIKE @CustomerName;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@CustomerName", customerName);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    customer = new Customer();
                    customer.ContragentId   = Convert.ToInt32(dataReader["CustomerId"]);
                    customer.ContragentName = dataReader["CustomerName"] as string;
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
        /// Возвращает массив строк состоящий из всех имен клиентов. 
        /// </summary>
        /// <returns></returns>
        public static string[] FindAllCustomersName()
        {
            IList<string> customersNameList = new List<string>();
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT CustomerName FROM Customers;", connection);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string name = dataReader["CustomerName"] as string;
                    customersNameList.Add(name);
                }//while

                connection.Close();
            }//using
            string[] customersName = new string[customersNameList.Count];
            for (int i = 0; i < customersName.Length; ++i)
                customersName[i] = customersNameList[i];

            return customersName;
        }//FindAllCustomersName

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
                
                const string query = "SELECT * FROM Customers WHERE CustomerId = @CustomerId;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    customer.ContragentId   = Convert.ToInt32(dataReader["CustomerId"]);
                    customer.ContragentName = dataReader["CustomerName"] as string;
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

        /// <summary>
        /// Возвращает список из всех приходов в базе.
        /// </summary>
        /// <returns></returns>
        public static IList<Purchase> FindPurchases()
        {
            IList<Purchase> purchases = new List<Purchase>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Purchases;", connection);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Purchase purchase = new Purchase();

                    purchase.OperationId = Convert.ToInt32(dataReader["PurchaseId"]);
                    purchase.Employee = (dataReader["EmployeeId"] != DBNull.Value) ? FindEmployees(Convert.ToInt32(dataReader["EmployeeId"])) : null;
                    purchase.Contragent = FindSuppliers(Convert.ToInt32(dataReader["SupplierId"]));
                    purchase.ContragentEmployee = dataReader["SupplierEmployee"] as string;
                    //Переводим кол-во секунд Utc в DateTime.
                    TimeSpan ts = TimeSpan.FromSeconds(Convert.ToInt32(dataReader["PurchaseDate"]));
                    DateTime purchaseDate = new DateTime(1970, 1, 1);
                    purchaseDate += ts;
                    purchase.OperationDate = purchaseDate;

                    purchases.Add(purchase);
                }//while
                connection.Close();
            }//using

            return purchases;
        }//FindPurchase

        public static List<IOperation> FindPurchases(int supplierId)
        {
            List<IOperation> purchases = new List<IOperation>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(PurchaseDate, 'unixepoch') as PD "
                                   + "FROM Purchases "
                                   + "WHERE SupplierId = @SupplierId "
                                   + "ORDER BY PurchaseDate desc;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@SupplierId", supplierId);

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

                const string query = "SELECT *, datetime(SaleDate, 'unixepoch') as SD "
                                   + "FROM Sales "
                                   + "WHERE CustomerId = @CustomerId "
                                   + "ORDER BY SaleDate desc;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                    salesList.Add(CreateSale(dataReader));

                connection.Close();
            }//using

            return salesList;
        }//FindSales


        /// <summary>
        /// Возвращает объект класса Purchase, найденный по заданному Id. 
        /// </summary>
        /// <param name="saleId">Id прихода информацию о котором нужно вернуть.</param>
        /// <returns></returns>
        public static Purchase FindPurchase(int purchaseId)
        {
            Purchase purchase = new Purchase();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Purchases WHERE PurchaseId = @PurchaseId;", connection);

                cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    purchase.OperationId = Convert.ToInt32(dataReader["PurchaseId"]);
                    purchase.Employee = (dataReader["EmployeeId"] != DBNull.Value) ? FindEmployees(Convert.ToInt32(dataReader["EmployeeId"])) : null;
                    purchase.Contragent = FindSuppliers(Convert.ToInt32(dataReader["SupplierId"]));
                    purchase.ContragentEmployee = dataReader["SupplierEmployee"] as string;
                    //Переводим кол-во секунд Utc в DateTime.
                    TimeSpan ts = TimeSpan.FromSeconds(Convert.ToInt32(dataReader["PurchaseDate"]));
                    DateTime purchaseDate = new DateTime(1970, 1, 1);
                    purchaseDate += ts;
                    purchase.OperationDate = purchaseDate;

                }//while
                connection.Close();
            }//using

            return purchase;
        }//FindPurchase
        /// <summary>
        /// Возвращает общую сумму прихода, по указанному Id. 
        /// </summary>
        /// <param name="saleId">Id прихода, сумму которого надо найти.</param>
        /// <returns></returns>
        public static double FindTotalSumOfPurchase(int purchaseId)
        {
            double totalSum = 0;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT SUM(Price) FROM PurchaseDetails WHERE PurchaseId = @PurchaseId;", connection);

                cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

                totalSum = Convert.ToDouble(cmd.ExecuteScalar());

                connection.Close();
            }//using

            return totalSum;
        }//FindTotalSumOfPurchase


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
            List<Purchase> purchases = new List<Purchase>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT *, datetime(PurchaseDate, 'unixepoch') as PD "
                                   + "FROM Purchases;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Purchase purchase = CreatePurchase(dataReader);
                    if (purchase.OperationDetails.Any(sp => sp.SparePartId == sparePartId))
                        purchases.Add(purchase);
                }//while

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

                const string query = "SELECT *, datetime(SaleDate, 'unixepoch') as SD "
                                   + "FROM Sales;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Sale sale = CreateSale(dataReader);
                    if (sale.OperationDetails.Any(sp => sp.SparePartId == sparePartId))
                        salesList.Add(sale);
                }//while

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
            Purchase purchase = new Purchase();

            purchase.OperationId = Convert.ToInt32(dataReader["PurchaseId"]);
            purchase.Employee = (dataReader["EmployeeId"] != DBNull.Value) ? FindEmployees(Convert.ToInt32(dataReader["EmployeeId"])) : null;
            purchase.Contragent = FindSuppliers(Convert.ToInt32(dataReader["SupplierId"]));
            purchase.ContragentEmployee = dataReader["SupplierEmployee"] as string;
            purchase.OperationDate = Convert.ToDateTime(dataReader["PD"]);
            purchase.OperationDetails = FindPurchaseDetails(purchase.OperationId);

            return purchase;
        }//CreatePurchase
        private static Sale CreateSale(SQLiteDataReader dataReader)
        {
            Sale sale = new Sale();

            sale.OperationId = Convert.ToInt32(dataReader["SaleId"]);
            sale.Employee = (dataReader["EmployeeId"] != DBNull.Value) ? FindEmployees(Convert.ToInt32(dataReader["EmployeeId"])) : null;
            sale.Contragent = FindCustomers(Convert.ToInt32(dataReader["CustomerId"]));
            sale.ContragentEmployee = dataReader["CustomerEmployee"] as string;
            sale.OperationDate = Convert.ToDateTime(dataReader["SD"]);
            sale.OperationDetails = FindSaleDetails(sale.OperationId);

            return sale;
        }//CreateSale

        public static List<SparePart> FindPurchaseDetails(int purchaseId)
        {
            List<SparePart> sparePartsList = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT * FROM PurchaseDetails "
                                   + "WHERE PurchaseId = @PurchaseId;";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = FindSparePartById(Convert.ToInt32(dataReader["SparePartId"]));
                    sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                    sparePart.Count = Convert.ToDouble(dataReader["Quantity"]);

                    sparePartsList.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return sparePartsList;
        }//FindPurchaseDetails
        public static List<SparePart> FindSaleDetails(int saleId)
        {
            List<SparePart> sparePartsList = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT * FROM SaleDetails "
                                   + "WHERE SaleId = @SaleId;";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@SaleId", saleId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = FindSparePartById(Convert.ToInt32(dataReader["SparePartId"]));
                    sparePart.Price = Convert.ToDouble(dataReader["SellingPrice"]);
                    sparePart.Count = Convert.ToDouble(dataReader["Quantity"]);

                    sparePartsList.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return sparePartsList;
        }//FindSaleDetails






////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблице Employees.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает список из объектов типа Employee, состоящий из всех сотрудников.
        /// </summary>
        /// <returns></returns>
        public static IList<Employee> FindEmployees()
        {
            IList<Employee> employees = new List<Employee>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                
                const string query = "SELECT date(HireDate, \"Unixepoch\") AS 'HD', date(DismissalDate, \"Unixepoch\") AS 'DD', * FROM Employees;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmployeeId     = Convert.ToInt32(dataReader["EmployeeId"]);
                    employee.LastName       = dataReader["LastName"] as string;
                    employee.FirstName      = dataReader["FirstName"] as string;
                    employee.MiddleName     = dataReader["MiddleName"] as string;
                    employee.BirthDate      = (dataReader["BirthDate"] != DBNull.Value)     ? Convert.ToDateTime(dataReader["BirthDate"]) : (DateTime?)null;
                    employee.HireDate       = (dataReader["HireDate"] != DBNull.Value) ? Convert.ToDateTime(dataReader["HD"]) : (DateTime?)null;
                    employee.DismissalDate  = (dataReader["DismissalDate"] != DBNull.Value) ? Convert.ToDateTime(dataReader["DD"]) : (DateTime?)null;
                    employee.ContactInfoId  = (dataReader["ContactInfoId"] != DBNull.Value) ? Convert.ToInt32(dataReader["ContactInfoId"]) : (int?)null;
                    employee.Photo          = dataReader["Photo"] as string;    
                    employee.Note           = dataReader["Note"] as string;
                    employee.PassportNum    = dataReader["PassportNum"] as string;
                    employee.Title          = dataReader["Title"] as string;
                    employee.AccessLayer    = dataReader["AccessLayer"] as string;
                    employee.Login          = dataReader["Login"] as string;
                    employee.Password       = dataReader["Password"] as string;

                    employees.Add(employee);
                }//while 

                connection.Close();
            }//using

            return employees;
        }//FindAllEmployees
        /// <summary>
        /// Возвращает объект типа Employee, найденный по заданному Id.
        /// </summary>
        /// <param name="employeeId">Ид сотрудника, которого надо найти.</param>
        /// <returns></returns>
        public static Employee FindEmployees(int employeeId)
        {
            Employee employee = new Employee();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT date(HireDate, \"Unixepoch\") AS 'HD', date(DismissalDate, \"Unixepoch\") AS 'DD', * "
                                   + "FROM Employees WHERE EmployeeId = @EmployeeId;";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    
                    employee.EmployeeId = Convert.ToInt32(dataReader["EmployeeId"]);
                    employee.LastName = dataReader["LastName"] as string;
                    employee.FirstName = dataReader["FirstName"] as string;
                    employee.MiddleName = dataReader["MiddleName"] as string;
                    employee.BirthDate = (dataReader["BirthDate"] != DBNull.Value) ? Convert.ToDateTime(dataReader["BirthDate"]) : (DateTime?)null;
                    employee.HireDate = (dataReader["HireDate"] != DBNull.Value) ? Convert.ToDateTime(dataReader["HD"]) : (DateTime?)null;
                    employee.DismissalDate = (dataReader["DismissalDate"] != DBNull.Value) ? Convert.ToDateTime(dataReader["DD"]) : (DateTime?)null;
                    employee.ContactInfoId = (dataReader["ContactInfoId"] != DBNull.Value) ? Convert.ToInt32(dataReader["ContactInfoId"]) : (int?)null;
                    employee.Photo = dataReader["Photo"] as string;
                    employee.Note = dataReader["Note"] as string;
                    employee.PassportNum = dataReader["PassportNum"] as string;
                    employee.Title = dataReader["Title"] as string;
                    employee.AccessLayer = dataReader["AccessLayer"] as string;
                    employee.Login = dataReader["Login"] as string;
                    employee.Password = dataReader["Password"] as string;
                }//while
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

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmployeeId    = Convert.ToInt32(dataReader["EmployeeId"]);
                    employee.LastName      = dataReader["LastName"] as string;
                    employee.FirstName     = dataReader["FirstName"] as string;
                    employee.MiddleName    = dataReader["MiddleName"] as string;
                    employee.BirthDate     = (dataReader["BirthDate"] != DBNull.Value) ? Convert.ToDateTime(dataReader["BirthDate"]) : (DateTime?)null;
                    employee.HireDate      = (dataReader["HireDate"] != DBNull.Value) ? Convert.ToDateTime(dataReader["HD"]) : (DateTime?)null;
                    employee.DismissalDate = (dataReader["DismissalDate"] != DBNull.Value) ? Convert.ToDateTime(dataReader["DD"]) : (DateTime?)null;
                    employee.ContactInfoId = (dataReader["ContactInfoId"] != DBNull.Value) ? Convert.ToInt32(dataReader["ContactInfoId"]) : (int?)null;
                    employee.Photo         = dataReader["Photo"] as string;
                    employee.Note          = dataReader["Note"] as string;
                    employee.PassportNum   = dataReader["PassportNum"] as string;
                    employee.Title         = dataReader["Title"] as string;
                    employee.AccessLayer   = dataReader["AccessLayer"] as string;
                    employee.Login         = dataReader["Login"] as string;
                    employee.Password      = dataReader["Password"] as string;

                    employees.Add(employee);
                }//while 

                connection.Close();
            }//using

            return employees;       
        }//FindEmployees

        









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

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    contactInfo.Country     = dataReader["Country"] as string;
                    contactInfo.Region      = dataReader["Region"] as string;
                    contactInfo.City        = dataReader["City"] as string;
                    contactInfo.Street      = dataReader["Street"] as string;
                    contactInfo.House       = dataReader["House"] as string;
                    contactInfo.Room        = dataReader["Room"] as string;
                    contactInfo.Phone       = dataReader["Phone"] as string;
                    contactInfo.ExtPhone    = dataReader["ExtPhone"] as string;
                    contactInfo.Email       = dataReader["Email"] as string;
                    contactInfo.Website     = dataReader["Website"] as string;
                }//while 

                connection.Close();
            }//using

            return contactInfo;
        }//FindContactInfoById

















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
        public static IList<SparePart> SearchSparePartsAvaliablityByArticul(string articul, int limit, IList<int> withoutIDs)
        {
            IList<SparePart> spareParts = new List<SparePart>();

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
                          + "ON av.SparePartId = sp.SparePartId AND sp.Articul LIKE @Articul AND av.SparePartId NOT IN(" + notIn + ")" 
                          + "GROUP BY av.SparePartId LIMIT @Limit;";

                cmd.Parameters.AddWithValue("@Articul", articul + "%");
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
        /// Возвращает список размера не более limit, состоящий из запчастей в наличии чьи Title имеют совпадение с параметром title. 
        /// </summary>
        /// <param name="title">Строка по которой ищутся совпадения.</param>
        /// <param name="limit">Максимально возможное кол-во эл-тов.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSparePartsAvaliablityByTitle(string title, int limit)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId AND sp.Title LIKE @Title LIMIT @Limit", connection);

                cmd.Parameters.AddWithValue("@Title", title + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);
                

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart();
                    sparePart = CreateFullSparePart(dataReader);
                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;
        }//SearchSparePartsByTitle

        /// <summary>
        /// Возвращает список размера не более limit, состоящий из запчастей в Наличии, чьи Title имеют совпадение с параметром title. 
        /// </summary>
        /// <param name="title">Строка по которой ищутся совпадения.</param>
        /// <param name="limit">Максимально возможное кол-во эл-тов.</param>
        /// <param name="withoutIDs">Список Id товара который не должен входить в результирующий список.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSparePartsAvaliablityByTitle(string title, int limit, IList<int> withoutIDs)
        {
            IList<SparePart> spareParts = new List<SparePart>();

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
                          + "ON av.SparePartId = sp.SparePartId AND sp.Title LIKE @Title AND av.SparePartId NOT IN(" + notIn + ")"
                          + "GROUP BY av.SparePartId LIMIT @Limit;";

                cmd.Parameters.AddWithValue("@Title", title + "%");
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
        /// Возвращает полностью готовый для отображения в общей таблице список из заданного кол-ва эл-тов в Наличии, найдейнных по заданному параметру. 
        /// </summary>
        /// <param name="titleOrArticul">Title или Articul совпадение с которым нужно искать.</param>
        /// <param name="limit">Ограничение по максимальному кол-ву эл-тов.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSpAvaliabilityByTitleOrArticulToDisplay(string titleOrArticul, int limit)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = 
                   "SELECT DISTINCT a.SparePartId FROM SpareParts AS sp JOIN Avaliability AS a ON sp.SparePartId = a.SparePartId " +
                   "WHERE sp.Articul LIKE @TitleOrArticul OR sp.Title LIKE @TitleOrArticul LIMIT @Limit;";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@TitleOrArticul", titleOrArticul + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);


                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    spareParts.Add(FindSparePartByIdToDisplay(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;    
        
        }//SearchSpAvaliabilityByTitleOrArticulToDisplay
        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из всех эл-тов в Наличии, найдейнных по заданному параметру. 
        /// </summary>
        /// <param name="titleOrArticul">Title или Articul совпадение с которым нужно искать.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSpAvaliabilityByTitleOrArticulToDisplay(string titleOrArticul)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = 
                "SELECT DISTINCT a.SparePartId FROM SpareParts AS sp JOIN Avaliability AS a ON sp.SparePartId = a.SparePartId " +
                "WHERE sp.Articul LIKE @TitleOrArticul OR sp.Title LIKE @TitleOrArticul;";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@TitleOrArticul", titleOrArticul + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    spareParts.Add(FindSparePartByIdToDisplay(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;

        }//SearchSpAvaliabilityByTitleOrArticulToDisplay
        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из эл-тов в Наличии, найдейнных по заданным параметрам.
        /// </summary>
        /// <param name="title">Искомый Title</param>
        /// <param name="Articul">Искомый Articul</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSpAvaliabilityByTitleAndArticulToDisplay(string title, string articul)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT DISTINCT a.SparePartId FROM SpareParts AS sp JOIN Avaliability AS a " +
                                     "ON sp.SparePartId = a.SparePartId " +
                                     "AND sp.Articul LIKE @Articul OR sp.Title LIKE @Title;";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@Title", title + "%");
                cmd.Parameters.AddWithValue("@Articul", articul + "%");


                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    spareParts.Add(FindSparePartByIdToDisplay(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;

        }//SearchSpAvaliabilityByTitleAndArticulToDisplay
        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из заданного кол-ва эл-тов найдейнных по заданному параметру. 
        /// </summary>
        /// <param name="titleOrArticul">Title или Articul совпадение с которым нужно искать.</param>
        /// <param name="limit">Ограничение по максимальному кол-ву эл-тов.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSpByTitleOrArticulToDisplay(string titleOrArticul, int limit)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT SparePartId FROM SpareParts WHERE Articul LIKE @TitleOrArticul OR Title LIKE @TitleOrArticul LIMIT @limit;", connection);

                cmd.Parameters.AddWithValue("@TitleOrArticul", titleOrArticul + "%");
                cmd.Parameters.AddWithValue("@limit", limit);


                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    spareParts.Add(FindSparePartByIdToDisplay(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;
        }//SearchByTitleOrArticul
        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из заданного кол-ва эл-тов найдейнных по заданному параметру. 
        /// </summary>
        /// <param name="titleOrArticulOrManuf">Title или Articul или Manufacturer совпадение с которым нужно искать.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSpByTitleOrArticulOrManufacturerToDisplay(string titleOrArticulOrManuf)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT SparePartId FROM SpareParts AS sp LEFT JOIN Manufacturers AS m "
                                   + "ON sp.ManufacturerId = m.ManufacturerId "
                                   + "WHERE ToLower(sp.Articul) LIKE @TitleOrArticul "
                                   + "OR ToLower(sp.Title) LIKE @TitleOrArticul OR ToLower(m.ManufacturerName) LIKE @TitleOrArticul;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@TitleOrArticul", "%" + titleOrArticulOrManuf.ToLower() + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    spareParts.Add(FindSparePartByIdToDisplay(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;
        }//SearchByTitleOrArticul
        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из заданного кол-ва эл-тов найдейнных по заданному параметру. 
        /// </summary>
        /// <param name="titleOrArticulOrManuf">Title или Articul или Manufacturer совпадение с которым нужно искать.</param>
        /// <param name="limit">Ограничение по максимальному кол-ву эл-тов.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSpByTitleOrArticulOrManufacturerToDisplay(string titleOrArticulOrManuf, int limit)
        {
            IList<SparePart> spareParts = new List<SparePart>();

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
                    spareParts.Add(FindSparePartByIdToDisplay(Convert.ToInt32(dataReader["SparePartId"]), connection));
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
        public static IList<SparePart> SearchSpAvaliabilityByTitleOrArticulOrManufacturerToDisplay(string titleOrArticulOrManuf)
        {
            IList<SparePart> spareParts = new List<SparePart>();

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
                    spareParts.Add(FindSparePartByIdToDisplay(Convert.ToInt32(dataReader["SparePartId"]), connection));
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
        public static IList<SparePart> SearchSpAvaliabilityByTitleOrArticulOrManufacturerToDisplay(string titleOrArticulOrManuf, int limit)
        {
            IList<SparePart> spareParts = new List<SparePart>();

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
                    spareParts.Add(FindSparePartByIdToDisplay(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;

        }//SearchSpAvaliabilityByTitleOrArticulOrManufacturerToDispla
        
        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из всех эл-тов найдейнных по заданному параметру. 
        /// </summary>
        /// <param name="titleOrArticul">Title или Articul совпадение с которым нужно искать.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSpByTitleOrArticulToDisplay(string titleOrArticul)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT SparePartId FROM SpareParts WHERE Articul LIKE @TitleOrArticul OR Title LIKE @TitleOrArticul;", connection);

                cmd.Parameters.AddWithValue("@TitleOrArticul", titleOrArticul + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    spareParts.Add(FindSparePartByIdToDisplay(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;
        }//SearchByTitleOrArticul
        /// <summary>
        /// Возвращает полностью готовый для отображения в общей таблице список из всех эл-тов найдейнных по заданным параметрам.
        /// </summary>
        /// <param name="title">Искомый Title</param>
        /// <param name="Articul">Искомый Articul</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSpByTitleAndArticulToDisplay(string title, string articul)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT SparePartId FROM SpareParts WHERE Articul LIKE @Articul OR Title LIKE @Title;", connection);

                cmd.Parameters.AddWithValue("@Title", title + "%");
                cmd.Parameters.AddWithValue("@Articul", articul + "%");


                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    spareParts.Add(FindSparePartByIdToDisplay(Convert.ToInt32(dataReader["SparePartId"]), connection));
                }//while

                connection.Close();
            }//using
            return spareParts;
        }//SearchByTitleOrArticul

        /// <summary>
        /// Возвращает список размера не более limit, состоящий из запчастей чьи Title имеют совпадение с параметром title. 
        /// </summary>
        /// <param name="title">Строка по которой ищутся совпадения.</param>
        /// <param name="limit">Максимально возможное кол-во эл-тов.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSparePartsByTitle(string title, int limit)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM  SpareParts WHERE Title LIKE @Title LIMIT @Limit", connection);

                cmd.Parameters.AddWithValue("@Title", title + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = CreateSparePart(dataReader);
                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;
        }//SearchSparePartByTitle
        /// <summary>
        /// Возвращает список размера не более limit, состоящий из запчастей чьи Title имеют совпадение с параметром title. 
        /// </summary>
        /// <param name="title">Строка по которой ищутся совпадения.</param>
        /// <param name="limit">Максимально возможное кол-во эл-тов.</param>
        /// <param name="withoutIDs">Список Id товара который не должен входить в результирующий список.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSparePartsByTitle(string title, int limit, IList<int> withoutIDs)
        {
            IList<SparePart> spareParts = new List<SparePart>();

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

                var query = "SELECT * FROM SpareParts WHERE Title LIKE @Title AND SparePartId NOT IN(" + notIn + ") LIMIT @Limit;";
                
                cmd.Parameters.AddWithValue("@Title", title + "%");
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
        /// Возвращает список всех SparePart найденных по совпадению с заданным артикулом.
        /// </summary>
        /// <param name="articul">Артикул по которому ищутся совпадения.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSparePartsByArticul(string articul)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM SpareParts WHERE Articul LIKE @Articul;", connection);

                cmd.Parameters.AddWithValue("@Articul", articul + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart();
                    sparePart = CreateSparePart(dataReader);

                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;
        }//SearchSparePartsByArticul
        /// <summary>
        /// Возвращает список всех SparePart размером не более limit, найденных по совпадению с заданным артикулом.
        /// </summary>
        /// <param name="articul">Артикул по которому ищутся совпадения.</param>
        /// <param name="limit">Максимально возможное кол-во эл-тов.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSparePartsByArticul(string articul, int limit)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM SpareParts WHERE Articul LIKE @Articul LIMIT @Limit;", connection);

                cmd.Parameters.AddWithValue("@Articul", articul + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart();
                    sparePart = CreateSparePart(dataReader);

                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;
        }//SearchSparePartsByArticul
        /// <summary>
        /// Возвращает список всех SparePart размером не более limit, найденных по совпадению с заданным артикулом.
        /// </summary>
        /// <param name="articul">Артикул по которому ищутся совпадения.</param>
        /// <param name="limit">Максимально возможное кол-во эл-тов.</param>
        /// <param name="withoutIDs">Список Id товара который не должен входить в результирующий список.</param>
        /// <returns></returns>
        public static IList<SparePart> SearchSparePartsByArticul(string articul, int limit, IList<int> withoutIDs)
        {
            IList<SparePart> spareParts = new List<SparePart>();

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

                var query = "SELECT * FROM  SpareParts WHERE Articul LIKE @Articul AND SparePartId NOT IN(" + notIn + ") LIMIT @Limit;";

                cmd.Parameters.AddWithValue("@Articul", articul + "%");
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
        /// Возвращает массив строк, состоящий из названий всех spareParts совпадающих с переданным параметром.
        /// </summary>
        /// <param name="title">Строка по которой нужно искать совпадение.</param>
        /// <returns></returns>
        public static string[] SearchSparePartsTitleByTitle(string title)
        {
            IList<string> titlesList = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT Title FROM SpareParts WHERE Title LIKE @Title;", connection);

                cmd.Parameters.AddWithValue("@Title", title + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    titlesList.Add(dataReader["Title"] as string);
                }//while
                connection.Close();
            }//using

            //создаём массив string.
            string[] titles = new string[titlesList.Count];
            for (int i = 0; i < titles.Length; ++i)
                titles[i] = titlesList[i];

            return titles;        
        }//SearchSparePartsTitle
        /// <summary>
        /// Возвращает массив строк, состоящий из названий всех spareParts совпадающих с переданным параметром.
        /// </summary>
        /// <param name="title">Строка по которой нужно искать совпадение.</param>
        /// <param name="limit">Ограничение по максимальному кол-ву эл-тов.</param>
        /// <returns></returns>
        public static string[] SearchSparePartsTitleByTitle(string title, int limit)
        {
            IList<string> titlesList = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT Title FROM SpareParts WHERE Title LIKE @Title LIMIT @Limit;", connection);

                cmd.Parameters.AddWithValue("@Title", title + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    titlesList.Add(dataReader["Title"] as string);
                }//while
                connection.Close();
            }//using

            //создаём массив string.
            string[] titles = new string[titlesList.Count];
            for (int i = 0; i < titles.Length; ++i)
                titles[i] = titlesList[i];

            return titles;
        }//SearchSparePartsTitle
        /// <summary>
        /// Возвращает массив строк, состоящий из артикулов всех spareParts совпадающих с переданным параметром.
        /// </summary>
        /// <param name="title">Строка по которой нужно искать совпадение.</param>
        /// <returns></returns>
        public static string[] SearchSparePartsArticulByArticul(string articul)
        {
            IList<string> articulsList = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT Articul FROM SpareParts WHERE Articul LIKE @Articul", connection);

                cmd.Parameters.AddWithValue("@Articul", articul + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    articulsList.Add(dataReader["Articul"] as string);
                }//while
                connection.Close();
            }//using

            //создаём массив string.
            string[] articuls = new string[articulsList.Count];
            for (int i = 0; i < articuls.Length; ++i)
                articuls[i] = articulsList[i];

            return articuls;   
        }//SearchSparePartsArticul
        /// <summary>
        /// Возвращает массив строк, состоящий из артикулов всех spareParts совпадающих с переданным параметром.
        /// </summary>
        /// <param name="title">Строка по которой нужно искать совпадение.</param>
        /// <param name="limit">Ограничение по максимальному кол-ву эл-тов.</param>
        /// <returns></returns>
        public static string[] SearchSparePartsArticulByArticul(string articul, int limit)
        {
            IList<string> articulsList = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT Articul FROM SpareParts WHERE Articul LIKE @Articul LIMIT @Limit", connection);

                cmd.Parameters.AddWithValue("@Articul", articul + "%");
                cmd.Parameters.AddWithValue("@Limit", limit);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    articulsList.Add(dataReader["Articul"] as string);
                }//while
                connection.Close();
            }//using

            //создаём массив string.
            string[] articuls = new string[articulsList.Count];
            for (int i = 0; i < articuls.Length; ++i)
                articuls[i] = articulsList[i];

            return articuls;
        }//SearchSparePartsArticul       

        //Поиск по Manufacturer.
        /// <summary>
        /// Возвращает массив строк состоящий из имен производителей подходящих под заданный параметр.
        /// </summary>
        /// <param name="manufacturerName">Имя по которому ищутся совпадения.</param>
        /// <returns></returns>
        public static string[] SearchManufacturersName(string manufacturerName)
        {
            IList<string> manufacturersName = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT ManufacturerName FROM Manufacturers WHERE ManufacturerName LIKE @ManufacturerName;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerName + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    manufacturersName.Add(dataReader["ManufacturerName"] as string);
                }//while

                connection.Close();
            }//using

            //создаём массив string.
            string[] manuf = new string[manufacturersName.Count];
            for (int i = 0; i < manuf.Length; ++i)
                manuf[i] = manufacturersName[i];

            return manuf;
        }//FindAllManufacturersName
        /// <summary>
        /// Возвращает массив строк состоящий из имен производителей подходящих под заданный параметр.
        /// </summary>
        /// <param name="manufName">Имя по которому ищутся совпадения.</param>
        /// <param name="limit">Ограничение по максимально возможному кол-ву найденных совпадений.</param>
        /// <returns></returns>
        public static string[] SearchManufacturersName(string manufacturerName, int limit)
        {
            IList<string> manufacturersName = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT ManufacturerName FROM Manufacturers WHERE ManufacturerName LIKE @ManufacturerName LIMIT @limit;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerName + "%");
                cmd.Parameters.AddWithValue("@limit", limit);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    manufacturersName.Add(dataReader["ManufacturerName"] as string);
                }//while

                connection.Close();
            }//using

            //создаём массив string.
           string[] manuf = new string[manufacturersName.Count];
            for (int i = 0; i < manuf.Length; ++i)
                manuf[i] = manufacturersName[i];

            return manuf;
        }//FindAllManufacturersName

        //Поиск по Supplier.
        /// <summary>
        /// Возвращает массив строк состоящий из имен поставщиков подходящих под заданный параметр.
        /// </summary>
        /// <param name="supplierName">Имя по которому ищутся совпадения.</param>
        /// <returns></returns>
        public static string[] SearchSuppliersName(string supplierName)
        {
            IList<string> suppliersName = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT SupplierName FROM Suppliers WHERE SupplierName LIKE @SupplierName;", connection);

                cmd.Parameters.AddWithValue("@SupplierName", supplierName + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    suppliersName.Add(dataReader["SupplierName"] as string);
                }//while
                connection.Close();
            }//using

            //создаём массив string.
            string[] suppliers = new string[suppliersName.Count];
            for (int i = 0; i < suppliers.Length; ++i)
                suppliers[i] = suppliersName[i];

            return suppliers;
        }//SearchSuppliersName
        /// <summary>
        /// Возвращает массив строк состоящий из имен поставщиков подходящих под заданный параметр.
        /// </summary>
        /// <param name="supplierName">Имя по которому ищутся совпадения.</param>
        /// <param name="limit">Ограничение по максимально возможному кол-ву найденных совпадений.</param>
        /// <returns></returns>
        public static string[] SearchSuppliersName(string supplierName, int limit)
        {
            IList<string> suppliersName = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT SupplierName FROM Suppliers WHERE SupplierName LIKE @SupplierName LIMIT @limit;", connection);

                cmd.Parameters.AddWithValue("@SupplierName", supplierName + "%");
                cmd.Parameters.AddWithValue("@limit", limit);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    suppliersName.Add(dataReader["SupplierName"] as string);
                }//while
                connection.Close();
            }//using

            //создаём массив string.
            string[] suppliers = new string[suppliersName.Count];
            for (int i = 0; i < suppliers.Length; ++i)
                suppliers[i] = suppliersName[i];

            return suppliers;
        }//SearchSuppliersName





        #region Поиск по таблице Customers.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает коллекцию Customer по совпадению с заданным именем.
        /// </summary>
        /// <param name="customerName">Имя по совпадению с которым надо найти Customer-ов.</param>
        /// <returns></returns>
        public static IList<Customer> SearchCustomers(string customerName)
        {
            IList<Customer> customers = new List<Customer>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Customers WHERE CustomerName LIKE @CustomerName;", connection);

                cmd.Parameters.AddWithValue("@CustomerName", customerName + "%");
                
                var dataReader = cmd.ExecuteReader();
                
                while (dataReader.Read())
                {
                    Customer customer = new Customer();
                    customer.ContragentId = Convert.ToInt32(dataReader["CustomerId"]);
                    customer.ContragentName = dataReader["CustomerName"] as string;
                    customer.Code = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    customer.Entity = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    customer.ContactInfo = (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfoById(Convert.ToInt32(dataReader["ContactInfoId"])) : null;
                    customer.Description = (dataReader["Description"] == DBNull.Value) ? null : dataReader["Description"] as string;

                    customers.Add(customer);
                }//while

                connection.Close();
            }//using
            return customers;
        }//FindCustomerIdByName
























//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion





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
            SparePart sparePart = new SparePart
            (
                sparePartId    : Convert.ToInt32(dataReader["SparePartId"]),
                photo          : dataReader["Photo"] as string,
                articul        : dataReader["Articul"] as string,
                title          : dataReader["Title"] as string,
                description    : (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string,                
                manufacturerId : (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]),                              
                measureUnit           : dataReader["MeasureUnit"] as string             
            );

            return sparePart;        
        }//CreateSparePart
        /// <summary>
        /// /// Возвращает полный объект SparePart созданный из переданного dataReader.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private static SparePart CreateFullSparePart(SQLiteDataReader dataReader)
        {
            SparePart sparePart = new SparePart
            (
                sparePartId: Convert.ToInt32(dataReader["SparePartId"]),
                photo: (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                articul: dataReader["Articul"] as string,
                title: dataReader["Title"] as string,
                description: (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string,
                manufacturerId: (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]),                                
                purchaseId: Convert.ToInt32(dataReader["PurchaseId"]),
                measureUnit: dataReader["MeasureUnit"] as string,
                storageAdress : dataReader["StorageAdress"] as string,
                count: Convert.ToDouble(dataReader["Count"]),
                price: Convert.ToDouble(dataReader["Price"]),
                markup: (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"])
            );

            return sparePart;
        }//CreateFullSparePart
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





        //public static IList<Purchase> FindPurchasesByParameters(Purchase sale)
        //{
        //    IList<Purchase> sparePartsList = new List<Purchase>();

        //    using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
        //    {
        //        connection.Open();

        //        const string query = "SELECT * FROM Purchases WHERE PurchaseId = @PurchaseId AND ";
        //        SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId AND sp.Articul LIKE @Articul", connection);

        //        cmd.Parameters.AddWithValue("@Articul", articul + "%");

        //        var dataReader = cmd.ExecuteReader();
        //        while (dataReader.Read())
        //        {
        //            Purchase sale = new Purchase();


        //            sparePartsList.Add(sale);
        //        }//while
        //        connection.Close();
        //    }//using

        //    return sparePartsList;
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

/*На будущее*/
//1) Разобраться с Enum и добавить fieldNames
//2)Перенести определение ManufacturerName в методы класса PartsDAL (CreateSparePart, ...).