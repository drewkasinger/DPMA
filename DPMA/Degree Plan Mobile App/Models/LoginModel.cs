using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Degree_Plan_Mobile_App.Models
{
    public class LoginModel
    {
        [PrimaryKey, AutoIncrement]
        public int LoginId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
