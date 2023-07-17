namespace Textile.DAL.Models
{
    public class ClientModel
    {
        public int idClient { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string NumeroTelephoe { get; set; }
        public string CodePostal { get; set; }
        public string Fax { get; set; }
        public string MatriculeFiscal { get; set; }
        public string IBAN { get; set; }
        public bool Exonore { get; set; }
        public bool Active { get; set; }
    }
}
