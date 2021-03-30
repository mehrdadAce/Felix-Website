using FelixWebsite.Bdo.Enums;
using FelixWebsite.Bll.Helpers;
using FelixWebsite.Core.App_GlobalResources;
using FelixWebsite.Core.Helpers;
using FelixWebsite.Core.Models;
using System.Net.Mail;
using System.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class ActionController : Umbraco.Web.Mvc.SurfaceController
    {
        [HttpPost]
        public ActionResult SubmitForm(ActionForm model)
        {
            if (!ModelState.IsValid)
            {
                return ShowErrorMessage(model.Type);
            }

            var mailSetting = MailSettingsHelper.GetEmailSettingForAction(Umbraco, model.Type, model.StoreId, model.SecondHandBusinessBrandNodeId, model.ModelName);
            if (!MailSettingsHelper.EmailSettingIsValid(mailSetting))
            {
                return ShowErrorMessage(model.Type);
            }

            SendEmailToReceiver(model, mailSetting);
            SendEmailToSender(model, mailSetting);
            return ShowSuccesMessage(model.Type);
        }

        private ActionResult ShowErrorMessage(ActionType type)
        {
            switch (type)
            {
                case ActionType.TestDrive:
                    TempData[FelixResources.temp_is_testdrive_success] = false;
                    break;
                case ActionType.Quotation:
                    TempData[FelixResources.temp_is_quotation_success] = false;
                    break;
                default:
                    TempData[FelixResources.temp_is_testdrive_success] = false;
                    TempData[FelixResources.temp_is_quotation_success] = false;
                    break;
            }
            return RedirectToCurrentUmbracoUrl();
        }

        private ActionResult ShowSuccesMessage(ActionType type)
        {
            switch (type)
            {
                case ActionType.TestDrive:
                    TempData[FelixResources.temp_is_testdrive_success] = true;
                    break;
                case ActionType.Quotation:
                    TempData[FelixResources.temp_is_quotation_success] = true;
                    break;
            }
            return RedirectToCurrentUmbracoUrl();
        }

        private void SendEmailToReceiver(ActionForm model, EmailSetting mailSetting)
        {
            var emailSender = new MailAddress(mailSetting.From, mailSetting.FromName);
            var message = new MailMessage(emailSender, new MailAddress(mailSetting.To))
            {
                Subject = GetSubject(model.Type),
                Body = EmailHelper.RenderViewToString(ControllerContext, "~/Views/MailTemplates/ActionFormMail.cshtml", model)
            };
            message.AddCcMails(mailSetting.CC);

            EmailHelper.SendHTMLEmail(message);
        }

        private void SendEmailToSender(ActionForm model, EmailSetting mailSetting)
        {
            var emailSender = new MailAddress(mailSetting.From, mailSetting.FromName);
            var message = new MailMessage(emailSender, new MailAddress(model.Email))
            {
                Subject = GetSubject(model.Type),
                Body = EmailHelper.RenderViewToString(ControllerContext, "~/Views/MailTemplates/ConfirmationMail.cshtml", model)
            };

            EmailHelper.SendHTMLEmail(message);
        }

        private string GetSubject(ActionType type)
        {
            switch (type)
            {
                case ActionType.TestDrive:
                    return ConfigHelper.SubjectTestDrive;
                case ActionType.Quotation:
                    return ConfigHelper.SubjectQuotation;
                default:
                    return ConfigHelper.Subject;
            }
        }
    }
}