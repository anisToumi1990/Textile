using System;

namespace Textile.DAL.Models
{
    public class StockOuvertDetailModel
    {
        public int id { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public Nullable<double> Qte { get; set; }
        public Nullable<int> reference { get; set; }
        public Nullable<int> article { get; set; }
        public Nullable<int> articledetail { get; set; }
        public string imgURL { get; set; }
    }
}
