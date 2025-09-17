using GadgetHub_Server.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GadgetHub_Customer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Index(Customers Customers)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(Customers), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://localhost:7729/api/customers/Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

                        // Set success message in TempData
                        TempData["SuccessMessage"] = "Login successful! Welcome to the Customer Dashboard.";
                        return RedirectToAction("Home");

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid email or password.";
                    }
                }
            }
            return View(Customers);
        }

        public async Task<ActionResult> AddCustomer()
        {
            return View();
        }

        // POST: AddCustomer
        [HttpPost]
        public async Task<ActionResult> AddCustomer(Customers pr)
        {
            if (!ModelState.IsValid)
            {
                return View(pr);
            }

            // Get the next CustomerID
            int nextCustomerID;
            using (var httpClient = new HttpClient())
            {

                var response = await httpClient.GetAsync("http://localhost:7729/api/customers");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var Customers = JsonConvert.DeserializeObject<List<Customers>>(apiResponse);
                    nextCustomerID = Customers.Any() ? Customers.Max(p => p.CustomerID) + 1 : 1;
                }
                else
                {

                    ModelState.AddModelError("", "Unable to retrieve Customers.");
                    return View(pr);
                }
            }


            pr.CustomerID = nextCustomerID;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(pr), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:7729/api/customers", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Customer Registerd successfully!";
                        return RedirectToAction("Index"); // Redirect to a suitable action after successful addition
                    }
                    else
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", "Error adding the Customer: " + apiResponse);
                    }
                }
            }

            return View(pr); // Return the view with the current model if there was an error
        }

                  ////////////////////////////////////////////Get Products///////////////////////////////////////////////////////////

        public async Task<ActionResult> GetProduct()
        {
            List<Products> Products = new List<Products>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:7729/api/Products");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    Products = JsonConvert.DeserializeObject<List<Products>>(data);
                    // Filter products where Status is "Confirmed"
                    Products = Products.Where(p => p.Status == "Confirmed").ToList();
                }
            }

            return View(Products);
        }

        /////////////////////////////////////////////////////////////////////////////////////get Orders///////////////////////////////////////////////////////////////


        public async Task<ActionResult> GetOrder()
        {
            List<Orders> Orders = new List<Orders>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:7729/api/Orders");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    Orders = JsonConvert.DeserializeObject<List<Orders>>(data);
                }
            }

            return View(Orders);
        }

        /////////////////////////////////////////////////////////////////////add Orders////////////////////////////////////////////////////////////////////////////////////
        public async Task<ActionResult> AddOrder()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> AddOrder(Orders pr)
        {
            Orders p = new Orders();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(pr), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:7729/api/Orders", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode) // Check if the request was successful
                    {
                        JsonConvert.DeserializeObject<Orders>(apiResponse);
                        TempData["SuccessMessage"] = "Orders added successfully!"; // Set success message
                        return RedirectToAction("GetOrder"); // Redirect to admin list page or another view
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to add Orders. Please try again."; // Set error message
                    }
                }
            }
            return View(p);
        }

        ///////////////////////////////////////////////////////////////////update Orders////////////////////////////////////////////////////////////////////

        public async Task<ActionResult> UpdateOrder(string id)
        {
            Orders qr = new Orders();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync($"http://localhost:7729/api/Orders/{id}"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            qr = JsonConvert.DeserializeObject<Orders>(apiResponse);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error fetching Orders details.");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Request error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            return View(qr);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateOrder(Orders pr)
        {
            if (!ModelState.IsValid)
            {
                return View(pr);
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(pr), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync($"http://localhost:7729/api/Orders/{pr.OrderID}", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Orders updated successfully!";
                            return RedirectToAction("GetOrder");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Error updating the drug. Please try again.";
                            ModelState.AddModelError(string.Empty, "Error updating the Orders.");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Request error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            return View(pr);
        }


        ///////////////////////////////////////////////////////Delete Orders////////////////////////////////////////////////////
        [HttpGet]
        public async Task<ActionResult> DeleteOrder(string id)
        {
            Orders p = new Orders();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.DeleteAsync($"http://localhost:7729/api/Orders/{id}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                p = JsonConvert.DeserializeObject<Orders>(apiResponse);

                if (response.IsSuccessStatusCode)
                {

                    TempData["SuccessMessage"] = "Orders deleted successfully!";

                }
                else
                {
                    TempData["ErrorMessage"] = "Error deleting the Orders. Please try again.";
                    ModelState.AddModelError(string.Empty, "Error deleting the Orders.");
                }

                return RedirectToAction("GetOrder");
            }
        }

        ////////////////////////////////////////////////////////////////////get OrderRequest///////////////////////////////////////////////////////////////


        public async Task<ActionResult> GetOrderRequest()
        {
            List<OrderRequests> OrderRequests = new List<OrderRequests>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:7729/api/OrderRequests");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    OrderRequests = JsonConvert.DeserializeObject<List<OrderRequests>>(data);
                }
            }

            return View(OrderRequests);
        }
    }
}