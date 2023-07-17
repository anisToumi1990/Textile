using System.Collections.Generic;
namespace Textile.DAL.Models
{

    public class TextValueModel
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public double? Val1 { get; set; }
        public double? Val2 { get; set; }
        public double? Val3 { get; set; }
        public double? Val4 { get; set; }
        public double? Val5 { get; set; }
        public double? Val6 { get; set; }
        public bool? Bool1 { get; set; }
        public bool? Bool2 { get; set; }
        public TextValueModel()
        {
            Text = "";
            Value = "";
            Val1 = 0;
            Val2 = 0;
            Val3 = 0;
            Val4 = 0;
            Val5 = 0;
            Val6 = 0;
            Bool1 = false;
            Bool2 = false;
        }
        public TextValueModel(string text, string value, double? val1 = 0, double? val2 = 0, double? val3 = 0, double? val4 = 0, double? val5 = 0, double? val6 = 0, bool? bool1 = false, bool? bool2 = false)
        {
            Text = text;
            Value = value;
            Val1 = val1;
            Val2 = val2;
            Val3 = val3;
            Val4 = val4;
            Val5 = val5;
            Val6 = val6;
            Bool1 = bool1;
            Bool2 = bool2;
        }

        public static List<string> paiement = new List<string>(){
            "Chéque",
            "Espéce"
        };
        public static List<string> typeVente = new List<string>(){
            "Facture",
            "Bon de Livraison"
        };
    }
}