using System;
using System.Collections.Generic;
using System.Linq;
using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Extensions;
using FelixWebsite.Bll.Services.Base;
using FelixWebsite.Bll.Services.Interfaces;
using FelixWebsite.Dal.UnitOfWork;
using FelixWebsite.Core.Constants;
using FelixWebsite.Core.enums;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Core.Models;
using System.Web.UI.WebControls;
using AutoMapper;
using FelixWebsite.Shared.Constants;
using FelixWebsite.Bll.Helpers;
using System.Web;

namespace FelixWebsite.Bll.Services
{
    public class PhotoService : BaseService<PhotoInfo, Dal.Entities.PhotoInfo>, IPhotoService
    {
        private readonly ISchemeDamageService schemeDamageService;

        public PhotoService(UnitOfWork unitOfWork, ISchemeDamageService schemeDamageService) : base(unitOfWork)
        {
            this.schemeDamageService = schemeDamageService;
        }

        public IEnumerable<PhotoInfo> GetAllPhotoIdsWithUserInfoId(int id)
        {
            try
            {
                var photoIdsEntities = UnitOfWork.PhotoRepository.GetAllWithUserId(id);
                return AutoMapper.Mapper.Map<IEnumerable<PhotoInfo>>(photoIdsEntities);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something failed when getting all photo's of user with Id: {id}. The thrown error is: ", e);
                return null;
            }
        }

        public IEnumerable<PhotoInfo> GetAllPhotoIdsOfSchemeWithUserId(int id)
        {
            try
            {
                var photoIdsEntities = UnitOfWork.PhotoRepository.GetAllWithUserId(id);
                var entitiesForScheme = photoIdsEntities.Where(x => x.PhotoIdentifier >= PhotoIdentifier.BumperFront);
                return AutoMapper.Mapper.Map<IEnumerable<PhotoInfo>>(entitiesForScheme);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something failed when getting all photo's of user with Id: {id}. The thrown error is: ", e);
                return null;
            }
        }

        public PhotoInfo GetByImageType(string imageType, int userId, bool isOverview = false, int index = 0)
        {
            try
            {
                var identifier = SelectPhotoIdentifier(imageType);
                if (identifier == PhotoIdentifier.DamageInside || identifier == PhotoIdentifier.DamageOutside)
                {
                    if (isOverview)
                    {
                        var damagePhotos = GetAllByImageType(imageType, userId);
                        var photoInfo = damagePhotos.ToList()[index];
                        return Mapper.Map<PhotoInfo>(photoInfo);
                    }
                    else { return null; }
                }

                var photoIdEntity = UnitOfWork.PhotoRepository.GetWithPhotoIdentifierAndUserId(identifier, userId);
                return Mapper.Map<PhotoInfo>(photoIdEntity);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something failed when getting an image with type: {imageType} and userid: {userId}. The thrown error is: ", e);
                return null;
            }
        }

        public OverviewModel GetOverviewModelData(int id, IMediaService mediaService, UmbracoHelper umbraco)
        {
            return GetOverviewModel(id, mediaService, umbraco, true);
        }
        public OverviewDamageModel GetOverviewModelDataDamages(int id, IMediaService mediaService, UmbracoHelper umbraco, ILogger logger, HttpServerUtilityBase server)
        {
            var model = GetOverviewModel(id, mediaService, umbraco, false);
            var damageModel = Mapper.Map<OverviewDamageModel>(model);
            damageModel.SchemePictures = PhotoHelper.GetPhotoInfosForScheme(id, this, mediaService, logger);
            return damageModel;
        }

