using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace PartsApp
{
    static class PartsDAL
    {
        private const string SparePartConfig = "SparePartConfig";
        //public enum FieldNames{  SparePartId = "SparePartId", Photo = "Photo", Articul = "Articul", Title = "Title", 
        //                               ExtInfoId = "ExtInfoId", Description = "Description", ManufacturerId = "ManufacturerId", 
        //                             Unit = "Unit", PurchaseId = "PurchaseId", Price = "Price", Markup = "Markup", Count = "Count"};

        #region ************Модификация данных в БД.****************************************************************************
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*Модификация таблицы Avaliability.*/
        /// <summary>
        /// Добавляет запись в таблицу Avaliability.
        /// </summary>
        /// <param name="sparePart">Запись добавляемая в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void AddSparePartAvaliability(SparePart sparePart, SQLiteCommand cmd)
        {
            var query = "INSERT INTO Avaliability VALUES (@SparePartId, @PurchaseId, @Price, @Markup, @Storage, @StorageAdress, @Count);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();
            
            cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);
            cmd.Parameters.AddWithValue("@PurchaseId", sparePart.PurchaseId);
            cmd.Parameters.AddWithValue("@Price", sparePart.Price);
            cmd.Parameters.AddWithValue("@Markup", sparePart.Markup);
            cmd.Parameters.AddWithValue("@StorageAdress", sparePart.StorageAdress);
            if (sparePart.Count == 0)
            {
                cmd.Parameters.AddWithValue("@Storage", SparePart.VirtStorage);
                cmd.Parameters.AddWithValue("@Count", sparePart.VirtCount);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Storage", SparePart.MainStorage);
                cmd.Parameters.AddWithValue("@Count", sparePart.Count);
            }

            cmd.ExecuteNonQuery();    
        }//AddSparePartAvaliability 
        public static void AddSparePartAvaliability(SparePart sparePart, SQLiteConnection openConnection)
        {
            var cmd = new SQLiteCommand("INSERT INTO Avaliability VALUES (@SparePartId, @PurchaseId, @Price, @Markup, @Storage, @Count);", openConnection);

            cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);
            cmd.Parameters.AddWithValue("@PurchaseId", sparePart.PurchaseId);
            cmd.Parameters.AddWithValue("@Price", sparePart.Price);
            cmd.Parameters.AddWithValue("@Markup", sparePart.Markup);
            if (sparePart.Count == 0)
            {
                cmd.Parameters.AddWithValue("@Storage", SparePart.VirtStorage);
                cmd.Parameters.AddWithValue("@Count", sparePart.VirtCount);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Storage", SparePart.MainStorage);
                cmd.Parameters.AddWithValue("@Count", sparePart.Count);
            }

            cmd.ExecuteNonQuery();
        }//AddSparePartAvaliability
        /// <summary>
        /// Обновляет количество в заданной записи таблицы Avaliability.
        /// </summary>
        /// <param name="sparePartId">Ид товара искомой записи</param>
        /// <param name="purchaseId">Ид прихода искомой записи</param>        
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
        /// <param name="purchaseId">Id прихода с изменяемой наценкой</param>
        /// <param name="markup">Значение наценки на которое стоит поменять текущее значение.</param>
        public static void UpdateSparePartMarkup(int sparePartId, int purchaseId, double markup)
        {

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                var cmd = new SQLiteCommand("UPDATE Avaliability SET Markup = @Markup WHERE SparePartId = @SparePartId AND PurchaseId = @PurchaseId;", connection);

                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
                cmd.Parameters.AddWithValue("@Markup", markup);
                cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using 
        }//UpdateSparePartMarkup
        /// <summary>
        /// Метод обновления значения Markup у записей с заданным SparePartId и PurchaseId.
        /// </summary>
        /// <param name="sparePartId">Id запчасти с изменяемой наценкой</param>
        /// <param name="purchaseId">Id прихода с изменяемой наценкой</param>
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
        /// <param name="sparePartsId">Список Id запчастей изменяемых записей.</param>
        /// <param name="purchasesId">Список Id приходов изменяемых записей</param>
        /// <param name="markups">Значения наценки на которое стоит поменять текущие значения.</param>
        //public static void UpdateSparePartMarkup(IList<int> sparePartsId, IList<int> purchasesId, IList<double> markups)
        //{
        //    //Проверяем равенство размеров всех переданных коллекций.
        //    if (sparePartsId.Count != purchasesId.Count || purchasesId.Count != markups.Count)
        //        throw new InvalidEnumArgumentException("Кол-во записей в передаваемых коллекциях не совпадает");

        //    using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
        //    {
        //        connection.Open();

        //        for (int i = 0; i < sparePartsId.Count; ++i)
        //            UpdateSparePartMarkup(sparePartsId[i], purchasesId[i], markups[i], connection);
                
        //        connection.Close();
        //    }//using 
        //}//UpdateSparePartMarkup
        /// <summary>
        /// Изменяет наценку у записей с заданными SparePartId и PurchaseId на заданную Markup
        /// </summary>
        /// <param name="changeMarkupDict">Словарь типа (sparePartId, IDictionary(purchaseId, markup))</param>
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
        /// <param name="purchaseId">Ид прихода искомой записи</param>
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

        //Модификация таблицы SpareParts.
        public static void AddSparePart(SparePart sparePart)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                //Вставляем запись в табл. "SparePart"
                const string query = "INSERT INTO SpareParts(Photo, Articul, Title, Description, ExtInfoId, ManufacturerId, Unit) " +
                                     "VALUES(@Photo, @Articul, @Title, @Description, @ExtInfoId, @ManufacturerId, @Unit);";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@Photo", sparePart.Photo);
                cmd.Parameters.AddWithValue("@Articul", sparePart.Articul);
                cmd.Parameters.AddWithValue("@Title", sparePart.Title);
                cmd.Parameters.AddWithValue("@Description", sparePart.Description);
                cmd.Parameters.AddWithValue("@ExtInfoId", sparePart.ExtInfoId);
                cmd.Parameters.AddWithValue("@ManufacturerId", sparePart.ManufacturerId);
                cmd.Parameters.AddWithValue("@Unit", sparePart.Unit);

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
                                   + "Description = @Description, ExtInfoId = @ExtInfoId, ManufacturerId = @ManufacturerId, Unit = @Unit " 
                                   + "WHERE SparePartId = @SparePartId;";
                 

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);
                cmd.Parameters.AddWithValue("@Photo", sparePart.Photo);
                cmd.Parameters.AddWithValue("@Articul", sparePart.Articul);
                cmd.Parameters.AddWithValue("@Title", sparePart.Title);
                cmd.Parameters.AddWithValue("@Description", sparePart.Description);
                cmd.Parameters.AddWithValue("@ExtInfoId", sparePart.ExtInfoId);
                cmd.Parameters.AddWithValue("@ManufacturerId", sparePart.ManufacturerId);
                cmd.Parameters.AddWithValue("@Unit", sparePart.Unit);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using
        
        }//UpdateSparePart

        //Модификация таблицы Suppliers.
        public static void AddSupplier(Contragent supplier)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                //Вставляем запись в табл. "Supplier"
                var cmd = new SQLiteCommand("INSERT INTO Suppliers(SupplierName, Code, Entity, ContactInfoId, Description) VALUES (@SupplierName, @Code, @Entity, @ContactInfoId, @Description);", connection);

                cmd.Parameters.AddWithValue("@SupplierName", supplier.ContragentName);
                cmd.Parameters.AddWithValue("@Code", supplier.Code);
                cmd.Parameters.AddWithValue("@Entity", supplier.Entity);
                cmd.Parameters.AddWithValue("@ContactInfoId", supplier.ContactInfoId);
                cmd.Parameters.AddWithValue("@Description", supplier.Description);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using

        }//AddSupplier

        //Модификация таблицы Customers.
        public static void AddCustomer(Contragent customer)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                //Вставляем запись в табл. "customer"
                var cmd = new SQLiteCommand("INSERT INTO customers(CustomerName, Code, Entity, ContactInfoId, Description) VALUES (@CustomerName, @Code, @Entity, @ContactInfoId, @Description);", connection);

                cmd.Parameters.AddWithValue("@CustomerName", customer.ContragentName);
                cmd.Parameters.AddWithValue("@Code", customer.Code);
                cmd.Parameters.AddWithValue("@Entity", customer.Entity);
                cmd.Parameters.AddWithValue("@ContactInfoId", customer.ContactInfoId);
                cmd.Parameters.AddWithValue("@Description", customer.Description);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using

        }//AddCustomer
        //Модификация таблицы SpSuppliers.
        public static void AddSparePartsSuppliers(int sparePartId, int supplierId)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                var cmd = new SQLiteCommand("INSERT INTO SpSuppliers VALUES (@SparePartId, @SupplierId)", connection);

                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
                cmd.Parameters.AddWithValue("@SupplierId", supplierId);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using
        }//AddSparePartsSuppliers
        public static void AddSparePartsSuppliers(int sparePartId, int supplierId, SQLiteConnection openConnection)
        {
            var cmd = new SQLiteCommand("INSERT INTO SpSuppliers VALUES (@SparePartId, @SupplierId)", openConnection);

            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@SupplierId", supplierId);

            cmd.ExecuteNonQuery();
        }//AddSparePartsSuppliers

        //Модификация таблицы Categories.
        public static void AddCategory(string categoryName, string description)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                //Вставляем запись в табл. "Category"
                var cmd = new SQLiteCommand("INSERT INTO Categories(CategoryName, Description) VALUES(@CategoryName, @Description);", connection);

                cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                cmd.Parameters.AddWithValue("@Description", description);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using
        }//AddCategory
        public static void AddCategory(string categoryName, string description, SQLiteConnection openConnection)
        {
            //Вставляем запись в табл. "Categories"
            var cmd = new SQLiteCommand("INSERT INTO Categories(CategoryName, Description) VALUES(@CategoryName, @Description);", openConnection);

            cmd.Parameters.AddWithValue("@CategoryName", categoryName);
            cmd.Parameters.AddWithValue("@Description", description);

            cmd.ExecuteNonQuery();
        }//AddCategory

        //Модификация таблицы SpCategories.
        public static void AddSparePartsCategories(int sparePartId, int categoryId)
        {
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                var cmd = new SQLiteCommand("INSERT INTO SpCategories VALUES (@SparePartId, @CategoryId)", connection);

                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                cmd.ExecuteNonQuery();

                connection.Close();
            }//using      
        }//AddSparePartsCategories
        public static void AddSparePartsCategories(int sparePartId, int categoryId, SQLiteConnection openConnection)
        {
            var cmd = new SQLiteCommand("INSERT INTO SpCategories VALUES (@SparePartId, @CategoryId)", openConnection);

            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);

            cmd.ExecuteNonQuery();
        }//AddSparePartsCategories

        //Модификация таблицы Manufacturers
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


        //Модификация таблицы ContactInfo.
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
                string query = String.Format("INSERT INTO ContactInfo (Country, Region, City, Street, House, Room, Phone, ExtPhone1, ExtPhone2, Website, Email)"
                                           + "VALUES (@Country, @Region, @City, @Street, @House, @Room, @Phone, @ExtPhone1, @ExtPhone2, @Website, @Email);"
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
                cmd.Parameters.AddWithValue("@ExtPhone1", contactInfo.ExtPhone1);
                cmd.Parameters.AddWithValue("@ExtPhone2", contactInfo.ExtPhone2);
                cmd.Parameters.AddWithValue("@Website",   contactInfo.Website);
                cmd.Parameters.AddWithValue("@Email",     contactInfo.Email);

                id = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }//using
            return id;
        }//AddContactInfo
        /// <summary>
        /// Метод добавляет новую запись в таблицу ContactInfo и возвращает Id вставленной записи.
        /// </summary>
        /// <param name="contactInfo">объект типа ContactInfo данные которого будут добавлены в базу</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        /// <returns></returns>
        public static int AddContactInfo(ContactInfo contactInfo, SQLiteConnection openConnection)
        {
            string query = String.Format("INSERT INTO ContactInfo (Country, Region, City, Street, House, Room, Phone, ExtPhone1, ExtPhone2, Website, Email)"
                                       + "VALUES (@Country, @Region, @City, @Street, @House, @Room, @Phone, @ExtPhone1, @ExtPhone2, @Website, @Email);"
                                       + "SELECT ContactInfoId FROM ContactInfo WHERE rowid = last_insert_rowid();");

            var cmd = new SQLiteCommand(query, openConnection);

            cmd.Parameters.AddWithValue("@Country", contactInfo.Country);
            cmd.Parameters.AddWithValue("@Region", contactInfo.Region);
            cmd.Parameters.AddWithValue("@City", contactInfo.City);
            cmd.Parameters.AddWithValue("@Street", contactInfo.Street);
            cmd.Parameters.AddWithValue("@House", contactInfo.House);
            cmd.Parameters.AddWithValue("@Room", contactInfo.Room);
            cmd.Parameters.AddWithValue("@Phone", contactInfo.Phone);
            cmd.Parameters.AddWithValue("@ExtPhone1", contactInfo.ExtPhone1);
            cmd.Parameters.AddWithValue("@ExtPhone2", contactInfo.ExtPhone2);
            cmd.Parameters.AddWithValue("@Website", contactInfo.Website);
            cmd.Parameters.AddWithValue("@Email", contactInfo.Email);    
              
            return Convert.ToInt32(cmd.ExecuteScalar());
        }//AddContactInfo

        //Модификация таблиц Purchase и Avaliability.
