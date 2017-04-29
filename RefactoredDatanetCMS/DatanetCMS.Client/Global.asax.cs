using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using DatanetCMS.Client.App_Start;
using StackExchange.Profiling;
using Umbraco.Web;

namespace DatanetCMS.Client
{
    public class Global: UmbracoApplication
    {
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            base.OnApplicationStarted(sender, e);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

       
    }
}