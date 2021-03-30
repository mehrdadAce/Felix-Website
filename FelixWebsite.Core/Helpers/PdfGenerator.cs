using PdfSharp.Pdf;
using PdfSharp.Drawing;
using FelixWebsite.Bdo.Models.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using FelixWebsite.Core.App_GlobalResources;
using FelixWebsite.Bdo.Enums;
using Umbraco.Web.PublishedContentModels;
using System.Web;
using System.Web.Hosting;
using Umbraco.Core.Logging;
using FelixWebsite.Bll.Services.Interfaces;
using Umbraco.Core.Services;
using Umbraco.Core.Models;
using Umbraco.Web;
using FelixWebsite.Bll.Services;
using System.Diagnostics;
using OfficeOpenXml.Style.XmlAccess;
using System.Windows.Forms;
using System.Drawing;
using Umbraco.Core;
using FelixWebsite.Bll.Helpers;
using System.IO;

namespace FelixWebsite.Core.Helpers
{
    public static class PdfGenerator
    {
        private static int pageWidth = 500;
        private static int pageXOffset = 50;
        private static int pageYOffset = 100;

        private static int xOffset = 300;
        private static int yOffset = 185;

        private static int textYOffset = 15;
        private static int imageWidth = 240;
        private static int imageHeight = 150;
        private static int heightBetweenLines = 20;
        private static XFont boldFont;
        private static XFont normalFont;
        private static XFont smallFont;
        private static List<string> schemeUrls;
        private static Font normalFontForms;
        private static IMediaService mediaService;
        private static string applicationPath;
        private static ILogger logger;

        static PdfGenerator()
        {
            boldFont = new XFont("Verdana", 11, XFontStyle.Bold);
            normalFont = new XFont("Verdana", 11, XFontStyle.Regular);
            smallFont = new XFont("Verdana", 10, XFontStyle.Regular);
            normalFontForms = new Font("Verdana", 11, FontStyle.Regular);
            schemeUrls = new List<string>();
        }

        public static string GeneratePdf(UserInformation userInformation, int businessPageId, IPhotoService photoService, IMediaService mediaService, ISchemeDamageService schemeDamageService,
            ILogger logger, HttpServerUtilityBase server, string applicationPath, Umbraco.Web.UmbracoHelper umbraco, bool isTakeOver)
        {
            try
            {
                PdfGenerator.mediaService = mediaService;
                PdfGenerator.applicationPath = applicationPath;
                PdfGenerator.logger = logger;
                List<PdfModel> pdfModels = GetPdfPhotos(userInformation, photoService, mediaService, logger);

                if (umbraco.TypedContent(businessPageId).OfType<GroupHome>() != null)
                {
                    businessPageId = umbraco.TypedContent(businessPageId).OfType<GroupHome>().Children()
                        .OfType<Businesses>().FirstOrDefault().Children()
                        .OfType<BusinessBrand>().FirstOrDefault()?.Id ?? 0;
                }

                PdfSchemeModel schemeModel = null;
                if (!userInformation.IsTakeOver)
                {
                    var allSchemeDamagesForUser = schemeDamageService.GetAllForUserId(userInformation.Id);
                    var allSchemeDamagePhotos = PhotoHelper.GetPhotoInfosForScheme(userInformation.Id, photoService, mediaService, logger);
                    SetSchemeEntries(allSchemeDamagePhotos);
                    allSchemeDamagesForUser = GetSchemeDamages(allSchemeDamagesForUser, allSchemeDamagePhotos);
                    schemeModel = new PdfSchemeModel(allSchemeDamagesForUser, allSchemeDamagePhotos);
                }

                var businessPage = umbraco.TypedContent(businessPageId).OfType<BusinessBrand>();
                if (businessPage == null)
                {
                    var businessCombined = umbraco.TypedContent(businessPageId).OfType<BusinessCombined>();
                    businessPage = businessCombined.BusinessItems.OfType<BusinessBrand>().FirstOrDefault();
                }
                var brandPage = businessPage.Brand.FirstOrDefault().OfType<Brand>();
                var businessBrandLogo = businessPage?.Logo;
                var brandLogo = brandPage?.Logo;
                var backgroundColor = brandPage?.Color?.Color;

                var businessBrandLogoMedia = mediaService.GetById(businessBrandLogo.Id);
                var businessBrandLogoUrl = businessBrandLogoMedia.GetUrl("umbracoFile", logger);
                var brandLogoMedia = mediaService.GetById(brandLogo.Id);
                var brandLogoUrl = brandLogoMedia.GetUrl("umbracoFile", logger);
                var logoType = GetLogoType(businessPage.Name);

                if (!isTakeOver)
                {
                    schemeUrls.Add("/img/acquisition/scheme/schema-voertuig.png");
                    schemeUrls.AddRange(GetAllColoredImagesForDamages(userInformation.Id, schemeDamageService, schemeModel.SchemeDamages));
                }
                return CreatePdf(userInformation, pdfModels, schemeModel, isTakeOver, businessBrandLogoUrl, brandLogoUrl, backgroundColor, logoType, logger);
            }
            catch (Exception e)
            {
                logger.Error(typeof(PdfGenerator), "Something went wrong: ", e);
                return string.Empty;
            }
        }

