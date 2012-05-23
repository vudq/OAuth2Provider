﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using CrackerJack.OAuth;
using CrackerJack.OAuth.Request;
using CrackerJack.OAuth.Response;

namespace WebApiSample.Controllers
{
    public class OAuthController : ApiController
    {

        public ActionResult Token()
        {
            try
            {
                var oauthRequest = new TokenRequest(Request, MvcApplication.ServiceLocator);

                var token = oauthRequest.Authorize();

                if (token.RedirectsUri.HasValue())
                {

                    var redirectUri = OAuthResponse
                        .TokenResponse(token.AccessToken, token.ExpiresIn, token.RefreshToken)
                        .SetLocation(token.RedirectsUri)
                        .BuildQueryMessage().LocationUri;

                    return Redirect(redirectUri);
                }

                var response = OAuthResponse
                            .TokenResponse(token.AccessToken, token.ExpiresIn, token.RefreshToken)
                            .BuildJsonMessage();

                return this.OAuth(response);
            }
            catch (OAuthException ex)
            {
                var response = new ErrorResponseBuilder(ex).BuildJsonMessage();
                return this.OAuth(response);
            }
        }
    }
}
