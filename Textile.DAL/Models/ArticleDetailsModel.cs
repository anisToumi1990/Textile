using System;

namespace Textile.DAL.Models
{
    public class ArticleDetailsModel
    {
        public int id { get; set; }
        public string Code { get; set; }
        public string Designation { get; set; }
        public Nullable<int> article { get; set; }
        public Nullable<double> Qte { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public Nullable<bool> Disponible { get; set; }
        public string imgURL { get; set; }
    }
}
