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
    
    public partial class VenteDetails
    {
        public int id { get; set; }
        public Nullable<int> vente { get; set; }
        public Nullable<int> reference { get; set; }
        public Nullable<double> Qte { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public Nullable<decimal> PrixUnitaire { get; set; }
        public Nullable<decimal> Remise { get; set; }
    }
}
