﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Helper;
using PartsApp;

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
        }

        public int       EmployeeId    { get; set; }
        public string    LastName      { get; set; }
        public string    FirstName     { get; set; }
        public string    MiddleName    { get; set; }
        public DateTime? BirthDate     { get; set; }
        public DateTime? HireDate      { get; set; }
        public DateTime? DisableDate { get; set; }
        public string    Photo         { get; set; }
        public string    Note          { get; set; }
        public string    PassportNum   { get; set; }
        public string    Title         { get; set; }
        public string    AccessLayer   { get; set; }
        public string    Login         { get; set; }
        public string    Password      { get; set; }
        public bool IsDisabled { get { return DisableDate != default; } }
        public bool IsAdmin { get { return AccessLayer == AccessLayers.Admin.ToDescription(); } }

        public string FullName { get { return String.Format("{0} {1} {2}", LastName, FirstName, MiddleName); } }
    
        public ContactInfo ContactInfo { get; set; }

        /// <summary>
        /// Конструктор для добавления нового объекта в БД.
        /// </summary>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="birthDate"></param>
        /// <param name="hireDate"></param>
        /// <param name="disableDate"></param>
        /// <param name="photo"></param>
        /// <param name="note"></param>
        /// <param name="passportNum"></param>
        /// <param name="title"></param>
        /// <param name="accessLayer"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="contactInfo"></param>
        public Employee(int employeeId, string lastName, string firstName, string middleName, DateTime? birthDate,
                        DateTime? hireDate, DateTime? disableDate, string photo, string note, string passportNum,
                        string title, string accessLayer, string login, string password, ContactInfo contactInfo)
        {
            EmployeeId    = employeeId;
            LastName      = lastName;
            FirstName     = firstName;
            MiddleName    = middleName;
            BirthDate     = birthDate;
            HireDate      = hireDate;
            DisableDate   = disableDate;
            Photo         = photo;
            Note          = note;
            PassportNum   = passportNum;
            Title         = title;
            AccessLayer   = accessLayer;
            Login         = login;
            Password      = password;

            ContactInfo = contactInfo;
        }

        /// <summary>
        /// Конструктор для создания объекта из БД.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="birthDate"></param>
        /// <param name="hireDate"></param>
        /// <param name="disableDate"></param>
        /// <param name="photo"></param>
        /// <param name="note"></param>
        /// <param name="passportNum"></param>
        /// <param name="title"></param>
        /// <param name="accessLayer"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public Employee(int employeeId, string lastName, string firstName, string middleName, DateTime? birthDate,
                        DateTime? hireDate, DateTime? disableDate, string photo, string note, string passportNum,
                        string title, string accessLayer, string login, string password)
            : this(employeeId, lastName, firstName, middleName, birthDate, hireDate, disableDate,
                   photo, note, passportNum, title, accessLayer, login, password, null)
        { }


        /// <summary>
        /// Возвращает фамилию и инициалы. Пример : 'Иванов И. И.'.
        /// </summary>
        /// <returns></returns>
        public string GetShortFullName()
        {
            string shortMiddleName = (MiddleName != null) ? MiddleName.ToUpper()[0] + "." : "";
            return String.Format("{0} {1}. {2}", LastName, FirstName.ToUpper()[0], shortMiddleName);
        }

        public void TrySetContactInfo(ContactInfo info)
        {
            if (info != null)
            {
                ContactInfo = info;
            }
        }

    }
  
}
