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
    
    public partial class Articles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Articles()
        {
            this.ArticleDetails = new HashSet<ArticleDetails>();
        }
    
        public int id { get; set; }
        public Nullable<int> reference { get; set; }
        public string CodeArticle { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public string Couleur { get; set; }
    
        public virtual References References { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticleDetails> ArticleDetails { get; set; }
    }
}
