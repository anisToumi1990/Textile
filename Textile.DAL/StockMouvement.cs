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
    
    public partial class StockMouvement
    {
        public int id { get; set; }
        public string Mouvement { get; set; }
        public Nullable<double> Qte { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<int> Reference { get; set; }
        public Nullable<int> Facture { get; set; }
    }
}
