using Honeycombsoft.BLL.DTO;
using Honeycombsoft.BLL.Interfaces;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Hangfire;
using System;
using Honeycombsoft.BLL.Mail;
using Honeycombsoft.BLL.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using NLog;
using Honeycombsoft.BLL.Mail.DateEmail;


namespace Honeycombsoft.WEB.Controllers
{
    [Authorize(Roles="user,admin")]
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
            
        }

        public async Task<ActionResult> Index()
        {
    
            if (User.Identity.IsAuthenticated)
            {
                UserDTO user = await userService.FindByEmail(User.Identity.Name);
  
                ViewBag.dateSelectList = CreateSelectList(userService.GetAllDateStudy());

                return View(user);
            }
           
            return View();
        }

        public IEnumerable<SelectListItem> CreateSelectList(IList<BeginStudyDTO> list)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var item in list)
            {
                selectList.Add(new SelectListItem{ Text = item.Date.ToString() , Value = item.Id.ToString()});
            }

            return selectList;
        }

        [HttpGet]
        public async Task<ActionResult> SetDate( string SelectDate, string Id)
        {
            bool success = false;
            BeginStudyDTO beginStudy = null;

            try
            {
                beginStudy = await userService.FindDateById(int.Parse(SelectDate));
                success = await userService.SetDateStudy(User.Identity.Name, beginStudy.Date);
            }
            catch (ValidationException ex)
            {
                logger.Error(ex.Message + "! User - " + User.Identity.Name);
                Content(ex.Message);
            }
           
            if (success)
            {
                // Send message to email { 30 , 7 , 1 days}
                if(DateJob.CreateDateForSendEmail(beginStudy.Date, "month") != 0)
                    BackgroundJob.Schedule(() => EmailSender.SendEmail(User.Identity.Name , "30 days"), TimeSpan.FromDays(DateJob.CreateDateForSendEmail(beginStudy.Date, "month")));

                if (DateJob.CreateDateForSendEmail(beginStudy.Date, "week") != 0)
                    BackgroundJob.Schedule(() => EmailSender.SendEmail(User.Identity.Name , "7 days"), TimeSpan.FromDays(DateJob.CreateDateForSendEmail(beginStudy.Date, "week")));

                if (DateJob.CreateDateForSendEmail(beginStudy.Date, "day") != 0)
                    BackgroundJob.Schedule(() => EmailSender.SendEmail(User.Identity.Name , "1 day"), TimeSpan.FromDays(DateJob.CreateDateForSendEmail(beginStudy.Date, "day")));
               
                UserDTO userDto = await userService.FindById(Id);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

  
          
 
    } 
}