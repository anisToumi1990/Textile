using System;
namespace Textile.DAL.Models
{
    public class StockMouvementModel
    {
        public int id { get; set; }
        public string Mouvement { get; set; }
        public Nullable<double> Qte { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<int> Reference { get; set; }
        public string Facture { get; set; }
        public string Client { get; set; }
    }
}
