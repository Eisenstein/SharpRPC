﻿using System;
using System.Net;
using SharpRpc.Interaction;
using System.Linq;

namespace SharpRpc.ClientSide
{
    public class HttpRequestSender : IRequestSender
    {
        public string Protocol { get { return "http"; } }

        public Response Send(string host, int port, Request request)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException("Host name cannot be null, emtry, or whitespace");

            var httpWebRequest = WebRequest.CreateHttp(string.Format("http://{0}:{1}/{2}", host, port, request.Path));

            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/octet-stream";
            if (!string.IsNullOrEmpty(request.ServiceScope))
                httpWebRequest.Headers["scope"] = request.ServiceScope;

            using (var stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(request.Data, 0, request.Data.Length);
            }

            Response response;
            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                int dataLength;
                if (!httpWebResponse.Headers.AllKeys.Contains("data-length") || !int.TryParse(httpWebResponse.Headers["data-length"], out dataLength))
                    dataLength = 0;

                var responseData = new byte[dataLength];
                using (var stream = httpWebResponse.GetResponseStream())
                {
                    if (stream != null)
                        stream.Read(responseData, 0, responseData.Length);
                }
                int status;
                if (!httpWebResponse.Headers.AllKeys.Contains("status") || !int.TryParse(httpWebResponse.Headers["status"], out status))
                    status = (int)ResponseStatus.Unknown;
                response = new Response((ResponseStatus)status, responseData);
            }
            return response;
        }
    }
}