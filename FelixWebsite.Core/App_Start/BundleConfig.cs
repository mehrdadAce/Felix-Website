using System.Web.Optimization;

namespace FelixWebsite.Core.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        { 
            #region CSS
           // Shared CSS
            Bundle cssShared = new StyleBundle("~/Content/CSS").Include(
                "~/bootstrap/css/bootstrap.css",
                "~/ion-icons/css/ionicons.css",
                "~/css/shared/animate.css",
                "~/css/shared/footer.css",
                "~/css/shared/navigation.css",
                "~/css/shared/shared.css",               
                "~/css/shared/slider/settings.css",
                "~/css/shared/slider/slider.css",
                "~/css/toastr/toastr.min.css"
                );

            // Socials CSS
            Bundle cssSocials = new StyleBundle("~/Content/Socials").Include(
                "~/css/socials/jssocials.css",
                "~/css/socials/jssocials-theme-flat.css"
                );

            bundles.Add(cssShared);
            bundles.Add(cssSocials);
            #endregion

            #region Script bundles 

            // Shared Scripts
            Bundle scriptsShared = new ScriptBundle("~/Js/Shared").Include(
                "~/bootstrap/js/bootstrap.min.js",
                "~/scripts/wow.min.js",
                "~/scripts/smoothscroll.js",
                "~/scripts/nav-menu-collapser.js",
                "~/scripts/accordion.js",
                "~/scripts/actions/newsletter.js",
                "~/scripts/Toastr/toastr.min.js",
                "~/scripts/shared.js"
                );

            Bundle scriptsJQuery = new ScriptBundle("~/Js/JQuery").Include(
                "~/scripts/Jquery-v1.11.2.js"
                );

            // Slider Scripts             
            Bundle scriptsSlider = new ScriptBundle("~/Js/Slider").Include(
                "~/scripts/slider/jquery.themepunch.tools.min.js",
                "~/scripts/slider/jquery.themepunch.revolution.js",
                "~/scripts/slider/owl.carousel.min.js",
                "~/scripts/slider/rev-custom.js"
                );

            // Google Maps Scripts
            Bundle scriptsGoogleMaps = new ScriptBundle("~/Js/GoogleMaps").Include(
                "~/scripts/Google Maps/Spiderfier.js",
                "~/scripts/Google Maps/google-maps.js"
                );

            // Action Froms Scripts
            Bundle scriptsActions = new ScriptBundle("~/Js/Actions").Include(
                "~/scripts/Actions/form.js"
                );

            Bundle scriptsSocials = new ScriptBundle("~/Js/Socials").Include(
                "~/scripts/Socials/jssocials.min.js",
                "~/scripts/Socials/socials.js"
                );

            Bundle scriptsAcquisition = new ScriptBundle("~/Js/Acquisition").Include(
                "~/bootstrap/bootstrap-notify-master/bootstrap-notify.min.js",
                "~/scripts/Notifications.js",
                "~/scripts/Acquisition/Panels.js",
                "~/scripts/Acquisition/felix-transfer-tool.js",
                "~/scripts/Acquisition/AjaxCalls.js",
                "~/scripts/Acquisition/scheme.js"
                );

            Bundle scriptsCarDetails = new ScriptBundle("~/Js/CarDetails").Include(
                "~/scripts/second hand/umbraco-business-data.js",
                "~/scripts/second hand/carflow-car-details.js"
                );

            Bundle scriptsCarStock = new ScriptBundle("~/Js/CarStock").Include(
                "~/scripts/second hand/carflow-car-stock.js",
                "~/scripts/second hand/custom-checkboxes.js"
                );

            Bundle imageMap = new ScriptBundle("~/Js/ImageMap").Include(
                "~/scripts/image-map/dist/image-map.js"
                );

            bundles.Add(scriptsJQuery);
            bundles.Add(scriptsShared);
            bundles.Add(scriptsGoogleMaps);
            bundles.Add(scriptsSlider);
            bundles.Add(scriptsActions);
            bundles.Add(scriptsSocials);
            bundles.Add(scriptsAcquisition);
            bundles.Add(scriptsCarDetails);
            bundles.Add(scriptsCarStock);
            bundles.Add(imageMap);
            #endregion
        }
    }
}