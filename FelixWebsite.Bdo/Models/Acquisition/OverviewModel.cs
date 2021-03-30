using FelixWebsite.Core.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using File = System.IO.File;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class DamageEntry
    {
        public DamageEntry(string damageUrl, string imageName, int mediaId, bool isSchemePicture = false)
        {
            DamageUrl = damageUrl;
            ImageName = imageName;
            MediaId = mediaId;
            IsSchemePicture = isSchemePicture;
        }
        public string DamageUrl { get; set; }
        public string ImageName { get; set; }

        public int MediaId { get; set; }

        public bool IsSchemePicture { get; set; }
    }

    public static class ImageFileDownloadExtensions
    {
        public static void AddFile(this Dictionary<string, byte[]> files, Func<string, string> folderPath, string url, string fileName)
        {
            if (url != null)
            {
                try
                {
                    files.Add(fileName + Path.GetExtension(url), File.ReadAllBytes(folderPath(url)));
                }
                catch (Exception e)
                {
                    LogHelper.Error<OverviewModel>($"Something went wrong while getting the image file for '{fileName}'. The thrown error is: ", e); ; ;
                }
            }
        }

        public static void AddFile(this Dictionary<string, byte[]> files, IMediaService service, string url, string fileName)
        {
            if (url != null)
            {
                try
                {
                    var folderFileName = fileName + Path.GetExtension(url);
                    AddByteArrayToFiles(files, service, url, folderFileName);
                }
                catch (Exception e)
                {
                    LogHelper.Error<OverviewModel>($"Something went wrong while getting the image file for '{fileName}'. The thrown error is: ", e); ; ;
                }
            }
        }

        public static void AddDamageFiles(this Dictionary<string, byte[]> files, Func<string, string> folderPath, List<DamageEntry> damages, string fileName)
        {
            var number = 1;
            foreach (var damage in damages)
            {
                try
                {
                    files.Add(fileName + $" ({number++})" + Path.GetExtension(damage.DamageUrl), File.ReadAllBytes(folderPath(damage.DamageUrl)));
                }
                catch (Exception e)
                {
                    LogHelper.Error<OverviewModel>($"Something went wrong while getting the image file for '{fileName} ({number})'. The thrown error is: ", e); ; ;
                }
            }
        }

        public static void AddDamageFiles(this Dictionary<string, byte[]> files, IMediaService service, List<DamageEntry> damages, string fileName)
        {
            var number = 1;
            foreach (var damage in damages)
            {
                try
                {
                    var folderFileName = fileName + $" ({number++})" + Path.GetExtension(damage.DamageUrl);
                    AddByteArrayToFiles(files, service, damage.DamageUrl, folderFileName);
                }
                catch (Exception e)
                {
                    LogHelper.Error<OverviewModel>($"Something went wrong while getting the image file for '{fileName} ({number})'. The thrown error is: ", e); ; ;
                }
            }
        }

        private static void AddByteArrayToFiles(Dictionary<string, byte[]> files, IMediaService mediaService, string url, string fileName)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var stream = mediaService.GetMediaFileContentStream(url))
            {
                using (var ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    files.Add(fileName, ms.ToArray());
                }
            }
        }
    }

    public class OverviewModel
    {
        protected List<DamageEntry> OverviewUrls { get; set; }

        public string LeftFrontUrl { get; set; }
        public string RightFrontUrl { get; set; }
        public string LeftBackUrl { get; set; }
        public string RightBackUrl { get; set; }
        public string FrontseatsUrl { get; set; }
        public string BackseatsUrl { get; set; }
        public string DashboardUrl { get; set; }
        public string KmUrl { get; set; }
        public List<DamageEntry> DmgOutsideUrls { get; set; }
        public List<DamageEntry> DmgInsideUrls { get; set; }
        public string EnrollmentFrontUrl { get; set; }
        public string EnrollmentBackUrl { get; set; }
        public string ExaminationUrl { get; set; }

        public string ChassisNumberUrl { get; set; }

        public string ProofOfInsuranceUrl { get; set; }

        public int AmountOfDamagePictures { get; set; }

        public bool IsInsured { get; set; }

        public IHtmlString OverviewText { get; set; }

        public UserInformation UserInformation { get; set; }

        public bool IsTakeOver { get; set; }

        public OverviewModel()
        {
            DmgOutsideUrls = new List<DamageEntry>();
            DmgInsideUrls = new List<DamageEntry>();
            OverviewUrls = new List<DamageEntry>();
        }

        public Dictionary<string, byte[]> GetAllImages(Func<string, string> path, IMediaService mediaService)
        {
            var overviewUrls = OverviewUrls.Where(x => !x.IsSchemePicture);

            return GetImageFiles(overviewUrls, path, mediaService);
        }


        public Dictionary<string, byte[]> GetAllImagesWithoutScheme(Func<string, string> path, IMediaService mediaService)
        {
            var overviewUrls = OverviewUrls.Where(x => !x.IsSchemePicture);

            return GetImageFiles(overviewUrls, path, mediaService);
        }


        public Dictionary<string, byte[]> GetAllSchemeImages(Func<string, string> path, IMediaService mediaService)
        {
            var overviewUrls = OverviewUrls.Where(x => x.IsSchemePicture);

            return GetImageFiles(overviewUrls, path, mediaService);
        }

        public virtual void SetOverviewUrls()
        {
            OverviewUrls.Add(new DamageEntry(LeftFrontUrl, FelixResources.acquisition_photos_leftFrontTitle, 0));
            OverviewUrls.Add(new DamageEntry(RightFrontUrl, FelixResources.acquisition_photos_rightFrontTitle, 0));
            OverviewUrls.Add(new DamageEntry(LeftBackUrl, FelixResources.acquisition_photos_leftBackTitle, 0));
            OverviewUrls.Add(new DamageEntry(RightBackUrl, FelixResources.acquisition_photos_rightBackTitle, 0));

            OverviewUrls.Add(new DamageEntry(FrontseatsUrl, FelixResources.acquisition_photos_frontseatsTitle, 0));
            OverviewUrls.Add(new DamageEntry(BackseatsUrl, FelixResources.acquisition_photos_backseatsTitle, 0));
            OverviewUrls.Add(new DamageEntry(DashboardUrl, FelixResources.acquisition_photos_dashTitle, 0));
            OverviewUrls.Add(new DamageEntry(KmUrl, FelixResources.acquisition_photos_kmTitle, 0));

            OverviewUrls.Add(new DamageEntry(EnrollmentFrontUrl, FelixResources.acquisition_photos_frontEnrollmentTitle, 0));
            OverviewUrls.Add(new DamageEntry(EnrollmentBackUrl, FelixResources.acquisition_photos_backEnrollmentTitle, 0));
            OverviewUrls.Add(new DamageEntry(ExaminationUrl, FelixResources.acquisition_photos_examinationProofTitle, 0));

            OverviewUrls.Add(new DamageEntry(ChassisNumberUrl, FelixResources.acquisition_photos_chassisNumberTitle, 0));
            OverviewUrls.Add(new DamageEntry(ProofOfInsuranceUrl, FelixResources.acquisition_photos_proofOfInsuranceTitle, 0));
        }

        private Dictionary<string, byte[]> GetImageFiles(IEnumerable<DamageEntry> overviewUrls, Func<string, string> path, IMediaService mediaService)
        {
            var imageFiles = new Dictionary<string, byte[]>();

            foreach (var urlObject in overviewUrls)
            {
                if (urlObject == null || string.IsNullOrEmpty(urlObject.DamageUrl)) continue;
                if (urlObject.DamageUrl.Contains("img/acquisition"))
                {
                    imageFiles.AddFile(path, urlObject.DamageUrl, urlObject.ImageName);
                }
                else
                {
                    imageFiles.AddFile(mediaService, urlObject.DamageUrl, urlObject.ImageName);
                }
            }

            imageFiles.AddDamageFiles(mediaService, DmgOutsideUrls, "Schade buitenkant");
            imageFiles.AddDamageFiles(mediaService, DmgInsideUrls, "Schade binnenkant");

            return imageFiles;
        }
    }
}
