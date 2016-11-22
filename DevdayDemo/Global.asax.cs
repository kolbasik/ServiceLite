using System.Web;
using System.Web.Mvc;
using Boilerplate.Web.Mvc;
using DevdayDemo.Services;
using Ninject;
using Ninject.Web.Common;
using NWebsec.Csp;
using ServiceLite.Core;

namespace DevdayDemo
{
    public class Global : HttpApplication
    {
        private Bootstrapper bootstrapper;

        protected Global()
        {
            if (AppHost.ContainerType == AppHost.DiType.NInject)
                new OnePerRequestHttpModule().Init(this);
        }

        public void Application_Start()
        {
            lock (this)
            {
                if (AppHost.ContainerType == AppHost.DiType.NInject)
                {
                    bootstrapper = new Bootstrapper();
                    bootstrapper?.Initialize(() => new StandardKernel());
                }
            }
        }

        public void Application_End()
        {
            lock (this)
            {
                AppHostBase.Release();
                bootstrapper?.ShutDown();
            }
        }

        /// <summary>
        ///     Handles the Content Security Policy (CSP) violation errors. For more information see FilterConfig.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CspViolationReportEventArgs" /> instance containing the event data.</param>
        protected void NWebsecHttpHeaderSecurityModule_CspViolationReported(object sender, CspViolationReportEventArgs e)
        {
            // Log the Content Security Policy (CSP) violation.
            var violationReport = e.ViolationReport;
            var reportDetails = violationReport.Details;
            var violationReportString =
                string.Format(
                    "UserAgent:<{0}>\r\nBlockedUri:<{1}>\r\nColumnNumber:<{2}>\r\nDocumentUri:<{3}>\r\nEffectiveDirective:<{4}>\r\nLineNumber:<{5}>\r\nOriginalPolicy:<{6}>\r\nReferrer:<{7}>\r\nScriptSample:<{8}>\r\nSourceFile:<{9}>\r\nStatusCode:<{10}>\r\nViolatedDirective:<{11}>",
                    violationReport.UserAgent,
                    reportDetails.BlockedUri,
                    reportDetails.ColumnNumber,
                    reportDetails.DocumentUri,
                    reportDetails.EffectiveDirective,
                    reportDetails.LineNumber,
                    reportDetails.OriginalPolicy,
                    reportDetails.Referrer,
                    reportDetails.ScriptSample,
                    reportDetails.SourceFile,
                    reportDetails.StatusCode,
                    reportDetails.ViolatedDirective);
            var exception = new CspViolationException(violationReportString);
            DependencyResolver.Current.GetService<ILoggingService>().Log(exception);
        }
    }
}