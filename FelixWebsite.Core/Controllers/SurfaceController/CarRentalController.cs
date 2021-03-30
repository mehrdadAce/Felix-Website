using FelixWebsite.Core.Models.ViewModels;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;
using FelixWebsite.Core.Helpers;
using FelixWebsite.Core.App_GlobalResources;
using System.Net.Mail;
using FelixWebsite.Bll.Helpers;
using System;
using System.Globalization;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class CarRentalController : Umbraco.Web.Mvc.SurfaceController
    {
        [HttpPost]
        public ActionResult SubmitForm(CarRentalForm model)
        {
            if (ModelState.IsValid)
            {
                var dateArray = GetTimeArray(model.DateRange);
                if (dateArray != null && dateArray[1] >= dateArray[0] && IsInFuture(dateArray[0]) && IsInFuture(dateArray[1]))
                {
                    var business = GetBusinessById(model.BusinessId);
                    if (business != null)
                    {
                        if (!string.IsNullOrWhiteSpace(business.BusinessMail))
                        {
                            var emailBusiness = business.BusinessMail;
                            var emailCustomer = model.Emailadress;

                            var messageForBusiness = new MailMessage(emailBusiness, emailBusiness)
                            {
                                Subject = FelixResources.car_rental_subject_business + model.FirstName + " " + model.LastName,
                                Body = EmailHelper.RenderViewToString(ControllerContext, ConfigHelper.MailTemplateDirectory + "ToBusiness/CarRentalRequest.cshtml", model)
                            };

                            var messageForCustomer = new MailMessage(emailBusiness, emailCustomer)
                            {
                                Subject = FelixResources.car_rental_subject_customer,
                                Body = EmailHelper.RenderViewToString(ControllerContext, ConfigHelper.MailTemplateDirectory + "ToCustomer/CarRentalRequest.cshtml", model)
                            };

                            EmailHelper.SendHTMLEmail(messageForBusiness);
                            EmailHelper.SendHTMLEmail(messageForCustomer);

                            TempData[FelixResources.temp_is_carrental_success] = true;
                        }
                        else
                        {
                            LogHelper.Debug(GetType(), "No valid email for business, no emails were sent");
                            TempData[FelixResources.temp_is_carrental_success] = false;
                        }
                    }
                    else
                    {
                        LogHelper.Debug(GetType(), "No businessbrand found for businessid, no emails were sent");
                        TempData[FelixResources.temp_is_carrental_success] = false;
                    }
                }
                else
                {
                    LogHelper.Debug(GetType(), "The provided dates are invalid, no emails were sent");
                    TempData[FelixResources.temp_is_carrental_success] = false;
                }
            }
            else
            {
                LogHelper.Debug(GetType(), "ModelState of form invalid, no emails were sent");
                TempData[FelixResources.temp_is_carrental_success] = false;
            }
            return RedirectToCurrentUmbracoUrl();
        }

        private DateTime[] GetTimeArray(string dateString)
        {
            try
            {
                string[] dateStringArray = dateString.Split(new string[] { " TOT " }, StringSplitOptions.None);
                DateTime[] timeRange = new DateTime[2];
                timeRange[0] = DateTime.ParseExact(dateStringArray[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                timeRange[1] = DateTime.ParseExact(dateStringArray[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                return timeRange;
            }
            catch (Exception e)
            {
                LogHelper.Error(typeof(CarRentalController), $"Something went wrong when converting the provided dates. Error: ", e);
                return null;
            }
        }

        private bool IsInFuture(DateTime date)
        {
            DateTime today = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 0);
            return (today.Date + ts) <= date;
        }

        private BusinessBrand GetBusinessById(int id)
        {
            return Umbraco.AssignedContentItem.AncestorOrSelf(1).Children().OfType<Businesses>().FirstOrDefault().Children().OfType<BusinessBrand>().Where(x => x.Id == id).FirstOrDefault();
        }
    }
}