        public static List<PdfModel> GetPdfPhotos(UserInformation userInformation, IPhotoService photoService, IMediaService mediaService, ILogger logger)
        {
            var pdfModels = new List<PdfModel>();

            var mediaIds = photoService.GetAllPhotoIdsWithUserInfoId(userInformation.Id);
            var allMedia = mediaService.GetByIds(mediaIds.Select(x => x.MediaId));

            foreach (var media in allMedia)
            {
                var uFilePath = media.GetUrl("umbracoFile", logger);
                pdfModels.Add(new PdfModel { Path = uFilePath, Name = media.Name });
            }

            return pdfModels;
        }

        public static string CreatePdf(UserInformation userInformation, List<PdfModel> pdfModels, PdfSchemeModel schemeModel, bool isTakeOver,
            string businessBrandLogoUrl, string brandLogoUrl, string backgroundColor, LogoType logoType, ILogger logger)
        {
            try
            {
                PdfDocument doc = new PdfDocument();
                doc.Info.Title = "";
                logger.Info(typeof(PdfGenerator), "Made document");

                if (isTakeOver)
                {
                    CreatePdfForTakeOver(userInformation, pdfModels, doc, businessBrandLogoUrl, brandLogoUrl, backgroundColor, logoType);
                    logger.Info(typeof(PdfGenerator), "Made takeover pdf");
                }
                else
                {
                    CreatePdfForDamages(userInformation, pdfModels, schemeModel, doc, businessBrandLogoUrl, brandLogoUrl, backgroundColor, logoType);
                    logger.Info(typeof(PdfGenerator), "Made damage pdf");
                }

                string filename = $"{HostingEnvironment.ApplicationPhysicalPath}/pdf/{DateTime.Now:dd-MM-yyyy HH mm}-{userInformation.Name}.pdf";
                logger.Info(typeof(PdfGenerator), "Filename");

                doc.Save(filename);
                logger.Info(typeof(PdfGenerator), "Save file");

                // For testing purposes only! System.Diagnostics
                Process.Start(filename);
                return filename;
            }
            catch (Exception e)
            {
                logger.Error(typeof(PdfGenerator), "Something went wrong: ", e);
                return "";
            }
        }

        private static void CreatePdfForTakeOver(UserInformation userInformation, List<PdfModel> pdfModels,
            PdfDocument doc, string businessBrandLogoUrl, string brandLogoUrl, string backgroundColor, LogoType logoType)
        {
            var orderedPdfModels = OrderPdfModels(pdfModels);
            var outsidePhotos = GetPdfModelsForImageTypeList(orderedPdfModels, AcquisitionImageTypeHelper.OutsidePhotos);
            var insidePhotos = GetPdfModelsForImageTypeList(orderedPdfModels, AcquisitionImageTypeHelper.InsidePhotos);
            var damagePhotos = orderedPdfModels.Where(x => x.Name.Contains(AcquisitionImageTypeHelper.DamagePhotos[0])
            || x.Name.Contains(AcquisitionImageTypeHelper.DamagePhotos[1])).OrderBy(x => x.Name);
            var docPhotos = GetPdfModelsForImageTypeList(orderedPdfModels, AcquisitionImageTypeHelper.DocumentationPhotos);

            PdfLayoutHelper helper = new PdfLayoutHelper(doc, XUnit.FromCentimeter(2.5), XUnit.FromCentimeter(29.7 - 2.5));

            DrawFirstPage(doc, userInformation, true, businessBrandLogoUrl, brandLogoUrl, backgroundColor, logoType);
            DrawPage(doc, outsidePhotos, FelixResources.acquisition_photos_outside);
            DrawPage(doc, insidePhotos, FelixResources.acquisition_photos_inside);
            if (damagePhotos.Count() != 0)
                DamageSection(helper, damagePhotos, FelixResources.acquisition_photos_damage);
            DrawPage(doc, docPhotos, FelixResources.acquisition_photos_docs);
        }

