using FelixWebsite.Bdo.Models.JobOffer;
using FelixWebsite.Bll.Helpers.VDAB;
using FelixWebsite.Bll.Services.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Xml;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Helpers.EventHandlers
{
    public class JobOfferContentEventHandlers
    {
        private static ILog Log = LogManager.GetLogger(typeof(JobOfferContentEventHandlers));

        internal static void SavingHandler(IContentService sender, SaveEventArgs<IContent> e)
        {
            IVdabService vdabService = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IVdabService)) as IVdabService;

            foreach (var content in e.SavedEntities)
            {
                try
                {
                    if (content.ContentTypeId != Joboffer.GetModelContentType().Id)
                        return;

                    ConvertedUmbracoJobOffer jobOffer = FillDataInFrom(content);
                    jobOffer.Status = "Active";
                    XmlDocument xml = jobOffer.ConvertConvertedUmbracoJobOfferToVDABXML();

                    XmlDocument responseXML = new XmlDocument();
                    responseXML.LoadXml(System.Threading.Tasks.Task.Run(() => vdabService.PostToOnlineAssistant(xml)).GetAwaiter().GetResult());

                    // Job application rejected by online assistant.
                    if (!responseXML.InnerText.IsNullOrWhiteSpace())
                    {
                        e.Cancel = true;
                        e.Messages.Add(new EventMessage("Online assistant feedback", responseXML.InnerText, EventMessageType.Error));
                    }

                    e.Messages.Add(new EventMessage("Online assistant feedback", "Vacature werd goedgekeurd!", EventMessageType.Success));

                    XmlDocument res = new XmlDocument();
                    var response = System.Threading.Tasks.Task.Run(() => vdabService.PostToVdab(xml)).GetAwaiter().GetResult();
                    res.LoadXml(response);

                    // Check if jobapplication has been rejected during post request.
                    bool hasFileErrors = ShowFeedbackVDABCall(res.ChildNodes[1], e);

                    if (hasFileErrors == false)
                    {
                        content.SetValue("uniqeId", "FG-" + content.Key.ToString().Substring(0, 25));
                        e.Messages.Add(new EventMessage("VDAB Info", "Vacature werd succesvol gepost naar de vdab.", EventMessageType.Success));
                    }
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("Error while trying to push to VDAB", ex);
                    e.Messages.Add(new EventMessage("VDAB Info", "Vacature kon niet naar vdab gepost worden!", EventMessageType.Warning));
                }
            }
        }

        internal static void TrashingHandler(IContentService sender, MoveEventArgs<IContent> e)
        {
            try
            {
                IVdabService vdabService = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IVdabService)) as IVdabService;

                foreach (var content in e.MoveInfoCollection)
                {
                    if (content.Entity.ContentTypeId != Joboffer.GetModelContentType().Id)
                        return;

                    ConvertedUmbracoJobOffer jobOffer = FillDataInFrom(content.Entity);
                    jobOffer.Status = "Inactive";
                    XmlDocument xml = jobOffer.ConvertConvertedUmbracoJobOfferToVDABXML();

                    if (xml != null)
                    {
                        XmlDocument res = new XmlDocument();
                        res.LoadXml(System.Threading.Tasks.Task.Run(() => vdabService.PostToVdab(xml)).GetAwaiter().GetResult());

                        // Check if jobapplication has been rejected during post request.
                        bool hasFileErrors = ShowFeedbackVDABCall(res.ChildNodes[1], e);

                        if (hasFileErrors != false)
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error while trying to push to VDAB", ex);
                e.Messages.Add(new EventMessage("VDAB Info", "Vacature kon niet bij VDAB verwijderd worden!", EventMessageType.Warning));
            }
        }

        internal static void DeletingHandler(IContentService sender, DeleteEventArgs<IContent> e)
        {
            try
            {
                IVdabService vdabService = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IVdabService)) as IVdabService;

                foreach (var content in e.DeletedEntities)
                {
                    if (content.ContentTypeId != Joboffer.GetModelContentType().Id)
                        return;

                    ConvertedUmbracoJobOffer jobOffer = FillDataInFrom(content);
                    jobOffer.Status = "Inactive";
                    XmlDocument xml = jobOffer.ConvertConvertedUmbracoJobOfferToVDABXML();

                    if (xml != null)
                    {
                        XmlDocument res = new XmlDocument();
                        res.LoadXml(System.Threading.Tasks.Task.Run(() => vdabService.PostToVdab(xml)).GetAwaiter().GetResult());

                        // Check if jobapplication has been rejected during post request.
                        bool hasFileErrors = ShowFeedbackVDABCall(res.ChildNodes[1], e);

                        if (hasFileErrors != false)
                        {
                            return;
                        }
                    }
                    e.Messages.Add(new EventMessage("VDAB Info", "Vacature werd verwijderd op de VDAB.", EventMessageType.Success));
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error while trying to push to VDAB", ex);
                e.Messages.Add(new EventMessage("VDAB Info", "Vacature kon niet bij VDAB verwijderd worden!", EventMessageType.Warning));
            }
        }

        // Saving
        private static bool ShowFeedbackVDABCall(XmlNode xmlNode, SaveEventArgs<IContent> e)
        {
            bool hasContentErrors = false;
            foreach (XmlNode item in xmlNode)
            {
                if (item.Name == "Errors")
                {
                    foreach (XmlNode node in item.ChildNodes)
                    {
                        e.Messages.Add(new EventMessage("VDAB " + item.Name, node.InnerText, EventMessageType.Error));
                        hasContentErrors = true;
                    }
                }
                if (item.Name == "Warnings")
                {
                    foreach (XmlNode node in item.ChildNodes)
                    {
                        e.Messages.Add(new EventMessage("VDAB " + item.Name, node.InnerText, EventMessageType.Warning));
                    }
                }
            }
            return hasContentErrors;
        }

        // Trashing 
        private static bool ShowFeedbackVDABCall(XmlNode xmlNode, MoveEventArgs<IContent> e)
        {
            bool hasContentErrors = false;
            foreach (XmlNode item in xmlNode)
            {
                if (item.Name == "Errors")
                {
                    foreach (XmlNode node in item.ChildNodes)
                    {
                        e.Messages.Add(new EventMessage("VDAB " + item.Name, node.InnerText, EventMessageType.Error));
                        hasContentErrors = true;
                    }
                }
                if (item.Name == "Warnings")
                {
                    foreach (XmlNode node in item.ChildNodes)
                    {
                        e.Messages.Add(new EventMessage("VDAB " + item.Name, node.InnerText, EventMessageType.Warning));
                    }
                }
            }
            return hasContentErrors;
        }

        // Deleting
        private static bool ShowFeedbackVDABCall(XmlNode xmlNode, DeleteEventArgs<IContent> e)
        {
            bool hasContentErrors = false;
            foreach (XmlNode item in xmlNode)
            {
                if (item.Name == "Errors")
                {
                    foreach (XmlNode node in item.ChildNodes)
                    {
                        e.Messages.Add(new EventMessage("VDAB " + item.Name, node.InnerText, EventMessageType.Error));
                        hasContentErrors = true;
                    }
                }
                if (item.Name == "Warnings")
                {
                    foreach (XmlNode node in item.ChildNodes)
                    {
                        e.Messages.Add(new EventMessage("VDAB " + item.Name, node.InnerText, EventMessageType.Warning));
                    }
                }
            }
            return hasContentErrors;
        }

        private static ConvertedUmbracoJobOffer FillDataInFrom(IContent content)
        {
            ConvertedUmbracoJobOffer job = new ConvertedUmbracoJobOffer();

            // Set Unique Id for jobOffer
            if (!content.GetValue<string>("uniqeId").IsNullOrWhiteSpace())
            {
                job.Id = content.GetValue<string>("uniqeId");
            }
            else
            {
                job.Id = "FG-" + content.Key.ToString().Substring(0, 25);
            }

            job.ExperienceInMonths = Convert.ToInt16(content.GetTypedValue<Joboffer, string>(o => o.ExperienceInMonths));

            job.ValidFrom = content.GetValue<DateTime>("validFrom");
            job.ValidTo = content.GetValue<DateTime>("validTo");

            job.PositionTitle = content.Name;
            job.JobDescription = content.GetValue<string>("JobDescription");
            job.RequiredQualifications = content.GetValue<string>("RequiredQualifications");
            job.RenumerationDescription = content.GetValue<string>("RenumerationDescription");

            var businessId = content.Properties.Where(x => x.Alias == "Business").FirstOrDefault().Value.ToString();
            var umbracoHelper = new Umbraco.Web.UmbracoHelper(UmbracoContext.Current);
            var business = (IBusiness)umbracoHelper.TypedContent(businessId);

            job.CompanyInfo = new CompanyInfo()
            {
                Name = business.Name,
                Mail = business.BusinessMail,
                PhoneNumber = business.BusinessPhone,
                PostalCode = business.BusinessPostalCode,
                StreetName = business.BusinessStreetName,
                Municipality = business.BusinessMunicipality,
                BuildingNumber = business.BusinessBuildingNumber
            };

            var businessJobSettings = (IBusinessJobSettings)umbracoHelper.TypedContent(businessId);
            job.VdabName = businessJobSettings.VdabName;
            job.VdabJobEmail = businessJobSettings.JobEmail;

            job.Competences = TryJsonDecode<CompetencePattern>(content.GetValue<string>("CompetenceSearch"));
            job.LaborContract = TryJsonDecode<LaborContract>(content.GetValue<string>("LaborContract"));
            job.LaborSystem = TryJsonDecode<LaborSystem>(content.GetValue<string>("LaborSystem"));
            job.LaborArrangement = TryJsonDecode<LaborArrangement>(content.GetValue<string>("LaborArrangement"));
            job.Languages = TryJsonDecode<IEnumerable<LanguageCompetence>>(content.GetValue<string>("Languages"));
            job.DriverLicense = TryJsonDecode<IEnumerable<DriversLicense>>(content.GetValue<string>("DriversLicense"));
            job.DiplomaSelector = TryJsonDecode<IEnumerable<Study>>(content.GetValue<string>("DiplomaSelector"));

            return job;
        }

        private static T TryJsonDecode<T>(string value)
        {
            try
            {
                return Json.Decode<T>(value);
            }
            catch (Exception ex)
            {
                Log.Error("Error trying to decode", ex);
                return default(T);
            }
        }
    }
}