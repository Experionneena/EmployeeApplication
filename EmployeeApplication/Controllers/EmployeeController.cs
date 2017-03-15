using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeApplication.Models;
using System.Net.Http;
using System.Text;
using DTOs;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using EmployeeApplication.Scripts.Models;

namespace EmployeeApplication.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }   

        public ActionResult Register()
        {
            return View();
        }

      

        public ActionResult ApplyLeave()
        {
            return View();
        }

        public ActionResult DisplayStatus()
        {
            return View();
        }

        public JsonResult Login(EmployeeDto employeedto)
        {
            
                HttpClient client = new HttpClient();
                var param = JsonConvert.SerializeObject(employeedto);
                HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri("http://localhost:60073/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsync(string.Format("Employee/Login"), contentPost).Result;
                string stringData = response.Content.ReadAsStringAsync().Result;
                EmployeeDto employees = JsonConvert.DeserializeObject<EmployeeDto>(stringData);
                Session["EmployeeTableId"] = employees.Id;
                var role = employees.Role;
                Session["Role"] = employees.Role;
                return Json(new { role = role});
                //if (role == 1)
                //{
                //    return RedirectToAction("HrHome");
                //}
                //else if (role == 0)
                //{
                //    Session["Employees"] = employees;
                //    return View("EmployeeHome", employees);
                //}
                //else
                //    return new HttpStatusCodeResult(404, "Error");
            
           
        }

        public ActionResult HrHome()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:60073/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(string.Format("Employee/GetLeave")).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<EmployeeLeaveDto> employees = JsonConvert.DeserializeObject<List<EmployeeLeaveDto>>(stringData);
            return View("HrHome",employees);
        }

        public ActionResult EmployeeHome()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:60073/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(string.Format("Employee/GetLeave")).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<EmployeeDto> employees = JsonConvert.DeserializeObject<List<EmployeeDto>>(stringData);
       
            return View("EmployeeHome", employees);
        }

        [HttpPost]
        public ActionResult ChangeStatus(LeaveDto leavedto)
        {
            HttpClient client = new HttpClient();
            var param = JsonConvert.SerializeObject(leavedto);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("http://localhost:60073/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync(string.Format("Employee/ChangeStatus"), contentPost).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            EmployeeDto employees = JsonConvert.DeserializeObject<EmployeeDto>(stringData);
            return View("HrHome",employees);
        }


        [HttpPost]
        public ActionResult Add(EmployeeDto employeedto)
        {
            employeedto.Role = 0;
            HttpClient client = new HttpClient();
            var param = JsonConvert.SerializeObject(employeedto);
            HttpContent contentPost = new StringContent(param,Encoding.UTF8, "application/json");    
            client.BaseAddress = new Uri("http://localhost:60073/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync(string.Format("Register/Add"), contentPost).Result;
            if (response.IsSuccessStatusCode)
            {
                return View("HrHome");
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult AddLeave(LeaveDto leavedto)
        {
            leavedto.EmpTableId = (int)Session["EmployeeTableId"];
            HttpClient client = new HttpClient();
            var param = JsonConvert.SerializeObject(leavedto);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("http://localhost:60073/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync(string.Format("Leave/AddLeave"), contentPost).Result;
            if (response.IsSuccessStatusCode)
            {
                EmployeeDto employees = Session["Employees"] as EmployeeDto;
                return View("EmployeeHome",employees);
            }
            return View("Index");
        }

      
        public ActionResult ViewLeave(EmployeeDto employeedto)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:60073/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(string.Format("Employee/ViewAll")).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<EmployeeLeaveDto> employees = JsonConvert.DeserializeObject<List<EmployeeLeaveDto>>(stringData);
            foreach (var item in employees)
            {
                if (item.Status == "Approve")
                {
                    item.Status = "Approved";
                }
                else if (item.Status == "Reject")
                {
                    item.Status = "Rejected";
                }
            }
            return View("ViewLeave", employees);
         
       }

     
        public ActionResult ViewStatus(LeaveDto leavedto)
        {
            HttpClient client = new HttpClient();
            var param = JsonConvert.SerializeObject(leavedto);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("http://localhost:60073/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync(string.Format("Employee/ViewStatus"), contentPost).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<LeaveDto> leaves = JsonConvert.DeserializeObject<List<LeaveDto>>(stringData);
            foreach (var item in leaves)
            {
                if(item.Status == "Approve")
                {
                    item.Status = "Approved";
                }
                else if(item.Status == "Reject")
                {
                    item.Status = "Rejected";
                }
            }
                return View("DisplayStatus",leaves);


        }
     [HttpGet]
        public ActionResult Edit(LeaveDto leavedto)
        {
            HttpClient client = new HttpClient();
            var param = JsonConvert.SerializeObject(leavedto);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("http://localhost:60073/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync(string.Format("Employee/GetLeaveById"), contentPost).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            LeaveDto leave = JsonConvert.DeserializeObject<LeaveDto>(stringData);
            return PartialView("_Edit",leave);
        }
        public ActionResult Delete(LeaveDto leavedto)
        {
            HttpClient client = new HttpClient();
            var id = leavedto.Id;
            client.BaseAddress = new Uri("http://localhost:60073//api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.DeleteAsync(string.Format("Employee/Delete?id=" + id)).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<LeaveDto> leaves = JsonConvert.DeserializeObject<List<LeaveDto>>(stringData);
            return View("DisplayStatus", leaves);
        }
        [HttpPost]
        public ActionResult EditLeave(LeaveDto leavedto)
        {
            leavedto.EmpTableId = (int)Session["EmployeeTableId"];
            HttpClient client = new HttpClient();
            var param = JsonConvert.SerializeObject(leavedto);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri("http://localhost:60073/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PutAsync(string.Format("Employee/EditLeave"), contentPost).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<LeaveDto> leaves = JsonConvert.DeserializeObject<List<LeaveDto>>(stringData);
          
                return View("DisplayStatus",leaves);
            
          
        }

    }
}

