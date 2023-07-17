using System;
namespace Textile.DAL.Models
{
    public class StockOuvertModel
    {
        public int id { get; set; }
        public Nullable<int> reference { get; set; }
        public string CodeReference { get; set; }
        public Nullable<double> Qte { get; set; }

    }
}
