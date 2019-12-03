using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ViewFep.Models;

namespace ViewFep.Controllers
{
    public class HomeController : Controller
    {
        public async Task <IActionResult> Index()
        {
            List<Customer> customerList = new List<Customer>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://fpwebapi.azurewebsites.net/api/customer"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    customerList = JsonConvert.DeserializeObject<List<Customer>>(apiResponse);
                }
            }
            customerList = customerList.OrderByDescending(x => x.id).ToList();
            return View(customerList);
        }
        [HttpPost]
        public JsonResult cadastrar(string nome)
        {
            var customer = new Customer();
            customer.Name = nome;

            HttpClient client = new HttpClient();
           
            var contentString = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

            var response =  client.PostAsync("https://fpwebapi.azurewebsites.net/api/customer", contentString);
            return new JsonResult("ok"); 
        }



        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