        private static void CreatePdfForDamages(UserInformation userInformation, List<PdfModel> pdfModels, PdfSchemeModel schemeModel,
            PdfDocument doc, string businessBrandLogoUrl, string brandLogoUrl, string backgroundColor, LogoType logoType)
        {
            var orderedPdfModels = OrderPdfModels(pdfModels);
            var outsidePhotos = GetPdfModelsForImageTypeList(orderedPdfModels, AcquisitionImageTypeHelper.OutsidePhotos);
            var insidePhotos = GetPdfModelsForImageTypeList(orderedPdfModels, AcquisitionImageTypeHelper.InsidePhotosDamages);
            var damagePhotos = orderedPdfModels.Where(x => x.Name.Contains(AcquisitionImageTypeHelper.DamagePhotos[0])
            || x.Name.Contains(AcquisitionImageTypeHelper.DamagePhotos[1])).OrderBy(x => x.Name);
            var docPhotos = GetPdfModelsForImageTypeList(orderedPdfModels, AcquisitionImageTypeHelper.DocumentationPhotosDamages);

            PdfLayoutHelper helper = new PdfLayoutHelper(doc, XUnit.FromCentimeter(2.5), XUnit.FromCentimeter(29.7 - 2.5));

            DrawFirstPage(doc, userInformation, false, businessBrandLogoUrl, brandLogoUrl, backgroundColor, logoType);
            DrawDamagesPage2(helper, schemeModel);
            DrawPage(doc, outsidePhotos, FelixResources.acquisition_photos_outside);
            DrawPage(doc, insidePhotos, FelixResources.acquisition_photos_inside);
            if (damagePhotos.Count() != 0)
                DamageSection(helper, damagePhotos, FelixResources.acquisition_photos_damage);
            if (userInformation.IsInsured)
            {
                DrawPage(doc, docPhotos, FelixResources.acquisition_photos_docs);
            }
        }

        private static void DrawPage(PdfDocument doc, IEnumerable<PdfModel> pdfModels, string sectionTitle)
        {
            PdfPage page = doc.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            XGraphics gfx = XGraphics.FromPdfPage(page);

            if (pdfModels.Count() > 0)
                gfx.DrawString(sectionTitle, boldFont, XBrushes.Black, new XRect(pageXOffset, pageYOffset, 0, 0));

            var count = 0;
            foreach (var model in pdfModels)
            {
                string printableName;
                AcquisitionImageTypeHelper.AcquisitionImageTypeAndName.TryGetValue(model.Name, out printableName);
                DrawImage(gfx, normalFont, printableName, model.Path, count);
                count++;
            }
        }

        private static int DrawFirstPage(PdfDocument doc, UserInformation userInformation, bool isTakeOver, string businessBrandLogoUrl
            , string brandLogoUrl, string backgroundColor, LogoType logoType)
        {
            PdfPage page = doc.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            XGraphics gfx = XGraphics.FromPdfPage(page);
            var headerOffset = 0;

            if (!string.IsNullOrEmpty(businessBrandLogoUrl))
            {
                var businessBrandLogo = XImage.FromStream(mediaService.GetMediaFileContentStream(businessBrandLogoUrl));
                var brandLogo = XImage.FromStream(mediaService.GetMediaFileContentStream(brandLogoUrl));
                var rgbTuple = GetRgbaValues(backgroundColor);
                var brushColor = new XSolidBrush(new XColor { R = rgbTuple.Item1, G = rgbTuple.Item2, A = rgbTuple.Item3 });
                headerOffset = DrawHeader(gfx, businessBrandLogo, brandLogo, brushColor, logoType);
            }

            headerOffset += 60;

            DrawUserInformation(gfx, userInformation, headerOffset, isTakeOver);
            headerOffset += 40;
            return headerOffset;
        }

