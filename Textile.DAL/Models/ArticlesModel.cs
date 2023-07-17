using System;
namespace Textile.DAL.Models
{
    public class ArticlesModel
    {
        public int id { get; set; }
        public Nullable<int> reference { get; set; }
        public string CodeArticle { get; set; }
        public double? Quantite { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public string Couleur { get; set; }

    }
}
