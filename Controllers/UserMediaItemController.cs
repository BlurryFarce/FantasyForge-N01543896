using FantasyForge_N01543896.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FantasyForge_N01543896.Controllers
{
    public class UserMediaItemController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static UserMediaItemController() {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/api/");
        }

        // GET: UserMediaItem/List
        public ActionResult List() {
            //objective: communicate with our UserMediaItem data api to retrieve a list of UserMediaItems
            //curl https://localhost:44324/api/UserMediaItemdata/listkeepers


            string url = "UserMediaItemdata/listUserMediaItems";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<UserMediaItemDto> UserMediaItems = response.Content.ReadAsAsync<IEnumerable<UserMediaItemDto>>().Result;


            return View(UserMediaItems);
        }

        public ActionResult Error() {

            return View();
        }

        // GET: UserMediaItem/New
        public ActionResult New() {
            return View();
        }

        // POST: UserMediaItem/Create
        [HttpPost]
        public ActionResult Create(UserMediaItem UserMediaItem) {
            Debug.WriteLine("the json payload is :");

            //objective: add a new UserMediaItem into our system using the API
            //curl -H "Content-Type:application/json" -d @UserMediaItem.json https://localhost:44324/api/UserMediaItemdata/addUserMediaItem
            string url = "UserMediaItemdata/addUserMediaItem";


            string jsonpayload = jss.Serialize(UserMediaItem);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode) {
                return RedirectToAction("List");
            }
            else {
                return RedirectToAction("Error");
            }


        }

        // GET: UserMediaItem/Edit/5
        public ActionResult Edit(int id) {
            string url = "UserMediaItemdata/findUserMediaItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserMediaItemDto selectedKeeper = response.Content.ReadAsAsync<UserMediaItemDto>().Result;
            return View(selectedKeeper);
        }

        // POST: UserMediaItem/Update/5
        [HttpPost]
        public ActionResult Update(int id, UserMediaItem UserMediaItem) {

            string url = "UserMediaItemdata/updateUserMediaItem/" + id;
            string jsonpayload = jss.Serialize(UserMediaItem);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode) {
                return RedirectToAction("List");
            }
            else {
                return RedirectToAction("Error");
            }
        }

        // GET: UserMediaItem/Delete/5
        public ActionResult DeleteConfirm(int id) {
            string url = "UserMediaItemdata/findUserMediaItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserMediaItemDto selectedKeeper = response.Content.ReadAsAsync<UserMediaItemDto>().Result;
            return View(selectedKeeper);
        }

        // POST: UserMediaItem/Delete/5
        [HttpPost]
        public ActionResult Delete(int id) {
            string url = "UserMediaItemdata/deleteUserMediaItem/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode) {
                return RedirectToAction("List");
            }
            else {
                return RedirectToAction("Error");
            }
        }
    }
}