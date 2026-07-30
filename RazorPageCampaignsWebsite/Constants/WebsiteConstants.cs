namespace RazorPageCampaignsWebsite.Constants
{
  
    public static class WebsiteConstants
    {
        // URL path (can have hyphens)
        public static readonly string SITE_PATH = "campaigns";

        // Controller name (no hyphens)
        public static readonly string SITE_CONTROLLER = "campaigns";

        // Views folder (can be whatever you want)
        public static readonly string VIEW_FOLDER = "campaigns";

        // For backward compatibility
        public static readonly string SITE_NAME = SITE_CONTROLLER;
        public static readonly string SITE_VIEW_PATH = SITE_PATH + "/";
        public static readonly string SHARED_COMPONENTS_PATH = "~/Pages/Components";

    }
}
