using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using AC.Core.Utils;

namespace AC.Core.Handlers
{
    [ExcludeFromCodeCoverage]
    public class SetupJsHandler : IHttpHandler, IReadOnlySessionState
    {
        public bool IsReusable { get { return false; } }

        protected RequestContext RequestContext { get; set; }

        public SetupJsHandler()
        {
        }

        public SetupJsHandler(RequestContext requestContext)
        {
            RequestContext = requestContext;
        }

        public void ProcessRequest(HttpContext context)
        {
            var cultureUtility = DependencyResolver.Current.GetService<CultureUtility>();
            var cultureCode = cultureUtility.GetCultureCode();

            var sb = new StringBuilder();

            sb.AppendFormat("window.culture='{0}';", cultureCode);

            // CSRF
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            sb.AppendFormat("window.csrfToken='{0}:{1}';", cookieToken, formToken);
            sb.AppendFormat("window.csrfTokenName='{0}';", Constants.CsrfTokenName);

            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = "text/javascript";

            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }
    }
}
