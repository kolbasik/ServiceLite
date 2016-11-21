using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using ServiceLite.Core;
using Swashbuckle.Application;
using Swashbuckle.Swagger;

namespace ServiceLite.Swagger
{
    /// <summary>
    ///     https://github.com/domaindrivendev/Swashbuckle
    /// </summary>
    public sealed class SwaggerFeature : IPlugin
    {
        public SwaggerFeature()
        {
            IgnoreObsoleteActions = true;
            IgnoreObsoleteProperties = true;
            DescribeAllEnumsAsStrings = true;
            UseFullTypeNameInSchemaIds = true;
            UsePrettyPrint = true;
            UseResolveConflictingActions = false;
            UseApiVersions = true;
            ApiVersions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            UseUI = true;
        }

        public bool IgnoreObsoleteActions { get; set; }
        public bool IgnoreObsoleteProperties { get; set; }
        public bool DescribeAllEnumsAsStrings { get; set; }
        public bool UseFullTypeNameInSchemaIds { get; set; }
        public bool UsePrettyPrint { get; set; }
        public bool UseResolveConflictingActions { get; set; }
        public bool UseApiVersions { get; set; }
        public Dictionary<string, string> ApiVersions { get; }
        public bool UseUI { get; set; }

        public void Start(StartContext context)
        {
            var config = context.AppHost.GetRequired<HttpConfiguration>();

            var swagger = config.EnableSwagger(
                c =>
                {
                    if (IgnoreObsoleteActions)
                        c.IgnoreObsoleteActions();
                    if (IgnoreObsoleteProperties)
                        c.IgnoreObsoleteProperties();
                    if (DescribeAllEnumsAsStrings)
                        c.DescribeAllEnumsAsStrings();
                    if (UseFullTypeNameInSchemaIds)
                        c.UseFullTypeNameInSchemaIds();
                    if (UsePrettyPrint)
                        c.PrettyPrint();
                    if (UseResolveConflictingActions)
                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    if (UseApiVersions)
                        if (ApiVersions.Count == 1)
                        {
                            var apiVersion = ApiVersions.Single();
                            c.SingleApiVersion(apiVersion.Key, apiVersion.Value);
                        }
                        else if (ApiVersions.Count > 1)
                        {
                            var apiRegex = new Regex(
                                @"\b(?<version>v\d+)\b",
                                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            c.MultipleApiVersions(
                                (apiDesc, targetApiVersion) =>
                                {
                                    var match = apiRegex.Match(apiDesc.GetControllerAndActionAttributes<RoutePrefixAttribute>().First().Prefix);
                                    return match.Success && string.Equals(match.Groups["version"].Value, targetApiVersion, StringComparison.OrdinalIgnoreCase);
                                },
                                vc =>
                                {
                                    foreach (var apiVersion in ApiVersions)
                                        vc.Version(apiVersion.Key, apiVersion.Value);
                                });
                        }
                });

            if (UseUI)
                swagger.EnableSwaggerUi(c => c.EnableDiscoveryUrlSelector());
        }
    }
}