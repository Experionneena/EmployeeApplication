using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeApplication.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public int Empid { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Designation { get; set; }
        public string Band { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
    }
}