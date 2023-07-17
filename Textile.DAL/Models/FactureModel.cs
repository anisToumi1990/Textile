namespace Textile.DAL.Models
{
   public class FactureModel
    {
        public string Code { get; set; }
        public string Client { get; set; }
        public string Adresse { get; set; }
        public string TelephoneClient { get; set; }
        public string Reference { get; set; }
        public string Quantite { get; set; }
        public string PrixReferenceUnitaire { get; set; }
        public string PrixReferenceTotal { get; set; }
        public string Remise { get; set; }
        public string Total { get; set; }
    }
}
