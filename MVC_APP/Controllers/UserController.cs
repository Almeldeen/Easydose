using BLL;
using DLL;
using DLL.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MVC_APP.Controllers
{
    public class UserController : Controller
    {
        IUsersRepo users = new Users();
        IAdminRepo admin = new Admin();
        MedicalEntities db = new MedicalEntities();
        // GET: User
        public ActionResult Index()
        {
            var all = users.GetALL();
            return View(all);
        }
       
        public JsonResult GetSearchValue(string search)
        {         
            List<ProudctVM> allsearch = db.SearchResult(search).Take(10).Select(x => new ProudctVM{ Id = x.Id,   Name = x.Name}).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetSearchValueAddd(string search)
        {
            List<ProudctVM> allsearch = db.SearchResult(search).Where(x=> x.Show== "Visable").Take(10).Select(x => new ProudctVM { Id = x.Id, Name = x.Name }).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult ViewProudct(int? Id, int? frompage)
        {
            if (Id != null)
            {
                ViewBag.Name = frompage == (int)FromPage.Category ? db.Categories.Where(x => x.Id == Id).Select(x => x.Name).FirstOrDefault() : frompage == (int)FromPage.Category ? db.Brands.Where(x => x.Id == Id).Select(x => x.Name).FirstOrDefault() : null;
                ViewBag.id = Id;
                ViewBag.frompage = frompage;
            }
            else if (frompage == (int)FromPage.best)
            {
                ViewBag.id = null;
                ViewBag.frompage = (int)FromPage.best;
            }
            else if (frompage == (int)FromPage.dis)
            {
                ViewBag.id = null;
                ViewBag.frompage = (int)FromPage.dis;
            }
            else
            {
                ViewBag.id = null;
                ViewBag.frompage = null;
            }

            return View();
        }
        public ActionResult ProudctDetails(int proudct_id)
        {
            var res = admin.getProudct(proudct_id);
            return View(res);
        }
        public ActionResult ViewOffer(int OfferId,string OfferName)
        {
            ViewBag.id = OfferId;
            ViewBag.Name = OfferName;
            ViewBag.disprice =Math.Ceiling((decimal)db.Offers.Where(x => x.Id == OfferId).Select(x => x.OfferPrice).FirstOrDefault());
            ViewBag.price =Math.Ceiling((decimal) db.Offer_details.Where(x => x.Offer_Id == OfferId&&x.Active== "true").Select(x => x.Proudcut.TotalPrice).Sum());
            ViewBag.dispercent =Math.Ceiling(100-((ViewBag.disprice / ViewBag.price) * 100));
            return View();
        }
        public ActionResult SearchResult(string search)
        {
            ViewBag.serach = search;
            return View();
        }
        public ActionResult GetProudctsData(int? Id,string name, string category, string source, decimal? fromPrice, decimal? toPrice, int? frompage,string search, int pageNumber = 1, int pageSize = 12)
        {
            var res = users.ViewProudct(Id,name,category,source,fromPrice,toPrice, frompage,search);
            var pagedData = Pagination.PagedResult(res, pageNumber, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Carts()
        {
          
            return View();
        }
        public ActionResult getproudct_Cart(string ProudctIds)
        {
            var rse = users.getproudct_Cart(ProudctIds);
            return Json(rse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getproudct_Cart_Offer(string OfferIds)
        {
            var rse = users.getproudct_Cart_Offer(OfferIds);
            return Json(rse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuyForm()
        {
            return View();
        }
        public ActionResult AddOrder(OrderVM order)
        {
            var res = users.AddOrder(order);
            if (res.res)
            {
                TwilioClient.Init("AC05d1c07c0f1de9bf2dd0853a939f25c5", "0ad82b48d8be35b066212c994e99ec58");

                var message = MessageResource.Create(
                    body: " عزيزي " + order.ClientName + "\n تم تأكيد طلبك رقم " + res.id + " سوف يتم التوصيل ف الميعاد\n"
                   + "www.easydosee.com",
                    from: "Easydose",
                    to: new Twilio.Types.PhoneNumber("+2"+order.Phone)
                );
                MailMessage mail = new MailMessage();

                //set the addresses 
                mail.From = new MailAddress("Sales@easydosee.com", "EasyDose"); //IMPORTANT: This must be same as your smtp authentication address.
                mail.To.Add(order.E_Mail);

                //set the content 
                
                mail.Subject = "New Order Confirmation";
                order.Order_Id = res.id;
                mail.Body = CreateBody(order);
                mail.IsBodyHtml = true;
                //mail.Body = "This is from system.net.mail using C sharp with smtp authentication.";
                //send the message 
                SmtpClient smtp = new SmtpClient();

                //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                NetworkCredential Credentials = new NetworkCredential("Sales@easydosee.com", "Sales@1234");
                smtp.Host = "mail.easydosee.com";
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = Credentials;
                smtp.Port = 587;    //alternative port number is 8889
                smtp.EnableSsl = false;
                smtp.Timeout = 100000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = UTF8Encoding.UTF8;
                smtp.Send(mail);
               
            }
            return Json(res.res,JsonRequestBehavior.AllowGet);
        }
        private string CreateBody(OrderVM order)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Emailtemplat.html")))
            {

                body = reader.ReadToEnd();

            }

            body = body.Replace("{name}", order.ClientName); //replacing Parameters

            body = body.Replace("{id}",order.Order_Id.ToString());

            body = body.Replace("{adress}", order.Adress);
            body = body.Replace("{phone}", order.Phone);
            string items = string.Empty;
            int total = 0;
            if (order.ProudctIds != null)
            {
                string[] ids = order.ProudctIds.Split(new char[] { ',' });
                foreach (var item in ids)
                {
                    int id = Convert.ToInt32(item);
                    var pro = db.Order_Details.Where(x => x.Proudcut.Id == id && x.Proudcut.Show == "Visable").Select(x => new ProudctVM { Name = x.Proudcut.Name, TotalPrice = Math.Ceiling((decimal)x.Proudcut.TotalPrice) }).FirstOrDefault();
                    total += (int)pro.TotalPrice;
                    items += $"<tr><td width = '75%' align = 'left' style = 'font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding: 15px 10px 5px 10px;'> {pro.Name} </td >" +
               $"<td width = '25%' align = 'left' style = 'font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding: 15px 10px 5px 10px;' > {pro.TotalPrice} </td ></tr>";
                }
            }
            if (order.OfferIds != null)
            {
                string[] ids2 = order.OfferIds.Split(new char[] { ',' });
                foreach (var item in ids2)
                {
                    int id = Convert.ToInt32(item);
                    var pro = db.Order_Details.Where(x => x.Offer_Id == id).Select(x => new ProudctVM { Name = x.Offer.OfferName, TotalPrice = Math.Ceiling((decimal)x.Offer.OfferPrice) }).FirstOrDefault();
                    total += (int)pro.TotalPrice;
                    items += $"<tr><td width = '75%' align = 'left' style = 'font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding: 15px 10px 5px 10px;'> {pro.Name} </td >" +
               $"<td width = '25%' align = 'left' style = 'font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding: 15px 10px 5px 10px;' > {pro.TotalPrice} </td ></tr>";
                }
            }
            body = body.Replace("{item}",items);
            body = body.Replace("{Total}", total.ToString());

            return body;

        }
        public ActionResult getProudct_Dis()
        {
            var res = users.getProudct_Dis();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getProudct_Best()
        {
            var res = users.getProudct_Best();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        //sent feedback to your email
        public JsonResult EmailUser(string name,string phone,string email, string feedback)
        {
            bool result = false;
            if (email==""||email==null)
            {
                result = SendEmail("info@easydosee.com", "Hello, My name is " + name + ",my phone is " + phone + " and my massage is- ", "<p> " + feedback + " </p>");

            }
            else
            {
                result = SendEmail("info@easydosee.com", "Hello, My name is " + name + ",my phone is " + phone + ", my E-Mail is "+email+" and my massage is- ", "<p> " + feedback + " </p>");

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public bool SendEmail(string toemail, string subje, string emailbody)
        {
            try
            {
                string senderemail = System.Configuration.ConfigurationManager.AppSettings["senderemail"].ToString();
                string senderpassword = System.Configuration.ConfigurationManager.AppSettings["senderpassword"].ToString();
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(senderemail, senderpassword);
                MailMessage mailMessage = new MailMessage(senderemail, toemail, subje, emailbody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public ActionResult emailtest()
        {
            return View();
        }

    }
}