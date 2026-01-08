using Blackpool.Zengenti.CMS.Models.Components;
using Microsoft.AspNetCore.Html;

namespace RazorPageCampaignsWebsite.Helpers.Html
{
    public static class HtmlVideoContentExtensions
    {
        public static void AppendVideoEmbed(this IHtmlContentBuilder htmlContent, string videoUrl,
            string containerClass = "", string wrapperStyle = null)
        {
            if (string.IsNullOrEmpty(videoUrl))
                return;

            var defaultWrapperStyle = "text-align:center; margin:auto; min-height:375px; height:375px; width:92%;";

            var videoHtml = $@"
            <div class=""row equal zero-margin-all"">
                <div class=""col-md-12 col-sm-12 col-xs-12 zero-pad-all clearfix"" style=""margin-bottom:20px; margin-top:30px; margin-left:0px; margin-right:0px;"">
                    <div id=""video"" class=""editor {containerClass}"">
                        <div class=""iframe-wrapper"" style=""{wrapperStyle ?? defaultWrapperStyle}"">
                            <iframe id=""iframe"" src=""{videoUrl}"" frameborder=""0"" height=""100%"" width=""100%"" allowfullscreen></iframe>
                        </div>
                    </div>
                </div>
            </div>";

            htmlContent.AppendHtml(videoHtml);
        }

        public static void AppendVideoEmbed(this IHtmlContentBuilder htmlContent, Video video)
        {
            if (video == null || string.IsNullOrEmpty(video.Url))
                return;

            htmlContent.AppendVideoEmbed(video.Url);
        }
    }
}
