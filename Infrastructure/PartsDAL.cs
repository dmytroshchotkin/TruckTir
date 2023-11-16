using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Infrastructure;
using Infrastructure.Storage;
using Infrastructure.Storage.PropertiesHandlers;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace PartsApp
{
    public static class PartsDAL
    {
        private const string InvalidTypeMessage = "The object has invalid type";

        #region ************ Модификация данных в БД. ******************************************************************************
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region !!! Модификация таблицы Avaliability - метод с вызовом из формы
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Изменяет наценку у записей с заданными SparePartId и PurchaseId на заданную Markup
        /// </summary>
        /// <param name="changeMarkupDict">Словарь типа (sparePartId, IDictionary(saleId, markup))</param>
        public static void UpdateSparePartMarkup(List<Availability> availList)
        {
            AvailabilityDatabaseHandler.UpdateSparePartMarkup(availList);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы SpareParts.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void AddSparePart(SparePart sparePart)
        {
            SparePartRepository.AddSparePart(sparePart);
        }

        /// <summary>
        /// Метод модификации записи с заданным Id.
        /// </summary>
        /// <param name="avail">Товар инф-ция о котором модифицируется.</param>
        public static void UpdateSparePart(SparePart sparePart)
        {
            SparePartRepository.UpdateSparePart(sparePart);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблиц Suppliers и Customers.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        

        /// <summary>
        /// Добавляет переданный объект в БД.
        /// </summary>
        /// <param name="contragent">Контрагент.</param>
        public static void AddContragent(IContragent contragent)
        {
            if (contragent is Supplier)
            {
                SupplierRepository.AddSupplier(contragent as Supplier);
            }

            else if (contragent is Customer)
            {
                CustomerRepository.AddCustomer(contragent as Customer);
            }

            else
            {
                throw new ArgumentException(InvalidTypeMessage);
            }
        }

        /// <summary>
        /// Обновляет контрагента в таблице.
        /// </summary>
        /// <param name="contragent">Обновляемый контрагент</param>
        public static void UpdateContragent(IContragent contragent)
        {
            if (contragent is Supplier)
            {
                SupplierRepository.UpdateSupplier(contragent as Supplier);
            }

            else if (contragent is Customer)
            {
                CustomerRepository.UpdateCustomer(contragent as Customer);
            }

            else
            {
                throw new ArgumentException(InvalidTypeMessage);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Purchase.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Осуществляет полный цикл приходования товара, вставляя записи в таблицы Purchases, Avaliability и PurchaseDetails.
        /// Возвращает Id вставленной записи в табл. Operation.
        /// </summary>
        /// <param name="availList">Список приходуемого товара.</param>
        /// <returns></returns>
        public static int AddPurchase(List<Availability> availList)
        {
            return PurchaseRepository.AddPurchase(availList);
        }

        /// <summary>
        /// Обновляет запись в БД, данными из переданного объекта.
        /// </summary>
        /// <param name="purchase">Объект. данными которого будет обновлена запись в БД</param>
        public static void UpdatePurchase(int purchaseId, string description)
        {
            PurchaseRepository.UpdatePurchase(purchaseId, description);
        }

        /// <summary>
        /// Осуществляет возврат товара.
        /// </summary>
        /// <param name="operDetList">Список возвращаемого товара</param>
        /// <param name="note">Заметка по возврату</param>
        public static void AddReturn(Purchase purchase, string note)
        {
            PurchaseRepository.AddReturn(purchase, note);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Sales.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Осуществляет полный цикл продажи товара, вставляя записи в таблицы Sales, Avaliability и SaleDetails.
        /// Возвращает Id вставленной записи в табл. Sale.
        /// </summary>
        /// <param name="availabilityList">Список продаваемого товара.</param>
        /// <param name="purchase">Информация о продаже.</param>
        /// <returns></returns>
        public static int AddSale(Sale sale, List<OperationDetails> operDetList)
        {
            return SaleRepository.AddSale(sale, operDetList);
        }

        /// <summary>
        /// Обновляет запись в БД, данными из переданного объекта.
        /// </summary>
        /// <param name="saleId">Ид обновляемой записи в базе.</param>
        /// <param name="description">новое описание</param>
        public static void UpdateSale(int saleId, string description)
        {
            SaleRepository.UpdateSale(saleId, description);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Employees.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void AddEmployee(Employee employee)
        {
            EmployeeRepository.AddEmployee(employee);
        }

        /// <summary>
        /// Метод обновляющий значения заданного сотрудника.
        /// </summary>
        /// <param name="employee">Сотрудник, значения которого необходимо обновить в базе.</param>
        public static void UpdateEmployee(Employee employee)
        {
            EmployeeRepository.UpdateEmployee(employee);
        }

        /// <summary>
        /// Метод обновляющий значения заданного сотрудника, без обновления его пароля.
        /// </summary>
        /// <param name="employee">Сотрудник, значения которого необходимо обновить в базе.</param>
        public static void UpdateEmployeeWithoutPassword(Employee employee)
        {
            EmployeeRepository.UpdateEmployeeWithoutPassword(employee);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region ************ Точный поиск по БД. ***********************************************************************************
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       

        #region Поиск по таблицам SpareParts и Manufacturers.
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        /// <summary>
        /// Возвращает список запчастей с заданным артикулом. 
        /// </summary>
        /// <param name="articul">Артикул.</param>
        /// <returns></returns>
        public static List<SparePart> FindSparePartsByArticul(string articul)
        {
            return SparePartRepository.FindSparePartsByArticul(articul);
        }

        public static string[] FindAllManufacturersName()
        {
            return SparePartRepository.FindAllManufacturersName();
        }

        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion

        #region Поиск по таблице Suppliers.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает коллекцию из всех Supplier-ов.
        /// </summary>
        /// <returns></returns>
        public static IList<Supplier> FindSuppliers()
        {
            return SupplierRepository.FindSuppliers();
        }

        /// <summary>
        /// Возвращает объект типа Contragent по заданному Id.
        /// </summary>
        /// <param name="supplierId">Id поставщика, которого надо найти.</param>
        /// <returns></returns>
        public static Supplier FindSuppliers(int supplierId)
        {
            return SupplierRepository.FindSuppliers(supplierId);
        }

        /// <summary>
        /// Возвращает объект Supplier найденный по заданному SupplierName, или null если такого объекта не найдено.
        /// </summary>
        /// <param name="SupplierName">имя Supplier-а, которого надо найти.</param>
        /// <returns></returns>
        public static Supplier FindSuppliers(string supplierName)
        {
            return SupplierRepository.FindSupplier(supplierName);
        }

        /// <summary>
        /// Возвращает true если такой code уже есть в таблице Suppliers, иначе false.
        /// </summary>
        /// <param name="code">code наличие которого нужно проверить.</param>
        /// <returns></returns>
        public static bool IsSupplierCodeExist(string code)
        {
            return SupplierRepository.IsSupplierCodeExist(code);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблицe Customers.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает коллекцию из всех Customer.
        /// </summary>
        /// <returns></returns>
        public static IList<Customer> FindCustomers()
        {        
            return CustomerRepository.FindCustomers();
        }

        /// <summary>
        /// Возвращает объект Customer найденный по заданному customerName, или null если такого объекта не найдено.
        /// </summary>
        /// <param name="customerName">имя Customer-а, которого надо найти.</param>
        /// <returns></returns>
        public static Customer FindCustomers(string customerName)
        {
            return CustomerRepository.FindCustomers(customerName);
        }

        /// <summary>
        /// Возвращает объект типа Customer найденный по заданному Id.
        /// </summary>
        /// <param name="customerId">Id клиента, которого надо найти.</param>
        /// <returns></returns>
        public static Customer FindCustomers(int customerId)
        {
            return CustomerRepository.FindCustomer(customerId);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion               

        #region Поиск по таблице Purchases.
        /// <summary>
        /// Возвращает объект типа Purchase, найденный по заданному Id.
        /// </summary>
        /// <param name="saleId">Id поставки</param>
        /// <returns></returns>
        public static Purchase FindPurchase(int purchaseId)
        {
            return PurchaseRepository.FindPurchase(purchaseId);
        }

        public static List<Purchase> FindPurchases(int supplierId, SparePart spr)
        {
            return PurchaseRepository.FindPurchases(supplierId, spr);
        }

        /// <summary>
        /// Находит список возвращенного товара по заданному Id продажи.
        /// </summary>
        /// <param name="saleId">Id продажи</param>
        /// <returns></returns>
        public static List<OperationDetails> FindReturnDetails(int saleId)
        {
            return PurchaseRepository.FindReturnDetails(saleId);
        }
        #endregion

        #region Поиск по таблице Sales.
        /// <summary>
        /// Возвращает объект типа Sale, найденный по заданному Id.
        /// </summary>
        /// <param name="saleId">Id продажи</param>
        /// <returns></returns>
        public static Sale FindSale(int saleId)
        {
            return SaleRepository.FindSale(saleId);
        }

        public static List<Sale> FindSales(int customerId, Customer cust)
        {
            return SaleRepository.FindSales(customerId, cust);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблице Employees.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает список из объектов типа Employee, состоящий из всех сотрудников.
        /// </summary>
        /// <returns></returns>
        public static List<Employee> FindEmployees()
        {
            return EmployeeRepository.FindEmployees();
        }

        /// <summary>
        /// Возвращает объект типа Employee, найденный по заданному Id.
        /// </summary>
        /// <param name="employeeId">Ид сотрудника, которого надо найти.</param>
        /// <returns></returns>
        public static Employee FindEmployees(int employeeId)
        {
            return EmployeeRepository.FindEmployees(employeeId);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region !!! ВЫНЕСТИ В PRESENTATION Поиск по обеим таблицам Sales и Purchase для вывода рез-та в форме
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает список всех операций проведённых за указанный период.
        /// </summary>
        /// <param name="startDate">Минимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <param name="endDate">Максимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <returns></returns>
        public static List<IOperation> FindOperations(DateTime? startDate, DateTime? endDate)
        {
            List<IOperation> operationsList = new List<IOperation>();

            PurchaseRepository.FindPurchases(startDate, endDate).ForEach(p => operationsList.Add(p)); //Заполняем список операций всеми поставками.
            SaleRepository.FindSales(startDate, endDate).ForEach(s => operationsList.Add(s));     //Заполняем список операций всеми продажами.

            return operationsList;
        }

        /// <summary>
        /// Возвращает список всех операций производимых с заданным товаром.
        /// </summary>
        /// <param name="sparePartId">Ид искомого товара.</param>
        /// <returns></returns>
        public static List<IOperation> FindOperations(SparePart sparePart)
        {
            List<IOperation> operationsList = new List<IOperation>();

            PurchaseRepository.FindPurchases(sparePart).ForEach(p => operationsList.Add(p)); //Заполняем список операций всеми поставками.
            SaleRepository.FindSales(sparePart).ForEach(s => operationsList.Add(s));     //Заполняем список операций всеми продажами.

            return operationsList;
        }

        /// <summary>
        /// Возвращает список всех операций осуществлённых данным сотрудником.
        /// </summary>
        /// <param name="emp">Сотрудник по которому выдаются данные.</param>
        /// <param name="startDate">Минимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <param name="endDate">Максимальная дата для операции входящей в список. Если null, то ограничения нет.</param>
        /// <returns></returns>
        public static List<IOperation> FindOperations(Employee emp, DateTime? startDate, DateTime? endDate)
        {
            List<IOperation> operationsList = new List<IOperation>();

            PurchaseRepository.FindPurchases(emp, startDate, endDate).ForEach(p => operationsList.Add(p)); //Заполняем список операций всеми поставками.
            SaleRepository.FindSales(emp, startDate, endDate).ForEach(s => operationsList.Add(s));     //Заполняем список операций всеми продажами.

            return operationsList;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск совпадений SparePart по БД.
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает список из товаров, найденных по совпадению Артикула, Названия или Производителя с переданной строкой.
        /// </summary>
        /// <param name="titleOrArticulOrManuf">Строка с которой ищутся совпадения.</param>
        /// <param name="onlyInAvailability">true - если искать среди товара в наличии, false - среди всего товара в базе.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSpareParts(string titleOrArticulOrManuf, bool onlyInAvailability)
        {
            return SparePartRepository.SearchSpareParts(titleOrArticulOrManuf, onlyInAvailability, -1);
        }

        /// <summary>
        /// Возвращает список из товаров, найденных по совпадению Артикула, Названия или Производителя с переданной строкой.
        /// </summary>
        /// <param name="titleOrArticulOrManuf">Строка с которой ищутся совпадения.</param>
        /// <param name="onlyInAvailability">true - если искать среди товара в наличии, false - среди всего товара в базе.</param>
        /// <param name="limit">Максимальное кол-во эл-тов списка.</param>
        /// <returns></returns>
        public static List<SparePart> SearchSpareParts(string titleOrArticulOrManuf, bool onlyInAvailability, int limit)
        {
            return SparePartRepository.SearchSpareParts(titleOrArticulOrManuf, onlyInAvailability, limit);
        }

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
            return SparePartRepository.SearchSparePartsByTitle(title, withoutIDs, onlyInAvailability, limit);
        }

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
            return SparePartRepository.SearchSparePartsByArticul(articul, withoutIDs, onlyInAvailability, limit);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Вспомогательные методы.
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Метод регистрирующий в базе User-Defined Functions.
        /// </summary>
        public static void RegistrateUDFs()
        {
            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                //SQLiteCommand cmd = new SQLiteCommand("PRAGMA integrity_check", connection);
                //cmd.ExecuteNonQuery();  

                SQLiteFunction.RegisterFunction(typeof(LowerRegisterConverter));

                connection.Close();
            }
        }

        /// <summary>
        /// Метод создания бэкапа
        /// </summary>
        public static void CreateLocalBackup()
        {
            //Если нет папки для бэкапа, создаём её.
            if (System.IO.Directory.Exists(@"Data\Backup") == false)
            {
                System.IO.Directory.CreateDirectory(@"Data\Backup");
            }                

            //Создаём новый бэкап или обновляем существующий.
            using (SQLiteConnection source = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                using (SQLiteConnection dest = DbConnectionHelper.GetDatabaseConnection("BackupConfig") as SQLiteConnection)
                {
                    source.Open();
                    dest.Open();
                    source.BackupDatabase(dest, "main", "main", -1, null, 0);
                }
            }
        }

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
        }


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
            }
        }

        private static DriveService GetService(UserCredential credential)
        {
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API .NET Quickstart"
            });
        }

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
                    {
                        return file;
                    }                        
                }
            }

            return null;
        }

        private static void DeleteFile(DriveService service)
        {
            string fileId = "0B4jQdT8KbxhbVUxEMHRvanY5dDA";
            //Google.Apis.Drive.v3.FilesResource.CreateRequest request = service.Files.Get()

            Google.Apis.Drive.v3.FilesResource.DeleteRequest request = service.Files.Delete(fileId);

            request.Execute();
        }

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
            }

            Google.Apis.Drive.v3.Data.File file = request.ResponseBody;
            //return file.Id;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }

    [SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "ToLower")]
    class LowerRegisterConverter : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            string initialString = (args[0] as string);
            return (initialString != null) ? initialString.ToLower() : null;
        }
    }
}