        private static void DrawDamagesPage2(PdfLayoutHelper helper, PdfSchemeModel schemeModel)
        {
            var schemeDamages = schemeModel.SchemeDamages;
            var schemePictures = schemeModel.SchemePictures;
            if (schemeDamages == null || schemeDamages.Count() == 0) return;
            var pen = new XPen(new XColor { R = 0, G = 0, A = 0 });

            var cellWidth = 10;
            var cellHeight = 10;

            XUnit top = helper.GetLinePosition(0);

            foreach (var schemeUrl in schemeUrls)
            {
                DrawImageWithPath(helper, schemeUrl, 0, top.Point, 500, 358);
            }

            top = helper.GetLinePosition(pageYOffset);
            helper.Gfx.DrawString(FelixResources._2dehands_overview_scheme_photosDamage, boldFont, XBrushes.Black, new XPoint(pageXOffset, top.Point));

            var schemeDamageList = schemeDamages.OrderBy(x => x.OrderNumber).ToList();
            helper.GetLinePosition(350);

            for (var i = 0; i < schemeDamageList.Count; i++)
            {
                var textSize = TextRenderer.MeasureText(schemeDamageList[i].Remarks, normalFontForms);
                if (helper.IsTooLargeToFitOnPage(heightBetweenLines * 2 + textSize.Height)) { helper.CreatePage(); }
                var yLocation = helper.GetLinePosition(heightBetweenLines);
                var readableName = $"{schemeDamageList[i].OrderNumber} - {SchemeDamageService.GetReadableName(schemeDamageList[i].SchemeEntry)}";
                helper.Gfx.DrawString(readableName, normalFont, XBrushes.Black, new XPoint(pageXOffset, yLocation.Point));
                var xOffset = pageXOffset;
                yLocation = helper.GetLinePosition(heightBetweenLines).Point + 10;
                if (schemeDamageList[i].Damage)
                {
                    PdfTableGenerator.DrawCellWithCross(helper.Gfx, pen, xOffset, (int)yLocation.Point - 10, cellWidth, cellHeight);
                    helper.Gfx.DrawString(FelixResources.overname_scheme_damage_light, smallFont, XBrushes.Black, new XPoint(xOffset + 15, yLocation.Point));
                    xOffset += 110;
                }
                if (schemeDamageList[i].HeavyDamage)
                {
                    PdfTableGenerator.DrawCellWithCross(helper.Gfx, pen, xOffset, (int)yLocation.Point - 10, cellWidth, cellHeight);
                    helper.Gfx.DrawString(FelixResources.overname_scheme_damage_heavy, smallFont, XBrushes.Black, new XPoint(xOffset + 15, yLocation.Point));
                    xOffset += 110;
                }
                if (schemeDamageList[i].Dent)
                {
                    PdfTableGenerator.DrawCellWithCross(helper.Gfx, pen, xOffset, (int)yLocation.Point - 10, cellWidth, cellHeight);
                    helper.Gfx.DrawString(FelixResources.overname_scheme_damage, smallFont, XBrushes.Black, new XPoint(xOffset + 15, yLocation.Point));
                    xOffset += 110;
                }
                if (schemeDamageList[i].HailDamage)
                {
                    PdfTableGenerator.DrawCellWithCross(helper.Gfx, pen, xOffset, (int)yLocation.Point - 10, cellWidth, cellHeight);
                    helper.Gfx.DrawString(FelixResources.overname_scheme_hail_damage, smallFont, XBrushes.Black, new XPoint(xOffset + 15, yLocation.Point));
                    xOffset += 110;
                }
                if (schemeDamageList[i].Marks)
                {
                    PdfTableGenerator.DrawCellWithCross(helper.Gfx, pen, xOffset, (int)yLocation.Point - 10, cellWidth, cellHeight);
                    helper.Gfx.DrawString(FelixResources.overname_scheme_marks, smallFont, XBrushes.Black, new XPoint(xOffset + 15, yLocation.Point));
                    xOffset += 80;
                }
                var hasPictures = schemePictures.Any(x => x.SchemeEntry == schemeDamageList[i].SchemeEntry);
                if (!string.IsNullOrEmpty(schemeDamageList[i].Remarks))
                {
                    var textFormatter = new PdfSharp.Drawing.Layout.XTextFormatter(helper.Gfx);
                    var htmlString = new HtmlString(schemeDamageList[i].Remarks);
                    var hasEntryAnyDamages = schemeDamageList[i].Damage
                        || schemeDamageList[i].HeavyDamage
                        || schemeDamageList[i].Dent
                        || schemeDamageList[i].HailDamage
                        || schemeDamageList[i].Marks;
                    var remarksYLocation = helper.GetLinePosition(0);
                    textFormatter.DrawString(htmlString.ToString(), normalFont, XBrushes.DarkGray,
                        new XRect(pageXOffset, remarksYLocation.Point + (hasEntryAnyDamages ? 0 : -10), 500, 600));
                    var distance = textSize.Height;
                    if (!hasPictures) distance += heightBetweenLines;
                    yLocation = helper.GetLinePosition(distance);
                }
                if (hasPictures)
                {
                    var currentSchemePicture = schemePictures.FirstOrDefault(x => x.SchemeEntry == schemeDamageList[i].SchemeEntry);
                    var count = 0;
                    foreach (var picture in currentSchemePicture.SchemePictures)
                    {
                        if (count % 2 == 0)
                        {
                            var offset = count == 0 ? 170 : 160;
                            top = helper.GetLinePosition(offset);
                        }
                        DrawImage(helper, picture.MediaUrl, count, top, 0, 0, false);

                        count++;
                    }
                    yLocation = helper.GetLinePosition(heightBetweenLines);
                }
                if (string.IsNullOrEmpty(schemeDamageList[i].Remarks) && !hasPictures)
                {
                    yLocation = helper.GetLinePosition(heightBetweenLines);
                }
            }
        }

