using AutoMapper;
using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Services.Base;
using FelixWebsite.Bll.Services.Interfaces;
using FelixWebsite.Core.App_GlobalResources;
using FelixWebsite.Core.enums;
using FelixWebsite.Dal.UnitOfWork;
using FelixWebsite.Shared.Constants;
using FelixWebsite.Shared.enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;

namespace FelixWebsite.Bll.Services
{
    public class SchemeDamageService : BaseService<SchemeDamage, Dal.Entities.SchemeDamage>, ISchemeDamageService
    {
        public SchemeDamageService(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public SchemeDamage GetUserEntry(int userId, string imageType)
        {
            try
            {
                var schemeEntry = SelectSchemeEntry(imageType);
                var userEntry = Repository.GetAll().FirstOrDefault(x => x.UserId == userId && x.SchemeEntry == schemeEntry);
                return Mapper.Map<SchemeDamage>(userEntry);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(),
                    $"Er is iets misgegaan bij het ophalen van de user entry voor userId: {userId} en imageType: {imageType}", e);
                return null;
            }
        }

        public void DeleteAll(int userId)
        {
            try
            {
                var schemeEntries = Repository.GetAll().Where(x => x.UserId == userId);
                foreach (var schemeEntry in schemeEntries)
                {
                    schemeEntry.Delete(userId);
                }
                UnitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(),
                    $"Er is iets misgegaan bij het verwijderen van alle user entries: {userId}", e);
            }
        }

