﻿using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Boilerplate.Web.Mvc;
using Boilerplate.Web.Mvc.Filters;
using DevdayDemo.Constants;
using DevdayDemo.Services;

namespace DevdayDemo.Controllers
{
    public sealed class HomeController : Controller
    {
        private readonly IBrowserConfigService browserConfigService;
        private readonly IFeedService feedService;
        private readonly IManifestService manifestService;
        private readonly IOpenSearchService openSearchService;
        private readonly IRobotsService robotsService;
        private readonly ISitemapService sitemapService;

        public HomeController(
            IBrowserConfigService browserConfigService,
            IFeedService feedService,
            IManifestService manifestService,
            IOpenSearchService openSearchService,
            IRobotsService robotsService,
            ISitemapService sitemapService)
        {
            this.browserConfigService = browserConfigService;
            this.feedService = feedService;
            this.manifestService = manifestService;
            this.openSearchService = openSearchService;
            this.robotsService = robotsService;
            this.sitemapService = sitemapService;
        }

        [Route("", Name = HomeControllerRoute.GetIndex)]
        public ActionResult Index()
        {
            return View(HomeControllerAction.Index);
        }

        [Route("about", Name = HomeControllerRoute.GetAbout)]
        public ActionResult About()
        {
            return View(HomeControllerAction.About);
        }

        [Route("contact", Name = HomeControllerRoute.GetContact)]
        public ActionResult Contact()
        {
            return View(HomeControllerAction.Contact);
        }

        /// <summary>
        ///     Gets the Atom 1.0 feed for the current site. Note that Atom 1.0 is used over RSS 2.0 because Atom 1.0 is a
        ///     newer and more well defined format. Atom 1.0 is a standard and RSS is not. See
        ///     http://rehansaeed.com/building-rssatom-feeds-for-asp-net-mvc/
        /// </summary>
        /// <returns>The Atom 1.0 feed for the current site.</returns>
        [OutputCache(CacheProfile = CacheProfileName.Feed)]
        [Route("feed", Name = HomeControllerRoute.GetFeed)]
        public async Task<ActionResult> Feed()
        {
            // A CancellationToken signifying if the request is cancelled. See
            // http://www.davepaquette.com/archive/2015/07/19/cancelling-long-running-queries-in-asp-net-mvc-and-web-api.aspx
            var cancellationToken = Response.ClientDisconnectedToken;
            return new AtomActionResult(await feedService.GetFeed(cancellationToken));
        }

        [Route("search", Name = HomeControllerRoute.GetSearch)]
        public ActionResult Search(string query)
        {
            // You can implement a proper search function here and add a Search.cshtml page.
            // return this.View(HomeControllerAction.Search);

            // Or you could use Google Custom Search (https://cse.google.com/cse) to index your site and display your
            // search results in your own page.

            // For simplicity we are just assuming your site is indexed on Google and redirecting to it.
            return Redirect(string.Format("https://www.google.com/?q=site:{0} {1}", Url.AbsoluteRouteUrl(HomeControllerRoute.GetIndex), query));
        }

        /// <summary>
        ///     Gets the browserconfig XML for the current site. This allows you to customize the tile, when a user pins
        ///     the site to their Windows 8/10 start screen. See http://www.buildmypinnedsite.com and
        ///     https://msdn.microsoft.com/en-us/library/dn320426%28v=vs.85%29.aspx
        /// </summary>
        /// <returns>The browserconfig XML for the current site.</returns>
        [NoTrailingSlash]
        [OutputCache(CacheProfile = CacheProfileName.BrowserConfigXml)]
        [Route("browserconfig.xml", Name = HomeControllerRoute.GetBrowserConfigXml)]
        public ContentResult BrowserConfigXml()
        {
            Trace.WriteLine(string.Format("browserconfig.xml requested. User Agent:<{0}>.", Request.Headers.Get("User-Agent")));
            var content = browserConfigService.GetBrowserConfigXml();
            return Content(content, ContentType.Xml, Encoding.UTF8);
        }

