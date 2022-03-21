using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class OfferVM
    {
        public int Id { get; set; }
        public string OfferName { get; set; }
        public decimal? OfferPrice { get; set; }
        public string Image { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<int> Ads_id { get; set; }

    }
}
