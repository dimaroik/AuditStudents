using Honeycombsoft.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Honeycombsoft.BLL.DTO;
using System.Threading.Tasks;
using Honeycombsoft.BLL.Infrastructure;
using Hangfire;
using NLog;

namespace Honeycombsoft.WEB.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        IAdminManage adminManage;

        public AdminController(IAdminManage adminManage)
        {
            this.adminManage = adminManage;
        }
        public ActionResult Index()
        {
            return View();
        }

        public string GetData()
        {
            return JsonConvert.SerializeObject(adminManage.GetListUsers());
        }
        public string GetDataStudy()
        {
            return JsonConvert.SerializeObject(adminManage.GetAllDateStudy());
        }

        public ActionResult ShowBeginStudy()
        {
            return PartialView();
        }

       
        [HttpPost]
        public async Task AddStudyDate(BeginStudyDTO beginStudy)
        {
            try
            {
                await adminManage.AddStudyDate(beginStudy.Date);
            }
            catch (ValidationException ex)
            {
                BackgroundJob.Enqueue(() => logger.Error(ex.Message + ex.Property));
            }
            
        }
    }
}