using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Textile.DAL.Models
{
    public class PaiementModel
    {
        public int id { get; set; }
        public Nullable<int> client { get; set; }
        public Nullable<decimal> Montant { get; set; }
        public string TypePaiement { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DatePaiement { get; set; }
        public Nullable<System.DateTime> DateEcheance { get; set; }
    }
}