        public static IEnumerable<SchemeDamage> GetSchemeDamages(IEnumerable<SchemeDamage> schemeDamages, IEnumerable<SchemePictureModel> schemePictures)
        {
            var schemeEntriesForPictures = schemePictures.Select(x => x.SchemeEntry);
            var schemeEntriesForDamages = schemeDamages.Select(x => x.SchemeEntry);
            var schemeEntriesNotInDamages = schemeEntriesForPictures.Where(x => !schemeEntriesForDamages.Contains(x));
            var newSchemeDamages = new List<SchemeDamage>();
            foreach (var schemeEntry in schemeEntriesNotInDamages)
            {
                newSchemeDamages.Add(new SchemeDamage
                {
                    SchemeEntry = schemeEntry,
                    OrderNumber = (int)schemeEntry + 1
                });
            }
            var res = schemeDamages.ToList();
            res.AddRange(newSchemeDamages);
            return res;
        }

        private static void SetSchemeEntries(IEnumerable<SchemePictureModel> schemePictures)
        {
            foreach (var picture in schemePictures)
            {
                picture.SchemeEntry = SchemeDamageService.GetSchemeEntryForPhotoIdentifier(picture.PhotoIdentifier);
            }
            schemePictures.OrderBy(x => x.SchemeEntry);
        }

        private static void DamageSection(PdfLayoutHelper helper, IEnumerable<PdfModel> pdfModels, string sectionTitle)
        {
            helper.CreatePage();
            XUnit top = helper.GetLinePosition(textYOffset);
            helper.Gfx.DrawString(sectionTitle, boldFont, XBrushes.Black, pageXOffset, top, XStringFormats.TopLeft);

            var count = 0;
            foreach (var model in pdfModels)
            {
                if (count % 2 == 0)
                {
                    top = helper.GetLinePosition(170) + textYOffset;
                }
                DrawImage(helper, model.Path, count, top, 0, 0, false);
                count++;
            }
        }

        private static void DrawUserInformation(XGraphics gfx, UserInformation userInformation, int yHeaderImageOffset, bool isTakeOver)
        {
            var yOffset = yHeaderImageOffset;
            var widthBetweenTitleAndInfo = isTakeOver ? 130 : 150;
            var heightBetween = 0;

            gfx.DrawString(FelixResources.pdf_title, boldFont, XBrushes.Black,
                new XPoint(pageXOffset + 145, yOffset + heightBetweenLines * heightBetween++));
            gfx.DrawString(FelixResources.general_data, boldFont, XBrushes.Black,
                new XPoint(pageXOffset, yOffset + heightBetweenLines * heightBetween++));

            var userInfoList = GetUserInformationList(userInformation, isTakeOver);
            foreach (var pdfModel in userInfoList)
            {
                DrawInfo(gfx, pdfModel.Text, pdfModel.Information,
                    pageXOffset, yOffset + heightBetweenLines * heightBetween++, widthBetweenTitleAndInfo);
            }

            if (!string.IsNullOrEmpty(userInformation.Remarks))
            {
                var textFormatter = new PdfSharp.Drawing.Layout.XTextFormatter(gfx);
                var htmlString = new HtmlString(userInformation.Remarks);
                gfx.DrawString($"{FelixResources.acquisition_overview_remarksTitle}:", normalFont, XBrushes.Black, new XPoint(pageXOffset, yOffset + heightBetweenLines * heightBetween));
                textFormatter.DrawString(htmlString.ToString(), normalFont, XBrushes.Black, new XRect(pageXOffset + widthBetweenTitleAndInfo, yOffset + heightBetweenLines * heightBetween - 10, 300, 300));
            }
        }