        /// <summary>
        ///     Gets the manifest JSON for the current site. This allows you to customize the icon and other browser
        ///     settings for Chrome/Android and FireFox (FireFox support is coming). See https://w3c.github.io/manifest/
        ///     for the official W3C specification. See http://html5doctor.com/web-manifest-specification/ for more
        ///     information. See https://developer.chrome.com/multidevice/android/installtohomescreen for Chrome's
        ///     implementation.
        /// </summary>
        /// <returns>The manifest JSON for the current site.</returns>
        [NoTrailingSlash]
        [OutputCache(CacheProfile = CacheProfileName.ManifestJson)]
        [Route("manifest.json", Name = HomeControllerRoute.GetManifestJson)]
        public ContentResult ManifestJson()
        {
            Trace.WriteLine(string.Format("manifest.jsonrequested. User Agent:<{0}>.", Request.Headers.Get("User-Agent")));
            var content = manifestService.GetManifestJson();
            return Content(content, ContentType.Json, Encoding.UTF8);
        }

        /// <summary>
        ///     Gets the Open Search XML for the current site. You can customize the contents of this XML here. The open
        ///     search action is cached for one day, adjust this time to whatever you require. See
        ///     http://www.hanselman.com/blog/CommentView.aspx?guid=50cc95b1-c043-451f-9bc2-696dc564766d
        ///     http://www.opensearch.org
        /// </summary>
        /// <returns>The Open Search XML for the current site.</returns>
        [NoTrailingSlash]
        [OutputCache(CacheProfile = CacheProfileName.OpenSearchXml)]
        [Route("opensearch.xml", Name = HomeControllerRoute.GetOpenSearchXml)]
        public ContentResult OpenSearchXml()
        {
            Trace.WriteLine(string.Format("opensearch.xml requested. User Agent:<{0}>.", Request.Headers.Get("User-Agent")));
            var content = openSearchService.GetOpenSearchXml();
            return Content(content, ContentType.Xml, Encoding.UTF8);
        }

        /// <summary>
        ///     Tells search engines (or robots) how to index your site.
        ///     The reason for dynamically generating this code is to enable generation of the full absolute sitemap URL
        ///     and also to give you added flexibility in case you want to disallow search engines from certain paths. The
        ///     sitemap is cached for one day, adjust this time to whatever you require. See
        ///     http://rehansaeed.com/dynamically-generating-robots-txt-using-asp-net-mvc/
        /// </summary>
        /// <returns>The robots text for the current site.</returns>
        [NoTrailingSlash]
        [OutputCache(CacheProfile = CacheProfileName.RobotsText)]
        [Route("robots.txt", Name = HomeControllerRoute.GetRobotsText)]
        public ContentResult RobotsText()
        {
            Trace.WriteLine(string.Format("robots.txt requested. User Agent:<{0}>.", Request.Headers.Get("User-Agent")));
            var content = robotsService.GetRobotsText();
            return Content(content, ContentType.Text, Encoding.UTF8);
        }

        /// <summary>
        ///     Gets the sitemap XML for the current site. You can customize the contents of this XML from the
        ///     <see cref="SitemapService" />. The sitemap is cached for one day, adjust this time to whatever you require.
        ///     http://www.sitemaps.org/protocol.html
        /// </summary>
        /// <param name="index">
        ///     The index of the sitemap to retrieve. <c>null</c> if you want to retrieve the root
        ///     sitemap file, which may be a sitemap index file.
        /// </param>
        /// <returns>The sitemap XML for the current site.</returns>
        [NoTrailingSlash]
        [Route("sitemap.xml", Name = HomeControllerRoute.GetSitemapXml)]
        public ActionResult SitemapXml(int? index = null)
        {
            var content = sitemapService.GetSitemapXml(index);
            if (content == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Sitemap index is out of range.");
            return Content(content, ContentType.Xml, Encoding.UTF8);
        }
    }
}