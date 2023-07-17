using System.Collections.Generic;
using System.Linq;
using Textile.DAL;

namespace Textile.BLL
{
    public class ArticleDetailsBLL : GenericBLL<ArticleDetails>
    {
        public static List<ArticleDetails> GetArticleDetail(int article)
        {
            using (TextileEntities entities = new TextileEntities())
            {
                var articleDetails = entities.ArticleDetails.Where(elt => elt.article == article).ToList();
                return articleDetails;
            }
        }
    }
}
