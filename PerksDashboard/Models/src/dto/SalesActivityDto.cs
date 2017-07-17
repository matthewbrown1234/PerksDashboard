using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerksDashboard.Models.src.dto
{
    public class SalesActivityDto: ActivityDto
    {
        Int32 _salesId;
        string _invoiceId;

        public Int32 SalesId
        {
            get { return _salesId; }
            set { _salesId = value; }
        }
        public string InvoiceId
        {
            get { return _invoiceId; }
            set { _invoiceId = value; }
        }
    }
}