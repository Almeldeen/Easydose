using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class AdVM
    {
        public int Ad_Id { get; set; }
        public int? Offer_Id { get; set; }
        public string Name { get; set; }
        public string OfferName { get; set; }
        public string Image { get; set; }
        public string Place { get; set; }
    }
}
