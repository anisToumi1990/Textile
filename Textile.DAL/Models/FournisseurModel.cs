namespace Textile.DAL.Models
{
    public class FournisseurModel
    {
        public int idFournisseur { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string NumeroTelephone { get; set; }
        public string CodePostal { get; set; }
        public string IBAN { get; set; }
        public string Fax { get; set; }
        public string MatriculeFiscal { get; set; }
    }
}
