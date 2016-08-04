using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp.Models
{
    public class Employee
    {
        public enum AccessLayers
        {
            [System.ComponentModel.Description("Администратор")]
            Admin,
            [System.ComponentModel.Description("Обычный")]
            User
        }//AccessLayers

        public int       EmployeeId    { get; set; }
        public string    LastName      { get; set; }
        public string    FirstName     { get; set; }
        public string    MiddleName    { get; set; }
        public DateTime? BirthDate     { get; set; }
        public DateTime? HireDate      { get; set; }
        public DateTime? DismissalDate { get; set; }        
        public string    Photo         { get; set; }
        public string    Note          { get; set; }
        public string    PassportNum   { get; set; }
        public string    Title         { get; set; }
        public string    AccessLayer   { get; set; }
        public string    Login         { get; set; }
        public string    Password      { get; set; }

        private Lazy<ContactInfo> _contactInfo;
        public ContactInfo ContactInfo { get { return _contactInfo.Value; } }

        /// <summary>
        /// Конструктор для добавления нового объекта в БД.
        /// </summary>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="birthDate"></param>
        /// <param name="hireDate"></param>
        /// <param name="dismissalDate"></param>
        /// <param name="photo"></param>
        /// <param name="note"></param>
        /// <param name="passportNum"></param>
        /// <param name="title"></param>
        /// <param name="accessLayer"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="contactInfo"></param>
        public Employee(string lastName, string firstName, string middleName, DateTime? birthDate,
                        DateTime? hireDate, DateTime? dismissalDate, string photo, string note, string passportNum,
                        string title, string accessLayer, string login, string password, ContactInfo contactInfo)
        {
            LastName      = lastName;
            FirstName     = firstName;
            MiddleName    = middleName;
            BirthDate     = birthDate;
            HireDate      = hireDate;
            DismissalDate = dismissalDate;
            Photo         = photo;
            Note          = note;
            PassportNum   = passportNum;
            Title         = title;
            AccessLayer   = accessLayer;
            Login         = login;
            Password      = password;

            _contactInfo = new Lazy<ContactInfo>(() => contactInfo);
        }//

        /// <summary>
        /// Конструктор для создания объекта из БД.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="birthDate"></param>
        /// <param name="hireDate"></param>
        /// <param name="dismissalDate"></param>
        /// <param name="photo"></param>
        /// <param name="note"></param>
        /// <param name="passportNum"></param>
        /// <param name="title"></param>
        /// <param name="accessLayer"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public Employee(int employeeId, string lastName, string firstName, string middleName, DateTime? birthDate,
                        DateTime? hireDate, DateTime? dismissalDate, string photo, string note, string passportNum, 
                        string title, string accessLayer, string login, string password)
        {
            EmployeeId    = employeeId;
            LastName      = lastName;
            FirstName     = firstName;
            MiddleName    = middleName;
            BirthDate     = birthDate;
            HireDate      = hireDate;
            DismissalDate = dismissalDate;
            Photo         = photo;
            Note          = note;
            PassportNum   = passportNum;
            Title         = title;
            AccessLayer   = accessLayer;
            Login         = login;
            Password      = password;

            _contactInfo = new Lazy<ContactInfo>(() => PartsDAL.FindContactInfo(employeeId));
        }//


        /// <summary>
        /// Возвращает полное ФИО. Пример : 'Иванов Иван Иванович'.
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            return String.Format("{0} {1} {2}", LastName, FirstName, MiddleName);
        }//GetFullName
        /// <summary>
        /// Возвращает фамилию и инициалы. Пример : 'Иванов И. И.'.
        /// </summary>
        /// <returns></returns>
        public string GetShortFullName()
        {
            string shortMiddleName = (MiddleName != null) ? MiddleName.ToUpper()[0] + "." : "";
            return String.Format("{0} {1}. {2}", LastName, FirstName.ToUpper()[0], shortMiddleName);
        }//GetShortFullName
    }//Employee
  
}//namespace
