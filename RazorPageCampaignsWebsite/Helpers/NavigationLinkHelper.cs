
using Blackpool.Zengenti.CMS.Helpers;
using Blackpool.Zengenti.CMS.Models.Canvas.Common;
using Blackpool.Zengenti.CMS.Models.Canvas.Helpers;
using Blackpool.Zengenti.CMS.Models.Canvas.Paragraphs;
using RazorPageCampaignsWebsite.Constants;
using Zengenti.Contensis.Delivery;

namespace RazorPageCampaignsWebsite.Helpers
{
    public static class NavigationLinkHelper
    {

        public static string GetHomeLink()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                return string.Format("/{0}", WebsiteConstants.SITE_VIEW_PATH);
            }
            return "/";
        }

        public static string? GetLinkUrl(string? url)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                string? temp = (url != null) ? url.Replace(WebsiteConstants.SITE_VIEW_PATH, string.Empty).Trim() : url;
                return url;
            }
            return url;
        }

        public static FragmentParagraph GetLinkUrl(FragmentParagraph fp)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                if (fp.Value != null && fp.Value.Count > 0)
                {
                    var val = fp.Value.First();
                    string? content = val.Type != null ? val.Type.ToString() : string.Empty;
                    if (val.Value != null && ParagraphHelper.CheckListType(content) == StandardListEnum.link)
                    {
                        string? temp = (val.Properties != null && val.Properties.Link != null )
                           ? val.Properties.Link?.System?.Uri : string.Empty;

                        if (val.Properties != null && val.Properties.Link != null && val.Properties.Link.System != null && temp != null)
                        {
                            //temp = temp.Replace(WebsiteConstants.SITE_VIEW_PATH, string.Empty);
                            ContentLink? linktoChange = val.Properties?.Link;
                            if (linktoChange != null && linktoChange.System != null)
                            {
                                linktoChange.System.Uri = temp;
                            }
                        }

                    }

                }


            }
            return fp;
        }
    }


}