        private OverviewModel GetOverviewModel(int id, IMediaService mediaService, UmbracoHelper umbraco, bool isTakeOver)
        {
            try
            {
                var model = new OverviewModel();
                var userInformation = Mapper.Map<UserInformation>(UnitOfWork.AcquisitionRepository.GetById(id));
                var allPictureIds = GetAllPhotoIdsWithUserInfoId(id);
                var watch = new List<PhotoIdentifier>();

                if (id == -1)
                {
                    LogHelper.Debug(GetType(),
                        "The id passed was -1, this means that it was not set in the JS when the new user was added.");
                    return model;
                }

                foreach (var pictureId in allPictureIds)
                {
                    watch.Add(pictureId.PhotoIdentifier);
                    var url = pictureId.GetUrl(mediaService, umbraco);
                    if (pictureId.PhotoIdentifier == PhotoIdentifier.DamageInside ||
                        pictureId.PhotoIdentifier == PhotoIdentifier.DamageOutside)
                    {
                        var imageName = mediaService.GetById(pictureId.MediaId)?.Name;
                        AddToOverviewModel(model, url, pictureId, imageName?.Replace(" ", "").Replace("(", "").Replace(")", ""));
                    }
                    else
                    {
                        AddToOverviewModel(model, url, pictureId);
                    }
                }

                if (!watch.Contains(PhotoIdentifier.Examination))
                {
                    var examUrl = "/img/acquisition/keuring.png";
                    AddToOverviewModel(model, examUrl, new PhotoInfo(0, id, PhotoIdentifier.Examination));
                }

                model.AmountOfDamagePictures = model.DmgInsideUrls.Count + model.DmgOutsideUrls.Count;
                model.IsInsured = userInformation.IsInsured;
                model.UserInformation = userInformation;
                model.IsTakeOver = isTakeOver;

                return model;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong getting the overviewmodel. The error is: ", e);
                return null;
            }
        }

        public IEnumerable<PhotoInfo> GetAllByImageType(string imageType, int userId)
        {
            try
            {
                var identifier = SelectPhotoIdentifier(imageType);
                var photoEntities = UnitOfWork.PhotoRepository.GetAllWithIdentifier(identifier, userId);
                return AutoMapper.Mapper.Map<IEnumerable<PhotoInfo>>(photoEntities);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong while getting all the damages of type: {imageType} and userid: {userId}", e);
                return null;
            }
        }

