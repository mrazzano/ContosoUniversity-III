﻿using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace ContosoUniversity.ActionFilters
{
    public sealed class WhitespaceFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            var response = filterContext.HttpContext.Response;
            if (filterContext.HttpContext.Request.RawUrl == "/sitemap.xml")
                return;

            if (response.ContentType != "text/html" || response.Filter == null)
                return;

            response.Filter = new HelperClass(response.Filter);
        }

        private class HelperClass : Stream
        {
            private readonly Stream _base;

            public HelperClass(Stream responseStream)
            {
                if (responseStream == null)
                    throw new ArgumentNullException("responseStream");

                _base = responseStream;
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                var html = Encoding.UTF8.GetString(buffer, offset, count);
                var reg = new Regex(@"(?<=\s)\s+(?![^<>]*</pre>)");
                html = reg.Replace(html, string.Empty);
                buffer = Encoding.UTF8.GetBytes(html);
                _base.Write(buffer, 0, buffer.Length);
            }

            #region Other Members
            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
            public override bool CanRead { get { return false; } }
            public override bool CanSeek { get { return false; } }
            public override bool CanWrite { get { return true; } }
            public override long Length { get { throw new NotSupportedException(); } }
            public override long Position
            {
                get { throw new NotSupportedException(); }
                set { throw new NotSupportedException(); }
            }
            public override void Flush()
            {
                _base.Flush();
            }
            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }
            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }
            #endregion
        }
    }
}