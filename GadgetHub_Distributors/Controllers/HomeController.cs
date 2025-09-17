
using GadgetHub_Distributors.Models;
using GadgetHub_Server.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GadgetHub_Distributors.Controllers
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

        /////////////////////////////////////Distributors login////////////////////////////////////////////////////
        [HttpPost]
        public async Task<ActionResult> Index(Distributors Distributors)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(Distributors), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://localhost:7729/api/distributors/Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

                        // Set success message in TempData
                        TempData["SuccessMessage"] = "Login successful! Welcome to the Distributor Dashboard.";

                        // Redirect based on user role

                        return RedirectToAction("Home");

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid Email or password.";
                    }
                }
            }
            return View(Distributors);
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

        /////////////////////////////////////////////////////////////////////add products////////////////////////////////////////////////////////////////////////////////////
        public async Task<ActionResult> AddProduct()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> AddProduct(Products pr)
        {
            Products p = new Products();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(pr), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:7729/api/Products", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode) // Check if the request was successful
                    {
                        JsonConvert.DeserializeObject<Products>(apiResponse);
                        TempData["SuccessMessage"] = "Product added successfully!"; // Set success message
                        return RedirectToAction("GetProduct"); // Redirect to admin list page or another view
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to add Product. Please try again."; // Set error message
                    }
                }
            }
            return View(p);
        }

        ///////////////////////////////////////////////////////////////////update products////////////////////////////////////////////////////////////////////

        public async Task<ActionResult> UpdateProduct(string id)
        {
            Products qr = new Products();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync($"http://localhost:7729/api/Products/{id}"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            qr = JsonConvert.DeserializeObject<Products>(apiResponse);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error fetching Product details.");
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
        public async Task<ActionResult> UpdateProduct(Products pr)
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
                    using (var response = await httpClient.PutAsync($"http://localhost:7729/api/Products/{pr.ProductId}", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Product updated successfully!";
                            return RedirectToAction("GetProduct");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Error updating the drug. Please try again.";
                            ModelState.AddModelError(string.Empty, "Error updating the Product.");
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


         ///////////////////////////////////////////////////////Delete Products////////////////////////////////////////////////////
        [HttpGet]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            Products p = new Products();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.DeleteAsync($"http://localhost:7729/api/Products/{id}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                p = JsonConvert.DeserializeObject<Products>(apiResponse);

                if (response.IsSuccessStatusCode)
                {

                    TempData["SuccessMessage"] = "Product deleted successfully!";

                }
                else
                {
                    TempData["ErrorMessage"] = "Error deleting the Product. Please try again.";
                    ModelState.AddModelError(string.Empty, "Error deleting the Product.");
                }

                return RedirectToAction("GetProduct");
            }
        }

        /////////////////////////////////////////////////////////////////////////////get Quotations///////////////////////////////////////////////////////////////


        public async Task<ActionResult> GetQuotations()
        {
            List<Quotations> Quotations = new List<Quotations>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:7729/api/Quotations");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    Quotations = JsonConvert.DeserializeObject<List<Quotations>>(data);
                }
            }

            return View(Quotations);
        }

        public async Task<ActionResult> AddQuotation()
        {

            List<Products> products = new List<Products>();

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "http://localhost:7729/api/Products"; // API endpoint to get products
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<Products>>(data);
                }
            }

            var viewModel = new AddQuotationViewModel
            {
                Products = products
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddQuotation(AddQuotationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the ViewModel to your Entity Model
                var Quotations = new Quotations
                {
                    QuotationID = model.QuotationID,
                    productName = model.productName, // Selected Quotations name from dropdown
                    Price = model.Price,
                    Quantity = model.Quantity,

                };

                // Call the API to add the tender
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Quotations), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("http://localhost:7729/api/Quotations", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Quotation added successfully!";
                            return RedirectToAction("GetQuotations"); // Redirect to the list of Quotations
                        }
                        else
                        {
                            string errorMessage = await response.Content.ReadAsStringAsync();
                            ModelState.AddModelError("", "An error occurred while saving the Quotation: " + errorMessage);
                        }
                    }
                }
            }

            return View(model);
        }

        ///////////////////////////////////////////////////////////////////update Quotations////////////////////////////////////////////////////////////////////

        public async Task<ActionResult> UpdateQuotation(string id)
        {
            Quotations qr = new Quotations();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync($"http://localhost:7729/api/Quotations/{id}"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            qr = JsonConvert.DeserializeObject<Quotations>(apiResponse);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error fetching Quotation details.");
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
        public async Task<ActionResult> UpdateQuotation(Quotations pr)
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
                    using (var response = await httpClient.PutAsync($"http://localhost:7729/api/Quotations/{pr.QuotationID}", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Quotation updated successfully!";
                            return RedirectToAction("GetQuotations");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Error updating the Quotation. Please try again.";
                            ModelState.AddModelError(string.Empty, "Error updating the Quotation.");
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

        ///////////////////////////////////////////////////////Delete Quotation////////////////////////////////////////////////////
        [HttpGet]
        public async Task<ActionResult> DeleteQuotation(string id)
        {
            Quotations p = new Quotations();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.DeleteAsync($"http://localhost:7729/api/Quotations/{id}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                p = JsonConvert.DeserializeObject<Quotations>(apiResponse);

                if (response.IsSuccessStatusCode)
                {

                    TempData["SuccessMessage"] = "Quotation deleted successfully!";

                }
                else
                {
                    TempData["ErrorMessage"] = "Error deleting the Quotation. Please try again.";
                    ModelState.AddModelError(string.Empty, "Error deleting the Quotation.");
                }

                return RedirectToAction("GetQuotations");
            }
        }

        ////////////////////////////////////////////Get Orders///////////////////////////////////////////////////////////

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
                    // Filter Orders where Status is "Confirmed"
                    Orders = Orders.Where(p => p.Status == "Confirmed").ToList();
                }
            }

            return View(Orders);
        }

        ////////////////////////////////////////////////////////////////////////////////////////get QuotationRequests///////////////////////////////////////////////////////////////


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

        /////////////////////////////////////////////////////////////////////add QuotationRequests////////////////////////////////////////////////////////////////////////////////////
        public async Task<ActionResult> AddQuotationRequests()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> AddQuotationRequests(QuotationRequests pr)
        {
            QuotationRequests p = new QuotationRequests();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(pr), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:7729/api/QuotationRequests", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode) // Check if the request was successful
                    {
                        JsonConvert.DeserializeObject<QuotationRequests>(apiResponse);
                        TempData["SuccessMessage"] = "QuotationRequest added successfully!"; // Set success message
                        return RedirectToAction("GetQuotationRequest"); // Redirect to admin list page or another view
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to add QuotationRequest. Please try again."; // Set error message
                    }
                }
            }
            return View(p);
        }

        ///////////////////////////////////////////////////////////////////update QuotationRequests////////////////////////////////////////////////////////////////////

        public async Task<ActionResult> UpdateQuotationRequest(string id)
        {
            QuotationRequests qr = new QuotationRequests();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync($"http://localhost:7729/api/QuotationRequests/{id}"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            qr = JsonConvert.DeserializeObject<QuotationRequests>(apiResponse);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error fetching QuotationRequest details.");
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
        public async Task<ActionResult> UpdateQuotationRequest(QuotationRequests pr)
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
                    using (var response = await httpClient.PutAsync($"http://localhost:7729/api/QuotationRequests/{pr.QuotationRequestID}", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "QuotationRequest updated successfully!";
                            return RedirectToAction("GetQuotationRequest");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Error updating the QuotationRequest. Please try again.";
                            ModelState.AddModelError(string.Empty, "Error updating the QuotationRequest.");
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


        ///////////////////////////////////////////////////////Delete QuotationRequests////////////////////////////////////////////////////
        [HttpGet]
        public async Task<ActionResult> DeleteQuotationRequest(string id)
        {
            QuotationRequests p = new QuotationRequests();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.DeleteAsync($"http://localhost:7729/api/QuotationRequests/{id}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                p = JsonConvert.DeserializeObject<QuotationRequests>(apiResponse);

                if (response.IsSuccessStatusCode)
                {

                    TempData["SuccessMessage"] = "QuotationRequest deleted successfully!";

                }
                else
                {
                    TempData["ErrorMessage"] = "Error deleting the QuotationRequest. Please try again.";
                    ModelState.AddModelError(string.Empty, "Error deleting the QuotationRequest.");
                }

                return RedirectToAction("GetQuotationRequest");
            }
        }
    }
}