using System;
using System.Web.Configuration;

namespace FelixWebsite.Bll.Helpers
{
    public static class ConfigHelper
    {
        #region Facebook Settings

        public static string FacebookVersion => GetValue<string>("fb_auth_version");
        public static string FacebookReviewsRedirectUri => GetValue<string>("fb_redirect_uri");
        public static string FacebookClientId => GetValue<string>("fb_client_id");
        public static string FacebookSecret => GetValue<string>("fb_client_secret");
        public static string FacebookAuthUrl => GetValue<string>("fb_auth_url");
        public static string FacebookGraphUrl => GetValue<string>("fb_graph_url");
        public static string FacebookScope => GetValue<string>("fb_scope");

        #endregion Facebook Settings

        #region Google Settings

        public static object GoogleMapsApiKey => GetValue<string>("GoogleMapsApiKey");
        public static string GoogleRedirectUri => GetValue<string>("google_redirect_uri");
        public static string GoogleClientId => GetValue<string>("google_client_id");
        public static string GoogleSecret => GetValue<string>("google_client_secret");
        public static string GoogleScope => GetValue<string>("google_scope");
        public static string GoogleAuthUrl => GetValue<string>("google_auth_url");
        public static string GoogleGraphUrl => GetValue<string>("google_graph_url");

        #endregion Google Settings

        #region Insurance settings

        public static string CarrosserieApiAuthToken => GetValue<string>("Carrosserie_API_auth_token");
        public static string InsuranceApiUrl => GetValue<string>("Insurance_API_url");
        public static string DamageDeclarationApiUrl => GetValue<string>("DamageDeclaration_API_url");

        #endregion Insurance settings

        #region Acquisition Tool

        public static double UploadedPictureWidth => GetValue<double>("UploadedPictureWidth");

        #endregion Acquisition Tool

        #region Mail settings & Subjects

        public static string MailTemplateDirectory => GetValue<string>("MailTemplateDirectory");
        public static string SubjectTestDrive => GetValue<string>("SubjectTestDrive");
        public static string SubjectQuotation => GetValue<string>("SubjectQuotation");
        public static string Subject => GetValue<string>("Subject");

        public static string Url => GetValue<string>("DomainUrl");

        #endregion Mail settings & Subjects

        #region VDAB Competences

        public static string CompetencesUser => GetValue<string>("CompetentiesUser");
        public static string CompetencesPassword => GetValue<string>("CompetentiesPass");
        public static string UrlCompetencesPattern => GetValue<string>("UrlCompetentiePattern");
        public static string UrlCompetences => GetValue<string>("UrlCompetenties");

        #endregion VDAB Competences

        #region VDAB JobOffers

        public static string SupplierId => GetValue<string>("SupplierId");
        public static string NameSpaceXML => GetValue<string>("NameSpaceXML");
        public static string UrlJobApplication => GetValue<string>("UrlJobApplication");
        public static string AuthorisationKeyVDAB => GetValue<string>("AuthorisationVDAB");

        #endregion VDAB JobOffers

        private static T GetValue<T>(string key)
        {
            var value = WebConfigurationManager.AppSettings[key];
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}