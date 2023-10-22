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
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using static PartsApp.Helper.Validator;

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
        public static void UpdateSparePartMarkup(List<Availability> availList)
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
                            foreach (Availability avail in availList)
                            {
                                int sparePartId = avail.OperationDetails.SparePart.SparePartId;
                                int purchaseId  = avail.OperationDetails.Operation.OperationId;
                                float markup    = avail.Markup;

                                UpdateSparePartMarkup(sparePartId, purchaseId, markup, cmd);                  
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
            //Узнаем количество данного товара в наличии по данному приходу.
            float availCount = FindAvailability(operDet.SparePart).First(av => av.OperationDetails.Operation.OperationId == operDet.Operation.OperationId).OperationDetails.Count;

            //Если кол-во продаваемого товара с данного прихода равно всему кол-во товара данной записи, удаляем из таблицы эту запись, иначе обновляем кол-во товара в базе.
            if (availCount == operDet.Count)
                DeleteSparePartAvaliability(operDet.SparePart.SparePartId, operDet.Operation.OperationId, cmd);
            else
                UpdateSparePartСountAvaliability(operDet.SparePart.SparePartId, operDet.Operation.OperationId, availCount - operDet.Count, cmd);

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

                using (SQLiteTransaction trans = connection.BeginTransaction())
                {                    
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //Находим существующий manufacturerId в базе или добавляем новый объект если отсутствует.
                            int? manufId = (sparePart.Manufacturer != null) ? FindManufacturerId(sparePart.Manufacturer) : (int?)null;
                            cmd.Parameters.AddWithValue("@ManufacturerId", (manufId == 0) ? AddManufacturer(sparePart.Manufacturer, cmd) : manufId);

                            const string query = "INSERT INTO SpareParts(Photo, Articul, Title, Description, ManufacturerId, MeasureUnit) " +
                                                 "VALUES(@Photo, @Articul, @Title, @Description, @ManufacturerId, @MeasureUnit);";

                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@Photo", sparePart.Photo);
                            cmd.Parameters.AddWithValue("@Articul", sparePart.Articul);
                            cmd.Parameters.AddWithValue("@Title", sparePart.Title);
                            cmd.Parameters.AddWithValue("@Description", sparePart.Description);
                            cmd.Parameters.AddWithValue("@MeasureUnit", sparePart.MeasureUnit);

                        
                            cmd.ExecuteNonQuery();

                            trans.Commit();
                        }//try
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new System.Data.SQLite.SQLiteException(ex.Message);
                        }//catch
                    }//using cmd
                }//using transaction

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
                using (SQLiteTransaction trans = connection.BeginTransaction())
                {             
                    using (SQLiteCommand cmd = new SQLiteCommand(null, connection, trans))
                    {
                        try
                        {
                            //Находим существующий manufacturerId в базе или добавляем новый объект если отсутствует.
                            int? manufId = (sparePart.Manufacturer != null) ? FindManufacturerId(sparePart.Manufacturer) : (int?)null;
                            cmd.Parameters.AddWithValue("@ManufacturerId", (manufId == 0) ? AddManufacturer(sparePart.Manufacturer, cmd) : manufId);

                            const string query = "UPDATE SpareParts SET Photo = @Photo, Articul = @Articul, Title = @Title, "
                                               + "Description = @Description, ManufacturerId = @ManufacturerId, MeasureUnit = @MeasureUnit "
                                               + "WHERE SparePartId = @SparePartId;";

                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);
                            cmd.Parameters.AddWithValue("@Photo",       sparePart.Photo);
                            cmd.Parameters.AddWithValue("@Articul",     sparePart.Articul);
                            cmd.Parameters.AddWithValue("@Title",       sparePart.Title);
                            cmd.Parameters.AddWithValue("@Description", sparePart.Description);
                            cmd.Parameters.AddWithValue("@MeasureUnit", sparePart.MeasureUnit);
                            
                            cmd.ExecuteNonQuery();

                            trans.Commit();
                        }//try
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new System.Data.SQLite.SQLiteException(ex.Message);
                        }//catch
                    }//using cmd
                }//using transaction

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

        /// <summary>
        /// Обновляет контрагента в таблице.
        /// </summary>
        /// <param name="contragent">Обновляемый контрагент</param>
        public static void UpdateContragent(IContragent contragent)
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
                            ContactInfo contactInfo = FindContactInfo(contragent); 
                            if (contragent.ContactInfo != null)
                            {
                                //Если есть у объекта, но нет в базе -- добавляем запись в таблицу. Если есть в базе -- обновляем запись.
                                if (contactInfo != null)
                                {
                                    contragent.ContactInfo.ContactInfoId = contactInfo.ContactInfoId;
                                    UpdateContactInfo(contragent.ContactInfo, cmd);
                                }//if
                                else
                                    contragent.ContactInfo.ContactInfoId = AddContactInfo(contragent.ContactInfo, cmd);                                
                            }//if

                            //Вставляем запись в Customers или Suppliers.
                            UpdateContragent(contragent, cmd);

                            //Если есть в базе, но нет у объекта -- удаляем запись с базы
                            if (contactInfo != null && contragent.ContactInfo == null)
                                DeleteContactInfo(contactInfo.ContactInfoId, cmd);

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
        }//UpdateContragent



        /// <summary>
        /// Обновляет контрагента в таблице.
        /// </summary>
        /// <param name="contragent">Обновляемый контрагент</param>
        /// <param name="cmd"></param>
        public static void UpdateContragent(IContragent contragent, SQLiteCommand cmd)
        {
            string tableName = (contragent is Supplier) ? "Suppliers " : "Customers ";
            cmd.CommandText = "UPDATE " + tableName
                            + "SET ContragentName = @ContragentName, Code = @Code, Entity = @Entity, "
                            + "ContactInfoId = @ContactInfoId, Description = @Description, Balance = @Balance "
                            + "WHERE ContragentId = @ContragentId;";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ContragentId",   contragent.ContragentId);
            cmd.Parameters.AddWithValue("@ContragentName", contragent.ContragentName);
            cmd.Parameters.AddWithValue("@Code",           contragent.Code);
            cmd.Parameters.AddWithValue("@Entity",         contragent.Entity);
            cmd.Parameters.AddWithValue("@ContactInfoId", (contragent.ContactInfo != null) ? contragent.ContactInfo.ContactInfoId : (int?)null);
            cmd.Parameters.AddWithValue("@Description",    contragent.Description);
            cmd.Parameters.AddWithValue("@Balance",        contragent.Balance);

            cmd.ExecuteNonQuery();
        }//UpdateContragent








