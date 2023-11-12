using Infrastructure.Storage.PropertiesHandlers;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace Infrastructure.Storage
{
    public class SparePartRepository
    {
        #region Модификация таблицы SpareParts.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void AddSparePart(SparePart sparePart)
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
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new System.Data.SQLite.SQLiteException(ex.Message);
                        }
                    }
                }

                connection.Close();
            }
        }

        /// <summary>
        /// Метод модификации записи с заданным Id.
        /// </summary>
        /// <param name="avail">Товар инф-ция о котором модифицируется.</param>
        public static void UpdateSparePart(SparePart sparePart)
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
                            //Находим существующий manufacturerId в базе или добавляем новый объект если отсутствует.
                            int? manufId = (sparePart.Manufacturer != null) ? FindManufacturerId(sparePart.Manufacturer) : (int?)null;
                            cmd.Parameters.AddWithValue("@ManufacturerId", (manufId == 0) ? AddManufacturer(sparePart.Manufacturer, cmd) : manufId);

                            const string query = "UPDATE SpareParts SET Photo = @Photo, Articul = @Articul, Title = @Title, "
                                               + "Description = @Description, ManufacturerId = @ManufacturerId, MeasureUnit = @MeasureUnit "
                                               + "WHERE SparePartId = @SparePartId;";

                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@SparePartId", sparePart.SparePartId);
                            cmd.Parameters.AddWithValue("@Photo", sparePart.Photo);
                            cmd.Parameters.AddWithValue("@Articul", sparePart.Articul);
                            cmd.Parameters.AddWithValue("@Title", sparePart.Title);
                            cmd.Parameters.AddWithValue("@Description", sparePart.Description);
                            cmd.Parameters.AddWithValue("@MeasureUnit", sparePart.MeasureUnit);

                            cmd.ExecuteNonQuery();

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new System.Data.SQLite.SQLiteException(ex.Message);
                        }
                    }
                }

                connection.Close();
            }
        }

        public static SparePart CreateSparePart(SQLiteDataReader dataReader)
        {
            var result = new SparePart
            (
                sparePartId: Convert.ToInt32(dataReader["SparePartId"]),
                photo: dataReader["Photo"] as string,
                articul: dataReader["Articul"] as string,
                title: dataReader["Title"] as string,
                description: dataReader["Description"] as string,
                manufacturer: dataReader["ManufacturerName"] as string,
                measureUnit: dataReader["MeasureUnit"] as string
            );

            result.TrySetAvailabilities(new Lazy<List<Availability>>(() => AvailabilityHandler.FindAvailability(result)));
            return result;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблицам SpareParts. 
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        /// <summary>
        /// Возвращает объект типа SparePart, найденный по заданному Id, или null если такого объекта не найдено.
        /// </summary>
        /// <param name="sparePartId">Ид товара</param>
        /// <returns></returns>
        public static SparePart FindSparePart(int sparePartId)
        {
            SparePart sparePart = null;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
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
                    }
                }

                connection.Close();
            }

            return sparePart;
        }

        /// <summary>
        /// Возвращает список запчастей с заданным артикулом. 
        /// </summary>
        /// <param name="articul">Артикул.</param>
        /// <returns></returns>
        public static List<SparePart> FindSparePartsByArticul(string articul)
        {
            List<SparePart> sparePartsList = new List<SparePart>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
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
                    }
                }

                connection.Close();
            }

            return sparePartsList;
        }

        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #endregion

        #region Поиск совпадений SparePart по БД.
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
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
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
                    }
                }

                connection.Close();
            }
            return spareParts;
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
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
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
                    }
                }

                connection.Close();
            }

            return spareParts;
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
            List<SparePart> spareParts = new List<SparePart>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
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
                    }
                }

                connection.Close();
            }

            return spareParts;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы Manufacturers и поиск по ней
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Добавляет нового производителя в БД и возвращает его Id.
        /// </summary>
        /// <param name="manufacturerName">Имя добавляемого производителя</param>
        /// <returns></returns>
        private static int AddManufacturer(string manufacturerName, SQLiteCommand cmd)
        {
            string query = String.Format("INSERT INTO Manufacturers(ManufacturerName) VALUES(@ManufacturerName); " +
                                         "SELECT ManufacturerId FROM Manufacturers WHERE rowid = last_insert_rowid();");
            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerName);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Возвращает список Id-ков производителей с заданным именем.
        /// </summary>
        /// <param name="manufacturerName">Имя искомых производителей.</param>
        /// <returns></returns>
        private static int FindManufacturerId(string manufacturerName)
        {
            int manufacturerId = 0;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT ManufacturerId FROM Manufacturers WHERE ManufacturerName = @ManufacturerName;";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerName);

                    object executeScalar = cmd.ExecuteScalar();
                    manufacturerId = (executeScalar != null) ? Convert.ToInt32(executeScalar) : 0;
                }

                connection.Close();
            }

            return manufacturerId;
        }

        public static string[] FindAllManufacturersName()
        {
            IList<string> manufacturers = new List<string>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT ManufacturerName FROM Manufacturers;", connection);
                var dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    manufacturers.Add(dataReader["ManufacturerName"] as string);
                }
                connection.Close();
            }

            //создаём массив string.
            string[] manuf = new string[manufacturers.Count];
            for (int i = 0; i < manuf.Length; ++i)
                manuf[i] = manufacturers[i];

            return manuf;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
