using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DataModel;
using BLL;
namespace DLL
{
    public interface IUsersRepo
    {
        ALLVM GetALL();
        List<ProudctVM> ViewProudct(int? Id, string name, string category, string source, decimal? fromPrice, decimal? toPrice, int? frompage, string search);
        List<ProudctVM> getproudct_Cart(string ProudctIds);
        List<ProudctVM> getproudct_Cart_Offer(string OfferIds);
        List<ProudctVM> getProudct_Dis();
        List<ProudctVM> getProudct_Best();       
        Help AddOrder(OrderVM order);
    }
    public class Users : IUsersRepo
    {
        MedicalEntities db = new MedicalEntities();
        public ALLVM GetALL()
        {
            ALLVM aLLVM = new ALLVM();
            aLLVM.categories = db.Categories.Where(x=> x.Show== "Visable").Select(x => new CategoryVM { Id = x.Id, Name = x.Name, Image = x.Image }).ToList();
            aLLVM.brands = db.Brands.Where(x => x.Show == "Visable").Select(x => new BrandVM { Id = x.Id, Name = x.Name, Image = x.Image }).ToList();
            aLLVM.Slider = db.Ads.Where(x => x.Place == "slide").Select(x => new AdVM { Ad_Id = x.Ad_Id, Image = x.Image, Place = x.Place}).ToList();
          
            foreach (var item in aLLVM.Slider)
            {
                item.Offer_Id = db.Offer_details.Where(x => x.Offer.Ads_id == item.Ad_Id).Select(x => x.Offer_Id).FirstOrDefault();
                item.OfferName = db.Offer_details.Where(x => x.Offer.Ads_id == item.Ad_Id).Select(x => x.Offer.OfferName).FirstOrDefault();
            }
            aLLVM.AdsUp = db.Ads.Where(x => x.Place == "up").Select(x => new AdVM { Ad_Id = x.Ad_Id, Image = x.Image, Place = x.Place }).ToList();
            foreach (var item in aLLVM.AdsUp)
            {
                item.Offer_Id = db.Offer_details.Where(x => x.Offer.Ads_id == item.Ad_Id).Select(x => x.Offer_Id).FirstOrDefault();
                item.OfferName= db.Offer_details.Where(x => x.Offer.Ads_id == item.Ad_Id).Select(x => x.Offer.OfferName).FirstOrDefault();
            }
            aLLVM.AdsMid = db.Ads.Where(x => x.Place == "mid").Select(x => new AdVM { Ad_Id = x.Ad_Id, Image = x.Image, Place = x.Place }).ToList();
            foreach (var item in aLLVM.AdsMid)
            {
                item.Offer_Id = db.Offer_details.Where(x => x.Offer.Ads_id == item.Ad_Id).Select(x => x.Offer_Id).FirstOrDefault();
                item.OfferName = db.Offer_details.Where(x => x.Offer.Ads_id == item.Ad_Id).Select(x => x.Offer.OfferName).FirstOrDefault();
            }
            aLLVM.AdsDown = db.Ads.Where(x => x.Place == "down").Select(x => new AdVM { Ad_Id = x.Ad_Id, Image = x.Image, Place = x.Place }).ToList();
            foreach (var item in aLLVM.AdsDown)
            {
                item.Offer_Id = db.Offer_details.Where(x => x.Offer.Ads_id == item.Ad_Id).Select(x => x.Offer_Id).FirstOrDefault();
                item.OfferName = db.Offer_details.Where(x => x.Offer.Ads_id == item.Ad_Id).Select(x => x.Offer.OfferName).FirstOrDefault();
            }
            return aLLVM;
        }
        public List<ProudctVM> ViewProudct(int? Id, string name, string category, string source, decimal? fromPrice, decimal? toPrice, int? frompage, string search)
        {
            List<ProudctVM> res = new List<ProudctVM>();
            source = source == "الكل" ? null : source== "undefined" ? null:source;
            if (frompage == (int)FromPage.Category)
            {
                res = db.FilterProudct(name, Id, source, fromPrice, toPrice).Select(x => new ProudctVM { Name = x.Name, Image = x.Image, Id =(int)x.Id, Discount_num = x.Discount_num, Discount_per = x.Discount_per, Price = x.Price, TotalPrice = Math.Ceiling((decimal)x.TotalPrice) }).ToList();
            }
            else if (frompage == (int)FromPage.brand)
            {
                res = db.Proudcuts.Where(x => x.Brand_Id == Id && x.Show == "Visable").Select(x => new ProudctVM { Id = x.Id, Name = x.Name, TotalPrice = Math.Ceiling((decimal)x.TotalPrice), Image = x.Image, Price = x.Price, Discount_per = x.Discount_per, Discount_num = x.Discount_num }).ToList();
            }
            else if (frompage == (int)FromPage.best)
            {
                res = db.FilterProudctBest(name, Id, source, fromPrice, toPrice).Select(x => new ProudctVM { Name = x.Name, Image = x.Image, Id = (int)x.Id, Discount_num = x.Discount_num, Discount_per = x.Discount_per, Price = x.Price, TotalPrice = Math.Ceiling((decimal)x.TotalPrice) }).ToList();
            }
            else if (frompage == (int)FromPage.dis)
            {
                res = db.FilterProudct(name, Id, source, fromPrice, toPrice).Where(x=> x.Discount_per>0).Select(x => new ProudctVM { Name = x.Name, Image = x.Image, Id = (int)x.Id, Discount_num = x.Discount_num, Discount_per = x.Discount_per, Price = x.Price, TotalPrice = Math.Ceiling((decimal)x.TotalPrice) }).ToList();

            }
            else if (frompage == (int)FromPage.Search)
            {
                name = name == null ? search : name;
                List<ProudctVM> proudcuts = new List<ProudctVM>();
                if (category != "")
                {
                    string[] ids = category.Split(new char[] { ',' });
                    foreach (var item in ids)
                    {
                        int id = Convert.ToInt32(item);
                        proudcuts = db.FilterProudct(name, id, source, fromPrice, toPrice).Select(x => new ProudctVM { Name = x.Name, Image = x.Image, Id = (int)x.Id, Discount_num = x.Discount_num, Discount_per = x.Discount_per, Price = x.Price, TotalPrice = Math.Ceiling((decimal)x.TotalPrice) }).ToList();
                        res.AddRange(proudcuts);
                    }
                }
                else
                {
                    res = db.FilterProudct(name, null, source, fromPrice, toPrice).Select(x => new ProudctVM { Name = x.Name, Image = x.Image, Id = (int)x.Id, Discount_num = x.Discount_num, Discount_per = x.Discount_per, Price = x.Price, TotalPrice = Math.Ceiling((decimal)x.TotalPrice) }).ToList();


                }
            }
            else if (frompage == (int)FromPage.offer)
            {
                res = db.Offer_details.Where(x => x.Offer_Id == Id && x.Proudcut.Show == "Visable"&&x.Active== "true").Select(x => new ProudctVM { Id = x.Proudcut.Id, Name = x.Proudcut.Name, TotalPrice = Math.Ceiling((decimal)x.Proudcut.TotalPrice), Image = x.Proudcut.Image, BrandName = x.Proudcut.Brand.Name, /*CategoryName = x.Proudcut.Category.Name, Category_Id = x.Proudcut.Category_Id,*/ Brand_Id = x.Proudcut.Brand_Id, Price = x.Proudcut.Price, Discount_per = x.Proudcut.Discount_per }).ToList();
            }
            else if (name != null || category != "" || fromPrice != null || source != null || toPrice != null)
            {

                List<ProudctVM> proudcuts = new List<ProudctVM>();
                if (category != ""&&category!=null)
                {
                    string[] ids = category.Split(new char[] { ',' });
                    foreach (var item in ids)
                    {
                        int id = Convert.ToInt32(item);
                        proudcuts = db.FilterProudct(name, id, source, fromPrice, toPrice).Select(x => new ProudctVM { Name = x.Name, Image = x.Image, Id = (int)x.Id, Discount_num = x.Discount_num, Discount_per = x.Discount_per, Price = x.Price, TotalPrice = Math.Ceiling((decimal)x.TotalPrice) }).ToList();
                        res.AddRange(proudcuts);
                    }
                }
                else
                {
                    res = db.FilterProudct(name, null, source, fromPrice, toPrice).Select(x => new ProudctVM { Name = x.Name, Image = x.Image, Id =(int) x.Id, Discount_num = x.Discount_num, Discount_per = x.Discount_per, Price = x.Price, TotalPrice = Math.Ceiling((decimal)x.TotalPrice) }).ToList();
                   

                }
            }
            else
            {
                res = db.Proudcuts.Where(x => x.Show == "Visable").Select(x => new ProudctVM { Id = x.Id, Name = x.Name, TotalPrice = Math.Ceiling((decimal)x.TotalPrice), Image = x.Image, Price = x.Price, Discount_per = x.Discount_per, Discount_num = x.Discount_num }).ToList();

            }

            return res;
        }
        public List<ProudctVM> getproudct_Cart(string ProudctIds)
        {
            List<ProudctVM> proudcts = new List<ProudctVM>();
            try
            {
                string[] ids = ProudctIds.Split(new char[] { ',' });

                foreach (var item in ids)
                {
                    int id = Convert.ToInt32(item);
                    var pro = db.Proudcuts.Where(x => x.Id == id&&x.Show== "Visable").Select(x => new ProudctVM { Id = x.Id, Name = x.Name, TotalPrice = Math.Ceiling((decimal)x.TotalPrice), Image = x.Image,   Price = x.Price, Discount_per = x.Discount_per, Discount_num = x.Discount_num }).FirstOrDefault();
                    proudcts.Add(pro);
                }
                return proudcts;
            }
            catch (Exception)
            {

                return proudcts;
            }

        }
        public List<ProudctVM> getproudct_Cart_Offer(string OfferIds)
        {
            List<ProudctVM> proudcts = new List<ProudctVM>();
            try
            {
                string[] ids = OfferIds.Split(new char[] { ',' });

                foreach (var item in ids)
                {
                    int id = Convert.ToInt32(item);
                    var pro = db.Offers.Where(x => x.Id == id  ).Select(x => new ProudctVM { Id = x.Id, Name = x.OfferName, TotalPrice = Math.Ceiling((decimal)x.OfferPrice), Image = x.Ad.Image }).FirstOrDefault();
                    proudcts.Add(pro);
                }
                return proudcts;
            }
            catch (Exception)
            {

                return proudcts;
            }
        }
        public Help AddOrder(OrderVM order)
        {
            Help help = new Help();
            try
            {
               
                Order order1 = new Order();
                order1.ClientName = order.ClientName;
                order1.Phone = order.Phone;
                order1.Adress = order.Adress;
                order1.E_Mail = order.E_Mail;
                order1.Date = DateTime.Now;
                order1.Status =OrderStatus.NewOrder.ToString();
                db.Orders.Add(order1);
                int res = db.SaveChanges();
                help.id = order1.Order_Id;
                if (res != 1)
                {
                  help.res= false;
                    return help;
                }
                List<Order_Details> order_Details = new List<Order_Details>();
                if (order.ProudctIds!=null)
                {
                    string[] ids = order.ProudctIds.Split(new char[] { ',' });
                    string[] qui = order.Quantity.Split(new char[] { ',' });
                    for (int i = 0; i < ids.Length; i++)
                    {
                        Order_Details _Details = new Order_Details();
                        _Details.Order_Id = order1.Order_Id;
                        _Details.Proudct_Id = Convert.ToInt32(ids[i]);
                        _Details.Quantity = Convert.ToInt32(qui[i]);
                        order_Details.Add(_Details);
                    }
                }
                 if (order.OfferIds!=null)
                {
                    string[] ids = order.OfferIds.Split(new char[] { ',' });
                    string[] qui = order.Quantity_offer.Split(new char[] { ',' });
                    for (int i = 0; i < ids.Length; i++)
                    {
                        Order_Details _Details = new Order_Details();
                        _Details.Order_Id = order1.Order_Id;
                        _Details.Offer_Id = Convert.ToInt32(ids[i]);
                        _Details.Quantity = Convert.ToInt32(qui[i]);
                        order_Details.Add(_Details);
                    }
                }             
                db.Order_Details.AddRange(order_Details);
                res = db.SaveChanges();
                if (res > 0)
                {
                    help.res = true;
                    return help;
                }
                else
                {
                    help.res = false;
                    return help;
                }
            }
            catch (Exception ex)
            {

                help.res = false;
                return help;
            }


        }
       public List<ProudctVM> getProudct_Dis()
        {
            var pro = db.Proudcuts.Where(x => x.Discount_per > 0&&x.Show== "Visable").Take(10).Select(x => new ProudctVM { Id = x.Id, Name = x.Name, TotalPrice = Math.Ceiling((decimal)x.TotalPrice), Image = x.Image,   Price = x.Price, Discount_per = x.Discount_per, Discount_num = x.Discount_num }).ToList();
            return pro;
        }
        public List<ProudctVM> getProudct_Best()
        {
            var results = (from p in db.Order_Details
                           where p.Proudcut.Show== "Visable"
                           group p.Quantity by p.Proudcut into g
                          select new { Proudcut = g.Key, proudct_Count = g.Sum() }).ToList();

            var pro = results.OrderByDescending(x=> x.proudct_Count).Take(10).Select(x => new ProudctVM { Id = x.Proudcut.Id, Name = x.Proudcut.Name, TotalPrice = Math.Ceiling((decimal)x.Proudcut.TotalPrice), Image = x.Proudcut.Image, /*CategoryName = x.Proudcut.Category.Name, Category_Id = x.Proudcut.Category_Id,*/ Price = x.Proudcut.Price, Discount_per = x.Proudcut.Discount_per }).ToList();
            return pro;
        }
       


    }
}
