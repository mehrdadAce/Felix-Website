using FelixWebsite.Bdo.Enums;
using System;
using System.Linq;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Helpers
{
    public static class MailSettingsHelper
    {
        public static EmailSetting GetEmailSettingForAction(Umbraco.Web.UmbracoHelper Umbraco, ActionType type, int storeId, string secondHandBusinessBrandNodeId, string modelName)
        {
            var currentPage = Umbraco.AssignedContentItem;
            if (currentPage is Brand)
            {
                var brand = currentPage.OfType<Brand>();
                if (brand.Modellen.Contains(modelName))
                    return GetEmailSettingForType(brand, type);
            }

            if (currentPage is BusinessBrand)
            {
                var businessBrand = currentPage.OfType<BusinessBrand>();
                if (businessBrand.Brand.FirstOrDefault().OfType<Brand>().Modellen.Contains(modelName))
                    return GetEmailSettingForType(businessBrand, type);
            }

            if (currentPage is BusinessCombined)
            {
                var businessBrandList = currentPage.OfType<BusinessCombined>().BusinessItems.Select(x => x.OfType<BusinessBrand>()).ToList();
                var selectedBusiness = businessBrandList.FirstOrDefault(x => x.Id == storeId);
                if (selectedBusiness != null)
                    return GetEmailSettingForType(selectedBusiness, type);
            }

            if (currentPage is Tweedehandswagen)
            {
                var businessBrand = (BusinessBrand)Umbraco.TypedContent(secondHandBusinessBrandNodeId);
                return GetEmailSettingForType(businessBrand, type);
            }

            return null;
        }

        private static EmailSetting GetEmailSettingForType(IEmailSettingsActions mailSettingActions, ActionType type)
        {
            if (mailSettingActions == null)
                return null;

            switch (type)
            {
                case ActionType.TestDrive:
                    return mailSettingActions.TestEmailSettings as EmailSetting;
                case ActionType.Quotation:
                    return mailSettingActions.QuotationEmailSettings as EmailSetting;
                default:
                    return null;
            }
        }

        public static bool EmailSettingIsValid(EmailSetting emailSetting)
        {
            if (emailSetting == null)
            {
                Log4NetHelper.Instance.DebugLogErrorMessage("Fout: EmailSettingIsValid()", new Exception("EmailSettings is null."));
                return false;
            }

            if (string.IsNullOrWhiteSpace(emailSetting.To) || !emailSetting.To.IsValidEmail())
            {
                Log4NetHelper.Instance.DebugLogErrorMessage("Fout: EmailSettingIsValid()", new Exception($"Emailadres ({emailSetting.To}) van de ontvanger is niet geldig."));
                return false;
            }

            if (string.IsNullOrWhiteSpace(emailSetting.From) || !emailSetting.From.IsValidEmail())
            {
                Log4NetHelper.Instance.DebugLogErrorMessage("Fout: EmailSettingIsValid()", new Exception($"Emailadres ({emailSetting.To}) van de verzender is niet geldig."));
                return false;
            }

            return true;
        }
    }
}
