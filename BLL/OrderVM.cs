using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class OrderVM
    {
        public int Order_Id { get; set; }
        public int Client_id { get; set; }
        public string ClientName { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string E_Mail { get; set; }
        public string Adress { get; set; }
        public string Date { get; set; }
        public int Id { get; set; }

        public int Proudct_Id { get; set; }
        public string ProudctIds { get; set; }
        public string Quantity { get; set; }
        public string OfferIds { get; set; }
        public string Quantity_offer { get; set; }

    }
}
