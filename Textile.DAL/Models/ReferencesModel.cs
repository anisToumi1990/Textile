using System;

namespace Textile.DAL.Models
{
    public class ReferencesModel
    {

        public int id { get; set; }
        public string Libellé { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
    }
}
