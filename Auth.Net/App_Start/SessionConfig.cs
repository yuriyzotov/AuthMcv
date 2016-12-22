using Microsoft.Owin.Extensions;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Auth.Net.App_Start
{
    public static class SessionConfig
    {
        public static void RequireAspNetSession(this IAppBuilder app)
        {
            app.Use((context, next) =>
            {
                var httpContext = context.Get<HttpContextBase>(typeof(HttpContextBase).FullName);
                httpContext.SetSessionStateBehavior(SessionStateBehavior.Required);
                return next();
            });
            // To make sure the above `Use` is in the correct position:
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}