/*!!! Возможно можно убрать параметр IList<SparePart> передавая его св-вом объекта класса Purchase, PurchaseDetails. Надобность класса PurchaseDetail вообще под вопросом, ведь его можно спокойно заменить объектом уже созданного класса SparePart либо же класс SparePart нужно модифицировать в сторону разбиения на большее кол-во классов!*/
        /// <summary>
        /// Осуществляет полный цикл приходования товара, вставляя записи в таблицы Purchases, Avaliability и PurchaseDetails.
        /// Возвращает Id вставленной записи в табл. Purchase.
        /// </summary>
        /// <param name="spareParts">Список приходуемого товара.</param>
        /// <param name="purchase">Информация о приходе.</param>
        /// <returns></returns>
        public static int AddPurchase(IList<SparePart> spareParts, Purchase purchase)
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
                            foreach (SparePart sp in spareParts)
                            {
                                sp.PurchaseId = purchaseId;
                                var purchaseDetail = new PurchaseDetail(sp.PurchaseId, sp.SparePartId, (double)sp.Price, sp.Count);
                                AddPurchaseDetail(purchaseDetail, cmd);
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

        //Модификация таблицы Purchases
        /// <summary>
        /// Возвращает Id вставленной записи в таблицу Purchases.
        /// </summary>
        /// <param name="purchase">Приход который нужно добавить в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        /// <returns></returns>
        private static int AddPurchase(Purchase purchase, SQLiteCommand cmd)
        {
            int purchaseId = 0;
            
            string query = String.Format("INSERT INTO Purchases (EmployeeID, SupplierId, SupplierEmployee, PurchaseDate, Currency, ExcRate, Description)"
                                       + "VALUES (@EmployeeID, @SupplierId, @SupplierEmployee, @PurchaseDate, @Currency, @ExcRate, @Description);"
                                       + "SELECT PurchaseId FROM Purchases WHERE rowid = last_insert_rowid();");

            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@EmployeeID", purchase.EmployeeId);
            cmd.Parameters.AddWithValue("@SupplierId", purchase.SupplierId);
            cmd.Parameters.AddWithValue("@SupplierEmployee", purchase.SupplierEmployee);

            cmd.Parameters.AddWithValue("@Currency", purchase.Currency);
            cmd.Parameters.AddWithValue("@ExcRate", purchase.ExcRate);
            cmd.Parameters.AddWithValue("@Description", purchase.Description);

            //Переводим время в Utc формат.
            //DateTime dt = TimeZoneInfo.ConvertTimeToUtc(purchase.PurchaseDate);
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan tsInterval = purchase.PurchaseDate.Subtract(dt1970);
            Int32 seconds = Convert.ToInt32(tsInterval.TotalSeconds);

            cmd.Parameters.AddWithValue("@PurchaseDate", seconds);

            purchaseId = Convert.ToInt32(cmd.ExecuteScalar());     
                   
            return purchaseId;
        }//AddPurchase


        //Модификация таблицы PurchaseDetails
        /// <summary>
        /// Добавляет запись в таблицу PurchaseDetails.
        /// </summary>
        /// <param name="purchaseDetail">Запись добавляемая в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void AddPurchaseDetail(PurchaseDetail purchaseDetail, SQLiteCommand cmd)
        {
            string query = "INSERT INTO PurchaseDetails VALUES (@PurchaseId, @SparePartId, @Price, @Quantity);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@PurchaseId", purchaseDetail.PurchaseId);
            cmd.Parameters.AddWithValue("@SparePartId", purchaseDetail.SparePartId);
            cmd.Parameters.AddWithValue("@Price", purchaseDetail.Price);
            cmd.Parameters.AddWithValue("@Quantity", purchaseDetail.Quantity);

            cmd.ExecuteNonQuery();                
        }//AddPurchaseDetail

        //Модификация таблицы Sales и Avaliability.
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
                                AddSaleDetail(saleId, sp.SparePartId, (double)sp.Price, sp.Count, 0, cmd);

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

            var query = String.Format("INSERT INTO Sales (EmployeeID, CustomerId, CustomerEmployee, SaleDate, Currency, ExcRate, Description) "
                                    + "VALUES (@EmployeeID, @CustomerId, @CustomerEmployee, @SaleDate, @Currency, @ExcRate, @Description); "
                                    + "SELECT SaleId FROM Sales WHERE rowid = last_insert_rowid();");

            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@EmployeeID", sale.EmployeeId);
            cmd.Parameters.AddWithValue("@CustomerId", sale.CustomerId);
            cmd.Parameters.AddWithValue("@CustomerEmployee", sale.CustomerEmployee);

            cmd.Parameters.AddWithValue("@Currency", sale.Currency);
            cmd.Parameters.AddWithValue("@ExcRate", sale.ExcRate);
            cmd.Parameters.AddWithValue("@Description", sale.Description);
            
            //DateTime dt = TimeZoneInfo.ConvertTimeToUtc(sale.SaleDate);
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan tsInterval = sale.SaleDate.Subtract(dt1970);
            Int32 seconds = Convert.ToInt32(tsInterval.TotalSeconds);

            cmd.Parameters.AddWithValue("@SaleDate", seconds);

            saleId = Convert.ToInt32(cmd.ExecuteScalar());

            return saleId;
        }//AddSale

        //Модификация таблицы SaleDetails
        /// <summary>
        /// Добавляет запись в таблицу SaleDetails.
        /// </summary>
        /// <param name="saleId">Ид продажи</param>
        /// <param name="sparePartId">Ид товара</param>
        /// <param name="sellingPrice">Отпускная цена товара</param>
        /// <param name="quantity">Кол-во товара</param>
        /// <param name="discount">Процент скидки</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void AddSaleDetail(int saleId, int sparePartId, double sellingPrice, double quantity, double discount, SQLiteCommand cmd)
        {
            string query = "INSERT INTO SaleDetails VALUES (@SaleId, @SparePartId, @Quantity, @Discount, @SellingPrice);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@SaleId", saleId);
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@Discount", discount);
            cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
            
            cmd.ExecuteNonQuery();
        }//AddSaleDetail



