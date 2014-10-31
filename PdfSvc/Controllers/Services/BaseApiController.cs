using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

// PdfSvc - Converts PDF files to and from various file formats
// Copyright (C) 2014  Anand Khedkar - Ted Spence

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace PdfSvc.Controllers
{
    public class BaseApiController : ApiController
    {
        [NonAction]
        public HttpResponseMessage CamelCaseJsonResponse(dynamic data)
        {
            /* serialize the filing calendar detail to camel case */
            var resolver = new CamelCasePropertyNamesContractResolver();
            var serializerSettings = new JsonSerializerSettings() { ContractResolver = resolver };
            string json = JsonConvert.SerializeObject(data, serializerSettings);//, Formatting.Indented

            /* send the json in the response */
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }

        [NonAction]
        public HttpResponseMessage CamelCaseJsonErrorResponse(dynamic data, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            /* serialize the filing calendar detail to camel case */
            var resolver = new CamelCasePropertyNamesContractResolver();
            var serializerSettings = new JsonSerializerSettings() { ContractResolver = resolver };
            string json = JsonConvert.SerializeObject(data, serializerSettings);//, Formatting.Indented

            /* send the json in the response */
            var response = this.Request.CreateResponse(statusCode);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }
    }
}
