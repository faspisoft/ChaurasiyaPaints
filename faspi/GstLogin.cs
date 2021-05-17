using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace faspi
{
    class GstLogin
    {
        public string Gstin { get; set; }
        public string GstUser { get; set; }
        public string AuthToken { get; set; }
        public DateTime ValidTill { get; set; }
    }
}
