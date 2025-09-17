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

namespace GadgetHub_Admin.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult AdminDashboard()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(Admins Admins)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(Admins), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://localhost:7729/api/admins/Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

                        // Set success message in TempData
                        TempData["SuccessMessage"] = "Login successful! Welcome to the Admin Dashboard.";

                        // Redirect based on user role

                        return RedirectToAction("AdminDashboard");

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid email or password.";
                    }
                }
            }
            return View(Admins);
        }

        ////////////////////////////////////////////////////////////////////get admin///////////////////////////////////////////////////////////////


        public async Task<ActionResult> GetAdmin()
        {
            List<Admins> Admins = new List<Admins>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:7729/api/Admins");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    Admins = JsonConvert.DeserializeObject<List<Admins>>(data);
                }
            }

            return View(Admins);
        }

        //////////////////////////////////////////////////////////////////add admin////////////////////////////////////////////////////////////////////////////////////
        public async Task<ActionResult> AddAdmin()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> AddAdmin(Admins pr)
        {
            Admins p = new Admins();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(pr), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:7729/api/Admins", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode) // Check if the request was successful
                    {
                        JsonConvert.DeserializeObject<Admins>(apiResponse);
                        TempData["SuccessMessage"] = "Admin added successfully!"; // Set success message
                        return RedirectToAction("GetAdmin"); // Redirect to admin list page or another view
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to add admin. Please try again."; // Set error message
                    }
                }
            }
            return View(p);
        }

        
        //////////////////////////////////////////////////////////////Update Admin/////////////////////////////////////////////
        

        public async Task<ActionResult> UpdateAdmin(string id)
        {
            Admins qr = new Admins();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync($"http://localhost:7729/api/Admins/{id}"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            qr = JsonConvert.DeserializeObject<Admins>(apiResponse);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error fetching Admin details.");
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
        public async Task<ActionResult> UpdateAdmin(Admins pr)
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
                    using (var response = await httpClient.PutAsync($"http://localhost:7729/api/Admins/{pr.AdminID}", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Admin updated successfully!";
                            return RedirectToAction("GetAdmin");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Error updating the admin. Please try again.";
                            ModelState.AddModelError(string.Empty, "Error updating the Admin.");
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

     /////////////////////////////////////////////////////////////Delete Admin////////////////////////////////////////////////////////////
       

        [HttpGet]
        public async Task<ActionResult> DeleteAdmin(string id)
        {
            Admins p = new Admins();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.DeleteAsync($"http://localhost:7729/api/Admins/{id}"))
            {
                if (response.IsSuccessStatusCode) // Check if deletion was successful
                {
                    TempData["SuccessMessage"] = "Admin deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete admin. Please try again.";
                }
                
            }
            return RedirectToAction("GetAdmin");
        }

        //////////////////////////////////////////////get Distributors////////////////////////////////////////////////////////////////

        public async Task<ActionResult> GetDistributors()
        {
            List<Distributors> Distributors = new List<Distributors>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:7729/api/Distributors");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    Distributors = JsonConvert.DeserializeObject<List<Distributors>>(data);
                }
            }

            return View(Distributors);

        }

        ////////////////////////////////////////////////////////////add Distributors /////////////////////////////////////////////////////////////////////////////

        public async Task<ActionResult> AddDistributor()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> AddDistributor(Distributors pr)
        {
            Distributors p = new Distributors();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(pr), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:7729/api/Distributors", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Distributor added successfully!";
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        JsonConvert.DeserializeObject<Distributors>(apiResponse);

                        return RedirectToAction("GetDistributors");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to add Distributor. Please try again.";
                    }
                }
            }
            return View(p);
        }

        ////////////////////////////////////////////////// edit Distributor ///////////////////////////////////////////////


        public async Task<ActionResult> UpdateDistributor(string id)
        {
            Distributors qr = new Distributors();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync($"http://localhost:7729/api/Distributors/{id}"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            qr = JsonConvert.DeserializeObject<Distributors>(apiResponse);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error fetching Distributor details.");
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
        public async Task<ActionResult> UpdateDistributor(Distributors pr)
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
                    using (var response = await httpClient.PutAsync($"http://localhost:7729/api/Distributors/{pr.DistributorID}", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Distributor updated successfully!";
                            return RedirectToAction("GetDistributors");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Error updating the Distributor. Please try again.";
                            ModelState.AddModelError(string.Empty, "Error updating the Distributor.");
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

        /////////////////////////////////////////////// delete Distributor ///////////////////////////////////////////


        [HttpGet]
        public async Task<ActionResult> DeleteDistributor(string id)
        {
            Distributors p = new Distributors();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.DeleteAsync($"http://localhost:7729/api/Distributors/{id}"))
            {
                if (response.IsSuccessStatusCode) // Check if deletion was successful
                {
                    TempData["SuccessMessage"] = "Distributor deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete Distributor. Please try again.";
                }
                return RedirectToAction("GetDistributors");
            }
        }

        //////////////////////////////////////////////get Customers///////////////////////////////////////////////////////////
        public async Task<ActionResult> GetCustomer()
        {
            List<Customers> Customers = new List<Customers>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:7729/api/customers");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    Customers = JsonConvert.DeserializeObject<List<Customers>>(data);
                }
            }

            return View(Customers);
        }

        /////////////////////////////////////////////// delete Customer ///////////////////////////////////////////


        [HttpGet]
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            Customers p = new Customers();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.DeleteAsync($"http://localhost:7729/api/customers/{id}"))
            {
                if (response.IsSuccessStatusCode) // Check if deletion was successful
                {
                    TempData["SuccessMessage"] = "Customer deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete Customer. Please try again.";
                }
                return RedirectToAction("GetCustomer");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////get Products///////////////////////////////////////////////////////////////


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
                }
            }

            return View(Products);
        }

        ///////////////////////////////////////////////////////////Confirm Product//////////////////////////////////////////////////////

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmProduct(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var statusUpdate = new { Status = "Confirmed" };
                string jsonPayload = JsonConvert.SerializeObject(statusUpdate);
                Console.WriteLine("Payload: " + jsonPayload); // Log the payload for debugging
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"http://localhost:7729/api/Products/UpdateStatus/{id}")
                    {
                        Content = content
                    };
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Product confirmed successfully!";
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = $"Error confirming the Product. Status Code: {response.StatusCode}. Details: {responseContent}";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                }
            }

            return RedirectToAction("GetProduct");
        }

        ////////////////////////////////////////////////////////////////////////////////////////get Orders///////////////////////////////////////////////////////////////


        public async Task<ActionResult> Getorder()
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

        ///////////////////////////////////////////////////////////Confirm Orders//////////////////////////////////////////////////////

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmOrder(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var statusUpdate = new { Status = "Confirmed" };
                string jsonPayload = JsonConvert.SerializeObject(statusUpdate);
                Console.WriteLine("Payload: " + jsonPayload); // Log the payload for debugging
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"http://localhost:7729/api/Orders/UpdateStatus/{id}")
                    {
                        Content = content
                    };
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Orders confirmed successfully!";
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = $"Error confirming the Orders. Status Code: {response.StatusCode}. Details: {responseContent}";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                }
            }

            return RedirectToAction("Getorder");
        }

        ///////////////////////////////////////////////////////////////////////////////get QuotationRequests///////////////////////////////////////////////////////////////


        public async Task<ActionResult> GetQuotationRequest()
        {
            List<QuotationRequests> QuotationRequests = new List<QuotationRequests>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:7729/api/QuotationRequests");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    QuotationRequests = JsonConvert.DeserializeObject<List<QuotationRequests>>(data);
                }
            }

            return View(QuotationRequests);
        }

        ///////////////////////////////////////////////////////////Confirm QuotationRequest//////////////////////////////////////////////////////

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmQuotationRequest(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var statusUpdate = new { Status = "Confirmed" };
                string jsonPayload = JsonConvert.SerializeObject(statusUpdate);
                Console.WriteLine("Payload: " + jsonPayload); // Log the payload for debugging
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"http://localhost:7729/api/QuotationRequests/UpdateStatus/{id}")
                    {
                        Content = content
                    };
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "QuotationRequest confirmed successfully!";
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = $"Error confirming the QuotationRequest. Status Code: {response.StatusCode}. Details: {responseContent}";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                }
            }

            return RedirectToAction("GetQuotationRequest");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelQuotationRequest(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var statusUpdate = new { Status = "Cancelled" };
                string jsonPayload = JsonConvert.SerializeObject(statusUpdate);
                Console.WriteLine("Payload: " + jsonPayload); // Log the payload for debugging
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"http://localhost:7729/api/QuotationRequests/UpdateStatus/{id}")
                    {
                        Content = content
                    };
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "QuotationRequest Cancelled successfully!";
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = $"Error confirming the QuotationRequest. Status Code: {response.StatusCode}. Details: {responseContent}";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                }
            }

            return RedirectToAction("GetQuotationRequest"); 
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

        //////////////////////////////////////////////////////////////////add OrderRequests////////////////////////////////////////////////////////////////////////////////////
        public async Task<ActionResult> AddOrderRequest()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> AddOrderRequest(OrderRequests pr)
        {
            OrderRequests p = new OrderRequests();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(pr), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:7729/api/OrderRequests", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode) // Check if the request was successful
                    {
                        JsonConvert.DeserializeObject<OrderRequests>(apiResponse);
                        TempData["SuccessMessage"] = "OrderRequest added successfully!"; // Set success message
                        return RedirectToAction("GetOrderRequest"); // Redirect to admin list page or another view
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to add OrderRequest. Please try again."; // Set error message
                    }
                }
            }
            return View(p);
        }


        /////////////////////////////////////////////////////////////Delete Admin////////////////////////////////////////////////////////////


        [HttpGet]
        public async Task<ActionResult> DeleteOrderRequest(string id)
        {
            OrderRequests p = new OrderRequests();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.DeleteAsync($"http://localhost:7729/api/OrderRequests/{id}"))
            {
                if (response.IsSuccessStatusCode) // Check if deletion was successful
                {
                    TempData["SuccessMessage"] = "OrderRequest deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete OrderRequest. Please try again.";
                }
                return RedirectToAction("GetOrderRequest");
            }
        }
    }
}