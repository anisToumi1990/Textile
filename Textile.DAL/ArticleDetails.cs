//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Textile.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ArticleDetails
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
    
        public virtual Articles Articles { get; set; }
    }
}
