using System.Web.Hosting;
using System.Web.Http;
using DevdayDemo.ServiceLite.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

namespace DevdayDemo.ServiceLite.Features
{
    public sealed class WebApiFeature : Plugin
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

        public override void Configure(IAppHost appHost, IServiceCollection container)
        {
            if (HttpConfiguration == null)
            {
                HttpConfiguration = new HttpConfiguration(new HttpRouteCollection(HostingEnvironment.ApplicationVirtualPath));
            }
            appHost.Set(HttpConfiguration);
        }

        public override void Register(IAppHost appHost)
        {
            var app = appHost.Get<IAppBuilder>();
            var config = appHost.Get<HttpConfiguration>();

            Configure(config);

            app.UseWebApi(config);
        }

        private void Configure(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy;

            ConfigureFormatters(config);
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