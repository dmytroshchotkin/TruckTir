using PartsApp.Models;
using System;
using System.Data.SQLite;

namespace Infrastructure.Storage
{
    /// <summary>
    /// Методы извлечения и обновления ContactInfo для классов IContragent и Customer
    /// </summary>
    internal static class ContactInfoDatabaseHandler
    {
        #region Модификация таблицы ContactInfo.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Метод добавляет новую запись в таблицу ContactInfo и возвращает Id вставленной записи.
        /// </summary>
        /// <param name="contactInfo">объект типа ContactInfo данные которого будут добавлены в базу</param>
        /// <returns></returns>
        internal static int AddContactInfo(ContactInfo contactInfo, SQLiteCommand cmd)
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
        }

        /// <summary>
        /// Обновляем запись в таблице ContactInfo.
        /// </summary>
        /// <param name="contactInfo">Инф-ция для обновления.</param>
        /// <param name="cmd"></param>
        internal static void UpdateContactInfo(ContactInfo contactInfo, SQLiteCommand cmd)
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
        }

        /// <summary>
        /// Удаляет запись с заданным Id из таблицы ContactInfo.
        /// </summary>
        /// <param name="contactInfoId">Id удаляемой записи</param>
        /// <param name="cmd"></param>
        internal static void DeleteContactInfo(int contactInfoId, SQLiteCommand cmd)
        {
            cmd.CommandText = "DELETE FROM ContactInfo "
                            + "WHERE ContactInfoId = @ContactInfoId;";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ContactInfoId", contactInfoId);

            cmd.ExecuteNonQuery();
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблице ContactInfo
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает объект типа ContactInfo заполненный по заданному Id.
        /// </summary>
        /// <param name="contactInfoId">Id по которому находится информация.</param>
        /// <returns></returns>
        internal static ContactInfo FindContactInfo(int contactInfoId)
        {
            ContactInfo contactInfo = new ContactInfo();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT * FROM ContactInfo WHERE ContactInfoId = @ContactInfoId;";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@ContactInfoId", contactInfoId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        contactInfo = CreateContactInfo(dataReader);
                    }
                }

                connection.Close();
            }

            return contactInfo;
        }

        /// <summary>
        /// Возвращает объект типа ContactInfo, заполненный инф-цией из переданного SQLiteDataReader.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        internal static ContactInfo CreateContactInfo(SQLiteDataReader dataReader)
        {
            return new ContactInfo
            (
                contactInfoId: Convert.ToInt32(dataReader["ContactInfoId"]),
                country: dataReader["Country"] as string,
                region: dataReader["Region"] as string,
                city: dataReader["City"] as string,
                street: dataReader["Street"] as string,
                house: dataReader["House"] as string,
                room: dataReader["Room"] as string,
                phone: dataReader["Phone"] as string,
                extPhone: dataReader["ExtPhone"] as string,
                email: dataReader["Email"] as string,
                website: dataReader["Website"] as string
            );
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
