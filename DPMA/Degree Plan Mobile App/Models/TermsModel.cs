using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.Models
{
    public class TermsModel
    {
        [PrimaryKey, AutoIncrement]
        public int termId { get; set; }
        public int accountId { get; set; }
        public string termName { get; set; }
        public string termStart { get; set; }
        public string termEnd { get; set; }
    }
}