////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region ************Точный поиск по БД.*********************************************************************************
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       

        #region *****************Поиск по таблицам Avaliablility********************************************************************

        

        //Поиск по полям таблицы Avaliability.
        //Нахождение всех данных в таблице, без какой-либо фильтрации.
        public static IList<SparePart> FindAllAvaliability()
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId ;", connection);
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart(); 
                    #region //конструктор
                    /*
                    {
                        Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                        SparePartId= Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        //Price = Convert.ToDouble(dataReader["Price"]),
                        //Markup = Convert.ToInt32(dataReader["Markup"]),
                        //Count = Convert.ToDouble(dataReader["Count"]),
                        //Unit = dataReader["Unit"] as string
                    }; 
                    */
                    #endregion
                    sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                    sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                    sparePart.Articul = dataReader["Articul"] as string;
                    sparePart.Title = dataReader["Title"] as string;

                    sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                    sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                    sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                    //sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, connection);
                    //sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);

                    sparePart.Unit = dataReader["Unit"] as string;
                    sparePart.PurchaseId = Convert.ToInt32(dataReader["PurchaseId"]);
                    sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                    sparePart.Markup = (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"]);                 

                    //sparePart.Storage = dataReader["Storage"] as string;

                    if (dataReader["Storage"] as string == SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                    else sparePart.VirtCount = Convert.ToDouble(dataReader["Count"]);

                    //sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                    //sparePart.Avaliability = (sparePart.Storage !=SparePart.VirtStorage) ? sparePart.Count.ToString() : String.Format("({0})", sparePart.Count);

                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using
            return spareParts;
        }//FindAllSparePartsAvaliability
        public static IList<SparePart> FindAllAvaliability(SQLiteConnection openConnection)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId ;", openConnection);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                SparePart sparePart = new SparePart();
                #region //конструктор
                /*
                    {
                        Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                        SparePartId= Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        //Price = Convert.ToDouble(dataReader["Price"]),
                        //Markup = Convert.ToInt32(dataReader["Markup"]),
                        //Count = Convert.ToDouble(dataReader["Count"]),
                        //Unit = dataReader["Unit"] as string
                    }; 
                    */
                #endregion
                sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                sparePart.Articul = dataReader["Articul"] as string;
                sparePart.Title = dataReader["Title"] as string;

                sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                //sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, openConnection);
                //sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);

                sparePart.Unit = dataReader["Unit"] as string;
                sparePart.PurchaseId = Convert.ToInt32(dataReader["PurchaseId"]);
                sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                sparePart.Markup = (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"]);
                //sparePart.Storage = dataReader["Storage"] as string;
                if (dataReader["Storage"] as string == SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                else sparePart.VirtCount = Convert.ToDouble(dataReader["Count"]);

                //sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                //sparePart.Avaliability = (sparePart.Storage !=SparePart.VirtStorage) ? sparePart.Count.ToString() : String.Format("({0})", sparePart.Count);

                spareParts.Add(sparePart);
            }//while
            return spareParts;
        }//FindAllSparePartsAvaliability
        /// <summary>
        /// Возвращает единицу товара найденную по заданным параметрам.
        /// </summary>
        /// <param name="sparePartId">Ид товара искомой записи</param>
        /// <param name="purchaseId">Ид прихода искомой записи</param>
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
        /// Возвращает единицу товара найденную по заданным параметрам.
        /// </summary>
        /// <param name="sparePartId">Ид товара искомой записи</param>
        /// <param name="purchaseId">Ид прихода искомой записи</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        /// <returns></returns>
        public static SparePart FindSparePartAvaliability(int sparePartId, int purchaseId, SQLiteCommand cmd)
        {
            SparePart sparePart = new SparePart();
            cmd.Parameters.Clear();

            const string query = "SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId "
                               + "AND av.SparePartId = @SparePartId AND PurchaseId = @PurchaseId;";
            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@PurchaseId",  purchaseId);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                sparePart = CreateFullSparePart(dataReader);
            }//while    

            return sparePart;
        }//FindSparePartAvaliability
        /// <summary>
        /// Возвращает количество в наличии заданной единицы товара.
        /// </summary>
        /// <param name="sparePartId">Ид товара искомой записи</param>
        /// <param name="purchaseId">Ид прихода искомой записи</param>
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

        //Нахождение SparePart без разделения на приходы
        public static IList<SparePart> FindAllUniqueSparePartsAvaliability()
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId GROUP BY av.SparePartId;", connection);
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart();
                    #region //конструктор
                    /*
                    {
                        Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                        SparePartId= Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        //Price = Convert.ToDouble(dataReader["Price"]),
                        //Markup = Convert.ToInt32(dataReader["Markup"]),
                        //Count = Convert.ToDouble(dataReader["Count"]),
                        //Unit = dataReader["Unit"] as string
                    }; 
                    */
                    #endregion
                    sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                    sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                    sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                    sparePart.Articul = dataReader["Articul"] as string;
                    sparePart.Title = dataReader["Title"] as string;

                    sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                    sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                    sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                    //sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, connection);
                    //sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);

                    sparePart.Unit = dataReader["Unit"] as string;
                    //sparePart.PurchaseId = Convert.ToInt32(dataReader["PurchaseId"]);
                    //sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                    //sparePart.Markup = (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"]);
                    //sparePart.Storage = dataReader["Storage"] as string;
                    //sparePart.Count = Convert.ToDouble(dataReader["Count"]);

                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using
            return spareParts;
        }//FindAllUniqueSparePartsAvaliability
        public static IList<SparePart> FindAllUniqueSparePartsAvaliability(SQLiteConnection openConnection)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId GROUP BY av.SparePartId;", openConnection);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                SparePart sparePart = new SparePart();
                #region //конструктор
                /*
                    {
                        Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                        SparePartId= Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        //Price = Convert.ToDouble(dataReader["Price"]),
                        //Markup = Convert.ToInt32(dataReader["Markup"]),
                        //Count = Convert.ToDouble(dataReader["Count"]),
                        //Unit = dataReader["Unit"] as string
                    }; 
                    */
                #endregion
                sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                sparePart.Articul = dataReader["Articul"] as string;
                sparePart.Title = dataReader["Title"] as string;

                sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                //sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, openConnection);
                //sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);
                sparePart.Unit = dataReader["Unit"] as string;

                spareParts.Add(sparePart);
            }//while     

            return spareParts;
        }//FindAllUniqueSparePartsAvaliability
        ///// <summary>
        ///// Возвращает SP полностью заполненный полями из таблиц SpareParts и Avaliability.
        ///// </summary>
        ///// <param name="sparePartId">ИД искомой SP.</param>
        ///// <returns></returns>
        //public static SparePart FindSparePartAvaliabilityBySparePartId(int sparePartId)
        //{
        //    SparePart sparePart = new SparePart();

        //    using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
        //    {
        //        connection.Open();

        //        const string query = "SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId" 
        //                             + " AND sp.SparePartId = @SparePartId";

        //        SQLiteCommand cmd = new SQLiteCommand(connection);

        //        cmd.CommandText = query;
        //        cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
        //        cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

        //        var dataReader = cmd.ExecuteReader();
        //        CreateFullSparePart(dataReader);

        //        connection.Close();
        //    }//using
        //    return sparePart;
        //}//FindSparePartAvaliabilityByIds
        ///// <summary>
        ///// Возвращает SP полностью заполненный полями из таблиц SpareParts и Avaliability.
        ///// </summary>
        ///// <param name="sparePartId">ИД искомой SP.</param>
        ///// <param name="purchaseId">Id прихода.</param>
        ///// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        ///// <returns></returns>
        //public static SparePart FindSparePartAvaliabilityByIds(int sparePartId, int purchaseId, SQLiteConnection openConnection)
        //{
        //    SparePart sparePart = new SparePart();

        //    const string query = "SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId"
        //                             + " AND sp.SparePartId = @SparePartId AND av.PurchaseId = @PurchaseId;";

        //    SQLiteCommand cmd = new SQLiteCommand(openConnection);

        //    cmd.CommandText = query;
        //    cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
        //    cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

        //    var dataReader = cmd.ExecuteReader();
        //    CreateFullSparePart(dataReader);

        //    return sparePart;                
        //}//FindSparePartAvaliabilityByIds
        //Нахождение общего кол-ва по SparePartId.
        public static double FindAvaliabilityCountBySparePartId(int sparePartId)
        {
            double count;
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT SUM(Count) FROM Avaliability WHERE SparePartId = @SparePartId;", connection);
                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                var dataReader = cmd.ExecuteReader();
                dataReader.Read();

                count = Convert.ToDouble(dataReader["SUM(Count)"]);
                connection.Close();
            }//using
            return count;
        }//FindAvaliabilityCountBySparePartId
        public static double FindAvaliabilityCountBySparePartId(int sparePartId, SQLiteConnection openConnection)
        {
            SQLiteCommand cmd = new SQLiteCommand("SELECT SUM(Count) FROM Avaliability WHERE SparePartId = @SparePartId;", openConnection);
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

            var dataReader = cmd.ExecuteReader();
            dataReader.Read();

            return Convert.ToDouble(dataReader["SUM(Count)"]);
        }//FindAvaliabilityCountBySparePartId
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

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId AND av.SparePartId = @SparePartId;", openConnection);
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

        //Нахождение уникальных SparePart вместе с общим кол-вом.
        public static IList<SparePart> FindAllUniqueSparePartsAvaliabilityCount()
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                spareParts = FindAllUniqueSparePartsAvaliability(connection);
                SQLiteCommand cmd = new SQLiteCommand("SELECT SUM(Count), Storage FROM Avaliability WHERE SparePartId = @SparePartId GROUP BY Storage;", connection);

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@SparePartId";

                cmd.Parameters.Add(param);
                cmd.Prepare();
                for (int i = 0; i < spareParts.Count; ++i)
                {
                    param.Value = spareParts[i].SparePartId;

                    var dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        SparePart sparePart = spareParts[i];

                        if ((dataReader["Storage"] as string) == SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["SUM(Count)"]);
                        else sparePart.VirtCount = Convert.ToDouble(dataReader["SUM(Count)"]);

                    }//while
                    dataReader.Dispose();
                }//for

                connection.Close();
            }//using
            return spareParts;
        }//FindAllUniqueSparePartAvaliability
        //Найти SparePart по SparePartId с заполнением всех полей с таблицы SpareParts и общим кол-вом с табл. Avaliability 
        /// <summary>
        /// Найти SparePart по SparePartId с заполнением всех полей с таблицы SpareParts и общим кол-вом с табл. Avaliability 
        /// </summary>
        /// <param name="sparePartId">ИД необходимой SparePart</param>
        /// <returns></returns>
        public static SparePart FindUniqueSparePartsAvaliabilityCount(int sparePartId)
        {
            SparePart sparePart = new SparePart();
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                sparePart = FindSparePartById(sparePartId, connection);

                SQLiteCommand cmd = new SQLiteCommand("SELECT SUM(Count), Storage FROM Avaliability WHERE SparePartId = @SparePartId GROUP BY Storage;", connection);

                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);


                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    #region //конструктор
                    /*
                    {
                        Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                        SparePartId= Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        //Price = Convert.ToDouble(dataReader["Price"]),
                        //Markup = Convert.ToInt32(dataReader["Markup"]),
                        //Count = Convert.ToDouble(dataReader["Count"]),
                        //Unit = dataReader["Unit"] as string
                    }; 
                    */
                    #endregion

                    if ((dataReader["Storage"] as string) ==SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["SUM(Count)"]);
                    else sparePart.VirtCount = Convert.ToDouble(dataReader["SUM(Count)"]);

                    //sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                    //sparePart.Avaliability = (sparePart.Storage !=SparePart.VirtStorage) ? sparePart.Count.ToString() : String.Format("({0})", sparePart.Count);
                }//while

                connection.Close();
            }//using
            return sparePart;
        }//FindAllUniqueSparePartAvaliability
        /// <summary>
        /// Найти SparePart по SparePartId с заполнением всех полей с таблицы SpareParts и общим кол-вом с табл. Avaliability 
        /// </summary>
        /// <param name="sparePartId">ИД необходимой SparePart</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        /// <returns></returns>
        public static SparePart FindUniqueSparePartsAvaliabilityCount(int sparePartId, SQLiteConnection openConnection)
        {
            SparePart sparePart = new SparePart();

            sparePart = FindSparePartById(sparePartId, openConnection);

            SQLiteCommand cmd = new SQLiteCommand("SELECT SUM(Count), Storage FROM Avaliability WHERE SparePartId = @SparePartId GROUP BY Storage;", openConnection);

            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);


            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                #region //конструктор
                /*
                    {
                        Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                        SparePartId= Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        //Price = Convert.ToDouble(dataReader["Price"]),
                        //Markup = Convert.ToInt32(dataReader["Markup"]),
                        //Count = Convert.ToDouble(dataReader["Count"]),
                        //Unit = dataReader["Unit"] as string
                    }; 
                    */
                #endregion

                if ((dataReader["Storage"] as string) ==SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["SUM(Count)"]);
                else sparePart.VirtCount = Convert.ToDouble(dataReader["SUM(Count)"]);

                //sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                //sparePart.Avaliability = (sparePart.Storage !=SparePart.VirtStorage) ? sparePart.Count.ToString() : String.Format("({0})", sparePart.Count);
            }//while  

            return sparePart;
        }//FindAllUniqueSparePartAvaliability
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

                SQLiteCommand cmd = new SQLiteCommand("SELECT SUM(Count), Storage FROM Avaliability WHERE SparePartId = @SparePartId GROUP BY Storage;", connection);

                cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    #region //конструктор
                    /*
                    {
                        Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                        SparePartId= Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        //Price = Convert.ToDouble(dataReader["Price"]),
                        //Markup = Convert.ToInt32(dataReader["Markup"]),
                        //Count = Convert.ToDouble(dataReader["Count"]),
                        //Unit = dataReader["Unit"] as string
                    }; 
                    */
                    #endregion
                    if ((dataReader["Storage"] as string) ==SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["SUM(Count)"]);
                    else sparePart.VirtCount = Convert.ToDouble(dataReader["SUM(Count)"]);
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
            SQLiteCommand cmd = new SQLiteCommand("SELECT SUM(Count), Storage FROM Avaliability WHERE SparePartId = @SparePartId GROUP BY Storage;", openConnection);

            cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                #region //конструктор
                /*
                    {
                        Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                        SparePartId= Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        //Price = Convert.ToDouble(dataReader["Price"]),
                        //Markup = Convert.ToInt32(dataReader["Markup"]),
                        //Count = Convert.ToDouble(dataReader["Count"]),
                        //Unit = dataReader["Unit"] as string
                    }; 
                    */
                #endregion
                if ((dataReader["Storage"] as string) ==SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["SUM(Count)"]);
                else sparePart.VirtCount = Convert.ToDouble(dataReader["SUM(Count)"]);
            }//while    

            return sparePart;
        }//FindAllUniqueSparePartAvaliability
        //Есть ли товар в наличие.
        public static bool IsSparePartAvaliable(int sparePartId)
        {
            bool isAvaliable;
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT COUNT() FROM Avaliability WHERE SparePartId = @SparePartId;", connection);
                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                var dataReader = cmd.ExecuteReader();
                dataReader.Read();
                isAvaliable = (Convert.ToInt32(dataReader["COUNT()"]) == 0) ? false : true;


                connection.Close();
            }//using
            return isAvaliable;
        }//IsSparePartAvaliable
        public static bool IsSparePartAvaliable(int sparePartId, SQLiteConnection openConnection)
        {
            var cmd = new SQLiteCommand("SELECT COUNT() FROM Avaliability WHERE SparePartId = @SparePartId;", openConnection);
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

            var dataReader = cmd.ExecuteReader();
            dataReader.Read();

            return (Convert.ToInt32(dataReader["COUNT()"]) == 0) ? false : true;
        }//IsSparePartAvaliable
        /// <summary>
        /// Возвращает кол-во записей данной SparePart (со скольких приходов данная запчасть сейчас в наличии, 0 -- запчасти нет в наличии.) 
        /// </summary>
        /// <param name="sparePartId">Ид искомой SP</param>
        /// <returns></returns> 
        public static int FindCountOfEntrySparePartInAvaliability(int sparePartId)
        {
            int countOfEntry;
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                var cmd = new SQLiteCommand("SELECT COUNT() FROM SpareParts AS sp JOIN Avaliability AS av ON sp.SparePartId = av.SparePartId WHERE sp.SparePartId = @SparePartId;", connection);
                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                var dataReader = cmd.ExecuteReader();
                dataReader.Read();

                countOfEntry = Convert.ToInt32(dataReader["COUNT()"]);

                connection.Close();
            }//using
            return countOfEntry;
        }//FindCountOfEntrySparePart
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



        #endregion
        #region *****************Поиск по таблицам SpareParts. *********************************************************************


        //Поиск по полям таблицы SpareParts.
        public static IList<SparePart> FindAllSpareParts()
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM SpareParts;", connection);
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = CreateSparePart(dataReader);

                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using
            return spareParts;
        }//FindAllSpareParts
        public static IList<SparePart> FindAllSpareParts(SQLiteConnection openConnection)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            var cmd = new SQLiteCommand("SELECT * FROM SpareParts;", openConnection);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                SparePart sparePart = CreateSparePart(dataReader);
                spareParts.Add(sparePart);
            }//while    

            return spareParts;
        }//FindAllSpareParts
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
        /// <summary>
        /// Возвращает список запчастей с заданным артикулом, заполненных только полями таблицы SpareParts, остальные поля не заполнены. 
        /// </summary>
        /// <param name="sparePartArticul">Артикул искомых запчастей.</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        /// <returns></returns>
        public static IList<SparePart> FindSparePartsByArticul(string sparePartArticul, SQLiteConnection openConnection)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            IList<int> sparePartsId = FindSparePartsIdByArticul(sparePartArticul, openConnection);
            foreach (var sparePartId in sparePartsId)
                spareParts.Add(FindSparePartById(sparePartId, openConnection));

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
        public static IList<int> FindSparePartsIdByTitle(string sparePartTitle)
        {
            IList<int> sparePartsId = new List<int>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                var cmd = new SQLiteCommand("SELECT SparePartId FROM SpareParts WHERE Title LIKE @Title", connection);

                cmd.Parameters.AddWithValue("@Title", sparePartTitle);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    sparePartsId.Add(Convert.ToInt32(dataReader["SparePartId"]));
                }//while

                connection.Close();
            }//using
            return sparePartsId;
        }//FindSparePartsIdByTitle
        public static IList<int> FindSparePartsIdByTitle(string sparePartTitle, SQLiteConnection openConnection)
        {
            IList<int> sparePartsId = new List<int>();

            var cmd = new SQLiteCommand("SELECT SparePartId FROM SpareParts WHERE Title LIKE @Title", openConnection);

            cmd.Parameters.AddWithValue("@Title", sparePartTitle);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                sparePartsId.Add(Convert.ToInt32(dataReader["SparePartId"]));
            }//while    

            return sparePartsId;
        }//FindSparePartsIdByTitle
        //Возвращает SparePart полостью готовый для отображения в общей таблице.
        /// <summary>
        /// Возвращает SparePart полостью готовый для отображения в общей таблице.
        /// </summary>
        /// <param name="sparePartId">ИД искомого SparePart</param>
        /// <returns></returns>
        public static SparePart FindSparePartByIdToDisplay(int sparePartId)
        {
            SparePart sparePart = new SparePart();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                //Находим кол-во записей с данной запчастью.
                int countOfEntry = FindCountOfEntrySparePartInAvaliability(sparePartId, connection);
                //Если нет в наличии.
                if (countOfEntry == 0)
                    sparePart = FindSparePartById(sparePartId, connection);
                //Если товар в наличии только с одного прихода
                else if (countOfEntry == 1)
                    sparePart = FindAvaliabilityBySparePartId(sparePartId, connection)[0];
                //Если товар в наличии с многочисленных приходов.                
                else if (countOfEntry > 1)
                {
                    IList<SparePart> spareParts = new List<SparePart>();
                    spareParts = FindAvaliabilityBySparePartId(sparePartId, connection);
                    //Проверяем не имеют ли все вхождения одинаковую Наценку и Цену прихода.
                    bool isSamePrice = true, isSameMarkup = true;
                    for (int i = 0; i < spareParts.Count - 1; ++i)
                    {
                        for (int j = i + 1; j < spareParts.Count; ++j)
                        {
                            if (spareParts[i].Price != spareParts[j].Price) isSamePrice = false;
                            if (spareParts[i].Markup != spareParts[j].Markup) isSameMarkup = false;
                        }//for j
                        if (isSamePrice == false && isSameMarkup == false) break;
                    }//for i                             
                    sparePart = FindUniqueSparePartsAvaliabilityCount(spareParts[0], connection);
                    //Если цена у всех вхождений одинаковая присваиваем её в обобщенный SparePart.
                    if (isSamePrice == true)
                        sparePart.Price = spareParts[0].Price;
                    else sparePart.Price = null;
                    if (isSameMarkup == true)
                        sparePart.Markup = spareParts[0].Markup;
                    else sparePart.Markup = null;
                    sparePart.PurchaseId = -1; //Помечаем что у данной строки имеется подтаблица(т.е. болеее одного поставщика).                            
                }//if
                connection.Close();
            }//using
            return sparePart;
        }//FindSparePartById
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
                //Проверяем не имеют ли все вхождения одинаковую Наценку и Цену прихода.
                bool isSamePrice = true, isSameMarkup = true;
                for (int i = 0; i < spareParts.Count - 1; ++i)
                {
                    for (int j = i + 1; j < spareParts.Count; ++j)
                    {
                        if (spareParts[i].Price != spareParts[j].Price) isSamePrice = false;
                        if (spareParts[i].Markup != spareParts[j].Markup) isSameMarkup = false;
                    }//for j
                    if (isSamePrice == false && isSameMarkup == false) break;
                }//for i                             
                sparePart = FindUniqueSparePartsAvaliabilityCount(spareParts[0], openConnection);
                //Если цена у всех вхождений одинаковая присваиваем её в обобщенный SparePart.
                if (isSamePrice == true)
                    sparePart.Price = spareParts[0].Price;
                else sparePart.Price = null;
                if (isSameMarkup == true)
                    sparePart.Markup = spareParts[0].Markup;
                else sparePart.Markup = null;
                sparePart.PurchaseId = -1; //Помечаем что у данной строки имеется подтаблица(т.е. болеее одного поставщика).
            }//if    

            return sparePart;
        }//FindSparePartById
        //Возвращает полностью готовый к выводу список всех запчастей в БД с общим кол-вом.
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
        /// <summary>
        /// Возвращает готовый к выводу список запчастей, подготавливая его из переданного списка запчастей.
        /// </summary>
        /// <param name="sparePartsList">Список запчастей из которых осуществляется выборка</param>
        /// <returns></returns>
        public static IList<SparePart> FindAllSparePartsToDisplay(IList<SparePart> sparePartsList)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                for (int i = 0; i < sparePartsList.Count; ++i)
                {
                    spareParts.Add(FindSparePartByIdToDisplay(sparePartsList[i].SparePartId, connection));
                }//for
                connection.Close();
            }//using
            return spareParts;
        }//FindAllSparePartsToDisplay
        /// <summary>
        /// Возвращает готовый к выводу список запчастей, подготавливая его из переданного списка Ид запчастей.
        /// </summary>
        /// <param name="sparePartsId">Список Ид запчастей из которых осуществляется выборка.</param>
        /// <returns></returns>
        public static IList<SparePart> FindAllSparePartsToDisplay(IList<int> sparePartsId)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                for (int i = 0; i < sparePartsId.Count; ++i)
                {
                    spareParts.Add(FindSparePartByIdToDisplay(sparePartsId[i], connection));
                }//for
                connection.Close();
            }//using
            return spareParts;
        }//FindAllSparePartsToDisplay


















        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region *****************Поиск по полям остальных таблиц.*******************************************************************
        //поиск по полям таблицы Categories.
        public static string[] FindAllCategories()
        {
            IList<string> categories = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Categories;", connection);
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    categories.Add(dataReader["CategoryName"] as string);
                }//while
                connection.Close();
            }//using

            //создаём массив string.
            string[] categ = new string[categories.Count];
            for (int i = 0; i < categ.Length; ++i)
                categ[i] = categories[i];

            return categ;
        }//FindAllCategories
        public static string FindCategoryNameById(int categoryId)
        {
            string category = null;
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT CategoryName FROM Categories WHERE CategoryId = @CategoryId;", connection);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                var dataReader = cmd.ExecuteReader();

                dataReader.Read();
                category = dataReader["CategoryName"] as string;

                connection.Close();
            }//using
            return category;
        }//FindCategoryNameById
        public static string FindCategoryNameById(int categoryId, SQLiteConnection openConnection)
        {
            var cmd = new SQLiteCommand("SELECT CategoryName FROM Categories WHERE CategoryId = @CategoryId;", openConnection);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);

            var dataReader = cmd.ExecuteReader();
            dataReader.Read();

            return dataReader["CategoryName"] as string;
        }//FindCategoryNameById

        //Поиск по полям таблицы Manufacturers.
        public static string[] FindAllManufacturersName()
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
        public static string[] FindAllManufacturersName(SQLiteConnection openConnection)
        {
            IList<string> manufacturers = new List<string>();

            SQLiteCommand cmd = new SQLiteCommand("SELECT ManufacturerName FROM Manufacturers;", openConnection);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                manufacturers.Add(dataReader["ManufacturerName"] as string);
            }//while    

            //создаём массив string.
            string[] manuf = new string[manufacturers.Count];
            for (int i = 0; i < manuf.Length; ++i)
            {
                manuf[i] = manufacturers[i];
            }//for

            return manuf;
        }//FindAllManufacturersName
        public static string FindManufacturerNameById(int? manufacturerId)
        {
            string manufacturer = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT ManufacturerName FROM Manufacturers WHERE ManufacturerId = @ManufacturerId;", connection);

                cmd.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

                var dataReader = cmd.ExecuteReader();
                dataReader.Read();
                manufacturer = dataReader["ManufacturerName"] as string;

                connection.Close();
            }//using
            return manufacturer;
        }//FindManufacturerNameById
        public static string FindManufacturerNameById(int? manufacturerId, SQLiteConnection openConnection)
        {
            var cmd = new SQLiteCommand("SELECT ManufacturerName FROM Manufacturers WHERE ManufacturerId = @ManufacturerId;", openConnection);
            cmd.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

            var dataReader = cmd.ExecuteReader();
            dataReader.Read();

            return dataReader["ManufacturerName"] as string;
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
        /// <summary>
        /// Возвращает список Id-ков производителей с заданным именем.
        /// </summary>
        /// <param name="manufacturerName">Имя искомых производителей.</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        /// <returns></returns>
        public static IList<int> FindManufacturersIdByName(string manufacturerName, SQLiteConnection openConnection)
        {
            IList<int> manufacturersId = new List<int>();

            var cmd = new SQLiteCommand("SELECT ManufacturerId FROM Manufacturers WHERE ManufacturerName = @ManufacturerName;", openConnection);

            cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerName);

            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
                manufacturersId.Add(Convert.ToInt32(dataReader["ManufacturerId"]));    

            return manufacturersId;
        }//FindManufacturersIdByName

        //Поиск по полям таблицы Suppliers.
        public static IList<Contragent> FindAllSuppliers()
        {
            IList<Contragent> suppliers = new List<Contragent>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Suppliers;", connection);
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Contragent supplier = new Contragent();
                    #region конструктор
                    //{
                    //SupplierId = Convert.ToInt32(dataReader["SupplierId"]);
                    //SupplierName = dataReader["SupplierName"] as string;
                    //Code = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    //Entity = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    //ContactInfoId = (dataReader["ContactInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ContactInfoId"]);
                    //Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["ExtInfoId"] as string;
                    //};
                    #endregion
                    supplier.ContragentId = Convert.ToInt32(dataReader["SupplierId"]);
                    supplier.ContragentName = dataReader["SupplierName"] as string;
                    supplier.Code = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                    supplier.Entity = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                    supplier.ContactInfoId = (dataReader["ContactInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ContactInfoId"]);
                    supplier.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                    suppliers.Add(supplier);
                }//while
                connection.Close();
            }//using
            return suppliers;


        }//FindAllSuppliers
        public static IList<Contragent> FindAllSuppliers(SQLiteConnection openConnection)
        {
            IList<Contragent> suppliers = new List<Contragent>();

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Suppliers;", openConnection);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Contragent supplier = new Contragent();
                #region конструктор
                //{
                //SupplierId = Convert.ToInt32(dataReader["SupplierId"]);
                //SupplierName = dataReader["SupplierName"] as string;
                //Code = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                //Entity = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                //ContactInfoId = (dataReader["ContactInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ContactInfoId"]);
                //Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["ExtInfoId"] as string;
                //};
                #endregion
                supplier.ContragentId = Convert.ToInt32(dataReader["SupplierId"]);
                supplier.ContragentName = dataReader["SupplierName"] as string;
                supplier.Code = (dataReader["Code"] == DBNull.Value) ? String.Empty : dataReader["Code"] as string;
                supplier.Entity = (dataReader["Entity"] == DBNull.Value) ? String.Empty : dataReader["Entity"] as string;
                supplier.ContactInfoId = (dataReader["ContactInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ContactInfoId"]);
                supplier.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                suppliers.Add(supplier);
            }//while    

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
        /// Возвращает массив строк состоящий из всех имен поставщиков.
        /// </summary>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        /// <returns></returns>
        public static string[] FindAllSuppliersName(SQLiteConnection openConnection)
        {            
            SQLiteCommand cmd = new SQLiteCommand("SELECT SupplierName FROM Suppliers;", openConnection);
            var dataReader = cmd.ExecuteReader();

            IList<string> suppliersNameList = new List<string>();
            while (dataReader.Read())
            {
                suppliersNameList.Add(dataReader["SupplierName"] as string);
            }//while    

            string[] suppliersName = new string[suppliersNameList.Count];
            for (int i = 0; i < suppliersName.Length; ++i)
                suppliersName[i] = suppliersNameList[i];

            return suppliersName;
        }//FindAllSuppliersName
        public static string FindSupplierNameById(int supplierId)
        {
            string supplier = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT SupplierName FROM Suppliers WHERE SupplierId = @SupplierId;", connection);

                cmd.Parameters.AddWithValue("@SupplierId", supplierId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                    supplier = dataReader["SupplierName"] as string;

                connection.Close();
            }//using
            return supplier;
        }//FindSupplierNameById
        public static string FindSupplierNameById(int supplierId, SQLiteConnection openConnection)
        {
            string supplierName = null;
            var cmd = new SQLiteCommand("SELECT SupplierName FROM Suppliers WHERE SupplierId = @SupplierId;", openConnection);

            cmd.Parameters.AddWithValue("@SupplierId", supplierId);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
                supplierName = dataReader["SupplierName"] as string;

            return supplierName;
        }//FindSupplierNameById
        /// <summary>
        /// Возвращает Id поставщика по заданному имени. !!!Исправить, есть вероятность ошибки, в случае двух одинаковых имен поставщиков.!
        /// </summary>
        /// <param name="supplierName">Имя поставщика Id которого надо найти</param>
        /// <returns></returns>