//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
      
        #region Модификация таблицы Manufacturers
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Добавляет нового производителя в БД и возвращает его Id.
        /// </summary>
        /// <param name="manufacturerName">Имя добавляемого производителя</param>
        /// <returns></returns>
        public static int AddManufacturer(string manufacturerName, SQLiteCommand cmd)
        {
            string query = String.Format("INSERT INTO Manufacturers(ManufacturerName) VALUES(@ManufacturerName); " +
                                         "SELECT ManufacturerId FROM Manufacturers WHERE rowid = last_insert_rowid();");
            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerName);

            return Convert.ToInt32(cmd.ExecuteScalar());    
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
            cmd.Parameters.AddWithValue("@Country",  contactInfo.Country);
            cmd.Parameters.AddWithValue("@Region",   contactInfo.Region);
            cmd.Parameters.AddWithValue("@City",     contactInfo.City);
            cmd.Parameters.AddWithValue("@Street",   contactInfo.Street);
            cmd.Parameters.AddWithValue("@House",    contactInfo.House);
            cmd.Parameters.AddWithValue("@Room",     contactInfo.Room);
            cmd.Parameters.AddWithValue("@Phone",    contactInfo.Phone);
            cmd.Parameters.AddWithValue("@ExtPhone", contactInfo.ExtPhone);
            cmd.Parameters.AddWithValue("@Website",  contactInfo.Website);
            cmd.Parameters.AddWithValue("@Email",    contactInfo.Email);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }//AddContactInfo




        /// <summary>
        /// Обновляем запись в таблице ContactInfo.
        /// </summary>
        /// <param name="contactInfo">Инф-ция для обновления.</param>
        /// <param name="cmd"></param>
        private static void UpdateContactInfo(ContactInfo contactInfo, SQLiteCommand cmd)
        {
            cmd.CommandText = "UPDATE ContactInfo "
                            + "SET Country = @Country, Region = @Region, City = @City, Street = @Street, House = @House, "
                            + "Room = @Room, Phone = @Phone, ExtPhone = @ExtPhone, Website = @Website, Email = @Email "
                            + "WHERE ContactInfoId = @ContactInfoId;";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ContactInfoId", contactInfo.ContactInfoId);
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

            cmd.ExecuteNonQuery();
        }//UpdateContactInfo

        /// <summary>
        /// Удаляет запись с заданным Id из таблицы ContactInfo.
        /// </summary>
        /// <param name="contactInfoId">Id удаляемой записи</param>
        /// <param name="cmd"></param>
        private static void DeleteContactInfo(int contactInfoId, SQLiteCommand cmd)
        {
            cmd.CommandText = "DELETE FROM ContactInfo "
                            + "WHERE ContactInfoId = @ContactInfoId;";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ContactInfoId", contactInfoId);

            cmd.ExecuteNonQuery();
        }//DeleteContactInfo
















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
        /// <param name="purchase">Приход который нужно добавить в таблицу.</param>
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

        /// <summary>
        /// Обновляет запись в БД, данными из переданного объекта.
        /// </summary>
        /// <param name="purchase">Объект. данными которого будет обновлена запись в БД</param>
        public static void UpdatePurchase(int purchaseId, string description)
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
                            string query = "UPDATE Purchases SET Description = @Description "
                                         + "WHERE OperationId = @OperationId;";
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@Description", description);
                            cmd.Parameters.AddWithValue("@OperationId", purchaseId);

                            cmd.ExecuteNonQuery();

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
        }//UpdatePurchase

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
        /// <param name="purchase">Информация о продаже.</param>
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
                            //Добавляем контрагента, если такого нет в базе, иначе обновляем его баланс.
                            if (sale.Contragent.ContragentId == 0)
                                sale.Contragent.ContragentId = AddContragent(sale.Contragent, cmd);
                            else
                                UpdateContragent(sale.Contragent);

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
        /// <param name="purchase">Продажа которую нужно добавить в таблицу.</param>
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


        /// <summary>
        /// Обновляет запись в БД, данными из переданного объекта.
        /// </summary>
        /// <param name="saleId">Ид обновляемой записи в базе.</param>
        /// <param name="description">новое описание</param>
        public static void UpdateSale(int saleId, string description)
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
                            string query = "UPDATE Sales SET Description = @Description "
                                         + "WHERE OperationId = @OperationId;";

                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@Description", description);
                            cmd.Parameters.AddWithValue("@OperationId", saleId);

                            cmd.ExecuteNonQuery();

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
        }//UpdateSale

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

        #region Модификация таблицы Returns.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Осуществляет возврат товара.
        /// </summary>
        /// <param name="operDetList">Список возвращаемого товара</param>
        /// <param name="note">Заметка по возврату</param>
        public static void AddReturn(Purchase purchase, string note)
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
                                AddSparePartAvaliability(new Availability(operDet, null, (float)Markup.Types.Retail), cmd);
                            }//foreach
                           
                            trans.Commit();  //фиксируем изменения.
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
        }//AddReturn

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
        }//AddReturn



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion












        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region ************Точный поиск по БД.*********************************************************************************
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       

        #region *****************Поиск по таблицам Avaliablility********************************************************************
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       
        
       
       





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

                const string query = "SELECT * FROM SpareParts as sp LEFT JOIN Manufacturers AS m "
                                   + "ON m.ManufacturerId = sp.ManufacturerId WHERE SparePartId = @SparePartId;";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                            sparePart = CreateSparePart(dataReader);
                    }//using dataReader
                }//using cmd
                
                connection.Close();
            }//using

            return sparePart;
        }//FindSparePart

        /// <summary>
        /// Возвращает список запчастей с заданным артикулом. 
        /// </summary>
        /// <param name="articul">Артикул.</param>
        /// <returns></returns>
        public static List<SparePart> FindSparePartsByArticul(string articul)
        {
            List<SparePart> sparePartsList = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM SpareParts as sp LEFT JOIN Manufacturers AS m "
                                   + "ON m.ManufacturerId = sp.ManufacturerId WHERE ToLower(Articul) LIKE @Articul;";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Articul", articul.ToLower());

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                            sparePartsList.Add(CreateSparePart(dataReader));
                    }//using dataReader

                }//using cmd

                connection.Close();
            }//using

            return sparePartsList;
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
      
        /// <summary>
        /// Возвращает список Id-ков производителей с заданным именем.
        /// </summary>
        /// <param name="manufacturerName">Имя искомых производителей.</param>
        /// <returns></returns>
        public static int FindManufacturerId(string manufacturerName)
        {
            int manufacturerId = 0;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT ManufacturerId FROM Manufacturers WHERE ManufacturerName = @ManufacturerName;";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerName);

                    object executeScalar = cmd.ExecuteScalar();
                    manufacturerId = (executeScalar != null) ? Convert.ToInt32(executeScalar) : 0;
                }//using cmd

                connection.Close();
            }//using

            return manufacturerId;       
        }//FindManufacturerId
        




























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
                    Supplier supplier = CreateSupplier(dataReader);

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
            Supplier supplier = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM  Suppliers WHERE ContragentId = @ContragentId;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ContragentId", supplierId);

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    supplier = CreateSupplier(dataReader);
                }//while

                connection.Close();
            }//using

            return supplier;
        }//FindSuppliers

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

                const string query = "SELECT * FROM Suppliers WHERE ToLower(ContragentName) LIKE @ContragentName;";
                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ContragentName", supplierName.ToLower());

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    supplier = CreateSupplier(dataReader);
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




        private static Supplier CreateSupplier(SQLiteDataReader dataReader)
        {
            return new Supplier
            (
                contragentId    : Convert.ToInt32(dataReader["ContragentId"]),
                contragentName  : dataReader["ContragentName"] as string,
                code            : dataReader["Code"] as string,
                entity          : dataReader["Entity"] as string,   
                contactInfo     : (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfo(Convert.ToInt32(dataReader["ContactInfoId"])) : null,
                description     : dataReader["Description"] as string
            );            
        }//CreateSupplier









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
                    customers.Add(CreateCustomers(dataReader));


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

                const string query = "SELECT * FROM Customers WHERE ToLower(ContragentName) LIKE @ContragentName;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@ContragentName", customerName.ToLower());

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        customer = CreateCustomers(dataReader);
                }//using dataReader

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
                    customer.ContactInfo = (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfo(Convert.ToInt32(dataReader["ContactInfoId"])) : null;
                    customer.Description = (dataReader["Description"] == DBNull.Value) ? null : dataReader["Description"] as string;
                }//while

                connection.Close();
            }//using

            return customer;        
        }//FindCustomerByName










        private static Customer CreateCustomers(SQLiteDataReader dataReader)
        {
            return new Customer
            (
                contragentId   : Convert.ToInt32(dataReader["ContragentId"]),
                contragentName : dataReader["ContragentName"] as string,
                code           : dataReader["Code"] as string,
                entity         : dataReader["Entity"] as string,
                contactInfo    : (dataReader["ContactInfoId"] != DBNull.Value) ? FindContactInfo(Convert.ToInt32(dataReader["ContactInfoId"])) : null,
                description    : dataReader["Description"] as string,
                balance        : (double)dataReader["Balance"]
            );
        }//CreateCustomers








        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблице Purchases и Sales.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        



        /// <summary>
        /// Возвращает список всех операций проведённых за указанный период.
        /// </summary>
        /// <param name="startDate">Минимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <param name="endDate">Максимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <returns></returns>
        public static List<IOperation> FindOperations(DateTime? startDate, DateTime? endDate)
        {
            List<IOperation> operationsList = new List<IOperation>();

            FindPurchases(startDate, endDate).ForEach(p => operationsList.Add(p)); //Заполняем список операций всеми поставками.
            FindSales(startDate, endDate).ForEach(s => operationsList.Add(s));     //Заполняем список операций всеми продажами.

            return operationsList;
        }//FindOperations

        /// <summary>
        /// Возвращает список всех операций производимых с заданным товаром.
        /// </summary>
        /// <param name="sparePartId">Ид искомого товара.</param>
        /// <returns></returns>
        public static List<IOperation> FindOperations(SparePart sparePart)
        {
            List<IOperation> operationsList = new List<IOperation>();

            FindPurchases(sparePart).ForEach(p => operationsList.Add(p)); //Заполняем список операций всеми поставками.
            FindSales(sparePart).ForEach(s => operationsList.Add(s));     //Заполняем список операций всеми продажами.
                                    
            return operationsList;
        }//FindOperations

        /// <summary>
        /// Возвращает список всех операций осуществлённых данным сотрудником.
        /// </summary>
        /// <param name="emp">Сотрудник по которому выдаются данные.</param>
        /// <param name="startDate">Минимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <param name="endDate">Максимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <returns></returns>
        public static List<IOperation> FindOperations(Employee emp,  DateTime? startDate, DateTime? endDate)
        {
            List<IOperation> operationsList = new List<IOperation>();

            FindPurchases(emp, startDate, endDate).ForEach(p => operationsList.Add(p)); //Заполняем список операций всеми поставками.
            FindSales(emp, startDate, endDate).ForEach(s => operationsList.Add(s));     //Заполняем список операций всеми продажами.

            return operationsList;
        }//FindOperations





        /// <summary>
        /// Возвращает объект типа Purchase, найденный по заданному Id.
        /// </summary>
        /// <param name="saleId">Id поставки</param>
        /// <returns></returns>
        public static Purchase FindPurchase(int purchaseId)
        {
            Purchase purchase = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                    }//using dataReader
                }//using cmd

                connection.Close();
            }//using

            return purchase;
        }//FindPurchase

        /// <summary>
        /// Возвращает объект типа Sale, найденный по заданному Id.
        /// </summary>
        /// <param name="saleId">Id продажи</param>
        /// <returns></returns>
        public static Sale FindSale(int saleId)
        {
            Sale sale = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                            sale = CreateSale(dataReader);
                    }//using dataReader
                }//using cmd

                connection.Close();
            }//using

            return sale;
        }//FindSale

        public static List<IOperation> FindPurchases(int supplierId, SparePart spr)
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


        public static List<Purchase> FindPurchases(SparePart sparePart)
        {
            List<Purchase> purchases = new List<Purchase>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                }//using dataReader

                connection.Close();
            }//using
            
            return purchases;
        }//FindPurchases
        public static List<Sale> FindSales(SparePart sparePart)
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
                cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);

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
        /// Возвращает список операций приходования осуществленных данным сотрудником.
        /// </summary>
        /// <param name="emp">Сотрудник по которому выдаются данные.</param>
        /// <param name="startDate">Минимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <param name="endDate">Максимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <returns></returns>
        public static List<Purchase> FindPurchases(Employee emp, DateTime? startDate, DateTime? endDate)
        {
            List<Purchase> purchases = new List<Purchase>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                }//using dataReader

                connection.Close();
            }//using

            return purchases;
        }//FindPurchases
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

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                        salesList.Add(CreateSale(dataReader));
                }//using dataReader

                connection.Close();
            }//using

            return salesList;
        }//FindSales

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

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                }//using dataReader

                connection.Close();
            }//using

            return purchases;
        }//FindPurchases
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

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                employee           : (dataReader["EmployeeId"] != DBNull.Value) ? FindEmployees(Convert.ToInt32(dataReader ["EmployeeId"])) : null,
                contragent         : FindCustomers(Convert.ToInt32(dataReader["ContragentId"])),
                contragentEmployee : dataReader["ContragentEmployee"] as string,
                operationDate      : Convert.ToDateTime(dataReader["OD"]),
                description        : dataReader["Description"] as string
            );
        }//CreateSale




        /// <summary>
        /// Возвращает детали операции для заданного прихода.
        /// </summary>
        /// <param name="purchase">Приход.</param>
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
        /// <param name="purchase">Приход.</param>
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

        /// <summary>
        /// Находит список возвращенного товара по заданному Id продажи.
        /// </summary>
        /// <param name="saleId">Id продажи</param>
        /// <returns></returns>
        public static List<OperationDetails> FindReturnDetails(int saleId)
        {
            List<OperationDetails> operDetList = new List<OperationDetails>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
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
                            operDetList.Add(CreateOperationDetails(dataReader, (IOperation)null));
                    }//using dataReader
                }//using cmd

                connection.Close();
            }//using

            return operDetList;
        }//FindReturnDetails

        /// <summary>
        /// Возвращает минимальную закупочную цену для переданного товара.
        /// </summary>
        /// <param name="sparePartId">Ид товара для которого находится мин. закупочная цена</param>
        /// <returns></returns>
        public static float FindMinSparePartPurchasePrice(int sparePartId)
        {
            float minPrice = 0;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT MIN(Price) FROM PurchaseDetails "
                                   + "WHERE SparePartId = @SparePartId;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SparePartId", sparePartId);

                    minPrice = Convert.ToSingle(cmd.ExecuteScalar());
                }//using cmd

                connection.Close();
            }//using

            return minPrice;
        }//FindMinSparePartPurchasePrice


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
                sparePart: FindSparePart(Convert.ToInt32(dataReader["SparePartId"])),
                operation: operat,
                count: Convert.ToSingle(dataReader["Count"]),
                price: Convert.ToSingle(dataReader["Price"])
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
                birthDate      : (dataReader["BirthDate"] != DBNull.Value) ? GetDateTime(dataReader["BirthDate"] as string) : (DateTime?)null,
                hireDate       : (dataReader["HireDate"] != DBNull.Value) ? GetDateTime(dataReader["HD"] as string) : (DateTime?)null,
                dismissalDate  : (dataReader["DismissalDate"] != DBNull.Value) ? GetDateTime(dataReader["DD"] as string) : (DateTime?)null,                
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
        public static ContactInfo FindContactInfo(int contactInfoId)
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
        }//FindContactInfo

        /// <summary>
        /// Возвращает объект типа ContactInfo, найденный по заданному Id сотрудника, или null если ничего не найдено.
        /// </summary>
        /// <param name="employeeId">Id сотрудника.</param>
        /// <returns></returns>
        public static ContactInfo FindContactInfo(Employee employee)
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
                cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);

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
        /// Возвращает объект типа ContactInfo, найденный по заданному Id контрагента, или null если ничего не найдено.
        /// </summary>
        /// <param name="employeeId">Id сотрудника.</param>
        /// <returns></returns>
        public static ContactInfo FindContactInfo(IContragent contragent)
        {
            ContactInfo contactInfo = null;

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                string tableName = (contragent is Supplier) ? "Suppliers" : "Customers";
                string query = "SELECT ci.* FROM " + tableName + " as c "
                             + "JOIN ContactInfo as ci "
                             + "ON c.ContactInfoId = ci.ContactInfoId "
                             + "WHERE ContragentId = @ContragentId;";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@ContragentId", contragent.ContragentId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                        contactInfo = CreateContactInfo(dataReader);
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
        /// Возвращает список из товаров, найденных по совпадению Артикула, Названия или Производителя с переданной строкой.
        /// </summary>
        /// <param name="titleOrArticulOrManuf">Строка с которой ищутся совпадения.</param>
        /// <param name="onlyInAvailability">true - если искать среди товара в наличии, false - среди всего товара в базе.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSpareParts(string titleOrArticulOrManuf, bool onlyInAvailability)
        {
            return SearchSpareParts(titleOrArticulOrManuf, onlyInAvailability, -1);
        }//SearchSpareParts

        /// <summary>
        /// Возвращает список из товаров, найденных по совпадению Артикула, Названия или Производителя с переданной строкой.
        /// </summary>
        /// <param name="titleOrArticulOrManuf">Строка с которой ищутся совпадения.</param>
        /// <param name="onlyInAvailability">true - если искать среди товара в наличии, false - среди всего товара в базе.</param>
        /// <param name="limit">Максимальное кол-во эл-тов списка.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSpareParts(string titleOrArticulOrManuf, bool onlyInAvailability, int limit)
        {
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                string query = "SELECT DISTINCT sp.*, m.* FROM SpareParts AS sp "
                             + ((onlyInAvailability) ? "JOIN Avaliability AS a ON sp.SparePartId = a.SparePartId " : String.Empty)
                             + "LEFT JOIN Manufacturers AS m ON m.ManufacturerId = sp.ManufacturerId "
                             + "WHERE ToLower(sp.Articul) LIKE @TitleOrArticul OR ToLower(sp.Title) LIKE @TitleOrArticul "
                             + "OR ToLower(m.ManufacturerName) LIKE @TitleOrArticul "
                             + "LIMIT @limit;";
                

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@TitleOrArticul", "%" + titleOrArticulOrManuf.ToLower() + "%");
                    cmd.Parameters.AddWithValue("@limit", limit);

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                            spareParts.Add(CreateSparePart(dataReader));
                    }//using dataReader
                }//using cmd

                connection.Close();
            }//using
            return spareParts;
        }//SearchSpareParts


        /// <summary>
        /// Возвращает список из товаров, найденных по совпадению Названия с переданной строкой.
        /// </summary>
        /// <param name="title">Название товара.</param>
        /// <param name="withoutIDs">Список Id товаров которые игнорируются при поиске.</param>
        /// <param name="onlyInAvailability">true - если искать среди товара в наличии, false - среди всего товара в базе.</param>
        /// <param name="limit">Максимальное кол-во эл-тов списка.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSparePartsByTitle(string title, IList<int> withoutIDs, bool onlyInAvailability, int limit)
        {
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                
                //Формируем строку с Id товара который пропускается при поиске.                
                StringBuilder notIn = new StringBuilder();
                foreach(int id in withoutIDs)                    
                    notIn.Append(id + ", ");

                if (withoutIDs.Count > 0)
                    notIn.Remove(notIn.Length - 2, 2); //убираем последний добавленный пробел и запятую ", ".

                string query = "SELECT sp.*, m.* FROM SpareParts AS sp "
                             + "LEFT JOIN Manufacturers AS m ON m.ManufacturerId = sp.ManufacturerId "
                             + ((onlyInAvailability) ? "JOIN Avaliability AS av ON av.SparePartId = sp.SparePartId " : String.Empty)
                             + "WHERE ToLower(sp.Title) LIKE @Title AND sp.SparePartId NOT IN(" + notIn + ")"
                             + "GROUP BY sp.SparePartId "
                             + "LIMIT @Limit;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Title", title.ToLower() + "%");
                    cmd.Parameters.AddWithValue("@Limit", limit);
                    cmd.Parameters.AddWithValue("@NotIn", notIn);

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                            spareParts.Add(CreateSparePart(dataReader));
                    }//using dataReader
                }//using cmd

                connection.Close();
            }//using

            return spareParts;
        }//SearchSparePartsByTitle  

        /// <summary>
        /// Возвращает список из товаров, найденных по совпадению Артикула с переданной строкой.
        /// </summary>
        /// <param name="articul">Артикул товара.</param>
        /// <param name="withoutIDs">Список Id товаров которые игнорируются при поиске.</param>
        /// <param name="onlyInAvailability">true - если искать среди товара в наличии, false - среди всего товара в базе.</param>
        /// <param name="limit">Максимальное кол-во эл-тов списка.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSparePartsByArticul(string articul, IList<int> withoutIDs, bool onlyInAvailability, int limit)
        {
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                //Формируем строку с Id товара который пропускается при поиске.                
                StringBuilder notIn = new StringBuilder();
                foreach (int id in withoutIDs)
                    notIn.Append(id + ", ");

                if (withoutIDs.Count > 0)
                    notIn.Remove(notIn.Length - 2, 2); //убираем последний добавленный пробел и запятую ", ".

                string query = "SELECT sp.*, m.* FROM SpareParts AS sp "
                             + "LEFT JOIN Manufacturers AS m ON m.ManufacturerId = sp.ManufacturerId "
                             + ((onlyInAvailability) ? "JOIN Avaliability AS av ON av.SparePartId = sp.SparePartId " : String.Empty)
                             + "WHERE ToLower(sp.Articul) LIKE @Articul AND sp.SparePartId NOT IN(" + notIn + ")"
                             + "GROUP BY sp.SparePartId "
                             + "LIMIT @Limit;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Articul", articul.ToLower() + "%");
                    cmd.Parameters.AddWithValue("@Limit", limit);
                    cmd.Parameters.AddWithValue("@NotIn", notIn);

                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                            spareParts.Add(CreateSparePart(dataReader));
                    }//using dataReader
                }//using cmd

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
                description    : dataReader["Description"] as string,
                manufacturer   : dataReader["ManufacturerName"] as string,
                measureUnit    : dataReader["MeasureUnit"] as string             
            );     
        }//CreateSparePart

        /// <summary>
        /// Метод создания бэкапа
        /// </summary>
        public static void CreateLocalBackup()
        {           
            //Если нет папки для бэкапа, создаём её.
            if (System.IO.Directory.Exists(@"Data\Backup") == false)            
                System.IO.Directory.CreateDirectory(@"Data\Backup");                

            //Создаём новый бэкап или обновляем существующий.
            using (SQLiteConnection source = GetDatabaseConnection(SparePartConfig) as SQLiteConnection)
            {
                using (SQLiteConnection dest = GetDatabaseConnection("BackupConfig") as SQLiteConnection)
                {
                    source.Open();
                    dest.Open();
                    source.BackupDatabase(dest, "main", "main", -1, null, 0);
                }//using
            }//using

        }//CreateLocalBackup


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


        #region GoogleDriveAPI 

        public static void CreateBackupInGoogleDrive()
        {                        
            UserCredential credential = GetCredential();
            DriveService service = GetService(credential);

            string fileName = "TruckTirDB.db";
            
            Google.Apis.Drive.v3.Data.File backupFile = GetFile(service, fileName);  //Находим существующий файл бэкапа (Если строку заккоментировать, выдаёт ошибку "Файл используетс другим процессом", ПОЧЕМУ??)
                        
            //Записываем новый файл.
            UploadFileToDrive(service, fileName);

            ////Удаляем старый файл бэкапа, если он существует. 
            //if (backupFile?.Id != null)
            //    service.Files.Delete(backupFile?.Id).Execute();
        }//CreateBackupInGoogleDrive


        private static UserCredential GetCredential()
        {
            string[] Scopes = { DriveService.Scope.Drive };
            
            using (var stream = new System.IO.FileStream("client_secret.json", System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                string credPath = @"Data\Backup";
                //string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                //credPath = System.IO.Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                            GoogleClientSecrets.Load(stream).Secrets,
                            Scopes,
                            "user",
                            System.Threading.CancellationToken.None,
                            new FileDataStore(credPath, true)).Result;
            }//using
        }//GetCredential

        private static DriveService GetService(UserCredential credential)
        {
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API .NET Quickstart"
            });
        }//GetService


        /// <summary>
        /// Возвращаем файл по заданному имени.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fileName">Имя файла</param>
        /// <returns></returns>
        private static Google.Apis.Drive.v3.Data.File GetFile(DriveService service, string fileName)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            //listRequest.PageSize = 5;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

            if (files != null && files.Count > 0)
            {
                //Перебираем все файлы и возвращаем Id нужного.
                foreach (Google.Apis.Drive.v3.Data.File file in files)
                {
                    if (file.Name == fileName)
                        return file;
                }//foreach                    
            }//if         

            return null;
        }//GetFile

        private static void DeleteFile(DriveService service)
        {           
            string fileId = "0B4jQdT8KbxhbVUxEMHRvanY5dDA";
            //Google.Apis.Drive.v3.FilesResource.CreateRequest request = service.Files.Get()

            Google.Apis.Drive.v3.FilesResource.DeleteRequest request = service.Files.Delete(fileId);

            request.Execute();            
        }//DeleteFile

        private static void UploadFileToDrive(DriveService service, string fileName)
        {
            string uploadFileName = $"TruckTirDB_{DateTime.Now.ToShortDateString()}.db";
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = uploadFileName;            

            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream("Data\\" + fileName, System.IO.FileMode.Open))
            {                
                request = service.Files.Create(fileMetadata, stream, "application/zip");   //Работает только с типом "application/zip", c др. типами не загружает в облако.
                request.Upload();
            }//using

            Google.Apis.Drive.v3.Data.File file = request.ResponseBody;
            //return file.Id;
        }//UploadFileToDrive

        #endregion
























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