        private static IEnumerable<PdfUserInformation> GetUserInformationList(UserInformation userInformation, bool isTakeOver)
        {
            var userInfoList = new List<PdfUserInformation>();
            userInfoList.Add(new PdfUserInformation($"{FelixResources.general_fullName}:", userInformation.Name));
            userInfoList.Add(new PdfUserInformation($"{FelixResources.general_email}:", $"{userInformation.Email}"));
            userInfoList.Add(new PdfUserInformation($"{FelixResources.general_cellNumber}:", userInformation.Gsm));
            userInfoList.Add(new PdfUserInformation($"{FelixResources._2dehands_licenseplate}:", userInformation.LicensePlate));
            if (isTakeOver)
            {
                userInfoList.Add(new PdfUserInformation($"{FelixResources._2dehands_tyrestate}:", userInformation.TyreStateString));
                userInfoList.Add(new PdfUserInformation($"{FelixResources._2dehands_manual}:", userInformation.ManualString));
            }
            else if (userInformation.IsInsured)
            {
                userInfoList.Add(new PdfUserInformation($"{FelixResources._2dehands_chassisnumber}:", userInformation.Chassisnumber));
                userInfoList.Add(new PdfUserInformation($"{FelixResources.overname_agent_name_readable}:", userInformation.AgentName));
                userInfoList.Add(new PdfUserInformation($"{FelixResources.overname_agent_email_readable}:", userInformation.AgentEmail));
                userInfoList.Add(new PdfUserInformation($"{FelixResources.overname_agent_city_readable}:", userInformation.AgentCity));
                userInfoList.Add(new PdfUserInformation($"{FelixResources.overname_agent_cellphone_readable}:", userInformation.AgentCellphone));
                userInfoList.Add(new PdfUserInformation($"{FelixResources.overname_insurance_readable}:", userInformation.Insurance));
            }
            userInfoList.Add(new PdfUserInformation($"{FelixResources.general_date_created}:", userInformation.CreatedDate.ToString("dd MMMM yyyy HH:mm")));
            return userInfoList;
        }

        private static void DrawInfo(XGraphics gfx, string infoTitle, string infoText, int xLocation, int yLocation, int widthBetweenTitleAndInfo)
        {
            gfx.DrawString(infoTitle, normalFont, XBrushes.Black, new XPoint(xLocation, yLocation));
            if (string.IsNullOrEmpty(infoText))
                infoText = FelixResources.overname_field_is_empty_readable;
            gfx.DrawString(infoText, normalFont, XBrushes.Black, new XPoint(xLocation + widthBetweenTitleAndInfo, yLocation));
        }

        private static void DrawImage(XGraphics gfx, XFont font, string text, string path, int count)
        {
            try
            {
                XImage image = XImage.FromStream(mediaService.GetMediaFileContentStream(path));

                var tempXOffset = count % 2 == 0 ? pageXOffset : xOffset;
                var tempYOffset = textYOffset + pageYOffset + count / 2 * yOffset;

                gfx.DrawString(text, font, XBrushes.Black, new XRect(tempXOffset + 80, tempYOffset, 24, 20), XStringFormats.Center);
                gfx.DrawImage(image, new XRect(tempXOffset, tempYOffset + 25, imageWidth, imageHeight));
            }
            catch (Exception e)
            {
                logger.Error(typeof(PdfGenerator), $"Afbeelding met pad: {path} kon niet worden getekend: ", e);
            }
        }

