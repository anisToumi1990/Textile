using Textile.DAL;
using System.Collections.Generic;
using System.Linq;

namespace Textile.BLL
{
    public class ArticlesBLL : GenericBLL<Articles>
    {
        public static List<Articles> GetArticles(int reference)
        {
            using (TextileEntities entities = new TextileEntities())
            {
                var referenceDetails = entities.Articles.Where(elt => elt.reference == reference).ToList();
                return referenceDetails;
            }
        }
    }
}
