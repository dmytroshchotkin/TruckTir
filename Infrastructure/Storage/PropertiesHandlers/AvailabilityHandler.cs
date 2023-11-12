using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Infrastructure.Storage.PropertiesHandlers
{
    /// <summary>
    /// Методы извлечения и обновления Availability для классов IOperation
    /// </summary>
    internal static class AvailabilityHandler
    {
        #region Модификация таблицы Avaliability.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Добавляет запись в таблицу Avaliability.
        /// </summary>
        /// <param name="avail">Запись добавляемая в таблицу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        internal static void AddSparePartAvaliability(Availability avail, SQLiteCommand cmd)
        {
            var query = "INSERT INTO Avaliability VALUES (@SparePartId, @OperationId, @Price, @Markup, @StorageAdress, @Count);";

            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@SparePartId", avail.OperationDetails.SparePart.SparePartId);
            cmd.Parameters.AddWithValue("@OperationId", avail.OperationDetails.Operation.OperationId);
            cmd.Parameters.AddWithValue("@Price", avail.OperationDetails.Price);
            cmd.Parameters.AddWithValue("@Markup", avail.Markup);
            cmd.Parameters.AddWithValue("@StorageAdress", avail.StorageAddress);
            cmd.Parameters.AddWithValue("@Count", avail.OperationDetails.Count);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Обновляет количество в заданной записи таблицы Avaliability.
        /// </summary>
        /// <param name="sparePartId">Ид товара искомой записи</param>
        /// <param name="saleId">Ид прихода искомой записи</param>        
        /// <param name="newCount">Новое кол-во, которое будет записано в базу.</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void UpdateSparePartСountAvaliability(int sparePartId, int purchaseId, double newCount, SQLiteCommand cmd)
        {
            string query = "UPDATE Avaliability SET Count = @Count WHERE SparePartId = @SparePartId AND OperationId = @OperationId;";
            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@OperationId", purchaseId);
            cmd.Parameters.AddWithValue("@Count", newCount);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Метод обновления значения Markup у записей с заданным SparePartId и PurchaseId.
        /// </summary>
        /// <param name="sparePartId">Id запчасти с изменяемой наценкой</param>
        /// <param name="saleId">Id прихода с изменяемой наценкой</param>
        /// <param name="markup">Значение наценки на которое стоит поменять текущее значение.</param>
        /// <param name="openConnection">Открытый connection. В методе не закрывается!</param>
        internal static void UpdateSparePartMarkup(int sparePartId, int purchaseId, double markup, SQLiteCommand cmd)
        {
            const string query = "UPDATE Avaliability SET Markup = @Markup WHERE SparePartId = @SparePartId AND OperationId = @OperationId;";
            cmd.CommandText = query;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@Markup", markup);
            cmd.Parameters.AddWithValue("@OperationId", purchaseId);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Изменяет наценку у записей с заданными SparePartId и PurchaseId на заданную Markup
        /// </summary>
        /// <param name="changeMarkupDict">Словарь типа (sparePartId, IDictionary(saleId, markup))</param>
        internal static void UpdateSparePartMarkup(List<Availability> availList)
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
                            foreach (Availability avail in availList)
                            {
                                int sparePartId = avail.OperationDetails.SparePart.SparePartId;
                                int purchaseId = avail.OperationDetails.Operation.OperationId;
                                float markup = avail.Markup;

                                UpdateSparePartMarkup(sparePartId, purchaseId, markup, cmd);
                            }

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
        /// Удаляет заданную запись из таблицы Avaliability.
        /// </summary>
        /// <param name="sparePartId">Ид товара искомой записи</param>
        /// <param name="saleId">Ид прихода искомой записи</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        private static void DeleteSparePartAvaliability(int sparePartId, int purchaseId, SQLiteCommand cmd)
        {
            const string query = "DELETE FROM Avaliability WHERE SparePartId = @SparePartId AND OperationId = @OperationId;";
            cmd.CommandText = query;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SparePartId", sparePartId);
            cmd.Parameters.AddWithValue("@OperationId", purchaseId);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Уменьшает кол-во или удаляет запись из таблицы Avaliability.
        /// </summary>
        /// <param name="avail">уменьшаемый или удаляемый товар</param>
        /// <param name="cmd">Команда, без CommandText и Параметров.</param>
        internal static void SaleSparePartAvaliability(OperationDetails operDet, SQLiteCommand cmd)
        {
            //Узнаем количество данного товара в наличии по данному приходу.
            float availCount = FindAvailability(operDet.SparePart).First(av => av.OperationDetails.Operation.OperationId == operDet.Operation.OperationId).OperationDetails.Count;

            //Если кол-во продаваемого товара с данного прихода равно всему кол-во товара данной записи, удаляем из таблицы эту запись, иначе обновляем кол-во товара в базе.
            if (availCount == operDet.Count)
                DeleteSparePartAvaliability(operDet.SparePart.SparePartId, operDet.Operation.OperationId, cmd);
            else
                UpdateSparePartСountAvaliability(operDet.SparePart.SparePartId, operDet.Operation.OperationId, availCount - operDet.Count, cmd);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблице Avaliablility.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       

        internal static List<Availability> FindAvailability(SparePart sparePart)
        {
            List<Availability> availabilityList = new List<Availability>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
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
                    }
                }

                connection.Close();
            }

            return availabilityList;
        }

        private static Availability CreateAvailability(SQLiteDataReader dataReader, SparePart sparePart)
        {
            return new Availability
            (
                operationDetails: CreateOperationDetails(dataReader, sparePart),
                storageAddress: dataReader["StorageAdress"] as string,
                markup: Convert.ToSingle(dataReader["Markup"])
            );
        }

        private static OperationDetails CreateOperationDetails(SQLiteDataReader dataReader, SparePart sparePart)
        {
            return new OperationDetails
            (
                sparePart: sparePart,
                operation: PurchaseRepository.FindPurchase(Convert.ToInt32(dataReader["OperationId"])),
                count: Convert.ToSingle(dataReader["Count"]),
                price: Convert.ToSingle(dataReader["Price"])
            );
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
