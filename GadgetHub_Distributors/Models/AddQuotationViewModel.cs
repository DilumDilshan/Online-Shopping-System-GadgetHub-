using GadgetHub_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GadgetHub_Distributors.Models
{
    public class AddQuotationViewModel
    {
        public int QuotationID { get; set; }
        public string productName { get; set; } // Selected drug name from dropdown
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public List<Products> Products { get; set; } // List of products for the dropdown
    }
}
