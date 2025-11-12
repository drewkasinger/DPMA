using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.Models
{
    internal class CoursesModel
    {
        [PrimaryKey, AutoIncrement]
        public int courseId { get; set; }
        public int termId { get; set; }
        public string statusPicker { get; set; }
        public string courseName { get; set; }
        public string courseStart { get; set; }
        public string courseEnd { get; set; }
    }
}
