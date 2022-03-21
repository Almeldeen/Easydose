using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class ProudctVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string OfferStatus { get; set; }
        public string OrderStatus { get; set; }
        public string OfferName { get; set; }
        public List<string> OfferNames { get; set; }
        public string Show { get; set; }
        public decimal? Price { get; set; }
        public Nullable<int> Discount_per { get; set; }
        public Nullable<int> Discount_num { get; set; }
        public decimal? TotalPrice { get; set; }
        public Nullable<int> Category_Id { get; set; }
        public List<int?> Category_Ids { get; set; }
        public Nullable<int> Brand_Id { get; set; }
        public string CategoryName { get; set; }
        public List<string> CategoryNames { get; set; }
        public string BrandName { get; set; }
        public int? Quantity { get; set; }
        public string Description_A { get; set; }
        public string Description_E { get; set; }
        public string wherestatus { get; set; }
        public string KindOfOrder { get; set; }

    }
}
