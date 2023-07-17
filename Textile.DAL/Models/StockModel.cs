using System;
namespace Textile.DAL.Models
{
    public class StockModel
    {
        public int id { get; set; }
        public string reference { get; set; }
        public double? qte { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
    }
}
