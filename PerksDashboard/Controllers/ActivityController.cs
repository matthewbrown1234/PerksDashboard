using PerksDashboard.Models.src.dao;
using PerksDashboard.Models.src.dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PerksDashboard.Controllers
{
    public class ActivityController : ApiController
    {
        public HttpResponseMessage getActivityDetails(Int32 activityId, char activityType)
        {
            if (activityType.Equals('s'))
            {
                SalesActivityDto salesActivity = new SalesActivityDto();
                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["PerksDb"].ConnectionString))
                {
                    salesActivity = Activity.getSalesActivity(con1, activityId);
                }
                return Request.CreateResponse(HttpStatusCode.OK, salesActivity);
            }
            if (activityType.Equals('r'))
            {
                RecognitionActivityDto recognitionActivity = new RecognitionActivityDto();
                using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["PerksDb"].ConnectionString))
                {
                    recognitionActivity = Activity.getRecognitionActivity(con1, activityId);
                }
                return Request.CreateResponse(HttpStatusCode.OK, recognitionActivity);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);

        }
    }
}
