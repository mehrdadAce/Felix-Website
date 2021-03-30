using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Helpers;
using FelixWebsite.Bll.Services.Interfaces;
using FelixWebsite.Core.Helpers;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Controllers.UmbracoAuthorizedControllers
{
    public class AcquisitionToolController : UmbracoAuthorizedController
    {
        private IAcquisitionInformationService acquisitionInformationService;
        private IPhotoService photoService;
        private IMediaService mediaService;
        private ISchemeDamageService schemeDamageService;
        public AcquisitionToolController(IAcquisitionInformationService acquisitionInformationService, IPhotoService photoService,
            ISchemeDamageService schemeDamageService)
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
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAcquisitionWithId(int id)
        {
            var information = acquisitionInformationService.GetById(id);
            return Json(information, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileResult DownloadAcquisitionFiles(int id, bool isTakeOver)
        {
            try
            {
                var information = acquisitionInformationService.GetById(id);

                var model = isTakeOver 
                    ? photoService.GetOverviewModelData(id, mediaService, Umbraco)
                    : photoService.GetOverviewModelDataDamages(id, mediaService, Umbraco, Logger, Server);      

                var service = Services.MediaService;

                model.SetOverviewUrls();
                var images = model.GetAllImagesWithoutScheme(Server.MapPath, service);
                var schemeImages = model.GetAllSchemeImages(Server.MapPath, service);
                var folderName = $"{information.LicensePlate} {information.Name}";
                var grouphome = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault();
                var appPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                var businessPageId = information.UmbracoPageId != 0 ? information.UmbracoPageId : grouphome?.Id ?? 0;
                var pdf = PdfGenerator.GeneratePdf(information, businessPageId, photoService, mediaService, schemeDamageService, Logger, Server, appPath, Umbraco,
                   isTakeOver);

                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var image in images)
                        {
                            var file = archive.CreateEntry(image.Key);
                            using (var fileStream = file.Open())
                            {
                                fileStream.Write(image.Value, 0, image.Value.Length);
                            }
                        }

                        foreach (var image in schemeImages)
                        {
                            var file = archive.CreateEntry($"Schade - {image.Key}");
                            using (var fileStream = file.Open())
                            {
                                fileStream.Write(image.Value, 0, image.Value.Length);
                            }
                        }

                        if (!string.IsNullOrEmpty(pdf))
                        {
                            var pdfFile = archive.CreateEntry($"{folderName}.pdf");
                            using (var fileStream = pdfFile.Open())
                            {
                                using (var pdfStream = System.IO.File.Open(pdf, FileMode.Open))
                                {
                                    pdfStream.CopyTo(fileStream);
                                }
                            }
                        }
                    }

                    return File(memoryStream.ToArray(), "application/zip", $"{folderName}.zip");
                }
            }
            catch(Exception e)
            {
                Logger.Error(GetType(), "Bij het downloaden vd foto's/pdf is iets misgegaan: ", e);
                return null;
            }
        }
    }
}
