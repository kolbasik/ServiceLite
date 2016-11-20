using System.Web.Hosting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using ServiceLite.Core;

namespace ServiceLite.WebApi
{
    public sealed class WebApiFeature : IPlugin, IPreConfigurable, IPostStartable
    {
        public WebApiFeature()
        {
            IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
            CleanFormatters = true;
            XmlFormatterEnabled = false;
            XmlUseXmlSerializer = false;
            FormFormatterEnabled = false;
            JsonFormatterEnabled = true;
            JsonContractResolver = new CamelCasePropertyNamesContractResolver();
            JsonDefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            JsonReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        public HttpConfiguration HttpConfiguration { get; set; }
        public IncludeErrorDetailPolicy IncludeErrorDetailPolicy { get; set; }
        public bool CleanFormatters { get; set; }
        public bool XmlFormatterEnabled { get; set; }
        public bool XmlUseXmlSerializer { get; set; }
        public bool FormFormatterEnabled { get; set; }
        public bool JsonFormatterEnabled { get; set; }
        public IContractResolver JsonContractResolver { get; set; }
        public DefaultValueHandling JsonDefaultValueHandling { get; set; }
        public ReferenceLoopHandling JsonReferenceLoopHandling { get; set; }

        public void PreConfigure(ConfigurationContext context)
        {
            if (HttpConfiguration == null)
            {
                HttpConfiguration = new HttpConfiguration(new HttpRouteCollection(HostingEnvironment.ApplicationVirtualPath));
            }
            context.AppHost.Set(HttpConfiguration);
        }

        public void Start(StartContext context)
        {
            var config = context.AppHost.Get<HttpConfiguration>();

            config.MapHttpAttributeRoutes();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy;

            ConfigureFormatters(config);
        }

        public void PostStart(StartContext context)
        {
            var app = context.AppHost.Get<IAppBuilder>();
            var config = context.AppHost.Get<HttpConfiguration>();

            app.UseWebApi(config);
        }

        private void ConfigureFormatters(HttpConfiguration config)
        {
            var xml = config.Formatters.XmlFormatter;
            var form = config.Formatters.FormUrlEncodedFormatter;
            var json = config.Formatters.JsonFormatter;

            if (CleanFormatters)
                config.Formatters.Clear();

            config.Formatters.Remove(xml);
            if (XmlFormatterEnabled)
            {
                config.Formatters.Insert(0, xml);
                xml.UseXmlSerializer = XmlUseXmlSerializer;
            }
            config.Formatters.Remove(form);
            if (FormFormatterEnabled)
            {
                config.Formatters.Insert(0, form);
            }
            config.Formatters.Remove(json);
            if (JsonFormatterEnabled)
            {
                config.Formatters.Insert(0, json);
                json.SerializerSettings.ContractResolver = JsonContractResolver;
                json.SerializerSettings.DefaultValueHandling = JsonDefaultValueHandling;
                json.SerializerSettings.ReferenceLoopHandling = JsonReferenceLoopHandling;
            }
        }
    }
}