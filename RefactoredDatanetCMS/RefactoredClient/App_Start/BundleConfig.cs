using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace RefactoredClient
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/Js").Include(
                //plugins
                "~/Scripts/Plugins/jquery/mondernizr-2.6.2.js",
                "~/Scripts/Plugins/jquery/jquery-2.2.4.min.js",
                "~/Scripts/Plugins/moment/moment.min.js",
                "~/Scripts/Plugins/jquery/tether.min.js",
                "~/Scripts/Plugins/jquery/bootstrap.min.js",
                "~/Scripts/Plugins/jquery/bootbox.min.js",
                "~/Scripts/Plugins/jquery/boottypehead.js",
                "~/Scripts/Plugins/datepicker/datetimepicker.min.js",
                //angular
                "~/Scripts/Plugins/angular/angular.min.js",
                "~/Scripts/Plugins/angular/angular-ui.router.min.js",
                "~/Scripts/Plugins/angular/angular-animate.min.js",
                "~/Scripts/Plugins/angular/angular-aria.min.js",
                "~/Scripts/Plugins/angular/angular-messages.min.js",
                "~/Scripts/Plugins/angular/angular-sanitize.min.js",
                "~/Scripts/Plugins/angular/angular-sanitize.min.js",
                "~/Scripts/Plugins/angular-growl/angular-growl.min.js",
                "~/Scripts/Plugins/jquery/isteven-multi-select.js",
                "~/Scripts/Config/app.js",
                "~/Scripts/Plugins/ng-upload-file/ng-file-upload-shim.js",
                 "~/Scripts/Plugins/swiper/swiper.min.js",
                "~/Scripts/Plugins/ng-upload-file/ng-file-upload.min.js"
                ).IncludeDirectory("~/Scripts", "*.js", true));

            //CSS
            BundleTable.Bundles.Add(new StyleBundle("~/bundles/css").Include(
                 "~/Content/Style/reset.css",
                "~/Content/Style/bootstrap.min.css",
                "~/Content/Style/glyphicons.css",
                "~/Content/Style/datetimepicker.css",
                "~/Content/Style/isteven-multi-select.css",
                //plugins
                "~/Scripts/Plugins/angular-growl/angular-growl.min.css",
                "~/Content/Style/colors.css", 
                "~/Content/Style/base.css",
                //"~/Content/Style/style.css",
                "~/Content/Style/layout.css"
                ).IncludeDirectory("~/Scripts/Component/", "*.css", true));
            BundleTable.EnableOptimizations = false;
        }
    }
}