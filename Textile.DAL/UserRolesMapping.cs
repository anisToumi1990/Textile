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
    
    public partial class UserRolesMapping
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
    
        public virtual Roles Roles { get; set; }
        public virtual Users Users { get; set; }
    }
}
