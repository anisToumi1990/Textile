using Textile.DAL;
using Textile.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace Textile.BLL
{
    public class StockMouvementBLL : GenericBLL<StockMouvement>
    {
        public static List<StockMouvementModel> GetStockMouvement(List<StockMouvement> list)
        {
            var StockMouvementModelList = new List<StockMouvementModel>();
            var entities = new TextileEntities();
           // Get List of Vouccer
            foreach (var item in list)
            {
                var StockMouvementModel = new StockMouvementModel();
                StockMouvementModel.Mouvement = item.Mouvement;
                StockMouvementModel.Qte = item.Qte;
                StockMouvementModel.id = item.id;
                StockMouvementModel.Reference = item.Reference;
                StockMouvementModel.DateCreation = item.DateCreation;
                if (item.Facture != null)
                {
                    var facture = entities.Ventes.Find(item.Facture);
                    StockMouvementModel.Facture =  facture != null ? facture.Code : string.Empty ;
                    StockMouvementModel.Client = facture != null? entities.Client.Find(facture.client).Nom : string.Empty
                       ;
                }

                StockMouvementModelList.Add(StockMouvementModel);
            }
            return StockMouvementModelList;
        }
    }
}
