using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DataModel;
using BLL;
namespace DLL
{
    public interface IAdminRepo
    {
        AdminVM LoginAdmin(string Name, string Password);
        bool ChangeShowStatus(int Id, int ShowStatus, string from);
        bool ChangeShowStatus_All(string Ids, int ShowStatus, string from);
        bool EditMedia(MediaVM media);
        MediaVM Media();
        #region Category
        bool AddCategory(string Name, string Image);
        List<CategoryVM> LoadAllCategory();
        bool UpdateCatgory(int id, string name, string image);
        bool DeletCategory(int Id);
        bool DeletCategorys(string categoryIDs);
        #endregion
        #region Brand
        bool AddBrand(string Name, string Image);
        List<BrandVM> LoadAllBrand();
        bool UpdateBrand(int id, string name, string image);
        bool DeletBrand(int Id);
        #endregion
        #region Proudct
        bool AddProudct(ProudctVM proudct, string Category_Ids);
        List<ProudctVM> LoadAllProudct();
        List<ProudctVM> LoadAllProudctSearch(string search);
        List<ProudctVM> FilterProudct(int category_id, int brand_id);
        bool DeletProudct(int Id);
        bool DeletProudcts(string ProudctsIDs);
        ProudctVM getProudct(int id);
        bool UpdateProudct(ProudctVM proudct, string Category_Ids);
        bool AddDiscount(int Id, int? Discount_per, int? Discount_num);
        bool AddDiscounts(string ProudctsIDs, int? Discount_per, int? Discount_num);
        bool AddOffer_proudct(int proudct_id, string offer_id);
        bool AddOffer_proudcts(string ProudctsIDs, int offer_id);

