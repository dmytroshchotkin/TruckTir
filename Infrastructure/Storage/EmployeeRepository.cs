using PartsApp;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EmployeeRepository : BaseRepository<Employee>
    {
        private readonly List<Employee> _employees = new List<Employee>();
        protected override List<Employee> Items => _employees;

        public EmployeeRepository()
        {
        //    _employees = FindEmployees();
        }

        private void AddEmployeeToRepository(Employee employee)
        {
            if (_employees.Any(e => e.EmployeeId == employee.EmployeeId))
            {
                _employees.Remove(_employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId));
                _employees.Add(employee);
            }
        }

        #region Модификация таблицы Employees.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddEmployee(Employee employee)
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
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }

                connection.Close();
                AddEmployeeToRepository(employee);
            }
        }

        /// <summary>
        /// Добавляет объект типа Employee в таблицу Employees.
        /// </summary>
        /// <param name="employee">объект типа Employee добавляемый в БД.</param>
        public void AddEmployee(Employee employee, SQLiteCommand cmd)
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
            AddEmployeeToRepository(employee);
        }

        /// <summary>
        /// Метод обновляющий значения заданного сотрудника.
        /// </summary>
        /// <param name="employee">Сотрудник, значения которого необходимо обновить в базе.</param>
        public void UpdateEmployee(Employee employee)
        {
            if (_employees.Contains(employee))
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

                    connection.Close();
                    AddEmployeeToRepository(employee);
                }
            }
        }

        /// <summary>
        /// Метод обновляющий значения заданного сотрудника, без обновления его пароля.
        /// </summary>
        /// <param name="employee">Сотрудник, значения которого необходимо обновить в базе.</param>
        public void UpdateEmployeeWithoutPassword(Employee employee)
        {
            if (_employees.Contains(employee))
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
                    AddEmployeeToRepository(employee);
                }
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Модификация таблицы ContactInfo.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Метод добавляет новую запись в таблицу ContactInfo и возвращает Id вставленной записи.
        /// </summary>
        /// <param name="contactInfo">объект типа ContactInfo данные которого будут добавлены в базу</param>
        /// <returns></returns>
        private static int AddContactInfo(ContactInfo contactInfo, SQLiteCommand cmd)
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
        }

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
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Поиск по таблице Employees.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает список из объектов типа Employee, состоящий из всех сотрудников.
        /// </summary>
        /// <returns></returns>
        public List<Employee> FindEmployees()
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
        public Employee FindEmployees(int employeeId)
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
                }

                connection.Close();
            }

            return employee;
        }

        public IList<Employee> FindEmployees(string lastName, string firstName = null)
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
                }

                connection.Close();
            }

            return employees;
        }
        #endregion

        #region Поиск по таблице ContactInfo
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Возвращает объект типа ContactInfo заполненный по заданному Id.
        /// </summary>
        /// <param name="contactInfoId">Id по которому находится информация.</param>
        /// <returns></returns>
        private static ContactInfo FindContactInfo(int contactInfoId)
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
                }

                connection.Close();
            }

            return contactInfo;
        }

        /// <summary>
        /// Возвращает объект типа ContactInfo, найденный по заданному Id сотрудника, или null если ничего не найдено.
        /// </summary>
        /// <param name="employeeId">Id сотрудника.</param>
        /// <returns></returns>
        private static ContactInfo FindContactInfo(Employee employee)
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
                    }
                }

                connection.Close();
            }

            return contactInfo;
        }        
        #endregion

        #region CreateEmployee from DB
        /// <summary>
        /// Возвращает объект типа Employee созданный из данныз переданного SQLiteDataReader.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public Employee CreateEmployee(SQLiteDataReader dataReader)
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
                AddEmployeeToRepository(result);
            }

            return result;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region CreateContactInfo from DB
        /// <summary>
        /// Возвращает объект типа ContactInfo, заполненный инф-цией из переданного SQLiteDataReader.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private static ContactInfo CreateContactInfo(SQLiteDataReader dataReader)
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
