using FantasyForge_N01543896.Models;
using FantasyForge_N01543896.Models.ViewModels;
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
    public class UserController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static UserController() {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44318/api/");
        }

        // GET: User/List
        public ActionResult List() {
            //objective: communicate with our user data api to retrieve a list of users
            //curl https://localhost:44318/api/UserData/listusers


            string url = "userdata/listusers";
            HttpResponseMessage response = client.GetAsync(url).Result;
      

            IEnumerable<UserDto> animals = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;


            return View(animals);
        }

        // GET: User/Details/5
        public ActionResult Details(int id) {
            DetailsUser ViewModel = new DetailsUser();

            //objective: communicate with our animal data api to retrieve one user
            //curl https://localhost:44318/api/userdata/finduser/{id}

            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            Debug.WriteLine("User received : ");
            Debug.WriteLine(SelectedUser.UserName);

            ViewModel.SelectedUser = SelectedUser;

            //show associated personal list with this user
            url = "usermediaitemdata/listusermediaitemforuser/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<UserMediaItem> UserPersonalList = response.Content.ReadAsAsync<IEnumerable<UserMediaItem>>().Result;

            ViewModel.UserPersonalList = UserPersonalList;


            return View(ViewModel);
        }


        public ActionResult Error() {

            return View();
        }

        // GET: User/New
        public ActionResult New() {
            //information about all mediaitems in the system.
            //GET api/mediaitem/listmediaitems

            string url = "mediaitem/listmediaitem";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<MediaItemDto> MediaItems = response.Content.ReadAsAsync<IEnumerable<MediaItemDto>>().Result;

            return View(MediaItems);
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User user) {
            Debug.WriteLine("the json payload is :");
            //objective: add a new user into our system using the API
            //curl -H "Content-Type:application/json" -d @user.json https://localhost:44318/api/Userdata/adduser
            string url = "userdata/adduser";


            string jsonpayload = jss.Serialize(user);
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

        // GET: User/Edit/5
        public ActionResult Edit(int id) {
            UpdateUser ViewModel = new UpdateUser();

            //the existing animal information
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            ViewModel.SelectedUser = SelectedUser;

            // all mediaitem to choose from when updating this user
            //the existing animal information
            url = "mediaitemdata/listmediaitems/";
            response = client.GetAsync(url).Result;
            IEnumerable<MediaItemDto> MediaItems = response.Content.ReadAsAsync<IEnumerable<MediaItemDto>>().Result;

            ViewModel.MediaItems = MediaItems;

            return View(ViewModel);
        }

        // POST: User/Update/5
        [HttpPost]
        public ActionResult Update(int id, User user) {

            string url = "userdata/updateuser/" + id;
            string jsonpayload = jss.Serialize(user);
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

        // GET: User/Delete/5
        public ActionResult DeleteConfirm(int id) {
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto selecteduser = response.Content.ReadAsAsync<UserDto>().Result;
            return View(selecteduser);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id) {
            string url = "userdata/deleteuser/" + id;
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