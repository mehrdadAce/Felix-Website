using FelixWebsite.Core.App_GlobalResources;
using FelixWebsite.Core.Helpers;
using FelixWebsite.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class JobApplicationController : Umbraco.Web.Mvc.SurfaceController
    {
        [HttpPost]
        public ActionResult SubmitForm(JobApplicationForm model)
        {
            if (ModelState.IsValid && isFileValid(model.attachment))
            {
                try
                {
                    model.Message = model.Message.Replace("\r\n", "<br/>");
                    SendToFelix(model);
                    SendToSender(model);
                    TempData[FelixResources.temp_is_jobapplication_success] = true;
                }
                catch (Exception)
                {
                    TempData[FelixResources.temp_is_jobapplication_success] = false;
                }
            }
            else
            {
                TempData[FelixResources.temp_is_jobapplication_success] = false;
            }
            return RedirectToCurrentUmbracoPage();
        }

        private bool isFileValid(HttpPostedFileBase attachment)
        {
            return attachment != null && (Path.GetExtension(attachment.FileName) == ".pdf" || Path.GetExtension(attachment.FileName) == ".docx");
        }

        private void SendToSender(JobApplicationForm model)
        {
            var groupHome = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault();
            var jobsite = groupHome.Children.OfType<Joboffers>().First();
            var emailSetting = jobsite.SollicitationEmailSettings as EmailSetting;
            var emailSender = new MailAddress(emailSetting.From, emailSetting.FromName);

            var message = new MailMessage(emailSender, new MailAddress(model.Email))
            {
                Subject = FelixResources.jobapplication_success,
                Body = EmailHelper.RenderViewToString(ControllerContext, "~/Views/MailTemplates/ConfirmJobApplication.cshtml", model)
            };

            EmailHelper.SendHTMLEmail(message);
        }

        private void SendToFelix(JobApplicationForm model)
        {
            if (model.attachment != null && model.attachment.ContentLength > 0)
            {
                var groupHome = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault();
                var jobsite = groupHome.Children.OfType<Joboffers>().First();
                var emailSetting = jobsite.SollicitationEmailSettings as EmailSetting;
                var emailSender = new MailAddress(emailSetting.From, emailSetting.FromName);
                var emailReceiver = new MailAddress(emailSetting.To);

                var message = new MailMessage(emailSender, emailReceiver)
                {
                    Subject = FelixResources.contact_message + " " + model.Name + " (" + model.Email + ")",
                    Body = EmailHelper.RenderViewToString(ControllerContext, "~/Views/MailTemplates/JobApplicationFormMail.cshtml", model)
                };
                message.AddCcMails(emailSetting.CC);

                Attachment messageAttachment = new Attachment(model.attachment.InputStream, model.attachment.FileName);
                message.Attachments.Add(messageAttachment);

                EmailHelper.SendHTMLEmail(message);
            }
        }
    }
}
