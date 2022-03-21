using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DLL;
using BLL;
using DLL.DataModel;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MVC_APP.Controllers
{

    public class AdminController : Controller
    {
        MedicalEntities db = new MedicalEntities();
        IAdminRepo admin = new Admin();
        IUsersRepo user = new Users();
        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LoginAdmin(string Name, string Password)
        {
            var res = admin.LoginAdmin(Name, Password);
            if (res != null)
            {
                Session["Admin"] = res;
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            ViewBag.CountOrder = db.Orders.Count();
            ViewBag.CountProudct = db.Proudcuts.Count();
            return View();
        }
        public ActionResult ChangeShowStatus(int Id, int ShowStatus, string from)
        {
            bool res = admin.ChangeShowStatus(Id, ShowStatus, from);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeShowStatus_All(string Ids, int ShowStatus, string from)
        {
            bool res = admin.ChangeShowStatus_All(Ids, ShowStatus, from);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Media()
        {
            var res = admin.Media();
            return View(res);
        }
        public ActionResult EditMedia(MediaVM media)
        {
            bool res = admin.EditMedia(media);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #region Category
        public ActionResult Category()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(string Name)
        {
            var check = 1;
            try
            {
                bool res = false;
                
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    check = 2;
                    var pic = System.Web.HttpContext.Current.Request.Files["imageUpload"];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                    var fileName = Path.GetFileName(filebase.FileName);
                    var fileUrl = Server.MapPath("~/Image/");
                    string extension = Path.GetExtension(filebase.FileName);
                    string newFileName = Guid.NewGuid() + extension;
                    var path = Path.Combine(fileUrl, newFileName);
                    filebase.SaveAs(path);
                    res = admin.AddCategory(Name, newFileName);
                }

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                return Json(ex.Message, JsonRequestBehavior.AllowGet); ;
            }
           
        }
        public ActionResult LoadAllCategory()
        {
            var cat = admin.LoadAllCategory();
            return Json(cat, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateCatgory(int Id, string Name, int imagecha)
        {
            bool res = false;
            if (imagecha == 1)
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["imageUpload"];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                    var fileName = Path.GetFileName(filebase.FileName);
                    var fileUrl = Server.MapPath("/Image/");
                    string extension = Path.GetExtension(filebase.FileName);
                    string newFileName = Guid.NewGuid() + extension;
                    var path = Path.Combine(fileUrl, newFileName);
                    filebase.SaveAs(path);
                    res = admin.UpdateCatgory(Id, Name, newFileName);
                }
            }
            else
            {
                res = admin.UpdateCatgory(Id, Name, "");
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeletCategory(int Id)
        {
            bool res = admin.DeletCategory(Id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeletCategorys(string categoryIDs)
        {
            bool res = admin.DeletCategorys(categoryIDs);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Brand
        public ActionResult Brand()
        {
            return View();
        }
        public ActionResult AddBrand(string Name)
        {
            bool res = false;
            string newFileName = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["imageUpload"];
                HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                var fileName = Path.GetFileName(filebase.FileName);
                var fileUrl = Server.MapPath("/Image/");
                string extension = Path.GetExtension(filebase.FileName);
                newFileName = Guid.NewGuid() + extension;
                var path = Path.Combine(fileUrl, newFileName);
                filebase.SaveAs(path);

            }
            res = admin.AddBrand(Name, newFileName);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadAllBrand()
        {
            var brand = admin.LoadAllBrand();
            return Json(brand, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateBrand(int Id, string Name, int imagecha)
        {
            bool res = false;
            if (imagecha == 1)
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["imageUpload"];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                    var fileName = Path.GetFileName(filebase.FileName);
                    var fileUrl = Server.MapPath("/Image/");
                    string extension = Path.GetExtension(filebase.FileName);
                    string newFileName = Guid.NewGuid() + extension;
                    var path = Path.Combine(fileUrl, newFileName);
                    filebase.SaveAs(path);
                    res = admin.UpdateBrand(Id, Name, newFileName);
                }
            }
            else
            {
                res = admin.UpdateBrand(Id, Name, "");
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeletBrand(int Id)
        {
            bool res = admin.DeletBrand(Id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Proudct
        public ActionResult Proudcts()
        {
            ViewBag.Category = db.Categories.Select(x => new CategoryVM { Id = x.Id, Name = x.Name }).ToList();
            ViewBag.Brand = db.Brands.Select(x => new BrandVM { Id = x.Id, Name = x.Name }).ToList();
            return View();
        }
        public ActionResult EditProudct(int? id)
        {
            ViewBag.Category = db.Categories.Select(x => new CategoryVM { Id = x.Id, Name = x.Name }).ToList();
            ViewBag.Brand = db.Brands.Select(x => new BrandVM { Id = x.Id, Name = x.Name }).ToList();
          
            var pro = admin.getProudct((int)id);
            return View(pro);
        }
        public ActionResult AddProudct(ProudctVM proudct,string Category_Ids)
        {
            bool res = false;
            string newFileName = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["imageUpload"];
                HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                var fileName = Path.GetFileName(filebase.FileName);
                var fileUrl = Server.MapPath("/Image/");
                string extension = Path.GetExtension(filebase.FileName);
                newFileName = Guid.NewGuid() + extension;
                var path = Path.Combine(fileUrl, newFileName);
                filebase.SaveAs(path);
                proudct.Image = newFileName;

            }
            res = admin.AddProudct(proudct , Category_Ids);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MangeProudct()
        {
            ViewBag.Category = db.Categories.Select(x => new CategoryVM { Id = x.Id, Name = x.Name }).ToList();
            ViewBag.Brand = db.Brands.Select(x => new BrandVM { Id = x.Id, Name = x.Name }).ToList();
            return View();
        }
        public ActionResult LoadAllProudct(int pageNumber = 1, int pageSize = 12)
        {
            var pro = admin.LoadAllProudct();
            var pagedData = Pagination.PagedResult(pro, pageNumber, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadAllProudctSearch(string search,int pageNumber = 1, int pageSize = 12)
        {
            var pro = admin.LoadAllProudctSearch(search);
            var pagedData = Pagination.PagedResult(pro, pageNumber, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FilterProudct(int category_id, int brand_id)
        {
            var pro = admin.FilterProudct(category_id, brand_id);
            return Json(pro, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeletProudct(int Id)
        {
            bool res = admin.DeletProudct(Id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeletProudcts(string ProudctsIDs)
        {
            bool res = admin.DeletProudcts(ProudctsIDs);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateProudct(ProudctVM proudct, int imagecha, string Category_Ids)
        {
            bool res = false;
            if (imagecha == 1)
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["imageUpload"];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                    var fileName = Path.GetFileName(filebase.FileName);
                    var fileUrl = Server.MapPath("/Image/");
                    string extension = Path.GetExtension(filebase.FileName);
                    string newFileName = Guid.NewGuid() + extension;
                    var path = Path.Combine(fileUrl, newFileName);
                    filebase.SaveAs(path);
                    proudct.Image = newFileName;
                    res = admin.UpdateProudct(proudct,Category_Ids);
                }
            }
            else
            {
                proudct.Image = "";
                res = admin.UpdateProudct(proudct,Category_Ids);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddDiscount(int Id, int? Discount_per,int? Discount_num)
        {
            bool res = admin.AddDiscount(Id, Discount_per,Discount_num);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddDiscounts(string ProudctsIDs, int? Discount_per, int? Discount_num)
        {
            bool res = admin.AddDiscounts(ProudctsIDs, Discount_per, Discount_num);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOffer_proudct(int proudct_id, string offer_id)
        {
            bool res = admin.AddOffer_proudct(proudct_id,offer_id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
       public ActionResult AddOffer_proudcts(string ProudctsIDs, int offer_id)
        {
            bool res = admin.AddOffer_proudcts(ProudctsIDs, offer_id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Offer
        public ActionResult Ads()
        {
            return View();
        }
        public ActionResult AddAds()
        {
            bool res = false;
            List<AdVM> ads = new List<AdVM>();

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic1 = System.Web.HttpContext.Current.Request.Files["imageUpload1"];
                if (pic1 != null)
                {
                    HttpPostedFileBase filebase1 = new HttpPostedFileWrapper(pic1);
                    ads.Add(SaveImgPath(filebase1, 1));
                }

                var pic2 = System.Web.HttpContext.Current.Request.Files["imageUpload2"];
                if (pic2 != null)
                {
                    HttpPostedFileBase filebase2 = new HttpPostedFileWrapper(pic2);
                    ads.Add(SaveImgPath(filebase2, 2));
                }

                var pic3 = System.Web.HttpContext.Current.Request.Files["imageUpload3"];
                if (pic3 != null)
                {
                    HttpPostedFileBase filebase3 = new HttpPostedFileWrapper(pic3);
                    ads.Add(SaveImgPath(filebase3, 3));
                }

                var pic4 = System.Web.HttpContext.Current.Request.Files["imageUpload4"];
                if (pic4 != null)
                {
                    HttpPostedFileBase filebase4 = new HttpPostedFileWrapper(pic4);
                    ads.Add(SaveImgPath(filebase4, 4));
                }

                var pic5 = System.Web.HttpContext.Current.Request.Files["imageUpload5"];
                if (pic5 != null)
                {
                    HttpPostedFileBase filebase5 = new HttpPostedFileWrapper(pic5);
                    ads.Add(SaveImgPath(filebase5, 5));
                }

                res = admin.AddAds(ads);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        AdVM SaveImgPath(HttpPostedFileBase filebase, int id)
        {
            var fileName = Path.GetFileName(filebase.FileName);
            var fileUrl = Server.MapPath("/Image/");
            string extension = Path.GetExtension(filebase.FileName);
            string newFileName = Guid.NewGuid() + extension;
            var path = Path.Combine(fileUrl, newFileName);
            filebase.SaveAs(path);
            AdVM ad = new AdVM();
            ad.Image = newFileName;
            ad.Ad_Id = id;
            return ad;
        }
        public ActionResult AddUpAds()
        {
            bool res = false;
            List<AdVM> ads = new List<AdVM>();
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic1 = System.Web.HttpContext.Current.Request.Files["adsImgUp1"];
                if (pic1 != null)
                {
                    HttpPostedFileBase filebase1 = new HttpPostedFileWrapper(pic1);
                    ads.Add(SaveImgPath(filebase1, 6));
                }

                var pic2 = System.Web.HttpContext.Current.Request.Files["adsImgUp2"];
                if (pic2 != null)
                {
                    HttpPostedFileBase filebase2 = new HttpPostedFileWrapper(pic2);
                    ads.Add(SaveImgPath(filebase2, 7));
                }
                res = admin.AddUpAds(ads);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddMidAds()
        {
            bool res = false;
            List<AdVM> ads = new List<AdVM>();
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic1 = System.Web.HttpContext.Current.Request.Files["adsImgMid1"];
                if (pic1 != null)
                {
                    HttpPostedFileBase filebase1 = new HttpPostedFileWrapper(pic1);
                    ads.Add(SaveImgPath(filebase1, 8));
                }

                var pic2 = System.Web.HttpContext.Current.Request.Files["adsImgMid2"];
                if (pic2 != null)
                {
                    HttpPostedFileBase filebase2 = new HttpPostedFileWrapper(pic2);
                    ads.Add(SaveImgPath(filebase2, 9));
                }
                res = admin.AddMidAds(ads);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddDownAds()
        {
            bool res = false;
            List<AdVM> ads = new List<AdVM>();
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic1 = System.Web.HttpContext.Current.Request.Files["adsImgDown1"];
                if (pic1 != null)
                {
                    HttpPostedFileBase filebase1 = new HttpPostedFileWrapper(pic1);
                    ads.Add(SaveImgPath(filebase1, 10));
                }

                var pic2 = System.Web.HttpContext.Current.Request.Files["adsImgDown2"];
                if (pic2 != null)
                {
                    HttpPostedFileBase filebase2 = new HttpPostedFileWrapper(pic2);
                    ads.Add(SaveImgPath(filebase2, 11));
                }
                res = admin.AddDownAds(ads);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Offer()
        {
            ViewBag.ads = db.Ads.Select(x => new AdVM { Ad_Id = x.Ad_Id, Name = x.Name }).ToList();
            return View();
        }
        public ActionResult AddOffer(string Name, int Ad_Id,decimal price)
        {
            bool res = admin.AddOffer(Name, Ad_Id,price);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadAllOffer()
        {
            var brand = admin.LoadAllOffer();
            return Json(brand, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateOffer(int Id, string Name, int Ad_Id, decimal price)
        {

            bool res = admin.UpdateOffer(Id, Name, Ad_Id,price);

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeletOffer(int Id)
        {
            bool res = admin.DeletOffer(Id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Order
        public ActionResult Orders()
        {
            return View();
        }
        public ActionResult LoadAllOrders()
        {
            var res = admin.LoadAllOrders();
            return Json(res, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ChangeOrderStatus(int Order_Id, int Status)
        {
            var res = admin.ChangeOrderStatus(Order_Id, Status);
            int ordernum = db.Orders.Where(x => x.Status == OrderStatus.NewOrder.ToString()).Count();
            object obj = new { res, ordernum };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadOrderDetalis(int Order_Id)
        {
            var res = admin.LoadOrderDetalis(Order_Id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadOfferDetalis(int Offer_Id)
        {
            var res = admin.LoadOfferDetalis(Offer_Id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteOrder(int Order_Id)
        {
            var res = admin.DeleteOrder(Order_Id);

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region SMS
        public ActionResult SMS()
        {
            return View();
        }
        public ActionResult SendSMS(string phone ,string msg)
        {
            bool check = true;
            try
            {
                string[] phones = phone.Split(new char[] { ',' });

                if (phones!=null)
                {

                    TwilioClient.Init("AC05d1c07c0f1de9bf2dd0853a939f25c5", "0ad82b48d8be35b066212c994e99ec58");

                    foreach (var item in phones)
                    {
                        var message = MessageResource.Create(
                        body: msg,
                        from: "Easydose",
                        to: new Twilio.Types.PhoneNumber("+2" + item)
                    );
                    }
                    
                    //if (message.ErrorMessage == null)
                    //{
                    //    check = true;
                    //}
                    //else
                    //{
                    //    check = false;
                    //}
                }
                else
                {
                    check = false;
                }
               
            }
            catch (Exception ex)
            {

                check = false;
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}