        public IEnumerable<SchemeImage> GetSchemeOverviewModel(int userId)
        {
            try
            {
                var schemeOverviewList = GetSchemeImages().ToList();
                if (userId == 0) return schemeOverviewList;
                var schemeDamages = Mapper.Map<IEnumerable<PhotoInfo>>(
                    UnitOfWork.PhotoRepository.GetAllWithUserId(userId).Where(x => x.PhotoIdentifier >= PhotoIdentifier.BumperFront));
                foreach (var schemeDamage in schemeDamages)
                {
                    var orderNumber = (int)GetSchemeEntryForPhotoIdentifier(schemeDamage.PhotoIdentifier) + 1;
                    SetSchemeImageToRed(schemeOverviewList, orderNumber);
                }
                return schemeOverviewList;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(),
                    $"Er is iets misgegaan bij het aanmaken van het schema overview model", e);
                return new List<SchemeImage>();
            }
        }

        public IEnumerable<SchemeImage> GetSchemeOverviewModelWithAllDamages(int userId)
        {
            try
            {
                var schemeOverviewList = GetSchemeImages().ToList();
                if (userId == 0) return schemeOverviewList;
                var f = UnitOfWork.SchemeDamageRepository.GetAll().Where(x => x.UserId == userId).ToList();
                var schemeDamages = Mapper.Map<IEnumerable<SchemeDamage>>(f);
                var schemeEntriesForDamages = schemeDamages.Select(x => x.SchemeEntry);

                var schemePhotos = Mapper.Map<IEnumerable<PhotoInfo>>(
                    UnitOfWork.PhotoRepository.GetAllWithUserId(userId).Where(x => x.PhotoIdentifier >= PhotoIdentifier.BumperFront));
                var schemePhotosNotInDamages = schemePhotos.Where(x => !schemeEntriesForDamages.Contains(GetSchemeEntryForPhotoIdentifier(x.PhotoIdentifier)));

                foreach (var schemeDamage in schemeDamages)
                {
                    var orderNumber = (int)schemeDamage.SchemeEntry + 1; 
                    SetSchemeImageToRed(schemeOverviewList, orderNumber);
                }

                foreach (var photo in schemePhotosNotInDamages)
                {
                    var orderNumber = (int)GetSchemeEntryForPhotoIdentifier(photo.PhotoIdentifier) + 1;
                    SetSchemeImageToRed(schemeOverviewList, orderNumber);
                }
                return schemeOverviewList;
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(),
                    $"Er is iets misgegaan bij het aanmaken van het schema overview model", e);
                return new List<SchemeImage>();
            }
        }

        private void SetSchemeImageToRed(List<SchemeImage> schemeOverviewList, int orderNumber)
        {
            var listEntry = schemeOverviewList.FirstOrDefault(x => x.OrderNumber == orderNumber);
            var index = schemeOverviewList.IndexOf(listEntry);
            schemeOverviewList.Remove(listEntry);
            listEntry.IsRed = true;
            schemeOverviewList.Insert(index, listEntry);
        }

        public IEnumerable<SchemeDamage> GetAllForUserId(int userId)
        {
            try
            {
                var schemeDamages = Repository.GetAll().Where(x => x.UserId == userId).ToList();
                return Mapper.Map<IEnumerable<SchemeDamage>>(schemeDamages);
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(),
                    $"Er is iets misgegaan bij het ophalen van de scheme images for userId {userId}", e);
                return null;
            }
        }

        public static SchemeEntry SelectSchemeEntry(string imageType)
        {
            switch (imageType)
            {
                case SchemeDamageNames.BUMPER_FRONT:
                    return SchemeEntry.BumperFront;
                case SchemeDamageNames.HOOD:
                    return SchemeEntry.Hood;
                case SchemeDamageNames.LEFT_WHEEL_CAP_FRONT:
                    return SchemeEntry.LeftWheelCapFront;
                case SchemeDamageNames.RIGHT_WHEEL_CAP_FRONT:
                    return SchemeEntry.RightWheelCapFront;
                case SchemeDamageNames.ROOF:
                    return SchemeEntry.Roof;
                case SchemeDamageNames.LEFT_DOOR_FRONT:
                    return SchemeEntry.LeftDoorFront;
                case SchemeDamageNames.RIGHT_DOOR_FRONT:
                    return SchemeEntry.RightDoorFront;
                case SchemeDamageNames.LEFT_DOOR_BACK:
                    return SchemeEntry.LeftDoorBack;
                case SchemeDamageNames.RIGHT_DOOR_BACK:
                    return SchemeEntry.RightDoorBack;
                case SchemeDamageNames.TRUNK:
                    return SchemeEntry.Trunk;
                case SchemeDamageNames.LEFT_WHEEL_CAP_BACK:
                    return SchemeEntry.LeftWheelCapBack;
                case SchemeDamageNames.RIGHT_WHEEL_CAP_BACK:
                    return SchemeEntry.RightWheelCapBack;
                case SchemeDamageNames.BUMPER_BACK:
                    return SchemeEntry.BumperBack;
                case SchemeDamageNames.LEFT_WHEEL_BACK:
                    return SchemeEntry.LeftWheelBack;
                case SchemeDamageNames.LEFT_WHEEL_FRONT:
                    return SchemeEntry.LeftWheelFront;
                case SchemeDamageNames.RIGHT_WHEEL_BACK:
                    return SchemeEntry.RightWheelBack;
                case SchemeDamageNames.RIGHT_WHEEL_FRONT:
                    return SchemeEntry.RightWheelFront;
                case SchemeDamageNames.WINDSHIELD:
                    return SchemeEntry.Windshield;
                case SchemeDamageNames.UNDERNEATH:
                    return SchemeEntry.Underneath;
                case SchemeDamageNames.INSIDE:
                    return SchemeEntry.Inside;
                default:
                    LogHelper.Debug(typeof(PhotoService), $"The right identifier couldn't be found. Imagetype: {imageType}");
                    return SchemeEntry.BumperFront;
            }
        }

        private IEnumerable<SchemeImage> GetSchemeImages()
        {
            var schemeImages = new List<SchemeImage>();
            schemeImages.Add(new SchemeImage((int)SchemeEntry.BumperFront + 1, "/img/acquisition/scheme/colored-Bumper.png",
                SchemeDamageNames.BUMPER_FRONT, FelixResources.overname_scheme_bumper_front));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.Hood + 1, "/img/acquisition/scheme/colored-Motorkap.png",
                SchemeDamageNames.HOOD, FelixResources.overname_scheme_hood));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.LeftWheelCapFront + 1, "/img/acquisition/scheme/colored-LinkerBandkap.png",
                SchemeDamageNames.LEFT_WHEEL_CAP_FRONT, FelixResources.overname_scheme_left_wheel_cap_front));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.RightWheelCapFront + 1, "/img/acquisition/scheme/colored-RechterBandkap.png",
                SchemeDamageNames.RIGHT_WHEEL_CAP_FRONT, FelixResources.overname_scheme_right_wheel_cap_front));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.Roof + 1, "/img/acquisition/scheme/colored-Dak.png",
                SchemeDamageNames.ROOF, FelixResources.overname_scheme_roof));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.LeftDoorFront + 1, "/img/acquisition/scheme/colored-LinkerVoordeur.png",
                SchemeDamageNames.LEFT_DOOR_FRONT, FelixResources.overname_scheme_left_door_front));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.RightDoorFront + 1, "/img/acquisition/scheme/colored-RechterVoordeur.png",
                SchemeDamageNames.RIGHT_DOOR_FRONT, FelixResources.overname_scheme_right_door_front));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.LeftDoorBack + 1, "/img/acquisition/scheme/colored-LinkerAchterdeur.png",
                SchemeDamageNames.LEFT_DOOR_BACK, FelixResources.overname_scheme_left_door_back));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.RightDoorBack + 1, "/img/acquisition/scheme/colored-RechterAchterdeur.png",
                SchemeDamageNames.RIGHT_DOOR_BACK, FelixResources.overname_scheme_right_door_back));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.Trunk + 1, "/img/acquisition/scheme/colored-Koffer.png",
                SchemeDamageNames.TRUNK, FelixResources.overname_scheme_trunk));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.LeftWheelCapBack + 1, "/img/acquisition/scheme/colored-LinkerAchterBandkap.png",
                SchemeDamageNames.LEFT_WHEEL_CAP_BACK, FelixResources.overname_scheme_left_wheel_cap_back));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.RightWheelCapBack + 1, "/img/acquisition/scheme/colored-RechterAchterBandkap.png",
                SchemeDamageNames.RIGHT_WHEEL_CAP_BACK, FelixResources.overname_scheme_right_wheel_cap_back));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.BumperBack + 1, "/img/acquisition/scheme/colored-Achterbumper.png",
                SchemeDamageNames.BUMPER_BACK, FelixResources.overname_scheme_bumper_back));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.LeftWheelBack + 1, "/img/acquisition/scheme/colored-LinkerVoorband.png",
                SchemeDamageNames.LEFT_WHEEL_BACK, FelixResources.overname_scheme_left_wheel_back));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.LeftWheelFront + 1, "/img/acquisition/scheme/colored-LinkerVoorband.png",
                SchemeDamageNames.LEFT_WHEEL_FRONT, FelixResources.overname_scheme_left_wheel_front));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.RightWheelBack + 1, "img/acquisition/scheme/colored-LinkerVoorband.png",
                SchemeDamageNames.RIGHT_WHEEL_BACK, FelixResources.overname_scheme_right_wheel_back));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.RightWheelFront + 1, "/img/acquisition/scheme/colored-LinkerVoorband.png",
                SchemeDamageNames.RIGHT_WHEEL_FRONT, FelixResources.overname_scheme_right_wheel_front));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.Windshield + 1, "/img/acquisition/scheme/colored-Voorruit.png",
                SchemeDamageNames.WINDSHIELD, FelixResources.overname_scheme_windshield));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.Underneath + 1, "/img/acquisition/scheme/colored-Onderkant.png",
                SchemeDamageNames.UNDERNEATH, FelixResources.overname_scheme_underneath));
            schemeImages.Add(new SchemeImage((int)SchemeEntry.Inside + 1, "/img/acquisition/scheme/colored-Binnenkant.png",
                SchemeDamageNames.INSIDE, FelixResources.overname_scheme_inside));
            return schemeImages;
        }

        public static string GetReadableName(SchemeEntry schemeEntry)
        {
            switch (schemeEntry)
            {
                case SchemeEntry.BumperFront:
                    return FelixResources.overname_scheme_bumper_front;
                case SchemeEntry.Hood:
                    return FelixResources.overname_scheme_hood;
                case SchemeEntry.LeftWheelCapFront:
                    return FelixResources.overname_scheme_left_wheel_cap_front;
                case SchemeEntry.RightWheelCapFront:
                    return FelixResources.overname_scheme_right_wheel_cap_front;
                case SchemeEntry.Roof:
                    return FelixResources.overname_scheme_roof;
                case SchemeEntry.LeftDoorFront:
                    return FelixResources.overname_scheme_left_door_front;
                case SchemeEntry.RightDoorFront:
                    return FelixResources.overname_scheme_right_door_front;
                case SchemeEntry.LeftDoorBack:
                    return FelixResources.overname_scheme_left_door_back;
                case SchemeEntry.RightDoorBack:
                    return FelixResources.overname_scheme_right_door_back;
                case SchemeEntry.Trunk:
                    return FelixResources.overname_scheme_trunk;
                case SchemeEntry.LeftWheelCapBack:
                    return FelixResources.overname_scheme_left_wheel_cap_back;
                case SchemeEntry.RightWheelCapBack :
                    return FelixResources.overname_scheme_right_wheel_cap_back;
                case SchemeEntry.BumperBack:
                    return FelixResources.overname_scheme_bumper_back;
                case SchemeEntry.LeftWheelBack:
                    return FelixResources.overname_scheme_left_wheel_back;
                case SchemeEntry.LeftWheelFront:
                    return FelixResources.overname_scheme_left_wheel_front;
                case SchemeEntry.RightWheelBack:
                    return FelixResources.overname_scheme_right_wheel_back;
                case SchemeEntry.RightWheelFront:
                    return FelixResources.overname_scheme_right_wheel_front;
                case SchemeEntry.Windshield:
                    return FelixResources.overname_scheme_windshield;
                case SchemeEntry.Underneath:
                    return FelixResources.overname_scheme_underneath;
                case SchemeEntry.Inside:
                    return FelixResources.overname_scheme_inside;
                default:
                    LogHelper.Debug(typeof(PhotoService), $"The right identifier couldn't be found. Scheme entry: {schemeEntry}");
                    return FelixResources.overname_scheme_bumper_front;
            }
        }

        public static SchemeEntry GetSchemeEntryForPhotoIdentifier(PhotoIdentifier photoIdentifier)
        {
            switch (photoIdentifier)
            {
                case PhotoIdentifier.BumperFront:
                    return SchemeEntry.BumperFront;
                case PhotoIdentifier.Hood:
                    return SchemeEntry.Hood;
                case PhotoIdentifier.LeftWheelCapFront:
                    return SchemeEntry.LeftWheelCapFront;
                case PhotoIdentifier.RightWheelCapFront:
                    return SchemeEntry.RightWheelCapFront;
                case PhotoIdentifier.Roof:
                    return SchemeEntry.Roof;
                case PhotoIdentifier.LeftDoorFront:
                    return SchemeEntry.LeftDoorFront;
                case PhotoIdentifier.RightDoorFront:
                    return SchemeEntry.RightDoorFront;
                case PhotoIdentifier.LeftDoorBack:
                    return SchemeEntry.LeftDoorBack;
                case PhotoIdentifier.RightDoorBack:
                    return SchemeEntry.RightDoorBack;
                case PhotoIdentifier.Trunk:
                    return SchemeEntry.Trunk;
                case PhotoIdentifier.LeftWheelCapBack:
                    return SchemeEntry.LeftWheelCapBack;
                case PhotoIdentifier.RightWheelCapBack:
                    return SchemeEntry.RightWheelCapBack;
                case PhotoIdentifier.BumperBack:
                    return SchemeEntry.BumperBack;
                case PhotoIdentifier.LeftWheelBack:
                    return SchemeEntry.LeftWheelBack;
                case PhotoIdentifier.LeftWheelFront:
                    return SchemeEntry.LeftWheelFront;
                case PhotoIdentifier.RightWheelBack:
                    return SchemeEntry.RightWheelBack;
                case PhotoIdentifier.RightWheelFront:
                    return SchemeEntry.RightWheelFront;
                case PhotoIdentifier.Windshield:
                    return SchemeEntry.Windshield;
                case PhotoIdentifier.Underneath:
                    return SchemeEntry.Underneath;
                case PhotoIdentifier.Inside:
                    return SchemeEntry.Inside;
                default:
                    LogHelper.Debug(typeof(PhotoService), $"The right identifier couldn't be found. PhotoId: {photoIdentifier}");
                    return SchemeEntry.BumperFront;
            }
        }
    }
}
