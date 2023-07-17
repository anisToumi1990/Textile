using System;

namespace Textile.DAL.Models
{
   public class VenteDetailsModel
    {
        public int id { get; set; }
        public Nullable<int> vente { get; set; }
        public Nullable<int> reference { get; set; }
        public string CodeReference { get; set; }
        public Nullable<double> Qte { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public Nullable<decimal> PrixUnitaire { get; set; }
        public Nullable<decimal> _PrixTotal { get; set; }
        public string _PrixUnitaire { get; set; }
        public string PrixTotal { get; set; }
        public Nullable<decimal> Remise { get; set; }
        public string _Remise { get; set; }
        public string Mode { get; set; }
    }
}