        private static void DrawImage(PdfLayoutHelper helper, string path, int count, XUnit top, int imgWidth = 0, int imgheight = 0, bool hasImageTitle = true)
        {
            try
            {
                XImage image = XImage.FromStream(mediaService.GetMediaFileContentStream(path));

                XUnit tempXOffset = count % 2 == 0 ? pageXOffset : xOffset;

                var width = imgWidth == 0 ? imageWidth : imgWidth;
                var height = imgheight == 0 ? imageHeight : imgheight;

                var topPoint = hasImageTitle ? top.Point + 40 : top.Point;
                helper.Gfx.DrawImage(image, new XRect(tempXOffset.Point, topPoint, width, height));
            }
            catch (Exception e)
            {
                logger.Error(typeof(PdfGenerator), $"Afbeelding met pad: {path} kon niet worden getekend: ", e);
            }
        }

        private static void DrawImageWithPath(PdfLayoutHelper helper, string path, int count, XUnit top, int imgWidth = 0, int imgheight = 0, bool hasImageTitle = true)
        {
            try
            {
                var fullPath = applicationPath + path;

                XImage image = XImage.FromFile(fullPath);

                XUnit tempXOffset = count % 2 == 0 ? pageXOffset : xOffset;

                var width = imgWidth == 0 ? imageWidth : imgWidth;
                var height = imgheight == 0 ? imageHeight : imgheight;

                var topPoint = hasImageTitle ? top.Point + 40 : top.Point;
                helper.Gfx.DrawImage(image, new XRect(tempXOffset.Point, topPoint, width, height));
            }
            catch (Exception e)
            {
                logger.Error(typeof(PdfGenerator), $"Afbeelding met pad: {path} kon niet worden getekend: ", e);
            }
        }

        private static IEnumerable<PdfModel> GetPdfModelsForImageTypeList(IEnumerable<PdfModel> pdfModels, List<string> imageTypes)
        {
            return pdfModels.Where(x => imageTypes.Contains(x.Name));
        }

        private static IEnumerable<PdfModel> OrderPdfModels(IEnumerable<PdfModel> pdfModels)
        {
            var list = new PdfModel[pdfModels.Count() < 13 ? 13 : pdfModels.Count()];

            foreach (var pdfModel in pdfModels)
            {
                if (pdfModel.Name.Contains(AcquisitionImageTypeHelper.DamagePhotos[0])
                    || pdfModel.Name.Contains(AcquisitionImageTypeHelper.DamagePhotos[1])) continue;
                AcquisitionImageTypeHelper.AcquisitionImageTypeOrder.TryGetValue(pdfModel.Name, out int index);
                list[index] = pdfModel;
            }

            var resultList = list.ToList();

            resultList.AddRange(pdfModels.Where(x => x.Name.Contains(AcquisitionImageTypeHelper.DamagePhotos[0])));
            resultList.AddRange(pdfModels.Where(x => x.Name.Contains(AcquisitionImageTypeHelper.DamagePhotos[1])));

            resultList.RemoveAll(x => x == null);

            return resultList;
        }

        private static int DrawHeader(XGraphics gfx, XImage businessLogo, XImage brandLogo, XBrush color, LogoType logoType)
        {
            var businessLogoXOffset = 200;
            var headerOffset = 125;

            var pdfDimensions = SetLogoDimensions(logoType, businessLogo.PixelWidth, brandLogo.PixelWidth);

            gfx.DrawRectangle(color, new XRect(0, 0, businessLogoXOffset - 30, headerOffset));
            gfx.DrawImage(brandLogo, pdfDimensions.LogoXOffset, pdfDimensions.LogoYOffset, pdfDimensions.BrandLogoWidth
                , brandLogo.PixelHeight * pdfDimensions.BrandLogoRatio);
            gfx.DrawImage(businessLogo, businessLogoXOffset + pdfDimensions.HeaderXOffset, pdfDimensions.HeaderYOffset
                , pdfDimensions.BusinessLogoWidth, businessLogo.PixelHeight * pdfDimensions.BusinessLogoRatio);

            return headerOffset;
        }

