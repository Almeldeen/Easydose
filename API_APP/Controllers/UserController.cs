using BLL;
using DLL;
using DLL.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_APP.Controllers
{
    public class UserController : ApiController
    {
        IUsersRepo users = new Users();
        IAdminRepo admin = new Admin();
        MedicalEntities db = new MedicalEntities();
        [HttpGet]
        public IHttpActionResult Index()
        {
            try
            {
                var all = users.GetALL();
                return Ok(all);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet]
        public IHttpActionResult GetSearchValue(string search)
        {
            List<ProudctVM> allsearch = db.SearchResult(search).Take(10).Select(x => new ProudctVM { Id = x.Id, Name = x.Name }).ToList();
            return Ok(allsearch);
        }
        [HttpGet]
        public IHttpActionResult ProudctDetails(int proudct_id)
        {
            var res = admin.getProudct(proudct_id);
            return Ok(res);
        }
        [HttpGet]
        public IHttpActionResult GetProudctsData(int? Id, string name, string category, string source, decimal? fromPrice, decimal? toPrice, int? frompage, string search, int pageNumber = 1, int pageSize = 12)
        {
            var res = users.ViewProudct(Id, name, category, source, fromPrice, toPrice, frompage, search);
            var pagedData = Pagination.PagedResult(res, pageNumber, pageSize);
            return Ok(pagedData);
        }
        [HttpGet]
        public IHttpActionResult getproudct_Cart(string ProudctIds)
        {
            var rse = users.getproudct_Cart(ProudctIds);
            return Ok(rse);
        }
        [HttpPost]
        public IHttpActionResult AddOrder(string ClientName, string Phone, string E_Mail, string Adress, string ProudctIds, string Quantity)
        {
            OrderVM order = new OrderVM();
            order.ClientName = ClientName;
            order.Phone = Phone;
            order.Adress = E_Mail;
            order.E_Mail = Adress;
            order.ProudctIds = ProudctIds;
            order.Quantity = Quantity;
            Help res = users.AddOrder(order);
            return Ok(res);
        }
        [HttpGet]
        public IHttpActionResult getProudct_Dis()
        {
            var res = users.getProudct_Dis();
            return Ok(res);
        }
        [HttpGet]
        public IHttpActionResult getProudct_Best()
        {
            var res = users.getProudct_Best();
            return Ok(res);
        }
    }
}
