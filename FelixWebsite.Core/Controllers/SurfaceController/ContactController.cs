using FelixWebsite.Core.App_GlobalResources;
using FelixWebsite.Core.Helpers;
using FelixWebsite.Core.Models;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class ContactController : Umbraco.Web.Mvc.SurfaceController
    {
        [HttpPost]
        public ActionResult SubmitForm(ContactForm model)
        {
            if (ModelState.IsValid)
            {
                TempData[FelixResources.temp_is_contact_success] = SendEmail(model);
            }
            else
            {
                TempData[FelixResources.temp_is_contact_success] = false;
            }
            return new RedirectResult(Request.RawUrl);
        }

        private bool SendEmail(ContactForm model)
        {
            try
            {
                var root = Session.GetBreadcrumbRoot(CurrentPage);
                var contactEmailSettings = root as IEmailSettingsContact;
                if (contactEmailSettings == null)
                {
                    contactEmailSettings = Umbraco.TypedContentAtRoot().OfType<IEmailSettingsContact>().FirstOrDefault();
                }
                
                if (!string.IsNullOrEmpty(model.SecondHandBusinessBrandNodeId))
                {
                    contactEmailSettings = (BusinessBrand)Umbraco.TypedContent(model.SecondHandBusinessBrandNodeId);
                }

                var emailsettings = contactEmailSettings.ContactEmailSettings as EmailSetting;
                var emailSender = new MailAddress(emailsettings.From, emailsettings.FromName);
                var emailReceiver = new MailAddress(emailsettings.To);

                // TODO: afhankelijk van culture mail sturen met andere taal
                var message = new MailMessage(emailSender, emailReceiver)
                {
                    Subject = $"{FelixResources.contact_message} {model.Name} ({model.Email})",
                    Body = EmailHelper.RenderViewToString(ControllerContext, "~/Views/MailTemplates/ContactFormMail.cshtml", model)
                };
                message.AddCcMails(emailsettings.CC);

                EmailHelper.SendHTMLEmail(message);
                return true;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error(GetType(), "Error while sending contact email", ex);
                return false;
            }
        }
    }
}
