﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TextileEntities : DbContext
    {
        public TextileEntities()
            : base("name=TextileEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Fournisseur> Fournisseur { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserRolesMapping> UserRolesMapping { get; set; }
        public virtual DbSet<References> References { get; set; }
        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<ArticleDetails> ArticleDetails { get; set; }
        public virtual DbSet<StockOuvert> StockOuvert { get; set; }
        public virtual DbSet<StockOuvertDetail> StockOuvertDetail { get; set; }
        public virtual DbSet<Ventes> Ventes { get; set; }
        public virtual DbSet<VenteDetails> VenteDetails { get; set; }
        public virtual DbSet<Societe> Societe { get; set; }
        public virtual DbSet<Paiement> Paiement { get; set; }
        public virtual DbSet<StockMouvement> StockMouvement { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
