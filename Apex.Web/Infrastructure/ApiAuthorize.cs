using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Apex.Web.Infrastructure
{
    public class ApiAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request
                .CreateResponse(HttpStatusCode.Forbidden, "not login");
        }
    }
}
