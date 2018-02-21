using Honeycombsoft.BLL.DTO;
using Honeycombsoft.BLL.Infrastructure;
using Honeycombsoft.BLL.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Honeycombsoft.WEB.Models;
using Facebook;
using System.Web.Security;
using Honeycombsoft.BLL.Mail;
using Hangfire;
using NLog;


namespace Honeycombsoft.WEB.Controllers
{
    public class AccountController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public async Task<JsonResult> GetLoginModel(LoginModel d)
        {
            UserDTO userDto = new UserDTO { Email = d.Email, Password = d.Password };
            ClaimsIdentity claim = await userService.Authenticate(userDto);

            if (claim == null)
            {
                return new JsonResult { Data = d, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                UserDTO user = await userService.FindByEmail(userDto.Email);

                if (user.ConfirmEmail == true || user.Email == "gg@gmail.com")
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                }
                return new JsonResult { Data = d, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> RegisterData(RegisterModel u)
        {
            bool success = false;
            string message = "";
            await SetInitialDataAsync();

            if (u != null)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = u.Email,
                    Password = u.Password,
                    Name = u.Name,
                    LastName = u.LastName,
                    Age = u.Age,
                    RegisteredDate = DateTime.Now,
                    StudyDate = DateTime.Now,
                    Role = "user"
                };

                try
                {
                    success = await userService.Create(userDto);
                }
                catch (ValidationException ex)
                {
                    BackgroundJob.Enqueue(() => logger.Error(ex.Message + ex.Property));
                }

                if (success)
                {
                   
                    var user = await userService.FindByEmail(userDto.Email);

                    var callbackUrl = Url.Action("ConfirmMail", "Account", new { userId = user.Id },
                               protocol: Request.Url.Scheme);

                    BackgroundJob.Enqueue(() => EmailSender.SendEmailAsync(user.Email, callbackUrl));
                    
                    message = "Success";  
                }
                else
                    message = "Failed Email! Try another email!";
            }
            else
                message = "Failed!";

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult NotConfirm()
        {
            return View();
        }

        [HttpGet]
        public async Task< ActionResult> ConfirmMail(string userId)
        {
            var user = await userService.FindById(userId);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmMail(UserDTO user)
        {
            bool success = false;
            try
            {
                success = await userService.SetConfirmEmail(user.Email);
            }
            catch (ValidationException ex)
            {
                BackgroundJob.Enqueue(() => logger.Error(ex.Message + ex.Property));
            }
            
            if (success)
                return RedirectToAction("Login", "Account");
            else
                return RedirectToAction("NotConfirm", "Account");
            
        }

        private async Task SetInitialDataAsync()
        {
            await userService.SetInitialData(new UserDTO
            {
                Email = "qqq@gmail.com",
                Password = "ad46D_ewr3",
                Name = "Dima",
                LastName = "Roik",
                Role = "admin",
            }, new List<string> { "user", "admin" });
        }



        private Uri RediredtUri(string actionName)
        {
           
                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action(actionName);

                return uriBuilder.Uri; 
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new
            {

                client_id = "2070909226529567",
                client_secret = "35c64a08fb35dc17a2ea23fb55096b2a",

                redirect_uri = RediredtUri("LoginFacebook1").AbsoluteUri,
                response_type = "code",
                scope = "email"

            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public async Task<ActionResult> LoginFacebook1(string code)
        {
            dynamic me = CreateFacebookToken("LoginFacebook1", code);

            string email = me.email;                
         
            UserDTO userDto = new UserDTO { Email = email };
            ClaimsIdentity claim = await userService.Authenticate(userDto);

            if (claim == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль.");
            }
            else
            {
                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, claim);

                return RedirectToAction("Index", "Home"); 
            }
            return RedirectToAction("Login", "Account");
        }
        public dynamic CreateFacebookToken(string actionName, string code)
        {
            var fb = new FacebookClient();

            dynamic result = fb.Post("oauth/access_token", new
            {

                client_id = "2070909226529567",
                client_secret = "35c64a08fb35dc17a2ea23fb55096b2a",

                redirect_uri = RediredtUri(actionName).AbsoluteUri,
                code = code

            });

            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;

            fb.AccessToken = accessToken;
            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

            return me;
        }

        public ActionResult Facebook()
        {

            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new
            {

                client_id = "2070909226529567",
                client_secret = "35c64a08fb35dc17a2ea23fb55096b2a",

                redirect_uri = RediredtUri("FacebookCallBack").AbsoluteUri,
                response_type="code",
                scope="email"

            });

            return Redirect(loginUrl.AbsoluteUri);

        }

        public ActionResult FacebookCallBack(string code)
        {

            dynamic me = CreateFacebookToken("FacebookCallBack", code);

            string email = me.email;
            string name = me.first_name;
            string lastName = me.last_name;
           
            RegisterModel model = new RegisterModel { Name = name, LastName = lastName, Email = email };

            return View(model);

        }

        [HttpPost]
        public async Task<ActionResult> FacebookCallBack(RegisterModel model)
        {
                bool success = false;
                await SetInitialDataAsync();
            
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    LastName = model.LastName,
                    Age = model.Age,
                    RegisteredDate = DateTime.Now,
                    StudyDate = DateTime.Now,
                    Role = "user"
                };

                try
                {
                    success = await userService.Create(userDto);
                }
                catch (ValidationException ex)
                {
                    BackgroundJob.Enqueue(() => logger.Error(ex.Message + ex.Property));
                }
                
                if (success)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Index", "Home");
                }
                         
            return View(model);
        }

    }
}
    