        #endregion
        #region Orders
        List<OrderVM> LoadAllOrders();
        bool ChangeOrderStatus(int Order_Id, int Status);
        List<ProudctVM> LoadOrderDetalis(int Order_Id);
        List<ProudctVM> LoadOfferDetalis(int Offer_Id);
        bool DeleteOrder(int Order_Id);
        #endregion
        #region Offer
        bool AddAds(List<AdVM> ads);
        bool AddUpAds(List<AdVM> ads);
        bool AddMidAds(List<AdVM> ads);
        bool AddDownAds(List<AdVM> ads);
        bool AddOffer(string Name, int Ad_Id, decimal price);
        List<OfferVM> LoadAllOffer();
        bool UpdateOffer(int id, string name, int Ad_Id, decimal price);
        bool DeletOffer(int Id);
        #endregion
    }
    public class Admin : IAdminRepo
    {
        MedicalEntities db = new MedicalEntities();
        public AdminVM LoginAdmin(string Name, string Password)
        {
            var adm = db.admins.Where(x => x.Name == Name && x.Password == Password).Select(x => new AdminVM { Id = x.Id, Name = x.Name, Password = x.Password }).FirstOrDefault();
            return adm;
        }
        public bool ChangeShowStatus(int Id, int ShowStatus, string from)
        {
            try
            {

                if (from == "Proudct")
                {
                    var item = db.Proudcuts.Find(Id);
                    item.Show = ShowStatus == 1 ? Show_Status.Visable.ToString() : Show_Status.unVisable.ToString();
                }
                else if (from == "Brand")
                {
                    var item = db.Brands.Find(Id);
                    item.Show = ShowStatus == 1 ? Show_Status.Visable.ToString() : Show_Status.unVisable.ToString();
                }
                else if (from == "Category")
                {
                    var item = db.Categories.Find(Id);
                    item.Show = ShowStatus == 1 ? Show_Status.Visable.ToString() : Show_Status.unVisable.ToString();
                }
                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }


        }
        public bool ChangeShowStatus_All(string Ids, int ShowStatus, string from)
        {

            try
            {
                string[] ids = Ids.Split(new char[] { ',' });

                if (from == "Proudct")
                {
                    foreach (var items in ids)
                    {
                        var Id = Convert.ToInt32(items);
                        var item = db.Proudcuts.Find(Id);
                        item.Show = ShowStatus == 1 ? Show_Status.Visable.ToString() : Show_Status.unVisable.ToString();
                    }

                }
                else if (from == "Brand")
                {
                    foreach (var items in ids)
                    {
                        var Id = Convert.ToInt32(items);
                        var item = db.Brands.Find(Id);
                        item.Show = ShowStatus == 1 ? Show_Status.Visable.ToString() : Show_Status.unVisable.ToString();
                    }

                }
                else if (from == "Category")
                {
                    foreach (var items in ids)
                    {
                        var Id = Convert.ToInt32(items);
                        var item = db.Categories.Find(Id);
                        item.Show = ShowStatus == 1 ? Show_Status.Visable.ToString() : Show_Status.unVisable.ToString();
                    }

                }
                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        public MediaVM Media()
        {
            var res = db.Media.Where(x=> x.Id==1).Select(x => new MediaVM {Id=x.Id,Facebook=x.Facebook,Whatsapp=x.Whatsapp,Massenger=x.Massenger }).FirstOrDefault();
            return res;
        }
        public bool EditMedia(MediaVM media)
        {
            try
            {
                var medi = db.Media.Find(1);
                medi.Facebook = media.Facebook;
                medi.Whatsapp = media.Whatsapp;
                medi.Massenger = media.Massenger;
                db.SaveChanges();
                
                    return true;
                
            }
            catch (Exception ex)
            {

                return false;
            }
           

        }
        #region Category
        public bool AddCategory(string Name, string Image)
        {
            Category category = new Category();
            category.Name = Name;
            category.Image = Image;
            category.Show = "unVisable";
            db.Categories.Add(category);
            int res = db.SaveChanges();
            if (res == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<CategoryVM> LoadAllCategory()
        {
            var cat = db.Categories.Select(x => new CategoryVM { Id = x.Id, Name = x.Name, Image = x.Image, Show = x.Show }).ToList();
            return cat;
        }
        public bool UpdateCatgory(int id, string name, string image)
        {
            var cat = db.Categories.Find(id);
            cat.Name = name;
            if (image != "")
            {
                cat.Image = image;
            }

            int res = db.SaveChanges();
            if (res == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeletCategory(int id)
        {
            try
            {
                var cat = db.Categories.Find(id);
                db.Categories.Remove(cat);
                int res = db.SaveChanges();
                if (res == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }


        }
        public bool DeletCategorys(string categoryIDs)
        {
            try
            {
                string[] ids = categoryIDs.Split(new char[] { ',' });
                var cats = new List<Category>();
                foreach (var item in ids)
                {
                    int id = Convert.ToInt32(item);
                    var cat = db.Categories.Where(x => x.Id == id).FirstOrDefault();
                    cats.Add(cat);
                }
                db.Categories.RemoveRange(cats);
                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion
        #region Brand
        public bool AddBrand(string Name, string Image)
        {
            Brand brand = new Brand();
            brand.Name = Name;
            brand.Image = Image;
            brand.Show = "unVisable";
            db.Brands.Add(brand);
            int res = db.SaveChanges();
            if (res == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<BrandVM> LoadAllBrand()
        {
            var brands = db.Brands.Select(x => new BrandVM { Id = x.Id, Name = x.Name, Image = x.Image, Show = x.Show }).ToList();
            return brands;
        }
        public bool UpdateBrand(int id, string name, string image)
        {
            var brand = db.Brands.Find(id);
            brand.Name = name;
            if (image != "")
            {
                brand.Image = image;
            }

            int res = db.SaveChanges();
            if (res == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeletBrand(int id)
        {
            try
            {
                var brand = db.Brands.Find(id);
                db.Brands.Remove(brand);
                int res = db.SaveChanges();
                if (res == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }


        }
        #endregion
        #region Proudct
        public bool AddProudct(ProudctVM proudct, string Category_Ids)
        {
            Proudcut pro = new Proudcut();
            string[] ids = Category_Ids.Split(new char[] { ',' });
            pro.Name = proudct.Name;
            pro.Price = proudct.Price;          
            pro.Brand_Id = proudct.Brand_Id == -1 ? null : proudct.Brand_Id;
            pro.Image = proudct.Image;
            pro.Discount_per = 0;
            pro.Discount_num = 0;
            pro.TotalPrice = (decimal?)(pro.Price - (((decimal?)pro.Discount_per / 100) * pro.Price));
            pro.Show = "unVisable";
            pro.Description_A = proudct.Description_A;
            pro.Description_E = proudct.Description_E;
            pro.wherestatus = proudct.wherestatus == "-1" ? null : proudct.wherestatus;
            db.Proudcuts.Add(pro);
            foreach (var item in ids)
            {
                int id = Convert.ToInt32(item);
                Proudcut_Details proudcut_Details = new Proudcut_Details();
                proudcut_Details.Category_Id = id;
                proudcut_Details.Proudct_Id = pro.Id;
                db.Proudcut_Details.Add(proudcut_Details);
            }
            int res = db.SaveChanges();
            if (res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public List<ProudctVM> LoadAllProudct()
        {
            var pro = db.Proudcut_Details.Select(x => new ProudctVM { Id = x.Proudcut.Id, Name = x.Proudcut.Name, TotalPrice = x.Proudcut.TotalPrice, Image = x.Proudcut.Image,Price = x.Proudcut.Price, Discount_per = x.Proudcut.Discount_per, Discount_num = x.Proudcut.Discount_num, Show = x.Proudcut.Show, Description_E = x.Proudcut.Description_E, Description_A = x.Proudcut.Description_A, wherestatus = x.Proudcut.wherestatus }).Distinct().ToList();
            foreach (var item in pro)
            {
                item.CategoryNames = db.Proudcut_Details.Where(x => x.Proudct_Id == item.Id).Select(x => x.Category.Name).ToList();


                item.OfferNames = db.Offer_details.Where(x => x.Proudct_id == item.Id && x.Active == "true").Select(x => x.Offer.OfferName).ToList();
            }
            return pro;
        }
        public List<ProudctVM> LoadAllProudctSearch(string search)
        {
            var proids = db.SearchResult(search).Select(x => x.Id ).Distinct().ToList();
            var pro = new List<ProudctVM>();
            if (proids!=null)
            {
                foreach (var item in proids)
                {
                    pro.Add (db.Proudcut_Details.Where(x => x.Proudct_Id == item).Select(x => new ProudctVM { Id = x.Proudcut.Id, Name = x.Proudcut.Name, TotalPrice = x.Proudcut.TotalPrice, Image = x.Proudcut.Image, Price = x.Proudcut.Price, Discount_per = x.Proudcut.Discount_per, Discount_num = x.Proudcut.Discount_num, Show = x.Proudcut.Show, Description_E = x.Proudcut.Description_E, Description_A = x.Proudcut.Description_A, wherestatus = x.Proudcut.wherestatus }).FirstOrDefault());
                }

                foreach (var item in pro)
                {
                    item.CategoryNames = db.Proudcut_Details.Where(x => x.Proudct_Id == item.Id).Select(x => x.Category.Name).ToList();
                   
                    //item.OfferStatus = db.Offer_details.Where(x => x.Proudct_id == item.Id).Select(x => x.Active).FirstOrDefault();
                    
                    item.OfferNames = db.Offer_details.Where(x => x.Proudct_id == item.Id&& item.OfferStatus == "true").Select(x => x.Offer.OfferName).ToList();
                    
                }
            }
           
            return pro;
        }
        public List<ProudctVM> FilterProudct(int category_id, int brand_id)
        {
            List<ProudctVM> proudct = new List<ProudctVM>();
            if (category_id != 0 && brand_id == 0)
            {
                proudct = db.Proudcut_Details.Where(x => x.Category_Id == category_id).Select(x => new ProudctVM { Id = x.Proudcut.Id, Name = x.Proudcut.Name, TotalPrice = x.Proudcut.TotalPrice, Image = x.Proudcut.Image, BrandName = x.Proudcut.Brand.Name, CategoryName = x.Category.Name, Category_Id = x.Category_Id, Brand_Id = x.Proudcut.Brand_Id, Price = x.Proudcut.Price, Discount_per = x.Proudcut.Discount_per, Discount_num = x.Proudcut.Discount_num, Show = x.Proudcut.Show, Description_E = x.Proudcut.Description_E, Description_A = x.Proudcut.Description_A, wherestatus = x.Proudcut.wherestatus }).ToList();
            }
            else if (category_id == 0 && brand_id != 0)
            {
                proudct = db.Proudcut_Details.Where(x => x.Proudcut.Brand_Id == brand_id).Select(x => new ProudctVM { Id = x.Id, Name = x.Proudcut.Name, TotalPrice = x.Proudcut.TotalPrice, Image = x.Proudcut.Image, BrandName = x.Proudcut.Brand.Name, CategoryName = x.Category.Name, Category_Id = x.Category_Id, Brand_Id = x.Proudcut.Brand_Id, Price = x.Proudcut.Price, Discount_per = x.Proudcut.Discount_per, Discount_num = x.Proudcut.Discount_num, Show = x.Proudcut.Show, Description_E = x.Proudcut.Description_E, Description_A = x.Proudcut.Description_A, wherestatus = x.Proudcut.wherestatus }).ToList();

            }
            else if (category_id != 0 && brand_id != 0)
            {
                proudct = db.Proudcut_Details.Where(x => x.Category_Id == category_id && x.Proudcut.Brand_Id == brand_id).Select(x => new ProudctVM { Id = x.Id, Name = x.Proudcut.Name, TotalPrice = x.Proudcut.TotalPrice, Image = x.Proudcut.Image, BrandName = x.Proudcut.Brand.Name, CategoryName = x.Category.Name, Category_Id = x.Category_Id, Brand_Id = x.Proudcut.Brand_Id, Price = x.Proudcut.Price, Discount_per = x.Proudcut.Discount_per, Discount_num = x.Proudcut.Discount_num, Show = x.Proudcut.Show, Description_E = x.Proudcut.Description_E, Description_A = x.Proudcut.Description_A, wherestatus = x.Proudcut.wherestatus }).ToList();

            }
            else if (category_id == 0 && brand_id == 0)
            {
                proudct = db.Proudcut_Details.Select(x => new ProudctVM { Id = x.Proudcut.Id, Name = x.Proudcut.Name, TotalPrice = x.Proudcut.TotalPrice, Image = x.Proudcut.Image, BrandName = x.Proudcut.Brand.Name, CategoryName = x.Category.Name, Category_Id = x.Category_Id, Brand_Id = x.Proudcut.Brand_Id, Price = x.Proudcut.Price, Discount_per = x.Proudcut.Discount_per, Discount_num = x.Proudcut.Discount_num, Show = x.Proudcut.Show, Description_E = x.Proudcut.Description_E, Description_A = x.Proudcut.Description_A, wherestatus = x.Proudcut.wherestatus }).ToList();

            }
            return proudct;
        }
        public bool DeletProudct(int Id)
        {
            try
            {
                var brand = db.Proudcuts.Find(Id);
                List<Proudcut_Details> pro = db.Proudcut_Details.Where(x=> x.Proudct_Id==Id).ToList();
                db.Proudcut_Details.RemoveRange(pro);
                db.Proudcuts.Remove(brand);
                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }
        public bool DeletProudcts(string ProudctsIDs)
        {
            try
            {
                string[] ids = ProudctsIDs.Split(new char[] { ',' });
                var proudcuts = new List<Proudcut>();
                var proudcuts2 = new List<Proudcut_Details>();
                foreach (var item in ids)
                {
                    int id = Convert.ToInt32(item);
                    var proudcut = db.Proudcuts.Where(x => x.Id == id).FirstOrDefault();
                    var detals = db.Proudcut_Details.Where(x => x.Proudct_Id == id).ToList();
                    proudcuts2.AddRange(detals);
                    proudcuts.Add(proudcut);
                }
                db.Proudcut_Details.RemoveRange(proudcuts2);
                db.Proudcuts.RemoveRange(proudcuts);
                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public ProudctVM getProudct(int id)
        {
            var pro = db.Proudcut_Details.Where(x => x.Proudcut.Id == id).Select(x => new ProudctVM { Id = x.Proudcut.Id, Name = x.Proudcut.Name, TotalPrice = Math.Ceiling((decimal)x.Proudcut.TotalPrice), Image = x.Proudcut.Image, BrandName = x.Proudcut.Brand.Name, Brand_Id = x.Proudcut.Brand_Id, Price = x.Proudcut.Price, Discount_per = x.Proudcut.Discount_per, Discount_num = x.Proudcut.Discount_num, Description_A = x.Proudcut.Description_A, Description_E = x.Proudcut.Description_E, wherestatus = x.Proudcut.wherestatus }).FirstOrDefault();
            pro.Category_Ids = db.Proudcut_Details.Where(x => x.Proudct_Id == id).Select(x => x.Category_Id).ToList();
            return pro;
        }
        public bool UpdateProudct(ProudctVM proudct, string Category_Ids)
        {
            Proudcut pro = new Proudcut();
            try
            {
                pro = db.Proudcuts.Find(proudct.Id);
                var proudcut_Details = db.Proudcut_Details.Where(x=> x.Proudct_Id== proudct.Id).ToList();
                pro.Name = proudct.Name;
                if (proudct.Image != "")
                {
                    pro.Image = proudct.Image;
                }
                pro.Price = proudct.Price;
                pro.Discount_num = (int)(((double)pro.Discount_per / 100) * (int)pro.Price);
                pro.TotalPrice = (decimal)(proudct.Price - (((decimal)pro.Discount_per / 100) * pro.Price));
                pro.Brand_Id = proudct.Brand_Id;
               
                pro.Description_A = proudct.Description_A;
                pro.Description_E = proudct.Description_E;
                pro.wherestatus = proudct.wherestatus == "-1" ? null : proudct.wherestatus;
               
                if (Category_Ids!=null)
                {
                    string[] ids = Category_Ids.Split(new char[] { ',' });
                    for(int i=0;i<ids.Length;i++)
                    {       
                        int id = Convert.ToInt32(ids[i]);
                        proudcut_Details[i].Category_Id = id;
                    }
                }
              
                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public bool AddDiscount(int Id, int? Discount_per, int? Discount_num)
        {
            try
            {
                var pro = db.Proudcuts.Find(Id);
                pro.Discount_per = Discount_per;
                pro.Discount_num = Discount_num;
                if (Discount_num == null)
                {
                    pro.TotalPrice = Math.Ceiling((decimal)(pro.Price - (((decimal)Discount_per / 100) * pro.Price)));
                    pro.Discount_num = (int)(((double)Discount_per / 100) * (int)pro.Price);
                }
                else if (Discount_per == null)
                {
                    pro.TotalPrice = pro.Price - Discount_num;
                    pro.Discount_per = (int)(((double)Discount_num / (int)pro.Price) * 100);
                }

                int res = db.SaveChanges();
                if (res == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;

            }

        }
        public bool AddDiscounts(string ProudctsIDs, int? Discount_per, int? Discount_num)
        {
            try
            {
                string[] ids = ProudctsIDs.Split(new char[] { ',' });
                var proudcuts = new List<Proudcut>();
                foreach (var item in ids)
                {
                    int id = Convert.ToInt32(item);
                    var pro = db.Proudcuts.Where(x => x.Id == id).FirstOrDefault();
                    pro.Discount_per = Discount_per;
                    pro.Discount_num = Discount_num;
                    if (Discount_num == null)
                    {
                        pro.TotalPrice = Math.Ceiling((decimal)(pro.Price - (((decimal)Discount_per / 100) * pro.Price)));
                        pro.Discount_num = (int)(((double)Discount_per / 100) * (int)pro.Price);
                    }
                    else if (Discount_per == null)
                    {
                        pro.TotalPrice = pro.Price - Discount_num;
                        pro.Discount_per = (int)(((double)Discount_num / (int)pro.Price) * 100);
                    }
                }
                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool AddOffer_proudct(int proudct_id, string offer_id)
        {
            try
            {
                string[] offerids = offer_id.Split(new char[] { ',' });
                if (offerids[0] == "0")
                {
                    var offer = db.Offer_details.Where(x => x.Proudct_id == proudct_id).ToList();
                    foreach (var item in offer)
                    {
                        item.Active = "false";
                    }
                    
                }
                else
                {
                    foreach (var item in offerids)
                    {
                        int id = Convert.ToInt32(item);
                        var offer = db.Offer_details.Where(x => x.Proudct_id == proudct_id && id == x.Offer_Id).FirstOrDefault();
                        if (offer == null)
                        {
                            Offer_details offer_Details = new Offer_details();
                            offer_Details.Proudct_id = proudct_id;
                            offer_Details.Offer_Id = id;
                            offer_Details.Active = "true";
                            db.Offer_details.Add(offer_Details);
                        }
                        else
                        {

                            offer.Active = "true";
                        }
                    }
                 
                   
                }

                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }
        public bool AddOffer_proudcts(string ProudctsIDs, int offer_id)
        {
            try
            {
                string[] ids = ProudctsIDs.Split(new char[] { ',' });
                foreach (var item in ids)
                {
                    int id = Convert.ToInt32(item);
                    if (offer_id == 0)
                    {
                        var offer = db.Offer_details.Where(x => x.Proudct_id == id).FirstOrDefault();
                        offer.Active = "false";
                    }
                    else
                    {
                        var offer= db.Offer_details.Where(x => x.Proudct_id == id&&offer_id==x.Offer_Id).FirstOrDefault();
                        if (offer == null)
                        {
                            Offer_details offer_Details = new Offer_details();
                            offer_Details.Proudct_id = id;
                            offer_Details.Offer_Id = offer_id;
                            offer_Details.Active = "true";
                            db.Offer_details.Add(offer_Details);
                        }
                        else
                        {
                            offer.Active = "true";
                        }
                        
                    }

                }

                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion
        #region Orders
        public List<OrderVM> LoadAllOrders()
        {
            var orders = db.Orders.Select(x => new OrderVM { Order_Id = x.Order_Id, ClientName = x.ClientName, Phone = x.Phone, E_Mail = x.E_Mail, Adress = x.Adress, Date = x.Date.ToString(), Status = x.Status }).OrderByDescending(x=> x.Date).ToList();
            return orders;
        }
        public bool ChangeOrderStatus(int Order_Id, int Status)
        {
            try
            {
                var order = db.Orders.Find(Order_Id);
                order.Status = Status == 1 ? OrderStatus.NewOrder.ToString() : Status == 2 ? OrderStatus.Connected.ToString() : OrderStatus.Finshed.ToString();
                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public List<ProudctVM> LoadOrderDetalis(int Order_Id)
        {
            var pro = db.Order_Details.Where(x => x.Order_Id == Order_Id&&x.Offer_Id==null).Select(x => new ProudctVM { Name = x.Proudcut.Name, Image = x.Proudcut.Image, Price = x.Proudcut.Price, Discount_per = x.Proudcut.Discount_per, TotalPrice = x.Proudcut.TotalPrice, Quantity = x.Quantity,OrderStatus=x.Order.Status,KindOfOrder="منتج",Description_A=x.Proudcut.Description_A}).ToList();
             pro.AddRange(db.Order_Details.Where(x => x.Order_Id == Order_Id && x.Proudct_Id == null).Select(x => new ProudctVM { Name = x.Offer.OfferName, Image = x.Offer.Ad.Image, Price = x.Offer.OfferPrice, Quantity = x.Quantity, OrderStatus = x.Order.Status, TotalPrice = x.Offer.OfferPrice,KindOfOrder="بوكس",Id=(int)x.Offer_Id }).ToList());

            return pro;
        }
        public List<ProudctVM> LoadOfferDetalis(int Offer_Id)
        {
            var pro = db.Offer_details.Where(x => x.Offer_Id == Offer_Id).Select(x => new ProudctVM { Name = x.Proudcut.Name, Description_A = x.Proudcut.Description_A, Image = x.Proudcut.Image }).ToList();
            return pro;
        }
        public bool DeleteOrder(int Order_Id)
        {
            try
            {
                var orderdetails = db.Order_Details.Where(x => x.Order_Id == Order_Id).ToList();
                var order = db.Orders.Where(x => x.Order_Id == Order_Id).FirstOrDefault();
                db.Order_Details.RemoveRange(orderdetails);
                db.Orders.Remove(order);
                int res = db.SaveChanges();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }



        }
        #endregion
        #region Offer
        public bool AddAds(List<AdVM> ads)
        {
            try
            {
                int res = 0;
                for (int i = 0; i < ads.Count; i++)
                {
                    Ad ad = new Ad();
                    int id = ads[i].Ad_Id;
                    ad = db.Ads.Where(x => x.Ad_Id == id).FirstOrDefault();
                    ad.Image = ads[i].Image;
                    res = db.SaveChanges();
                }
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public bool AddUpAds(List<AdVM> ads)
        {
            try
            {
                int res = 0;
                for (int i = 0; i < ads.Count; i++)
                {
                    Ad ad = new Ad();
                    int id = ads[i].Ad_Id;
                    ad = db.Ads.Where(x => x.Ad_Id == id).FirstOrDefault();
                    ad.Image = ads[i].Image;
                    res = db.SaveChanges();
                }
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public bool AddMidAds(List<AdVM> ads)
        {
            try
            {
                int res = 0;
                for (int i = 0; i < ads.Count; i++)
                {
                    Ad ad = new Ad();
                    int id = ads[i].Ad_Id;
                    ad = db.Ads.Where(x => x.Ad_Id == id).FirstOrDefault();
                    ad.Image = ads[i].Image;
                    res = db.SaveChanges();
                }
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public bool AddDownAds(List<AdVM> ads)
        {
            try
            {
                int res = 0;
                for (int i = 0; i < ads.Count; i++)
                {
                    Ad ad = new Ad();
                    int id = ads[i].Ad_Id;
                    ad = db.Ads.Where(x => x.Ad_Id == id).FirstOrDefault();
                    ad.Image = ads[i].Image;
                    res = db.SaveChanges();
                }
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public bool AddOffer(string Name, int Ad_Id, decimal price)
        {
            Offer offer = new Offer();
            offer.OfferName = Name;
            offer.Ads_id = Ad_Id;
            offer.OfferPrice = price;
            db.Offers.Add(offer);
            int res = db.SaveChanges();
            if (res == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<OfferVM> LoadAllOffer()
        {
            var offers = db.Offers.Select(x => new OfferVM { Id = x.Id, OfferName = x.OfferName, Image = x.Ad.Image, Ads_id = x.Ads_id ,OfferPrice=Math.Ceiling((decimal) x.OfferPrice)}).ToList();
            return offers;
        }
        public bool UpdateOffer(int id, string name, int Ad_Id, decimal price)
        {
            var offer = db.Offers.Find(id);
            offer.OfferName = name;
            offer.Ads_id = Ad_Id;
            offer.OfferPrice = price;
            int res = db.SaveChanges();
            if (res == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeletOffer(int id)
        {
            try
            {
                var offer = db.Offers.Find(id);
                db.Offers.Remove(offer);
                int res = db.SaveChanges();
                if (res == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }


        }
        #endregion

    }
}
