using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PerksDashboard;
using PerksDashboard.Models.src.dao;
using System.Configuration;
using System.Data.SqlClient;
using PerksDashboard.Models.src.dto;

namespace PerksDashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult LoadData(List<ActivityDto> model)
        {

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            Int32 recordsTotal = 0;

            List<ActivityDto> data = new List<ActivityDto>();
            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["PerksDb"].ConnectionString))
            {
                recordsTotal = Activity.getActivityCount(con1);
            }
            using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["PerksDb"].ConnectionString))
            {
                data = Activity.getAllActivities(con1, Int32.Parse(start), Int32.Parse(length));
            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }
    }

}