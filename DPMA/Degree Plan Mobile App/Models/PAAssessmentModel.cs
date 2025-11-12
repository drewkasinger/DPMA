using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.Models
{
    internal class PAAssessmentModel
    {
        [PrimaryKey, AutoIncrement]
        public int PAID { get; set; }
        public int courseId { get; set; }
        public string PA { get; set; }
        public DateTime PAStart { get; set; }
        public DateTime PAEnd { get; set; }
    }
}