/*!!!*/ public static int FindSupplierIdByName(string supplierName)
        {
            //могут быть поставщики с одинаковыми именами.
            int supplierId;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT SupplierId FROM Suppliers WHERE SupplierName LIKE @SupplierName;", connection);

                cmd.Parameters.AddWithValue("@SupplierName", supplierName);

                supplierId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }//using
            return supplierId;
        }//FindSupplierIdByName
        /// <summary>
        /// Возвращает объект Contragent, заполненный данными с таблицы Suppliers по заданному Id поставки. 
        /// </summary>
        /// <param name="purchaseId">Id поставки, по которой находятся данные о поставщике.</param>
        /// <returns></returns>
        public static Contragent FindSupplierByPurchaseId(int purchaseId)
        {
            Contragent supplier = null;

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
                    supplier = new Contragent
                    (
                        dataReader["SupplierName"] as string,
                        dataReader["Code"] as string,
                        dataReader["Entity"] as string,
                        dataReader["ContactInfoId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["ContactInfoId"]),
                        dataReader["Description"] as string
                    );
                }//while

                connection.Close();
            }//using

            return supplier;
        }//FindSupplierByPurchaseId

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



        //Поиск по полям таблицы SpSuppliers.
        public static IList<int> FindSuppliersIdBySparePartId(int sparePartId)
        {
            IList<int> suppliersId = new List<int>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT SupplierId FROM SpSuppliers WHERE SparePartId = @SparePartId;", connection);

                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    suppliersId.Add(Convert.ToInt32(dataReader["SupplierId"]));
                }//while
                connection.Close();
            }//using

            return suppliersId;
        }//FindSuppliersIdBySparePartId
        public static IList<int> FindSuppliersIdBySparePartId(int sparePartId, SQLiteConnection openConnection)
        {
            IList<int> suppliersId = new List<int>();

            var cmd = new SQLiteCommand("SELECT SupplierId FROM SpSuppliers WHERE SparePartId = @SparePartId;", openConnection);

            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                suppliersId.Add(Convert.ToInt32(dataReader["SupplierId"]));
            }//while   

            return suppliersId;
        }//FindSuppliersIdBySparePartId
        public static IList<int> FindSparePartsIdBySupplierId(int sparePartId)
        {
            IList<int> sparePartsId = new List<int>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT SparePartId FROM SpSuppliers WHERE SupplierId = @SupplierId;", connection);

                cmd.Parameters.AddWithValue("@SupplierId", sparePartId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    sparePartsId.Add(Convert.ToInt32(dataReader["SparePartId"]));
                }//while
                connection.Close();
            }//using

            return sparePartsId;
        }//FindSuppliersIdBySparePartId
        public static IList<int> FindSparePartsIdBySupplierId(int sparePartId, SQLiteConnection openConnection)
        {
            IList<int> sparePartsId = new List<int>();

            var cmd = new SQLiteCommand("SELECT SparePartId FROM SpSuppliers WHERE SupplierId = @SupplierId;", openConnection);

            cmd.Parameters.AddWithValue("@SupplierId", sparePartId);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                sparePartsId.Add(Convert.ToInt32(dataReader["SparePartId"]));
            }//while    

            return sparePartsId;
        }//FindSuppliersIdBySparePartId


        //Поиск по полям таблицы Customers.
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
        /// Возвращает Id клиента по заданному имени. !!!Исправить, есть вероятность ошибки, в случае двух одинаковых имен поставщиков.!
        /// </summary>
        /// <param name="customerName">Имя поставщика Id которого надо найти</param>
        /// <returns></returns>
