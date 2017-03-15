using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeApplication.Controllers
{
    public class BaseController : Controller
    {
        public static string GetName(string name)
        {
            var nameGet = Resource.ResourceManager.GetString(name);
            return nameGet;
        }
    }
}