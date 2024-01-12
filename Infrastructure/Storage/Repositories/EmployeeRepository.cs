using Infrastructure.Storage;
using PartsApp;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Infrastructure.Storage.Repositories
{
    public class EmployeeRepository
    {
        #region Модификация таблицы Employees.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void AddEmployee(Employee employee)
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
                            //Вставляем запись в таблицу ContactInfo, если требуется.
                            if (employee.ContactInfo != null)
                            {
                                employee.ContactInfo.ContactInfoId = ContactInfoDatabaseHandler.AddContactInfo(employee.ContactInfo, cmd);
                            }
                            //Вставляем записm в табл. Employees.
                            AddEmployee(employee, cmd);

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

        public static void UpdateEmployee(Employee employee, bool updateWithoutPassword = false)
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
                            if (employee.ContactInfo != null)
                            {
                                ContactInfo contactInfo = FindContactInfo(employee);
                                if (contactInfo != null)
                                {
                                    employee.ContactInfo.ContactInfoId = contactInfo.ContactInfoId;
                                    ContactInfoDatabaseHandler.UpdateContactInfo(employee.ContactInfo, cmd);
                                }
                                else
                                {
                                    employee.ContactInfo.ContactInfoId = ContactInfoDatabaseHandler.AddContactInfo(employee.ContactInfo, cmd);
                                }
                            }

                            if (updateWithoutPassword)
                            {
                                UpdateEmployeeWithoutPassword(employee, cmd);                                
                            }
                            else
                            {
                                UpdateEmployee(employee, cmd);
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
        }

        /// <summary>
        /// Возвращает объект типа Employee созданный из данныз переданного SQLiteDataReader.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private static Employee CreateEmployee(SQLiteDataReader dataReader)
        {
            var result = new Employee
            (
                employeeId: Convert.ToInt32(dataReader["EmployeeId"]),
                lastName: dataReader["LastName"] as string,
                firstName: dataReader["FirstName"] as string,
                middleName: dataReader["MiddleName"] as string,
                birthDate: (dataReader["BirthDate"] != DBNull.Value) ? DateTimeParser.GetDateTime(dataReader["BirthDate"] as string) : (DateTime?)null,
                hireDate: (dataReader["HireDate"] != DBNull.Value) ? DateTimeParser.GetDateTime(dataReader["HD"] as string) : (DateTime?)null,
                dismissalDate: (dataReader["DismissalDate"] != DBNull.Value) ? DateTimeParser.GetDateTime(dataReader["DD"] as string) : (DateTime?)null,
                photo: dataReader["Photo"] as string,
                note: dataReader["Note"] as string,
                passportNum: dataReader["PassportNum"] as string,
                title: dataReader["Title"] as string,
                accessLayer: dataReader["AccessLayer"] as string,
                login: dataReader["Login"] as string,
                password: dataReader["Password"] as string
            );

            var contactInfo = FindContactInfo(result);
            if (result != null && contactInfo != null)
            {
                result.TrySetContactInfo(contactInfo);
            }

            return result;
        }

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
            cmd.Parameters.AddWithValue("@Password", employee.Password);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Метод обновляющий значения заданного сотрудника.
        /// </summary>
        /// <param name="employee">Сотрудник, значения которого необходимо обновить в базе.</param>
        private static void UpdateEmployee(Employee employee, SQLiteCommand cmd)
        {
            cmd.CommandText = "UPDATE Employees SET LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName, "
                               + "BirthDate = @BirthDate, HireDate = strftime('%s', @HireDate), ContactInfoId = @ContactInfoId, "
                               + "Photo = @Photo, Note = @Note, PassportNum = @PassportNum, Title = @Title, AccessLayer = @AccessLayer, "
                               + "Login = @Login, Password = @Password, DismissalDate = strftime('%s', @DismissalDate) "
                               + "WHERE EmployeeId = @EmployeeId;";

            cmd.Parameters.Clear();
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
            cmd.Parameters.AddWithValue("@Password", employee.Password);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Метод обновляющий значения заданного сотрудника, без обновления его пароля.
        /// </summary>
        /// <param name="employee">Сотрудник, значения которого необходимо обновить в базе.</param>
        private static void UpdateEmployeeWithoutPassword(Employee employee, SQLiteCommand cmd)
        {
            cmd.CommandText = "UPDATE Employees SET LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName, "
                               + "BirthDate = @BirthDate, HireDate = strftime('%s', @HireDate), ContactInfoId = @ContactInfoId, "
                               + "Photo = @Photo, Note = @Note, PassportNum = @PassportNum, Title = @Title, AccessLayer = @AccessLayer, "
                               + "Login = @Login, DismissalDate = strftime('%s', @DismissalDate) "
                               + "WHERE EmployeeId = @EmployeeId;";

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
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT date(HireDate, \"Unixepoch\") AS 'HD', date(DismissalDate, \"Unixepoch\") AS 'DD', * "
                                   + "FROM Employees;";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        employeesList.Add(CreateEmployee(dataReader));
                    }
                }

                connection.Close();
            }

            return employeesList;
        }

        /// <summary>
        /// Возвращает объект типа Employee, найденный по заданному Id.
        /// </summary>
        /// <param name="employeeId">Ид сотрудника, которого надо найти.</param>
        /// <returns></returns>
        public static Employee FindEmployees(int employeeId)
        {
            Employee employee = null;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();
                const string query = "SELECT date(HireDate, \"Unixepoch\") AS 'HD', date(DismissalDate, \"Unixepoch\") AS 'DD', * "
                                   + "FROM Employees WHERE EmployeeId = @EmployeeId;";

                var cmd = new SQLiteCommand(query, connection);

                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        employee = CreateEmployee(dataReader);
                    }
                }

                connection.Close();
            }

            return employee;
        }

        public static List<Employee> FindEmployees(string lastName, string firstName = null)
        {
            List<Employee> employees = new List<Employee>();

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
            {
                connection.Open();

                const string query = "SELECT date(HireDate, \"Unixepoch\") AS 'HD', date(DismissalDate, \"Unixepoch\") AS 'DD', * "
                                   + "FROM Employees WHERE LastName LIKE @LastName AND FirstName ;";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@LastName", lastName);

                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        employees.Add(CreateEmployee(dataReader));
                    }
                }

                connection.Close();
            }

            return employees;
        }
        #endregion

        #region Поиск объекта Employee в таблице ContactInfo
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает объект типа ContactInfo, найденный по заданному Id сотрудника, или null если ничего не найдено.
        /// </summary>
        /// <param name="employeeId">Id сотрудника.</param>
        /// <returns></returns>
        private static ContactInfo FindContactInfo(Employee employee)
        {
            ContactInfo contactInfo = null;

            using (SQLiteConnection connection = DbConnectionHelper.GetDatabaseConnection(DbConnectionHelper.SparePartConfig) as SQLiteConnection)
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
                        contactInfo = ContactInfoDatabaseHandler.CreateContactInfo(dataReader);
                    }
                }

                connection.Close();
            }

            return contactInfo;
        }
        #endregion
    }
}
