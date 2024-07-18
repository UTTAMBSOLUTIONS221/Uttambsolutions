using DBL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Jobs.Controllers
{
    public class BaseController : Controller
    {
        public UsermodelResponce SessionUserData
        {
            get
            {
                {
                    UsermodelResponce userDataSerializeModel = null;
                    if (base.User is ClaimsPrincipal)
                    {
                        string claim = BaseController.GetClaim((base.User as ClaimsPrincipal).Claims.ToList<Claim>(), "userData");
                        if (!string.IsNullOrEmpty(claim))
                        {
                            userDataSerializeModel = JsonConvert.DeserializeObject<UsermodelResponce>(claim);
                        }
                    }

                    base.ViewData["UserData"] = (userDataSerializeModel ?? new UsermodelResponce());
                    if (userDataSerializeModel == null)
                    {
                        string url = Url.Action("Signin", "Account");
                        HttpContext.Response.Redirect(url);
                    }

                    return userDataSerializeModel;
                }
            }
        }
        public static string GetClaim(List<Claim> claims, string key)
        {
            Claim claim = claims.FirstOrDefault((Claim c) => c.Type == key);
            if (claim == null)
            {
                return null;
            }
            return claim.Value;
        }

        public void Success(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Success, message, dismissable, "fa fa-check");
        }

        public void Information(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Information, message, dismissable, "fa fa-info-circle");
        }

        public void Warning(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Warning, message, dismissable, "fa fa-warning");
        }

        public void Danger(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Danger, message, dismissable, "fa fa-times-circle");
        }

        private void AddAlert(string alertStyle, string message, bool dismissable, string iconClass)
        {
            var alerts = new List<Alert>();

            string jsonData = TempData.ContainsKey(Alert.TempDataKey) ? (string)TempData[Alert.TempDataKey] : "";
            if (!string.IsNullOrEmpty(jsonData))
                alerts = JsonConvert.DeserializeObject<List<Alert>>(jsonData);

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable,
                IconClass = iconClass
            });

            TempData[Alert.TempDataKey] = JsonConvert.SerializeObject(alerts);
        }

        public string[] GetModelErrors()
        {
            List<string> errors = new List<string>();
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                    errors.Add(error.ErrorMessage);
            }

            return errors.ToArray();
        }
    }
}