        public PhotoInfo GetByMediaId(int mediaId, int userId)
        {
            try
            {
                var photoInfoEntities = UnitOfWork.PhotoRepository.GetAllWithUserId(userId);
                var photoInfoEntity = photoInfoEntities.FirstOrDefault(photoInfo => photoInfo.MediaId == mediaId);
                return AutoMapper.Mapper.Map<PhotoInfo>(photoInfoEntity);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), $"Something went wrong while getting a photoinfo with mediaid: {mediaId} and userId: {userId}. ", e);
                return null;
            }
        }

        public static PhotoIdentifier SelectPhotoIdentifier(string imageType)
        {
            if (imageType.Contains(AcquisitionImageTypes.DMG_OUTSIDE)) { return PhotoIdentifier.DamageOutside; }
            if (imageType.Contains(AcquisitionImageTypes.DMG_INSIDE)) { return PhotoIdentifier.DamageInside; }

            switch (imageType)
            {
                case AcquisitionImageTypes.LEFT_FRONT:
                    return PhotoIdentifier.LeftFront;
                case AcquisitionImageTypes.RIGHT_FRONT:
                    return PhotoIdentifier.RightFront;
                case AcquisitionImageTypes.LEFT_BACK:
                    return PhotoIdentifier.LeftBack;
                case AcquisitionImageTypes.RIGHT_BACK:
                    return PhotoIdentifier.RightBack;
                case AcquisitionImageTypes.FRONTSEATS:
                    return PhotoIdentifier.Frontseats;
                case AcquisitionImageTypes.BACKSEATS:
                    return PhotoIdentifier.Backseats;
                case AcquisitionImageTypes.DASH:
                    return PhotoIdentifier.Dashboard;
                case AcquisitionImageTypes.KM:
                case AcquisitionImageTypes.KM_DAMAGES:
                    return PhotoIdentifier.Km;
                case AcquisitionImageTypes.DMG_OUTSIDE:
                    return PhotoIdentifier.DamageOutside;
                case AcquisitionImageTypes.DMG_INSIDE:
                    return PhotoIdentifier.DamageInside;
                case AcquisitionImageTypes.ENROLLMENT_FRONT:
                case AcquisitionImageTypes.ENROLLMENT_FRONT_DAMAGES:
                    return PhotoIdentifier.EnrollmentFront;
                case AcquisitionImageTypes.ENROLLMENT_BACK:
                case AcquisitionImageTypes.ENROLLMENT_BACK_DAMAGES:
                    return PhotoIdentifier.EnrollmentBack;
                case AcquisitionImageTypes.EXAMINATION:
                    return PhotoIdentifier.Examination;
                case AcquisitionImageTypes.CHASSIS_NUMBER:
                    return PhotoIdentifier.ChassisNumber;
                case AcquisitionImageTypes.PROOF_OF_INSURANCE:
                    return PhotoIdentifier.ProofOfInsurance;

                case SchemeDamageNames.BUMPER_FRONT:
                    return PhotoIdentifier.BumperFront;
                case SchemeDamageNames.HOOD:
                    return PhotoIdentifier.Hood;
                case SchemeDamageNames.LEFT_WHEEL_CAP_FRONT:
                    return PhotoIdentifier.LeftWheelCapFront;
                case SchemeDamageNames.RIGHT_WHEEL_CAP_FRONT:
                    return PhotoIdentifier.RightWheelCapFront;
                case SchemeDamageNames.ROOF:
                    return PhotoIdentifier.Roof;
                case SchemeDamageNames.LEFT_DOOR_FRONT:
                    return PhotoIdentifier.LeftDoorFront;
                case SchemeDamageNames.RIGHT_DOOR_FRONT:
                    return PhotoIdentifier.RightDoorFront;
                case SchemeDamageNames.LEFT_DOOR_BACK:
                    return PhotoIdentifier.LeftDoorBack;
                case SchemeDamageNames.RIGHT_DOOR_BACK:
                    return PhotoIdentifier.RightDoorBack;
                case SchemeDamageNames.TRUNK:
                    return PhotoIdentifier.Trunk;
                case SchemeDamageNames.LEFT_WHEEL_CAP_BACK:
                    return PhotoIdentifier.LeftWheelCapBack;
                case SchemeDamageNames.RIGHT_WHEEL_CAP_BACK:
                    return PhotoIdentifier.RightWheelCapBack;
                case SchemeDamageNames.BUMPER_BACK:
                    return PhotoIdentifier.BumperBack;
                case SchemeDamageNames.LEFT_WHEEL_BACK:
                    return PhotoIdentifier.LeftWheelBack;
                case SchemeDamageNames.LEFT_WHEEL_FRONT:
                    return PhotoIdentifier.LeftWheelFront;
                case SchemeDamageNames.RIGHT_WHEEL_BACK:
                    return PhotoIdentifier.RightWheelBack;
                case SchemeDamageNames.RIGHT_WHEEL_FRONT:
                    return PhotoIdentifier.RightWheelFront;
                case SchemeDamageNames.WINDSHIELD:
                    return PhotoIdentifier.Windshield;
                case SchemeDamageNames.UNDERNEATH:
                    return PhotoIdentifier.Underneath;
                case SchemeDamageNames.INSIDE:
                    return PhotoIdentifier.Inside;
                default:
                    LogHelper.Debug(typeof(PhotoService), $"The right identifier couldn't be found. Imagetype: {imageType}");
                    return PhotoIdentifier.LeftFront;
            }
        }

        public static void AddToOverviewModel(OverviewModel model, string imageUrl, PhotoInfo photoInfo, string imageName = "")
        {
            switch (photoInfo.PhotoIdentifier)
            {
                case PhotoIdentifier.LeftFront:
                    model.LeftFrontUrl = imageUrl;
                    break;
                case PhotoIdentifier.RightFront:
                    model.RightFrontUrl = imageUrl;
                    break;
                case PhotoIdentifier.LeftBack:
                    model.LeftBackUrl = imageUrl;
                    break;
                case PhotoIdentifier.RightBack:
                    model.RightBackUrl = imageUrl;
                    break;
                case PhotoIdentifier.Frontseats:
                    model.FrontseatsUrl = imageUrl;
                    break;
                case PhotoIdentifier.Backseats:
                    model.BackseatsUrl = imageUrl;
                    break;
                case PhotoIdentifier.Dashboard:
                    model.DashboardUrl = imageUrl;
                    break;
                case PhotoIdentifier.Km:
                    model.KmUrl = imageUrl;
                    break;
                case PhotoIdentifier.DamageOutside:
                    model.DmgOutsideUrls.Add(new DamageEntry(imageUrl, imageName, photoInfo.MediaId));
                    break;
                case PhotoIdentifier.DamageInside:
                    model.DmgInsideUrls.Add(new DamageEntry(imageUrl, imageName, photoInfo.MediaId));
                    break;
                case PhotoIdentifier.EnrollmentFront:
                    model.EnrollmentFrontUrl = imageUrl;
                    break;
                case PhotoIdentifier.EnrollmentBack:
                    model.EnrollmentBackUrl = imageUrl;
                    break;
                case PhotoIdentifier.Examination:
                    model.ExaminationUrl = imageUrl;
                    break;
                case PhotoIdentifier.ChassisNumber:
                    model.ChassisNumberUrl = imageUrl;
                    break;
                case PhotoIdentifier.ProofOfInsurance:
                    model.ProofOfInsuranceUrl = imageUrl;
                    break;
            }
        }

        public static string GetImageLocationForIdentifier(PhotoIdentifier photoIdentifier)
        {
            var imageLocation = "/img/acquisition/scheme/OverviewParts";
            switch (photoIdentifier)
            {
                case PhotoIdentifier.BumperFront:
                    return $"{imageLocation}/Bumper.png";
                case PhotoIdentifier.Hood:
                    return $"{imageLocation}/Motorkap.png";
                case PhotoIdentifier.LeftWheelCapFront:
                    return $"{imageLocation}/LinkerBandkap.png";
                case PhotoIdentifier.RightWheelCapFront:
                    return $"{imageLocation}/RechterBandkap.png";
                case PhotoIdentifier.Roof:
                    return $"{imageLocation}/Dak.png";
                case PhotoIdentifier.LeftDoorFront:
                    return $"{imageLocation}/LinkerVoordeur.png";
                case PhotoIdentifier.RightDoorFront:
                    return $"{imageLocation}/RechterVoordeur.png"; 
                case PhotoIdentifier.LeftDoorBack:
                    return $"{imageLocation}/LinkerAchterdeur.png";
                case PhotoIdentifier.RightDoorBack:
                    return $"{imageLocation}/RechterAchterdeur.png";
                case PhotoIdentifier.Trunk:
                    return $"{imageLocation}/Koffer.png";
                case PhotoIdentifier.LeftWheelCapBack:
                    return $"{imageLocation}/LinkerAchterBandkap.png"; 
                case PhotoIdentifier.RightWheelCapBack:
                    return $"{imageLocation}/RechterAchterBandkap.png";
                case PhotoIdentifier.BumperBack:
                    return $"{imageLocation}/AchterBumper.png";
                case PhotoIdentifier.LeftWheelBack:
                    return $"{imageLocation}/LinkerAchterband.png";
                case PhotoIdentifier.LeftWheelFront:
                    return $"{imageLocation}/LinkerVoorband.png";
                case PhotoIdentifier.RightWheelBack:
                    return $"{imageLocation}/RechterAchterband.png";
                case PhotoIdentifier.RightWheelFront:
                    return $"{imageLocation}/RechterVoorband.png"; 
                case PhotoIdentifier.Windshield:
                    return $"{imageLocation}/Voorruit.png";
                case PhotoIdentifier.Underneath:
                    return $"{imageLocation}/Onderkant.png";
                case PhotoIdentifier.Inside:
                    return $"{imageLocation}/Binnenkant.png";
                default:
                    LogHelper.Debug(typeof(PhotoService), $"The right identifier couldn't be found. PhotoId: {photoIdentifier}");
                    return $"{imageLocation}/Bumper.png";
            }
        }
    }
}
