using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client;
        string server;
        string apikey = "?apikey=aWvoFVu5Ap6enHyHgVFS3NRqzLRn0lzx";

        public HomeController()
        {
            client = new HttpClient();
            server = "https://ksheebar-eval-prod.apigee.net/restaurantinfoproxy/api/RestaurantInfo/";
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            string url = server + "getall" + apikey;
            IEnumerable<Restaurant> restaurants = new List<Restaurant>();
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                restaurants = JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(json);
            }

            return View(restaurants);
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await RestaurantExists(id);
            
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurantId,RestaurantName,RestaurantAddress,RestaurantPopular,RestaurantPhone,RestaurantRating")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(restaurant);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(server+apikey, content);

                if (response.IsSuccessStatusCode) { return RedirectToAction(nameof(Index)); }
                
            }
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await RestaurantExists(id); ;
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RestaurantId,RestaurantName,RestaurantAddress,RestaurantPopular,RestaurantPhone,RestaurantRating")] Restaurant restaurant)
        {
            if (id != restaurant.RestaurantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string url = server + id + apikey;
                string json = JsonConvert.SerializeObject(restaurant);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode) { return RedirectToAction(nameof(Index)); }
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await RestaurantExists(id);
              
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string url = server + id +apikey;
            await client.DeleteAsync(url);

            return RedirectToAction(nameof(Index));
        }

        private async Task<Restaurant> RestaurantExists(string id)
        {
            string url = server + "getbyid/" + id +apikey;
            Restaurant restaurant = new Restaurant();
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                restaurant = JsonConvert.DeserializeObject<Restaurant>(json);
            }

            return restaurant;
        }
    }
}
