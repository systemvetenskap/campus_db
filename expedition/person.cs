using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace expedition
{
    public class person
    {
        public int id { get; set; }
        public string förnamn { get; set; }
        public string efternamn { get; set; }

        public string FullständigtNamn
        {
            get
            {
                return förnamn + " " + efternamn;
            }
        }
    }
}
