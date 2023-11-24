using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace PartsApp2
{
    static class PartsDAL2
    {
        //Добавление записи в таблицу "SparePartsProviders". 
        public static void AddSparePart(IList<SparePart> spareParts)
        {
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                //var insertSQL = new SQLiteCommand("INSERT INTO SparePart(Photo, Articul, Title, Category, ManufacturerId, Unit, Count, Price, Markup) VALUES(@Photo, @Articul, @Title, @Category, @ManufacturerId, @Unit, @Count, @Price, @Markup)", connection);
                for (int i = 0; i < spareParts.Count; ++i)
                    PartsDAL.AddSparePart(spareParts[i], connection);
                connection.Close();
            }        }        public static void AddSparePartsProviders(int sparePartId, IList<int> providersId, SQLiteConnection openConnection)
        {
            var insertSQL = new SQLiteCommand("INSERT INTO SparePartsProviders VALUES (@sparePartId, @providerId)", openConnection);

            SQLiteParameter param = new SQLiteParameter();
            param.ParameterName = "@sparePartId";

            SQLiteParameter param2 = new SQLiteParameter();
            param2.ParameterName = "@providerId";

            insertSQL.Parameters.Add(param);
            insertSQL.Parameters.Add(param2);
            insertSQL.Prepare();

            for (int i = 0; i < providersId.Count; ++i)
            {
                param.Value = sparePartId;
                param2.Value = providersId[i];

                insertSQL.ExecuteNonQuery();
            }        }
        //Добавление в БД новой записи, или увеличение количества уже существующей. 
        public static void AddSparePartsWithUpdateCount(IList<SparePart> spareParts)
        {
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                for (int i = 0; i < spareParts.Count; ++i)
                {
                    //Проверяем есть ли уже в БД запчасть с таким артикулом.
                    IList<int> sparePartsId = PartsDAL.FindSparePartsIdByArticul(spareParts[i].Articul, connection);
                    //если запчасти с таким артикулом в Базе нет, то записываем запись в БД.
                    if (sparePartsId.Count == 0)
                        AddSparePart(spareParts[i], connection);
                    //если запчасть с таким артикулом есть, необходимо сравнивать их по Описанию.
                    else
                    {
                        sparePartsId = PartsDAL.FindSparePartsIdByTitle(spareParts[i].Title, sparePartsId, connection);

                        //на данный момент у нас может либо быть найдена 1 запись с таким Описанием, либо ниодной.
                        //если ниодной, просто вставляем запись в БД.
                        if (sparePartsId.Count == 0)
                            AddSparePart(spareParts[i], connection);
                        //если точно такая же запись уже есть в БД, то увеличиваем количество.
                        else PartsDAL.ChangeSparePartCount(sparePartsId[0], spareParts[i].Count, connection);
                    }                }                connection.Close();
            }        }
        //Добавления во вспомогательные таблицы.
        public static void AddUnitOfMeasure(string unit)
        {
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();

                //Вставляем запись в табл. "UnitOfMeasure"
                var insertSQL = new SQLiteCommand("INSERT INTO UnitOfMeasure VALUES(@Unit)", connection);

                insertSQL.Parameters.AddWithValue("@Unit", unit);

                insertSQL.ExecuteNonQuery();

                connection.Close();
            }        }        public static void AddUnitOfMeasure(string unit, SQLiteConnection openConnection)
        {
            //Вставляем запись в табл. "UnitOfMeasure"
            var insertSQL = new SQLiteCommand("INSERT INTO UnitOfMeasure VALUES(@Unit)", openConnection);

            insertSQL.Parameters.AddWithValue("@Unit", unit);

            insertSQL.ExecuteNonQuery();
        }
        //Нахождение всех данных БД.
        public static IList<SparePart> FindAllSpareParts()
        {
            const string selectString = "SELECT * FROM SparePart";
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand selectCommand = new SQLiteCommand(selectString, connection);
                var dataReader = selectCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart
                    {
                        Photo = dataReader["Photo"] as string,
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        Price = Convert.ToDouble(dataReader["Price"]),
                        Markup = Convert.ToDouble(dataReader["Markup"]),
                        Count = Convert.ToDouble(dataReader["Count"]),
                        Unit = dataReader["Unit"] as string
                    };
                    sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                    sparePart.Providers = FindSparePartProvidersById(sparePart.Id, connection);

                    spareParts.Add(sparePart);
                }                connection.Close();
            }            return spareParts;
        }
        //Обновление данных в БД.
        public static void UpdateSparePartTitle(int id, string title)
        {
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var updateCom = new SQLiteCommand("UPDATE SparePart SET Title = @Title WHERE Id = @Id", connection);

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Id";
                param.Value = id;

                SQLiteParameter param2 = new SQLiteParameter();
                param2.ParameterName = "@Title";
                param2.Value = title;

                updateCom.Parameters.Add(param);
                updateCom.Parameters.Add(param2);
                updateCom.Prepare();

                updateCom.ExecuteNonQuery();

                connection.Close();
            }
        }
        public static void ChangeSparePartCount(int id, double count)
        {
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var updateCom = new SQLiteCommand("UPDATE SparePart SET Count = Count + @Count WHERE Id = @Id", connection);

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Id";
                param.Value = id;

                SQLiteParameter param2 = new SQLiteParameter();
                param2.ParameterName = "@Count";
                param2.Value = count;

                updateCom.Parameters.Add(param);
                updateCom.Parameters.Add(param2);
                updateCom.Prepare();

                updateCom.ExecuteNonQuery();

                connection.Close();
            }        }        public static void ChangeSparePartCount(int id, double count, SQLiteConnection openConnection)
        {
            var updateCom = new SQLiteCommand("UPDATE SparePart SET Count = Count + @Count WHERE Id = @Id", openConnection);

            SQLiteParameter param = new SQLiteParameter();
            param.ParameterName = "@Id";
            param.Value = id;

            SQLiteParameter param2 = new SQLiteParameter();
            param2.ParameterName = "@Count";
            param2.Value = count;

            updateCom.Parameters.Add(param);
            updateCom.Parameters.Add(param2);
            updateCom.Prepare();

            updateCom.ExecuteNonQuery();

        }
        //Поиск по БД.
        public static IList<SparePart> FindSparePartByArticul(string sparePartArticul)
        {
            IList<SparePart> spareParts = new List<SparePart>();
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT * FROM SparePart WHERE Articul LIKE @Articul";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Articul";
                param.Value = sparePartArticul;

                cmd.Parameters.Add(param);
                cmd.Prepare();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    var sparePart = new SparePart
                    {
                        Photo = dataReader["Photo"] as string,
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        Price = Convert.ToDouble(dataReader["Price"]),
                        Markup = Convert.ToDouble(dataReader["Markup"]),
                        Count = Convert.ToDouble(dataReader["Count"]),
                        Unit = dataReader["Unit"] as string
                    };
                    sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                    sparePart.Providers = FindSparePartProvidersById(sparePart.Id, connection);

                    spareParts.Add(sparePart);
                }                connection.Close();
            }
            return spareParts;
        }        public static IList<SparePart> FindSparePartByArticul(string sparePartArticul, SQLiteConnection openConnection)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            var cmd = new SQLiteCommand(null, openConnection);

            cmd.CommandText = "SELECT * FROM SparePart WHERE Articul LIKE @Articul";

            SQLiteParameter param = new SQLiteParameter();
            param.ParameterName = "@Articul";
            param.Value = sparePartArticul;

            cmd.Parameters.Add(param);
            cmd.Prepare();

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                var sparePart = new SparePart
                {
                    Photo = dataReader["Photo"] as string,
                    Id = Convert.ToInt32(dataReader["Id"]),
                    Articul = dataReader["Articul"] as string,
                    Title = dataReader["Title"] as string,
                    Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                    Price = Convert.ToDouble(dataReader["Price"]),
                    Markup = Convert.ToDouble(dataReader["Markup"]),
                    Count = Convert.ToDouble(dataReader["Count"]),
                    Unit = dataReader["Unit"] as string
                };
                sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                sparePart.Providers = FindSparePartProvidersById(sparePart.Id, openConnection);

                spareParts.Add(sparePart);
            }            return spareParts;
        }        public static IList<int> FindSparePartsIdByArticul(string sparePartArticul)
        {
            IList<int> sparePartsId = new List<int>();

            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {

                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT Id FROM SparePart WHERE Articul LIKE @Articul";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Articul";
                param.Value = sparePartArticul;

                cmd.Parameters.Add(param);
                cmd.Prepare();

                var dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    sparePartsId.Add(Convert.ToInt32(dataReader["Id"]));
                }                connection.Close();
            }            return sparePartsId;
        }        public static IList<int> FindSparePartsIdByArticul(string sparePartArticul, SQLiteConnection openConnection)
        {
            IList<int> sparePartsId = new List<int>();
            var cmd = new SQLiteCommand(null, openConnection);

            cmd.CommandText = "SELECT Id FROM SparePart WHERE Articul LIKE @Articul";

            SQLiteParameter param = new SQLiteParameter();
            param.ParameterName = "@Articul";
            param.Value = sparePartArticul;

            cmd.Parameters.Add(param);
            cmd.Prepare();

            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
                sparePartsId.Add(Convert.ToInt32(dataReader["Id"]));

            return sparePartsId;
        }        public static IList<int> FindSparePartsIdByTitle(string sparePartTitle)
        {
            IList<int> sparePartsId = new List<int>();

            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {

                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT Id FROM SparePart WHERE Title LIKE @Title";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Title";
                param.Value = sparePartTitle;

                cmd.Parameters.Add(param);
                cmd.Prepare();

                var dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    sparePartsId.Add(Convert.ToInt32(dataReader["Id"]));
                }                connection.Close();
            }            return sparePartsId;
        }        public static IList<int> FindSparePartsIdByTitle(string sparePartTitle, SQLiteConnection openConnection)
        {
            IList<int> sparePartsId = new List<int>();

            var cmd = new SQLiteCommand(null, openConnection);

            cmd.CommandText = "SELECT Id FROM SparePart WHERE Title LIKE @Title";

            SQLiteParameter param = new SQLiteParameter();
            param.ParameterName = "@Title";
            param.Value = sparePartTitle;

            cmd.Parameters.Add(param);
            cmd.Prepare();

            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
                sparePartsId.Add(Convert.ToInt32(dataReader["Id"]));

            return sparePartsId;
        }
        public static int FindManufacturerIdByTitle(string manufacturerTitle)
        {
            int ManufacturerId = 0;

            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT Id FROM Manufacturer WHERE Title = @Title";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Title";
                param.Value = manufacturerTitle;

                cmd.Parameters.Add(param);
                cmd.Prepare();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                    ManufacturerId = Convert.ToInt32(dataReader["Id"]);
                connection.Close();
            }            return ManufacturerId;
        }        public static int FindManufacturerIdByTitle(string manufacturerTitle, SQLiteConnection openConnection)
        {
            int ManufacturerId = 0;

            var cmd = new SQLiteCommand(null, openConnection);

            cmd.CommandText = "SELECT Id FROM Manufacturer WHERE Title = @Title";

            SQLiteParameter param = new SQLiteParameter();
            param.ParameterName = "@Title";
            param.Value = manufacturerTitle;

            cmd.Parameters.Add(param);
            cmd.Prepare();

            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
                ManufacturerId = Convert.ToInt32(dataReader["Id"]);


            return ManufacturerId;
        }        public static IList<int> FindSparePartsIdByTitle(string sparePartTitle, IList<int> sparePartsIdList)
        {
            IList<int> sparePartsId = new List<int>();

            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT Id, Title FROM SparePart WHERE Id LIKE @Id AND Title LIKE @Title";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Title";
                param.Value = sparePartTitle;

                SQLiteParameter param2 = new SQLiteParameter();
                param2.ParameterName = "@Id";

                cmd.Parameters.Add(param);
                cmd.Parameters.Add(param2);

                cmd.Prepare();

                for (int i = 0; i < sparePartsIdList.Count; ++i)
                {
                    param2.Value = sparePartsIdList[i];
                    var dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                        if ((dataReader["Title"] as string) == sparePartTitle) sparePartsId.Add(sparePartsIdList[i]);
                    dataReader.Dispose();
                }                connection.Close();
            }            return sparePartsId;
        }        public static IList<int> FindSparePartsIdByTitle(string sparePartTitle, IList<int> sparePartsIdList, SQLiteConnection openConnection)
        {
            IList<int> sparePartsId = new List<int>();

            var cmd = new SQLiteCommand(null, openConnection);

            cmd.CommandText = "SELECT Title FROM SparePart WHERE Id LIKE @Id AND Title LIKE @Title";

            SQLiteParameter param = new SQLiteParameter();
            param.ParameterName = "@Title";
            param.Value = sparePartTitle;

            SQLiteParameter param2 = new SQLiteParameter();
            param2.ParameterName = "@Id";

            cmd.Parameters.Add(param);
            cmd.Parameters.Add(param2);

            cmd.Prepare();

            for (int i = 0; i < sparePartsIdList.Count; ++i)
            {
                param2.Value = sparePartsIdList[i];
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                    if ((dataReader["Title"] as string) == sparePartTitle) sparePartsId.Add(sparePartsIdList[i]);
                dataReader.Dispose();
            }
            return sparePartsId;
        }        public static IList<SparePart> FindSparePartsByTitle(string sparePartTitle)
        {
            IList<SparePart> spareParts = new List<SparePart>();
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT * FROM SparePart WHERE Title LIKE @Title";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Title";
                param.Value = sparePartTitle;

                cmd.Parameters.Add(param);
                cmd.Prepare();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    var sparePart = new SparePart
                    {
                        Photo = dataReader["Photo"] as string,
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        Price = Convert.ToDouble(dataReader["Price"]),
                        Markup = Convert.ToDouble(dataReader["Markup"]),
                        Count = Convert.ToDouble(dataReader["Count"]),
                        Unit = dataReader["Unit"] as string
                    };
                    sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                    sparePart.Providers = FindSparePartProvidersById(sparePart.Id, connection);

                    spareParts.Add(sparePart);
                }                connection.Close();
            }            return spareParts;
        }        public static IList<SparePart> FindSparePartsByArticulOrTitle(string TitleOrArticul)
        {
            IList<SparePart> spareParts = new List<SparePart>();
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT * FROM SparePart WHERE Title LIKE @TitleOrArticul OR Articul LIKE @TitleOrArticul";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@TitleOrArticul";
                param.Value = TitleOrArticul;

                cmd.Parameters.Add(param);
                cmd.Prepare();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    var sparePart = new SparePart
                    {
                        Photo = dataReader["Photo"] as string,
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        Price = Convert.ToDouble(dataReader["Price"]),
                        Markup = Convert.ToDouble(dataReader["Markup"]),
                        Count = Convert.ToDouble(dataReader["Count"]),
                        Unit = dataReader["Unit"] as string
                    };
                    sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                    sparePart.Providers = FindSparePartProvidersById(sparePart.Id, connection);

                    spareParts.Add(sparePart);
                }
                connection.Close();
            }
            return spareParts;
        }        public static IList<SparePart> FindSparePartsByArticuls(IList<string> articuls)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT * FROM SparePart WHERE Articul LIKE @Articul";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Articul";
                cmd.Parameters.Add(param);
                cmd.Prepare();


                for (int i = 0; i < articuls.Count; ++i)
                {
                    param.Value = articuls[i];
                    var dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        SparePart sparePart = new SparePart
                        {
                            Photo = dataReader["Photo"] as string,
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Articul = dataReader["Articul"] as string,
                            Title = dataReader["Title"] as string,
                            Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                            Price = Convert.ToDouble(dataReader["Price"]),
                            Markup = Convert.ToDouble(dataReader["Markup"]),
                            Count = Convert.ToDouble(dataReader["Count"]),
                            Unit = dataReader["Unit"] as string
                        };
                        sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                        sparePart.Providers = FindSparePartProvidersById(sparePart.Id, connection);

                        spareParts.Add(sparePart);
                    }                    dataReader.Dispose();  //Спорный момент с dataReader. Без Dispose выдаёт ошибку "dataReader already activate".
                }                connection.Close();
            }            return spareParts;
        }        public static SparePart FindSparePartById(int id)
        {
            SparePart sparePart = null;

            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT * FROM SparePart WHERE Id = @Id";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Id";
                param.Value = id;

                cmd.Parameters.Add(param);
                cmd.Prepare();

                var dataReader = cmd.ExecuteReader();

                dataReader.Read();

                sparePart = new SparePart
                {
                    Photo = dataReader["Photo"] as string,
                    Id = Convert.ToInt32(dataReader["Id"]),
                    Articul = dataReader["Articul"] as string,
                    Title = dataReader["Title"] as string,
                    Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                    Price = Convert.ToDouble(dataReader["Price"]),
                    Markup = Convert.ToDouble(dataReader["Markup"]),
                    Count = Convert.ToDouble(dataReader["Count"]),
                    Unit = dataReader["Unit"] as string
                };
                sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                sparePart.Providers = FindSparePartProvidersById(sparePart.Id, connection);

                connection.Close();
            }            return sparePart;
        }        public static SparePart FindSparePartById(int id, SQLiteConnection openConnection)
        {
            SparePart sparePart = null;
            var cmd = new SQLiteCommand(null, openConnection);

            cmd.CommandText = "SELECT * FROM SparePart WHERE Id = @Id";

            SQLiteParameter param = new SQLiteParameter();
            param.ParameterName = "@Id";
            param.Value = id;

            cmd.Parameters.Add(param);
            cmd.Prepare();

            var dataReader = cmd.ExecuteReader();

            dataReader.Read();

            sparePart = new SparePart
            {
                Photo = dataReader["Photo"] as string,
                Id = Convert.ToInt32(dataReader["Id"]),
                Articul = dataReader["Articul"] as string,
                Title = dataReader["Title"] as string,
                Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                Price = Convert.ToDouble(dataReader["Price"]),
                Markup = Convert.ToDouble(dataReader["Markup"]),
                Count = Convert.ToDouble(dataReader["Count"]),
                Unit = dataReader["Unit"] as string
            };
            sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
            sparePart.Providers = FindSparePartProvidersById(sparePart.Id, openConnection);

            return sparePart;
        }        public static IList<SparePart> FindSparePartById(IList<int> sparePartsId)
        {
            IList<SparePart> spareParts = new List<SparePart>();
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT * FROM SparePart WHERE Id = @Id";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Id";

                cmd.Parameters.Add(param);
                cmd.Prepare();

                for (int i = 0; i < sparePartsId.Count; ++i)
                {
                    param.Value = sparePartsId[i];
                    var dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        SparePart sparePart = new SparePart
                        {
                            Photo = dataReader["Photo"] as string,
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Articul = dataReader["Articul"] as string,
                            Title = dataReader["Title"] as string,
                            Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                            Price = Convert.ToDouble(dataReader["Price"]),
                            Markup = Convert.ToDouble(dataReader["Markup"]),
                            Count = Convert.ToDouble(dataReader["Count"]),
                            Unit = dataReader["Unit"] as string
                        };
                        sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                        sparePart.Providers = FindSparePartProvidersById(sparePart.Id, connection);

                        spareParts.Add(sparePart);
                    }                    dataReader.Dispose();  //Спорный момент с dataReader. Без Dispose выдаёт ошибку "dataReader already activate".
                }            }            return spareParts;
        }        public static IList<SparePart> FindSparePartById(IList<int> sparePartsId, SQLiteConnection openConnection)
        {
            IList<SparePart> spareParts = new List<SparePart>();
            var cmd = new SQLiteCommand("SELECT * FROM SparePart WHERE Id = @Id", openConnection);

            SQLiteParameter param = new SQLiteParameter();
            param.ParameterName = "@Id";

            cmd.Parameters.Add(param);
            cmd.Prepare();

            for (int i = 0; i < sparePartsId.Count; ++i)
            {
                param.Value = sparePartsId[i];
                var dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart
                    {
                        Photo = dataReader["Photo"] as string,
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        Price = Convert.ToDouble(dataReader["Price"]),
                        Markup = Convert.ToDouble(dataReader["Markup"]),
                        Count = Convert.ToDouble(dataReader["Count"]),
                        Unit = dataReader["Unit"] as string
                    };
                    sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                    sparePart.Providers = FindSparePartProvidersById(sparePart.Id, openConnection);

                    spareParts.Add(sparePart);
                }                dataReader.Dispose();  //Спорный момент с dataReader. Без Dispose выдаёт ошибку "dataReader already activate".
            }            return spareParts;
        }        


        //Удаление из БД.
        public static void DeleteSparePartByArticul(string sparePartArticul)
        {
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "DELETE FROM SparePart WHERE Articul LIKE @Articul";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Articul";
                param.Value = sparePartArticul;

                cmd.Parameters.Add(param);
                cmd.Prepare();

                cmd.ExecuteReader();

                connection.Close();
            }        }        public static void DeleteSparePartByTitle(string sparePartTitle)
        {
            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "DELETE FROM SparePart WHERE Title LIKE @Title";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Title";
                param.Value = sparePartTitle;

                cmd.Parameters.Add(param);
                cmd.Prepare();

                cmd.ExecuteReader();

                connection.Close();
            }        }
        //Поиск совпадений по БД.
        public static IList<SparePart> SearchByArticul(string searchArticul)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT * FROM SparePart WHERE Articul LIKE @Articul";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Articul";
                param.Value = searchArticul + "%";

                cmd.Parameters.Add(param);
                cmd.Prepare();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart
                    {
                        Photo = dataReader["Photo"] as string,
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        Price = Convert.ToDouble(dataReader["Price"]),
                        Markup = Convert.ToDouble(dataReader["Markup"]),
                        Count = Convert.ToDouble(dataReader["Count"]),
                        Unit = dataReader["Unit"] as string
                    };
                    sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                    sparePart.Providers = FindSparePartProvidersById(sparePart.Id, connection);

                    spareParts.Add(sparePart);
                }                connection.Close();
            }            return spareParts;
        }        public static IList<SparePart> SearchByTitle(string searchTitle)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT * FROM SparePart WHERE Title LIKE @Title";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@Title";
                param.Value = searchTitle + "%";

                cmd.Parameters.Add(param);
                cmd.Prepare();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart
                    {
                        Photo = dataReader["Photo"] as string,
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        Price = Convert.ToDouble(dataReader["Price"]),
                        Markup = Convert.ToDouble(dataReader["Markup"]),
                        Count = Convert.ToDouble(dataReader["Count"]),
                        Unit = dataReader["Unit"] as string
                    };
                    sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                    sparePart.Providers = FindSparePartProvidersById(sparePart.Id, connection);

                    spareParts.Add(sparePart);
                }                connection.Close();
            }            return spareParts;
        }        public static IList<SparePart> SearchByTitleOrArticul(string searchTitleOrArticul)
        {
            IList<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = GetDatabaseConnection("SparePartConfig") as SQLiteConnection)
            {
                connection.Open();
                var cmd = new SQLiteCommand(null, connection);

                cmd.CommandText = "SELECT * FROM SparePart WHERE Title LIKE @TitleOrArticul OR Articul LIKE @TitleOrArticul";

                SQLiteParameter param = new SQLiteParameter();
                param.ParameterName = "@TitleOrArticul";
                param.Value = searchTitleOrArticul + "%";

                cmd.Parameters.Add(param);
                cmd.Prepare();

                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    SparePart sparePart = new SparePart
                    {
                        Photo = dataReader["Photo"] as string,
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Articul = dataReader["Articul"] as string,
                        Title = dataReader["Title"] as string,
                        Manufacturer = PartsDAL.FindManufacturerTitleById(Convert.ToInt32(dataReader["ManufacturerId"])),
                        Price = Convert.ToDouble(dataReader["Price"]),
                        Markup = Convert.ToDouble(dataReader["Markup"]),
                        Count = Convert.ToDouble(dataReader["Count"]),
                        Unit = dataReader["Unit"] as string
                    };
                    sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);
                    sparePart.Providers = FindSparePartProvidersById(sparePart.Id, connection);

                    spareParts.Add(sparePart);
                }                connection.Close();
            }            return spareParts;
        }
        //Чтение из Excel файла.
        private static IList<SparePart> LoadExcelFile(string excelPath)
        {
            IList<SparePart> spareParts = new List<SparePart>();
            Excel.Application excelApp = new Excel.Application();
            int row = 2; //начинаем с двойки, т.к. пропускаем названия. 
            try
            {
                excelApp.Workbooks.Open(excelPath);

                Excel.Worksheet currentSheet = (Excel.Worksheet)excelApp.Workbooks[1].Worksheets[1];

                while (currentSheet.get_Range("A" + row).Value2 != null)
                {
                    for (char column = 'A'; column < 'J'; column++)
                    {
                        Excel.Range cell = currentSheet.get_Range(Convert.ToString(column) + row);
                        if (cell.Value2 == null) break;

                        //tempList.Add(cell != null ? cell.Value2.ToString() : "");

                        SparePart sparePart = new SparePart
                        {
                            Photo = cell.Value2.ToString(),
                            Articul = (currentSheet.get_Range(Convert.ToString(++column) + row)).Value2 as string,
                            Title = (currentSheet.get_Range(Convert.ToString(++column) + row)).Value2 as string,
                            Manufacturer = (currentSheet.get_Range(Convert.ToString(++column) + row)).Value2 as string,
                            Unit = (currentSheet.get_Range(Convert.ToString(++column) + row)).Value2 as string,
                            Count = Convert.ToDouble((currentSheet.get_Range(Convert.ToString(++column) + row)).Value2.ToString()),
                            Price = Convert.ToDouble((currentSheet.get_Range(Convert.ToString(++column) + row)).Value2.ToString()),
                            Markup = Convert.ToDouble((currentSheet.get_Range(Convert.ToString(++column) + row)).Value2.ToString())
                        };
                        sparePart.SellingPrice = CalculationOfSellingPrice(sparePart.Price, sparePart.Markup);

                        spareParts.Add(sparePart);
                    }                    row++;
                }            }            catch (Exception) { }
            // В случае если имя файла указано неправильно, ресурс должен быть освобожден! (можно исп-ть using)
            finally
            {
                excelApp.Quit();
            }

            return spareParts;
        }        public static void AddSparePartsFromExcelFile(string excelPath)
        {
            //Сравнить разницу в быстродействии двух вариантов!!!
            IList<SparePart> spareParts = PartsDAL.LoadExcelFile(excelPath);
            //1)Медленный вариант. Открытие connection для каждой итерации.
            //foreach (var part in spareParts)
            //    PartsDAL.AddSparePart(part);

            //2)Более быстрый вариант. connection открывается один раз для всей коллекции.
            //PartsDAL.AddSparePart(spareParts);
            PartsDAL.AddSparePartsWithUpdateCount(spareParts);
        }
        //Сохранение в Excel файл.
        public static void SaveToExcelFile(IList<SparePart> spareParts)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook ExcelWorkBook;
            Excel.Worksheet ExcelWorkSheet;
            //Книга.
            ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
            //Таблица.
            ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

            for (int i = 0; i < spareParts.Count; ++i)
            {
                int j = 0;
                ExcelApp.Cells[i + 1, ++j] = spareParts[i].Photo;
                ExcelApp.Cells[i + 1, ++j] = spareParts[i].Articul;
                ExcelApp.Cells[i + 1, ++j] = spareParts[i].Title;
                ExcelApp.Cells[i + 1, ++j] = spareParts[i].Manufacturer;
                ExcelApp.Cells[i + 1, ++j] = spareParts[i].Unit;
                ExcelApp.Cells[i + 1, ++j] = spareParts[i].Count;
                ExcelApp.Cells[i + 1, ++j] = spareParts[i].Price;
                ExcelApp.Cells[i + 1, ++j] = spareParts[i].Markup;
            }            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = true;
            ExcelApp.UserControl = true;
        }
        //Ф-ция расчета Цены продажи
        public static double CalculationOfSellingPrice(double price, double markup)
        {
            return price + (price * markup / 100);
        }
        //Коннект к базе данных.
        static private System.Data.Common.DbConnection GetDatabaseConnection(string name)
        {
            var settings = System.Configuration.ConfigurationManager.ConnectionStrings[name];
            var factory = System.Data.Common.DbProviderFactories.GetFactory(settings.ProviderName);

            var conn = factory.CreateConnection();
            conn.ConnectionString = settings.ConnectionString;

            return conn;
        }

    }


    class SparePart
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Articul { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string Unit { get; set; }
        public double Count { get; set; }
        public double Price { get; set; }
        public int Markup { get; set; }
        public double SellingPrice { get; set; }

        public SparePart() { }
        public SparePart(string photo, string articul, string title, string description, string category,
                         string manufacturer, string unit, double count, double price, int markup)
        {
            Photo = photo == null ? String.Empty : photo;
            Articul = articul;
            Title = title;
            Category = category;
            Manufacturer = manufacturer;
            Count = count;
            Unit = unit;
            Price = price;
            Markup = markup;
            SellingPrice = price * markup;     
        }
    }

}