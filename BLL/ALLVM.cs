using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class ALLVM
    {
       public List<CategoryVM> categories { set; get; }
       public List<BrandVM> brands { set; get; }
        public List<AdVM> Slider { set; get; }
        public List<AdVM> AdsUp { set; get; }
        public List<AdVM> AdsMid { set; get; }
        public List<AdVM> AdsDown { set; get; }
        public MediaVM media { set; get; }

}
    public enum FromPage
    {
        Category=1,
        brand=2,
        best=3,
        dis=4,
        Search=5,
        offer=6,
    }
    public enum OrderStatus
    {
        NewOrder = 1,
        Connected = 2,
        Finshed=3,
        
    }
    public enum Show_Status
    {
        Visable=1,
        unVisable=2,
    }
    public enum WhereStatus
    {
        imported = 1,
        local = 2,
    }
}
