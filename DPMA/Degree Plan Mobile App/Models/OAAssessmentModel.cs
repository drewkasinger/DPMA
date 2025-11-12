using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.Models
{
    internal class OAAssessmentModel
    {
        [PrimaryKey, AutoIncrement]
        public int OAID { get; set; }
        public int courseId { get; set; }
        public string OA { get; set; }
        public DateTime OAStart { get; set; }
        public DateTime OAEnd { get; set; }
    }
}