        private static PdfHeaderDimensions SetLogoDimensions(LogoType logoType, int businessLogoPixelWidth, int brandLogoPixelWidth)
        {
            var pdfDimensions = new PdfHeaderDimensions();
            switch (logoType)
            {
                case LogoType.Citroen:
                    pdfDimensions.LogoYOffset = 10;
                    pdfDimensions.HeaderXOffset = -30;
                    pdfDimensions.BusinessLogoWidth = pageWidth - 100;
                    pdfDimensions.BrandLogoWidth = 175;
                    pdfDimensions.BusinessLogoRatio = (double)pdfDimensions.BusinessLogoWidth / businessLogoPixelWidth;
                    pdfDimensions.BrandLogoRatio = (double)pdfDimensions.BrandLogoWidth / brandLogoPixelWidth;
                    break;

                case LogoType.Peugeot:
                    pdfDimensions.HeaderXOffset = -30;
                    pdfDimensions.LogoXOffset = 17;
                    pdfDimensions.LogoYOffset = 17;
                    pdfDimensions.HeaderYOffset = 15;
                    pdfDimensions.BusinessLogoWidth = pageWidth - 100;
                    pdfDimensions.BrandLogoWidth = 125;
                    pdfDimensions.BusinessLogoRatio = (double)pdfDimensions.BusinessLogoWidth / businessLogoPixelWidth;
                    pdfDimensions.BrandLogoRatio = (double)pdfDimensions.BrandLogoWidth / brandLogoPixelWidth;
                    break;

                case LogoType.Kia:
                    pdfDimensions.LogoXOffset = 15;
                    pdfDimensions.LogoYOffset = -5;
                    pdfDimensions.HeaderXOffset = 15;
                    pdfDimensions.HeaderYOffset = 30;
                    pdfDimensions.BusinessLogoWidth = pageWidth - 225;
                    pdfDimensions.BrandLogoWidth = 125;
                    pdfDimensions.BusinessLogoRatio = (double)(pdfDimensions.BusinessLogoWidth / businessLogoPixelWidth) / 2;
                    pdfDimensions.BrandLogoRatio = (double)pdfDimensions.BrandLogoWidth / brandLogoPixelWidth;
                    break;

                case LogoType.PeugeotEnVanGansen:
                    pdfDimensions.LogoXOffset = 17;
                    pdfDimensions.LogoYOffset = 17;
                    pdfDimensions.HeaderXOffset = 15;
                    pdfDimensions.HeaderYOffset = 30;
                    pdfDimensions.BusinessLogoWidth = pageWidth - 225;
                    pdfDimensions.BrandLogoWidth = 125;
                    pdfDimensions.BusinessLogoRatio = (double)(pdfDimensions.BusinessLogoWidth / businessLogoPixelWidth) / 2;
                    pdfDimensions.BrandLogoRatio = (double)pdfDimensions.BrandLogoWidth / brandLogoPixelWidth;
                    break;
            }
            return pdfDimensions;
        }

        private static Tuple<byte, byte, byte> GetRgbaValues(string backgroundColor)
        {
            var color = ColorConverter.ConvertHexToRgba(backgroundColor, 1);
            var colorSplit = color.Split(new[] { '(', ',' });
            var r = byte.Parse(colorSplit[1]);
            var g = byte.Parse(colorSplit[2]);
            var b = byte.Parse(colorSplit[3]);
            return new Tuple<byte, byte, byte>(r, g, b);
        }

        private static LogoType GetLogoType(string businessName)
        {
            var lowerCaseBusinessName = businessName.ToLower();
            if (lowerCaseBusinessName.Contains("citroen")
                || lowerCaseBusinessName.Contains("citroën")
                || lowerCaseBusinessName.Contains("felix herentals")
                || lowerCaseBusinessName.Contains("felix brecht")
                || lowerCaseBusinessName.Equals("felix"))
            {
                return LogoType.Citroen;
            }
            else if (lowerCaseBusinessName.Contains("peugeot") && lowerCaseBusinessName.Contains("van gansen"))
            {
                return LogoType.PeugeotEnVanGansen;
            }
            else if (lowerCaseBusinessName.Contains("peugeot"))
            {
                return LogoType.Peugeot;
            }
            return LogoType.Kia;
        }

        private static IEnumerable<string> GetAllColoredImagesForDamages(int userId, ISchemeDamageService schemeDamageService,
            IEnumerable<SchemeDamage> schemeDamages)
        {
            var schemeImages = schemeDamageService.GetSchemeOverviewModel(userId);
            var damageOrderNumbers = schemeDamages.Select(x => x.OrderNumber);
            var schemeImagesForDamages = schemeImages.Where(x => damageOrderNumbers.Contains(x.OrderNumber));

            var result = new List<string>();
            foreach (var schemeImage in schemeImagesForDamages)
            {
                result.Add(schemeImage.ImageLocation);
            }

            return result;
        }
    }
}