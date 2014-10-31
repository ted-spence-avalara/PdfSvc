﻿using Newtonsoft.Json;
using PdfSvc.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    public class RenderPdfController : Controller
    {
        // GET: RenderPdf
        [HttpPost]
        public ActionResult Render(HttpPostedFileBase pdf)
        {
            // Read in bytes
            byte[] pdf_bytes = null;
            using (var ms = new MemoryStream()) {
                pdf.InputStream.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                pdf_bytes = ms.GetBuffer();
            }

            // Convert
            FileConverter fc = new FileConverter();
            var jr = new JsonResult();
            jr.MaxJsonLength = Int32.MaxValue;
            jr.Data = fc.PdfToPng(pdf_bytes);
            return jr;
        }
    }
}