/*!!!*/ public static int FindCustomerIdByName(string customerName)
        {
            //могут быть клиенты с одинаковыми именами.
            int customerId;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT CustomerId FROM Customers WHERE CustomerName LIKE @CustomerName;", connection);

                cmd.Parameters.AddWithValue("@CustomerName", customerName);

                customerId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }//using
            return customerId;
        }//FindCustomerIdByName

        //Поиск по полям таблицы SpCategories.
        public static IList<int> FindCategoriesIdBySparePartId(int sparePartId)
        {
            IList<int> categoriesId = new List<int>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT SupplierId FROM SpCategories WHERE SparePartId = @SparePartId;", connection);

                cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    categoriesId.Add(Convert.ToInt32(dataReader["SupplierId"]));
                }//while
                connection.Close();
            }//using

            return categoriesId;
        }//FindCategoriesIdBySparePartId
        public static IList<int> FindCategoriesIdBySparePartId(int sparePartId, SQLiteConnection openConnection)
        {
            IList<int> categoriesId = new List<int>();

            var cmd = new SQLiteCommand("SELECT SupplierId FROM SpCategories WHERE SparePartId = @SparePartId;", openConnection);

            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                categoriesId.Add(Convert.ToInt32(dataReader["SupplierId"]));
            }//while

            return categoriesId;
        }//FindCategoriesIdBySparePartId
        public static IList<int> FindSparePartsIdByCategoryId(int categoryId)
        {
            IList<int> sparePartsId = new List<int>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT SparePartId FROM SpCategories WHERE CategoryId = @CategoryId;", connection);

                cmd.Parameters.AddWithValue("@SparePartId", categoryId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    sparePartsId.Add(Convert.ToInt32(dataReader["SparePartIdId"]));
                }//while
                connection.Close();
            }//using

            return sparePartsId;
        }//FindSparePartsIdByCategoryId
        public static IList<int> FindSparePartsIdByCategoryId(int categoryId, SQLiteConnection openConnection)
        {
            IList<int> sparePartsId = new List<int>();
            var cmd = new SQLiteCommand("SELECT SparePartId FROM SpCategories WHERE CategoryId = @CategoryId;", openConnection);

            cmd.Parameters.AddWithValue("@CategoryId", categoryId);

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                sparePartsId.Add(Convert.ToInt32(dataReader["SparePartIdId"]));
            }//while

            return sparePartsId;
        }//FindSparePartsIdByCategoryId

        
        //Поиск по полям таблицы UnitsOfMeasure.
        /// <summary>
        /// Находим все единицы измерения.
        /// </summary>
        /// <returns></returns>
        public static IList<string> FindAllUnitsOfMeasure()
        { 

            IList<string> units = new List<string>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM UnitsOfMeasure;", connection);
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    units.Add(dataReader["Unit"] as string);
                }//while
                connection.Close();
            }//using

            //создаём массив string.
            //string[] unit = new string[units.Count];
            //for (int i = 0; i < unit.Length; ++i)
            //    unit[i] = units[i];

            return units;
        }//FindAllUnitsOfMeasure
        /// <summary>
        /// Возвращает минимальную единицу продажи переданное единицы измерения.
        /// </summary>
        /// <param name="unit">единица измерения</param>
        /// <returns></returns>
        public static double FindMinUnitSaleOfUnit(string unit)
        {
            double minUnitSale;
            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT MinSaleUnit FROM UnitsOfMeasure WHERE Unit LIKE @Unit;", connection);

                cmd.Parameters.AddWithValue("@Unit", unit);

                minUnitSale = Convert.ToDouble(cmd.ExecuteScalar());

                connection.Close();
            }//using

            return minUnitSale;
        }//FindMinUnitSaleOfUnit

        //Поиск по полям таблицы Purchases.
        /// <summary>
        /// Возвращает объект класса Purchase, найденный по заданному Id. 
        /// </summary>
        /// <param name="purchaseId">Id прихода информацию о котором нужно вернуть.</param>
        /// <returns></returns>
        public static Purchase FindPurchaseById(int purchaseId)
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
                    purchase.PurchaseId = purchaseId;
                    purchase.EmployeeId = (dataReader["EmployeeId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["EmployeeId"]);
                    purchase.SupplierId = Convert.ToInt32(dataReader["SupplierId"]);
                    purchase.SupplierEmployee = dataReader["SupplierEmployee"] as string;
                    //Переводим кол-во секунд Utc в DateTime.
                    TimeSpan ts = TimeSpan.FromSeconds(Convert.ToInt32(dataReader["PurchaseDate"])); 
                    DateTime purchaseDate = new DateTime(1970, 1, 1); 
                    purchaseDate += ts;
                    purchase.PurchaseDate = purchaseDate;

                    purchase.Currency = dataReader["Currency"] as string;
                    purchase.ExcRate = Convert.ToDouble(dataReader["ExcRate"]);
                }//while
                connection.Close();
            }//using

            return purchase;
        }//FindPurchaseById

        #region Поиск по полям Markups.

        /// <summary>
        /// Возвращает список из всех типов и значений наценки.
        /// </summary>
        /// <returns></returns>
        public static IList<KeyValuePair<double, string>> FindAllMarkups()
        {
            IList<KeyValuePair<double, string>> markups = new List<KeyValuePair<double, string>>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT MarkupValue, MarkupType FROM Markups;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    double key   = Convert.ToDouble(dataReader["MarkupValue"]);
                    string value = dataReader["MarkupType"] as string; 

                    markups.Add(new KeyValuePair<double, string>(key, value));
                }//while

                connection.Close();
            }//using

            return markups;        
        }//FindAllMarkups

        /// <summary>
        /// Находит числовое значение наценки по заданному типу.
        /// </summary>
        /// <param name="markupType"></param>
        /// <returns></returns>
        public static double FindMarkupValue(string markupType)
        {
            double? markup = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                markup = FindAllMarkups().Where(mark => mark.Value == markupType).Select(mark => mark.Key).First();
                
                connection.Close();
            }//using

            return (double)markup;
        }//FindMarkupValue
        /// <summary>
        /// Находит тип наценки по числовому ключу.
        /// </summary>
        /// <param name="markupValue"></param>
        /// <returns></returns>
        public static string FindMarkupType(double markupValue)
        {
            //if (markupValue == null) return null;
            string markupType = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                markupType = FindAllMarkups().Where(mark => mark.Key == markupValue).Select(mark => mark.Value).First();

                connection.Close();
            }//using

            return markupType;
        }//FindMarkupValue



































        #endregion





        #endregion


































        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region ************Поиск совпадений по БД.*****************************************************************************
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*Поиск по Avaliability.*/
        public static IList<SparePart> SearchSparePartsAvaliablityByArticul(string articul)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId AND sp.Articul LIKE @Articul", connection);

                cmd.Parameters.AddWithValue("@Articul", articul + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart();
                    //{
                    //    Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                    //    SparePartId= Convert.ToInt32(dataReader["Id"]),
                    //    Articul = dataReader["Articul"] as string,
                    //    Title = dataReader["Title"] as string,
                    //    Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                    //    //Price = Convert.ToDouble(dataReader["Price"]),
                    //    //Markup = Convert.ToInt32(dataReader["Markup"]),
                    //    //Count = Convert.ToDouble(dataReader["Count"]),
                    //    //Unit = dataReader["Unit"] as string
                    //};
                    sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                    sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                    sparePart.Articul = dataReader["Articul"] as string;
                    sparePart.Title = dataReader["Title"] as string;

                    sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                    sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                    sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                    //sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, connection);
                    //sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);

                    sparePart.Unit = dataReader["Unit"] as string;
                    sparePart.PurchaseId = Convert.ToInt32(dataReader["PurchaseId"]);
                    sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                    sparePart.Markup = (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"]);
                    //sparePart.Storage = dataReader["Storage"] as string;
                    if (dataReader["Storage"] as string ==SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                    else sparePart.VirtCount = Convert.ToDouble(dataReader["Count"]);


                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;
        }//SearchSparePartsByArticul
        public static IList<SparePart> SearchSparePartsAvaliablityByArticul(string articul, SQLiteConnection openConnection)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId AND sp.Articul LIKE @Articul", openConnection);

            cmd.Parameters.AddWithValue("@Articul", articul + "%");

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                SparePart sparePart = new SparePart();
                sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                sparePart.Articul = dataReader["Articul"] as string;
                sparePart.Title = dataReader["Title"] as string;

                sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                //sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, openConnection);
                //sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);

                sparePart.Unit = dataReader["Unit"] as string;
                sparePart.PurchaseId = Convert.ToInt32(dataReader["PurchaseId"]);
                sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                sparePart.Markup = (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"]);
                //sparePart.Storage = dataReader["Storage"] as string;
                if (dataReader["Storage"] as string ==SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                else sparePart.VirtCount = Convert.ToDouble(dataReader["Count"]);


                spareParts.Add(sparePart);
            }//while    

            return spareParts;
        }//SearchSparePartsByArticul
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
        public static IList<SparePart> SearchSparePartsAvaliablityByTitle(string title)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId AND sp.Title LIKE @Title", connection);

                cmd.Parameters.AddWithValue("@Title", title + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart();
                    //{
                    //    Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                    //    SparePartId= Convert.ToInt32(dataReader["Id"]),
                    //    Articul = dataReader["Articul"] as string,
                    //    Title = dataReader["Title"] as string,
                    //    Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                    //    //Price = Convert.ToDouble(dataReader["Price"]),
                    //    //Markup = Convert.ToInt32(dataReader["Markup"]),
                    //    //Count = Convert.ToDouble(dataReader["Count"]),
                    //    //Unit = dataReader["Unit"] as string
                    //};
                    sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                    sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                    sparePart.Articul = dataReader["Articul"] as string;
                    sparePart.Title = dataReader["Title"] as string;

                    sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                    sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                    sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                    //sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, connection);
                    //sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);

                    sparePart.Unit = dataReader["Unit"] as string;
                    sparePart.PurchaseId = Convert.ToInt32(dataReader["PurchaseId"]);
                    sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                    sparePart.Markup = (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"]);
                    //sparePart.Storage = dataReader["Storage"] as string;
                    if (dataReader["Storage"] as string ==SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                    else sparePart.VirtCount = Convert.ToDouble(dataReader["Count"]);


                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;
        }//SearchSparePartsByTitle
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
                    //sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                    //sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                    //sparePart.Articul = dataReader["Articul"] as string;
                    //sparePart.Title = dataReader["Title"] as string;

                    //sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                    //sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                    //sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                    ////sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, connection);
                    ////sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);

                    //sparePart.Unit = dataReader["Unit"] as string;
                    //sparePart.PurchaseId = Convert.ToInt32(dataReader["PurchaseId"]);
                    //sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                    //sparePart.Markup = (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"]);
                    ////sparePart.Storage = dataReader["Storage"] as string;
                    //if (dataReader["Storage"] as string == SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                    //else sparePart.VirtCount = Convert.ToDouble(dataReader["Count"]);

                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;
        }//SearchSparePartsByTitle
        public static IList<SparePart> SearchSparePartsAvaliablityByTitle(string title, SQLiteConnection openConnection)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Avaliability as av JOIN SpareParts as sp ON av.SparePartId = sp.SparePartId AND sp.Title LIKE @Title", openConnection);

            cmd.Parameters.AddWithValue("@Title", title + "%");

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                SparePart sparePart = new SparePart();
                //{
                //    Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                //    SparePartId= Convert.ToInt32(dataReader["Id"]),
                //    Articul = dataReader["Articul"] as string,
                //    Title = dataReader["Title"] as string,
                //    Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"])),
                //    //Price = Convert.ToDouble(dataReader["Price"]),
                //    //Markup = Convert.ToInt32(dataReader["Markup"]),
                //    //Count = Convert.ToDouble(dataReader["Count"]),
                //    //Unit = dataReader["Unit"] as string
                //};
                sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                sparePart.Articul = dataReader["Articul"] as string;
                sparePart.Title = dataReader["Title"] as string;

                sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                ////sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, openConnection);
                //sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);

                sparePart.Unit = dataReader["Unit"] as string;
                sparePart.PurchaseId = Convert.ToInt32(dataReader["PurchaseId"]);
                sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                sparePart.Markup = (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"]);
                //sparePart.Storage = dataReader["Storage"] as string;
                if (dataReader["Storage"] as string ==SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                else sparePart.VirtCount = Convert.ToDouble(dataReader["Count"]);


                spareParts.Add(sparePart);
            }//while    

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

        //Поиск по SpareParts.
        public static IList<SparePart> SearchSparePartsByTitle(string title)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM SpareParts WHERE Title LIKE @Title;", connection);

                cmd.Parameters.AddWithValue("@Title", title + "%");

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart();

                    sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                    sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                    sparePart.Articul = dataReader["Articul"] as string;
                    sparePart.Title = dataReader["Title"] as string;

                    sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                    sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                    sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                    //sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, connection);
                    //sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);

                    sparePart.Unit = dataReader["Unit"] as string;
                    sparePart.PurchaseId = Convert.ToInt32(dataReader["PurchaseId"]);
                    sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                    sparePart.Markup = (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"]);
                    //sparePart.Storage = dataReader["Storage"] as string;
                    if (dataReader["Storage"] as string == SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                    else sparePart.VirtCount = Convert.ToDouble(dataReader["Count"]);


                    spareParts.Add(sparePart);
                }//while
                connection.Close();
            }//using

            return spareParts;
        }//SearchSparePartsByTitle
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
                    //sparePart.Photo = (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string;
                    //sparePart.SparePartId = Convert.ToInt32(dataReader["SparePartId"]);
                    //sparePart.Articul = dataReader["Articul"] as string;
                    //sparePart.Title = dataReader["Title"] as string;

                    //sparePart.ExtInfoId = (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]);
                    //sparePart.Description = (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string;

                    //sparePart.ManufacturerId = (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]);
                    ////sparePart.Manufacturer = (sparePart.ManufacturerId == null) ? String.Empty : FindManufacturerNameById(sparePart.ManufacturerId, connection);
                    ////sparePart.Manufacturer = (dataReader["ManufacturerId"] == DBNull.Value) ? String.Empty : FindManufacturerNameById(Convert.ToInt32(dataReader["ManufacturerId"]), connection);

                    //sparePart.Unit = dataReader["Unit"] as string;
                    //sparePart.PurchaseId = Convert.ToInt32(dataReader["PurchaseId"]);
                    //sparePart.Price = Convert.ToDouble(dataReader["Price"]);
                    //sparePart.Markup = (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"]);
                    ////sparePart.Storage = dataReader["Storage"] as string;
                    //if (dataReader["Storage"] as string == SparePart.MainStorage) sparePart.Count = Convert.ToDouble(dataReader["Count"]);
                    //else sparePart.VirtCount = Convert.ToDouble(dataReader["Count"]);


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


















//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion






        private static SparePart CreateSparePart(SQLiteDataReader dataReader)
        {
            SparePart sparePart = new SparePart
            (
                sparePartId    : Convert.ToInt32(dataReader["SparePartId"]),
                photo          : (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                articul        : dataReader["Articul"] as string,
                title          : dataReader["Title"] as string,
                description    : (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string,
                extInfoId      : (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]),  
                manufacturerId : (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]),                              
                unit           : dataReader["Unit"] as string             
            );

            return sparePart;        
        }//CreateSparePart
        private static SparePart CreateFullSparePart(SQLiteDataReader dataReader)
        {
            SparePart sparePart = new SparePart
            (
                sparePartId: Convert.ToInt32(dataReader["SparePartId"]),
                photo: (dataReader["Photo"] == DBNull.Value) ? String.Empty : dataReader["Photo"] as string,
                articul: dataReader["Articul"] as string,
                title: dataReader["Title"] as string,
                description: (dataReader["Description"] == DBNull.Value) ? String.Empty : dataReader["Description"] as string,
                extInfoId: (dataReader["ExtInfoId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ExtInfoId"]),
                manufacturerId: (dataReader["ManufacturerId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataReader["ManufacturerId"]),                                
                purchaseId: Convert.ToInt32(dataReader["PurchaseId"]),
                unit: dataReader["Unit"] as string,
                storage: dataReader["Storage"] as string,
                storageAdress : dataReader["StorageAdress"] as string,
                count: Convert.ToDouble(dataReader["Count"]),
                price: Convert.ToDouble(dataReader["Price"]),
                markup: (dataReader["Markup"] == DBNull.Value) ? (double?)null : Convert.ToDouble(dataReader["Markup"])
            );

            return sparePart;
        }//CreateFullSparePart
        //Коннект к базе данных.
        static private System.Data.Common.DbConnection GetDatabaseConnection(string name)
        {
            var settings = System.Configuration.ConfigurationManager.ConnectionStrings[name];
            var factory = System.Data.Common.DbProviderFactories.GetFactory(settings.ProviderName);

            var conn = factory.CreateConnection();
            conn.ConnectionString = settings.ConnectionString;

            return conn;
        }//GetDatabaseConnection

    }//PartsDAL

    public class SparePart
    {
        //const string _mainStorage = "Осн. скл.";
        //const string _virtStorage = "Вирт. скл.";
        public static string MainStorage { get { return "Осн. скл."; } }
        public static string VirtStorage { get { return "Вирт. скл."; } }
        
        #region Св-ва класса.
        public int SparePartId { get; set; }
        [DisplayName("Фото")]
        public string Photo { get; set; }
        [DisplayName("Номер прихода")]
        public int PurchaseId { get; set; }
        [DisplayName("Поставщик")]
        public string SupplierName { get; set; }
        [DisplayName("Производитель")]
        public string Manufacturer { get; set; }
        public int? ManufacturerId { get; set; }  
        [DisplayName("Артикул")]
        public string Articul { get; set; }
        [DisplayName("Название")]
        public string Title { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        public int? ExtInfoId { get; set; }

        [DisplayName("Адрес склада")]
        public string StorageAdress { get; set; }     
        [DisplayName("Ед. изм.")]
        public string Unit { get; set; }
        [DisplayName("Осн.Скл.")]
        public double Count { get; set; }
        [DisplayName("Вирт. скл.")]
        public double VirtCount { get; set; }        
        [Browsable(false)]
        private string _avaliability;
        [DisplayName("Наличие")]
        public string Avaliability
        {
            get { return (VirtCount == 0) ? Count.ToString() : (Count == 0) ? String.Format("({0})", VirtCount) : String.Format("{0} ({1})", Count, VirtCount); }
            set { _avaliability = value; }
        }

        [DisplayName("Цена закупки")]
        public double? Price { get; set; }        
        [Browsable(false)]
        public double? Markup { get; set; }
        [DisplayName("Тип наценки")]
        public string MarkupType { get; set; }
        [Browsable(false)]
        private double _excRate = 1;
        [Browsable(false)]
        public double ExcRate
        {
            get { return _excRate; }
            set { _excRate = value; }
        }
        //[Browsable(false)]
        //private double? _sellingPrice;
        [DisplayName("Цена продажи")]
        public double? SellingPrice
        {
            get 
            {
                //return (Price == null || Markup == null) ? (double?)null : Math.Round(((double)(Price + (Price * Markup / 100)) * ExcRate), 2, MidpointRounding.AwayFromZero);
                if (Price == null || Markup == null)
                    return null;
                else 
                {
                    double sellPrice = (double)(Price + (Price * Markup / 100)) / ExcRate;
                    return Math.Round(sellPrice, 2, MidpointRounding.AwayFromZero);
                }//else
            }//get
            set { Markup = (value * 100 / Price) - 100; }
        }        
        #endregion

        public SparePart() { }
        public SparePart(SparePart sparePart)
        {
            this.SparePartId    = sparePart.SparePartId;
            this.Photo          = sparePart.Photo;
            this.Articul        = sparePart.Articul;
            this.Title          = sparePart.Title;
            this.Description    = sparePart.Description;
            this.ExtInfoId      = sparePart.ExtInfoId;
            this.ManufacturerId = sparePart.ManufacturerId;
            this.Manufacturer   = (ManufacturerId == null) ? null : PartsDAL.FindManufacturerNameById(ManufacturerId);/*!!!*/
            this.Unit           = sparePart.Unit;
            this.Count          = sparePart.Count;
            this.VirtCount      = sparePart.VirtCount;
            this.StorageAdress  = sparePart.StorageAdress;
            this.Price          = sparePart.Price;
            this.Markup         = sparePart.Markup;
            this.MarkupType     = sparePart.MarkupType;
            this.ExcRate        = sparePart.ExcRate;
            this.PurchaseId     = sparePart.PurchaseId;
        }

        public SparePart(int sparePartId, string photo, string articul, string title, string description, int? extInfoId,
                         int? manufacturerId, string unit)
        {
            this.SparePartId    = sparePartId;
            this.Photo          = photo;
            this.Articul        = articul;
            this.Title          = title;
            this.ExtInfoId      = extInfoId;
            this.Description    = description;
            this.ManufacturerId = manufacturerId;
            this.Manufacturer   = (ManufacturerId == null) ? null : PartsDAL.FindManufacturerNameById(ManufacturerId);
            this.Unit           = unit;
        }

        public SparePart(int sparePartId, string photo, string articul, string title, string description, int? extInfoId,
                         int? manufacturerId, int purchaseId, string unit, string storage, string storageAdress, double count, 
                         double price, double? markup)
        {          
            this.SparePartId    = sparePartId;
            this.Photo          = photo;
            this.Articul        = articul;
            this.Title          = title;
            this.ExtInfoId      = extInfoId;
            this.Description    = description;
            this.ManufacturerId = manufacturerId;
            this.Manufacturer   = (manufacturerId == null) ? null : PartsDAL.FindManufacturerNameById(manufacturerId); /*!!!*/
            this.Unit           = unit;
            this.PurchaseId     = purchaseId;
            this.SupplierName   = PartsDAL.FindSupplierByPurchaseId(purchaseId).ContragentName; /*!!!*/
            if (storage == MainStorage) this.Count = count; 
            else this.VirtCount = count;
            this.StorageAdress  = storageAdress;
            this.Price          = price;
            this.Markup         = markup;
            this.MarkupType     = MarkupTypes.GetMarkupType(markup);
        }

        public override string ToString()
        {
            return String.Format("Photo: {0}, Articul: {1}, Title: {2}, Descrip {3},\n  Manuf: {4}, Unit: {5}, minUnit: {6}",
                    Photo, Articul, Title, Description, Manufacturer, Unit);

        }
    }//Parts

    class Contragent
    {
        public static string Customer { get { return "клиент";    } }
        public static string Supplier { get { return "поставщик"; } }


        [Browsable(false)]
        public int ContragentId { get; set; }
        [DisplayName("Название")]
        public string ContragentName { get; set; }
        [DisplayName("ИНН/ОКПО")]
        public string Code { get; set; }
        [DisplayName("Юр./Физ. лицо")]
        public string Entity { get; set; }
        [Browsable(false)]
        public int? ContactInfoId { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }

        public Contragent() { }
        public Contragent(string contragentName, string code, string entity, int? contactInfoId, string description)
        {
            ContragentName = contragentName;
            Code = code;
            Entity = entity;
            ContactInfoId = contactInfoId;
            Description = description;
        }
    }//Supplier

}//namespace

/*Задачи*/
//1)Добавить поле Category и работу с ним.

//public string Manufacturer
//        {
//            get { return (ManufacturerId == null) ? null : PartsDAL.FindManufacturerNameById(ManufacturerId); }
//            set { _manufacturer = value; }
//        }
//        public int? ManufacturerId { get; set; }

/*На будущее*/
//1) Разобраться с Enum и добавить fieldNames
//2)Перенести определение ManufacturerName в методы класса PartsDAL (CreateSparePart, ...).