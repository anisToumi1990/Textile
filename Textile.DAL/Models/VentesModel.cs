using System;
namespace Textile.DAL.Models
{
    public class VentesModel
    {
        public int id { get; set; }
        public string Code { get; set; }
        public Nullable<int> client { get; set; }
        public string NomClient { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public string Type { get; set; }
        public string TypePaiement { get; set; }
        public string TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
