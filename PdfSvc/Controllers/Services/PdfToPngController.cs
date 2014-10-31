﻿using PdfSvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class PdfToPngController : BaseApiController
    {
        IFileConverter _fileConverter;

        //ToDo: FileConveteter should be injected from constructor
        public PdfToPngController()
        {
            _fileConverter = new FileConverter();
        }

        public HttpResponseMessage Post(Byte[] pdfFile)
        {
            try
            {
               var pngFile = _fileConverter.PdfToPng(pdfFile);

               return CamelCaseJsonResponse(pngFile);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
