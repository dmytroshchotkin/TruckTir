using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsApp
{
    public class Employee
    {
        public int EmployeeId           { get; set; }
        public string LastName          { get; set; }
        public string FirstName         { get; set; }
        public string MiddleName        { get; set; }
        public DateTime? BirthDate      { get; set; }
        public DateTime? HireDate       { get; set; }
        public DateTime? DismissalDate  { get; set; }
        public int? ContactInfoId       { get; set; }
        public string Photo             { get; set; }
        public string Note              { get; set; }
        public string PassportNum       { get; set; }
        public string Title             { get; set; }
        public string AccessLayer       { get; set; }
        public string Password          { get; set; }
         

        public Employee() {}
        public Employee(int employeeId, string lastName, string firstName, string middleName, DateTime? birthDate, 
                        DateTime? hireDate, DateTime? dismissalDate, int? contactInfoId, string photo, string note, 
                        string passportNum, string title, string accessLayer, string password)
        {      
            EmployeeId      =  employeeId;
            LastName        =  lastName;
            FirstName       =  firstName;
            MiddleName      =  middleName;
            BirthDate       =  birthDate; 
            HireDate        =  hireDate;
            DismissalDate   =  dismissalDate;
            ContactInfoId   =  contactInfoId;
            Photo           =  photo; 
            Note            =  note;
            PassportNum     =  passportNum;
            Title           =  title;
            AccessLayer     =  accessLayer;
            Password        =  password;
        }

        public string GetFullName()
        {
            return String.Format("{0} {1} {2}", LastName, FirstName, MiddleName);
        }//GetFullName
    }//Employee
}//namespace
