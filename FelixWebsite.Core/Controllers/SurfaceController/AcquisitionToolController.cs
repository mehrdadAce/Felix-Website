using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AutoMapper;
using FelixWebsite.Bdo.Api;
using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Helpers;
using FelixWebsite.Bll.Services;
using FelixWebsite.Bll.Services.Interfaces;
using FelixWebsite.Core.Constants;
using FelixWebsite.Core.enums;
using FelixWebsite.Core.Helpers;
using FelixWebsite.Shared.Constants;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageResizer;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.SurfaceController
{
    public class AcquisitionToolController : BaseCarrosserieLinkedSurfaceController
    {
        private IAcquisitionInformationService acquisitionInformationService;
        private IPhotoService photoService;
        private IMediaService mediaService;
        private ISchemeDamageService schemeDamageService;

        public AcquisitionToolController(IAcquisitionInformationService acquisitionInformationService, IPhotoService photoService
            , ISchemeDamageService schemeDamageService)
        {
            this.acquisitionInformationService = acquisitionInformationService;
            this.photoService = photoService;
            mediaService = Services.MediaService;
            this.schemeDamageService = schemeDamageService;
        }

        [HttpGet]
        public ActionResult GetOverviewData(int id)
        {
            var model = photoService.GetOverviewModelData(id, mediaService, Umbraco);
            if (model != null)
            {
                var overname = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault()?.Children.OfType<Overname>().FirstOrDefault();
                model.OverviewText = overname?.OverviewText ?? null;
            }
            return PartialView("~/Views/Partials/Acquisition/Overview.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetOverviewDataDamages(int id)
        {
            var model = photoService.GetOverviewModelDataDamages(id, mediaService, Umbraco, Logger, Server);
            if (model != null)
            {
                var damage = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault()?.Children.OfType<Damage>().FirstOrDefault();
                model.OverviewText = damage?.OverviewText ?? new HtmlString(string.Empty);
            }
            return PartialView("~/Views/Partials/Acquisition/OverviewForDamages.cshtml", model);
        }

        [HttpGet]
        public JsonResult GetUserInformation(int id)
        {
            var model = acquisitionInformationService.GetById(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostTakeOverInformation(UserDataTakeOver userData, int businessPageId)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Niet alle velden zijn juist ingevuld", JsonRequestBehavior.AllowGet);
            }

            var userInformation = GetTakeOverInformationModel(userData, businessPageId);
            var modelId = acquisitionInformationService.SaveModel(userInformation);
            userInformation.Id = modelId;

            return Json(userInformation, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdateTakeOverInformation(UserDataTakeOver userData, int businessPageId, int userId)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Niet alle velden zijn juist ingevuld", JsonRequestBehavior.AllowGet);
            }

            var userInformation = GetTakeOverInformationModel(userData, businessPageId);
            userInformation.Id = userId;
            var modelId = acquisitionInformationService.UpdateModel(userInformation);
            userInformation.Id = modelId;

            return Json(userInformation, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostDamageInformation(UserDataDamage userData, int businessPageId)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Niet alle velden zijn juist ingevuld");
            }

            var userInformation = GetDamageInformationModel(userData, businessPageId);
            var modelId = acquisitionInformationService.SaveModel(userInformation);
            userInformation.Id = modelId;

            return Json(userInformation, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdateDamageInformation(UserDataDamage userData, int businessPageId, int userId)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Niet alle velden zijn juist ingevuld");
            }

            var userInformation = GetDamageInformationModel(userData, businessPageId);
            userInformation.Id = userId;
            var modelId = acquisitionInformationService.UpdateModel(userInformation);
            userInformation.Id = modelId;

            return Json(userInformation, JsonRequestBehavior.AllowGet);
        }

        private UserInformation GetDamageInformationModel(UserDataDamage userData, int businessPageId)
        {
            var userInformation = Mapper.Map<UserInformation>(userData);
            userInformation.IsAcquisitionFlowCompleted = false;
            userInformation.IsTakeOver = false;
            userInformation.UmbracoPageId = businessPageId;
            if (!userInformation.IsInsured)
            {
                userInformation.AgentName = null;
                userInformation.AgentEmail = null;
                userInformation.AgentCity = null;
                userInformation.AgentCellphone = null;
                userInformation.Insurance = null;
            }
            return userInformation;
        }

        private UserInformation GetTakeOverInformationModel(UserDataTakeOver userData, int businessPageId)
        {
            var userInformation = Mapper.Map<UserInformation>(userData);
            userInformation.IsAcquisitionFlowCompleted = false;
            userInformation.IsTakeOver = true;
            userInformation.UmbracoPageId = businessPageId;
            return userInformation;
        }

        [HttpPost]
        public ActionResult PostPhotos(string imageType, string id, string name, string licensePlate)
        {
            if (!AreAllImagesLandscape())
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("De foto moet in landschap genomen zijn.", JsonRequestBehavior.AllowGet);
            }

            PhotoInfo photoInfo = photoService.GetByImageType(imageType, short.Parse(id));

            var imageIds = new List<int>();
            if (photoInfo != null && !SchemeDamageNames.SCHEME_DAMAGE_NAMES.Contains(imageType))// Zal altijd 1 foto zijn, update staat niet toe dat je twee foto's tegelijk update
            {
                var mediaId = photoInfo.MediaId;
                DeleteMediaFromMediaFolder(mediaId);

                var folderName = CreateFolderName(licensePlate, name, id);
                var parentFolder = mediaService.GetById(GetMediaFolderId());
                var childFolder = parentFolder.Children().FirstOrDefault(child => child.Name == folderName);

                if (Request.Files.Count == 0) return Json("", JsonRequestBehavior.AllowGet);// Zou nooit mogen gebeuren
                var file = Request.Files[0];
                var newMediaId = CreateMedia(imageType, childFolder, file);
                photoInfo.MediaId = newMediaId;
                photoService.UpdateModel(photoInfo);
                imageIds.Add(newMediaId);
            }
            else
            {
                imageIds = CreateNewPhoto(imageType, id, name, licensePlate);
            }
            if (imageIds == null)
                imageIds = new List<int>();

            var mediaValues = PhotoHelper.GetMediaValuesList(imageIds, mediaService, Logger);

            return Json(mediaValues, JsonRequestBehavior.AllowGet);// nakijken dat in frontend naam wordt weergegeven, dan naam in damages zetten, naam terug naar delete sturen
        }

        [HttpPost]
        public ActionResult AddOverviewDamagePhoto(string imageType, int id, string name, string licensePlate, bool isTakeOver)
        {
            if (!AreAllImagesLandscape())
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("De foto moet in landschap genomen zijn.", JsonRequestBehavior.AllowGet);
            }

            var newDamageIds = CreateNewPhoto(imageType, id.ToString(), name, licensePlate);
            if (isTakeOver)
            {
                return GetOverviewData(id);
            }
            else
            {
                return GetOverviewDataDamages(id);
            }
        }

        [HttpGet]
        public JsonResult GetPhotoInfoesForScheme(int userId, string photoIdentifier)
        {
            var photoInfoes = photoService.GetAllByImageType(photoIdentifier, userId);
            var mediaIds = photoInfoes.Select(x => x.MediaId);
            var mediaValues = PhotoHelper.GetMediaValuesList(mediaIds, mediaService, Logger);
            return Json(mediaValues, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> FinishOverview(string message, string id, int businessPageId)
        {
            if (!CheckId(id)) return Json(null, JsonRequestBehavior.AllowGet);
            var userInformation = acquisitionInformationService.GetById(short.Parse(id));
            userInformation.IsAcquisitionFlowCompleted = true;
            if (message != string.Empty)
            {
                userInformation.Remarks = message;
            }
            acquisitionInformationService.UpdateModel(userInformation);

            bool sendPdfToClient;
            bool sendPdfToFelix;
            GetSendMailData(out sendPdfToClient, out sendPdfToFelix);

            var filename = PdfGenerator.GeneratePdf(userInformation, businessPageId, photoService, mediaService, schemeDamageService,
                    Logger, Server, HostingEnvironment.ApplicationPhysicalPath, Umbraco, IsTakeOver(Request));

            SendEmailToClient(userInformation, sendPdfToClient ? filename : string.Empty);
            SendEmailToFelix(id, sendPdfToFelix ? filename : string.Empty);
            if (!userInformation.IsTakeOver)
            {
                await SendDamageDeclarationToCarrosseriePlatform(userInformation, filename);
            }

            return Json(userInformation, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public ActionResult DeletePhoto(int userId, int mediaId)
        {
            var media = mediaService.GetById(mediaId);
            if (media == null) { return Json(false, JsonRequestBehavior.AllowGet); }

            var photoInfo = photoService.GetByMediaId(media.Id, userId);
            if (photoInfo == null) { return Json(false, JsonRequestBehavior.AllowGet); }

            var successFul = photoService.DeleteModel(photoInfo.Id, userId);
            DeleteMediaFromMediaFolder(media.Id);
            return Json(successFul, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public ActionResult UpdatePhotos(string imageType, string id, string name, string licensePlate, bool isTakeOver, bool isOverview = false, int index = 0, int mediaId = 0)
        {
            if (!AreAllImagesLandscape())
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("De foto moet in landschap genomen zijn.", JsonRequestBehavior.AllowGet);
            }

            if (!CheckId(id)) return Json(null, JsonRequestBehavior.AllowGet);

            var shortId = short.Parse(id);
            var folderName = CreateFolderName(licensePlate, name, id);
            var parentFolder = mediaService.GetById(GetMediaFolderId());
            var childFolder = parentFolder.Children().FirstOrDefault(child => child.Name == folderName);
            PhotoInfo photoInfo = photoService.GetByImageType(imageType, shortId, isOverview, index);

            if (photoInfo == null && PhotoService.SelectPhotoIdentifier(imageType) == PhotoIdentifier.Examination)
            {
                var examinationId = SaveNewImage(imageType, childFolder, id);
                return isTakeOver ? GetOverviewData(shortId) : GetOverviewDataDamages(shortId);
            }

            if (photoInfo == null) return Json(null, JsonRequestBehavior.AllowGet);

            mediaId = mediaId == 0 ? photoInfo.MediaId : mediaId;
            DeleteMediaFromMediaFolder(mediaId);
            if (Request.Files.Count == 0) return Json(null, JsonRequestBehavior.AllowGet);
            var file = Request.Files[0];
            var newMediaId = CreateMedia(imageType, childFolder, file);
            photoInfo.MediaId = newMediaId;
            photoService.UpdateModel(photoInfo);
            return isTakeOver ? GetOverviewData(shortId) : GetOverviewDataDamages(shortId);
        }

        private void SendEmailToFelix(string id, string pdfFilename)
        {
            var home = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault();
            if (home == null)
            {
                LogHelper.Debug(GetType(), "The homenode is null, no email sent to felix.");
                return;
            }

            EmailSetting mailSetting;
            if (IsTakeOver(Request))
            {
                var acquisition = home.Children.OfType<Overname>().FirstOrDefault();
                mailSetting = acquisition.TakeOverEmailSettings as EmailSetting;
            }
            else
            {
                var acquisition = home.Children.OfType<Damage>().FirstOrDefault();
                mailSetting = acquisition.TakeOverEmailSettings as EmailSetting;
            }

            string mediaLink;
            var sectionPath = GetSectionPath(IsTakeOver(Request));

            if (id == "-1")
            {
                mediaLink = "https://" + Request.Url.Host + "/umbraco#/" +
                            App_Plugins.AcquisitionTool.Constants.ACQUISITION_TOOL_ALIAS;
            }
            else
            {
                mediaLink = "https://" + Request.Url.Host + "/umbraco#/" + sectionPath + id;
            }

            var emailSender = new MailAddress(mailSetting.From, mailSetting.FromName);
            var acquisitionView = IsTakeOver(Request) ? "~/Views/MailTemplates/AcquisitionMailToFelix.cshtml" : "~/Views/MailTemplates/AcquisitionMailToFelixDamages.cshtml";

            var message = new MailMessage(emailSender, new MailAddress(mailSetting.To))
            {
                Subject = GetMailSubject(),
                Body = EmailHelper.RenderViewToString(ControllerContext, acquisitionView, mediaLink)
            };
            message.AddCcMails(mailSetting.CC);

            if (!string.IsNullOrEmpty(pdfFilename))
            {
                message.Attachments.Add(new Attachment(pdfFilename));
            }

            EmailHelper.SendHTMLEmail(message);
        }

        private string GetSectionPath(bool isTakeOver)
        {
            if (isTakeOver)
            {
                return App_Plugins.AcquisitionTool.Constants.ACQUISITION_TOOL_ALIAS + "/" +
                                 App_Plugins.AcquisitionTool.Constants.ACQUISITION_TOOL_TREE_ALIAS + "/" + "OvernameVoorbeeld.html%3FacquisitionId=";
            }
            else
            {
                return App_Plugins.DamageTool.Constants.DAMAGE_TOOL_ALIAS + "/" +
                              App_Plugins.DamageTool.Constants.DAMAGE_TOOL_TREE_ALIAS + "/" + "OvernameVoorbeeld.html%3FacquisitionId=";
            }
        }

        private void SendEmailToClient(UserInformation user, string pdfFilename)
        {
            EmailSetting mailSetting;
            if (IsTakeOver(Request))
            {
                var acquisition = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault().Children.OfType<Overname>().FirstOrDefault();
                mailSetting = acquisition.TakeOverEmailSettings as EmailSetting;
            }
            else
            {
                var acquisition = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault().Children.OfType<Damage>().FirstOrDefault();
                mailSetting = acquisition.TakeOverEmailSettings as EmailSetting;
            }

            var emailSender = new MailAddress(mailSetting.From, mailSetting.FromName);

            var message = new MailMessage(emailSender, new MailAddress(user.Email))
            {
                Subject = GetMailSubject(),
                Body = EmailHelper.RenderViewToString(ControllerContext, "~/Views/MailTemplates/AcquisitionMailToUser.cshtml", user)
            };

            if (!string.IsNullOrEmpty(pdfFilename))
            {
                message.Attachments.Add(new Attachment(pdfFilename));
            }

            EmailHelper.SendHTMLEmail(message);
        }

        private async Task<bool> SendDamageDeclarationToCarrosseriePlatform(UserInformation userInfo, string pdfFileName)
        {
            string carrosseriePlatFormLink = GetCarrosseriePlatformLinkFromBreadcrumbRoot();
            var damageDeclarationApiUrl = carrosseriePlatFormLink + ConfigHelper.DamageDeclarationApiUrl;
            var authToken = ConfigHelper.CarrosserieApiAuthToken;

            #region Additional files

            var additionalFiles = new List<DamageDeclaration.File>();

            //default images
            var photoInfos = photoService.GetAllPhotoIdsWithUserInfoId(userInfo.Id);
            var mediaItems = PhotoHelper.GetMediaValuesList(photoInfos.Select(i => i.MediaId), mediaService, Logger);
            foreach (var picture in mediaItems)
            {
                additionalFiles.Add(new DamageDeclaration.File
                {
                    Filename = $"{picture.MediaName}{Path.GetExtension(picture.MediaUrl)}",
                    Contents = PhotoHelper.ReadMediaToByteArray(picture.MediaUrl, mediaService)
                });
            }

            //Damage images
            var damagePhotoInfos = PhotoHelper.GetPhotoInfosForScheme(userInfo.Id, photoService, mediaService, Logger);
            foreach (var picture in damagePhotoInfos.SelectMany(p => p.SchemePictures))
            {
                additionalFiles.Add(new DamageDeclaration.File
                {
                    Filename = $"{picture.MediaName}{Path.GetExtension(picture.MediaUrl)}",
                    Contents = PhotoHelper.ReadMediaToByteArray(picture.MediaUrl, mediaService)
                });
            }

            #endregion Additional files

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Auth-Token", authToken);
                var json = await client.PostAsJsonAsync(damageDeclarationApiUrl,
                    new DamageDeclaration(userInfo.Name, userInfo.Email, userInfo.Gsm, userInfo.LicensePlate,
                            userInfo.IsInsured ? userInfo.Insurance : string.Empty, pdfFileName, additionalFiles));
                return await json.Content.ReadAsAsync<bool>();
            }
        }

        public ActionResult GeneratePdf(int id, int businessPageId)
        {
            var userInformation = acquisitionInformationService.GetById(id);

            var resultPdfLocation = PdfGenerator.GeneratePdf(userInformation, businessPageId, photoService, mediaService, schemeDamageService,
                Logger, Server, HostingEnvironment.ApplicationPhysicalPath, Umbraco, IsTakeOver(Request));

            return Json(userInformation, JsonRequestBehavior.AllowGet);
        }

        private List<int> CreateNewPhoto(string imageType, string id, string name, string licensePlate)
        {
            if (!CheckId(id)) return null;
            var folderName = CreateFolderName(licensePlate, name, id);
            var parentFolder = mediaService.GetById(GetMediaFolderId());
            LogHelper.Debug(GetType(), $"Using parent folder with id {parentFolder?.Id}");
            var rootMediaFolders = Umbraco.TypedMediaAtRoot().ToList();

            var isTakeOver = IsTakeOver(Request);
            if (parentFolder.Children().All(child => child.Name != folderName) &&
                (imageType == AcquisitionImageTypes.LEFT_FRONT
                || (!isTakeOver && SchemeDamageNames.SCHEME_DAMAGE_NAMES.Contains(imageType))))
            {
                var folder = mediaService.CreateMedia(folderName, parentFolder, "Folder");
                mediaService.Save(folder);
            }

            var childFolder = parentFolder.Children().FirstOrDefault(child => child.Name == folderName);
            var mediaIds = SaveNewImage(imageType, childFolder, id);
            return mediaIds;
        }

        private int CreateMedia(string imageType, IMedia childFolder, HttpPostedFileBase requestFile)
        {
            var media = mediaService.CreateMedia(imageType, childFolder.Id, "Image");

            //Picture resize when allready uploaded is useless!! (Keeping original file size)
            //MemoryStream stream = ResizePicture(requestFile);
            //media.SetValue("umbracoFile", requestFile.FileName, stream);

            media.SetValue("umbracoFile", requestFile.FileName, requestFile.InputStream);
            mediaService.Save(media);

            return media.Id;
        }

        private static MemoryStream ResizePicture(HttpPostedFileBase requestFile)
        {
            var width = ConfigHelper.UploadedPictureWidth;
            requestFile.InputStream.Seek(0, SeekOrigin.Begin);

            var stream = new MemoryStream();
            using (ImageFactory imageFactory = new ImageFactory(true))
            {
                ResizeLayer layer = new ResizeLayer(new Size((int)width, 0), ResizeMode.Max)
                {
                    Upscale = false
                };

                imageFactory.Load(requestFile.InputStream)
                            .Resize(layer)
                            .Save(stream);
            }

            return stream;
        }

        private void DeleteMediaFromMediaFolder(int mediaId)
        {
            try
            {
                var media = mediaService.GetById(mediaId);
                mediaService.Delete(media);
            }
            catch (Exception e)
            {
                return;
            }
        }

        private List<int> SaveNewImage(string imageType, IMedia childFolder, string userId)
        {
            try
            {
                if (Request.Files.Count == 0) return null;

                var mediaIds = new List<int>();
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    var requestFile = Request.Files[i];
                    var mediaId = CreateMedia(imageType, childFolder, requestFile);
                    var identifier = PhotoService.SelectPhotoIdentifier(imageType);
                    var photoIds = new PhotoInfo(mediaId, short.Parse(userId), identifier);
                    photoService.SaveModel(photoIds);
                    mediaIds.Add(mediaId);
                }

                return mediaIds;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Error thrown when trying to create a new image in the database. The error is: ", e);
                return null;
            }
        }

        private string CreateFolderName(string licensePlate, string name, string userId)
        {
            return licensePlate + " " + name + " " + userId;
        }

        private bool CheckId(string id)
        {
            if (id != "-1") return true;
            LogHelper.Debug(GetType(), "No userid was set when the userinformation was sent, check the Js configuration.");
            return false;
        }

        private string CreateImageName(string imageNameWithoutSpaces)
        {
            var arrayOfImageName = imageNameWithoutSpaces.ToCharArray();
            var secondToLastChar = arrayOfImageName[arrayOfImageName.Length - 2];
            int resultInt;
            bool isNumeric = int.TryParse(secondToLastChar.ToString(), out resultInt);
            if (isNumeric)
            {
                var r = imageNameWithoutSpaces.Insert(arrayOfImageName.Length, ")");
                return r.Insert(arrayOfImageName.Length - 2, " (");
            }
            else
            {
                var r = imageNameWithoutSpaces.Insert(arrayOfImageName.Length, ")");
                return r.Insert(arrayOfImageName.Length - 1, " (");
            }
        }

        private bool AreAllImagesLandscape()
        {
            for (var i = 0; i < Request.Files.Count; i++)
            {
                if (!IsImageLandscape(Request.Files[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsImageLandscape(HttpPostedFileBase request)
        {
            var byteArray = new byte[request.InputStream.Length];
            request.InputStream.Read(byteArray, 0, (int)request.InputStream.Length);
            var isLandscape = false;
            using (var stream = new MemoryStream(byteArray))
            {
                using (var image = System.Drawing.Image.FromStream(stream))
                {
                    isLandscape = image.Width > image.Height;
                }
            }
            return isLandscape;
        }

        private bool IsTakeOver(HttpRequestBase request)
        {
            return request.UrlReferrer.PathAndQuery.ToLower().Contains("overname");
        }

        private void GetSendMailData(out bool sendPdfToClient, out bool sendPdfToFelix)
        {
            var isTakeOver = IsTakeOver(Request);

            if (isTakeOver)
            {
                var umbracoPage = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault()?.Children.OfType<Overname>().FirstOrDefault();
                sendPdfToClient = umbracoPage.SendPdfToClient;
                sendPdfToFelix = umbracoPage.SendPdfToFelix;
            }
            else
            {
                var umbracoPage = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault()?.Children.OfType<Damage>().FirstOrDefault();
                sendPdfToClient = umbracoPage.SendPdfToClient;
                sendPdfToFelix = umbracoPage.SendPdfToFelix;
            }
        }

        private string GetMailSubject()
        {
            var subject = IsTakeOver(Request) ? "overname" : "schadeaangifte";
            return $"Er is een nieuwe {subject} toegevoegd";
        }

        private int GetMediaFolderId()
        {
            if (IsTakeOver(Request))
            {
                return Umbraco.TypedMediaAtRoot().FirstOrDefault(x => x.Name == "Overnames")?.Id ?? 0;
            }
            else
            {
                return Umbraco.TypedMediaAtRoot().FirstOrDefault(x => x.Name == "Schadeaangiftes")?.Id ?? 0;
            }
        }

        //public IEnumerable<MediaValuesModel> GetMediaValuesList(IEnumerable<int> mediaImageIds)
        //{
        //    var mediaValues = new List<MediaValuesModel>();
        //    foreach (var imageId in mediaImageIds)
        //    {
        //        var media = mediaService.GetById(imageId);
        //        var mediaName = media.Name;
        //        var mediaNameWithoutSpecialCharacters = mediaName.Replace(" ", "").Replace("(", "").Replace(")", "");
        //        var mediaUrl = media.GetUrl("umbracoFile", Logger);
        //        mediaValues.Add(new MediaValuesModel(media.Id, mediaNameWithoutSpecialCharacters, mediaUrl));
        //    }
        //    return mediaValues;
        //}
    }
}