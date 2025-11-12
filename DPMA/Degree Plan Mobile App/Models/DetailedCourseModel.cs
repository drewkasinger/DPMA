using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.Models
{
    internal class DetailedCourseModel
    {
        [PrimaryKey, AutoIncrement]
        public int dcID { get; set; }
        public int courseId { get; set; }
        public string statusPicker {  get; set; }
        public DateTime dueDate { get; set; }
        public DateTime startNotification { get; set; }
        public DateTime endNotification { get; set; }
        public string optionalNotes { get; set; }
        public string instructorName { get; set; }
        public string instructorPhone { get; set; }
        public string instructorEmail { get; set; }
    }
}
