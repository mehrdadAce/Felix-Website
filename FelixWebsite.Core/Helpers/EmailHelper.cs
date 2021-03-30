using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Web.Mvc;
using Umbraco.Core.Logging;

namespace FelixWebsite.Core.Helpers
{
    public static class EmailHelper
    {
        public static void AddCcMails(this MailMessage message, IEnumerable<string> emails)
        {
            foreach (var email in emails)
            {
                message.CC.Add(email);
            }
        }

        public static string RenderViewToString(ControllerContext controllerContext, string viewName, object model)
        {
            try
            {
                controllerContext.Controller.ViewData.Model = model;
                using (var sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                    var viewContext = new ViewContext(controllerContext, viewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(typeof(EmailHelper), $"Something went wrong when rendering razor to html string. No emails were sent. The thrown error is: ", e);
                return null;
            }
        }

        public static void SendHTMLEmail(MailMessage message)
        {
            try
            {
                var client = new SmtpClient();
                message.IsBodyHtml = true;
                client.Send(message);
            }
            catch (Exception e)
            {
                LogHelper.Error(typeof(EmailHelper), $"Something went wrong when sending an email from {message.From} to {message.To}. No emails were sent. The thrown error is: ", e);
            }
        }
    }
}
