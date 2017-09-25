using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OneBlog.Helpers
{
    public class AsyncHttpClient
    {
        private Uri _uri;
        private Dictionary<string, string> _headers;
        private string _encoding;

        public AsyncHttpClient Url(string url)
        {
            _uri = new Uri(url);
            return this;
        }

        public AsyncHttpClient Uri(Uri uri)
        {
            _uri = uri;
            return this;
        }


        public AsyncHttpClient Encoding(string encoding)
        {
            _encoding = encoding;
            Header("Encoding", encoding);
            return this;
        }


        public AsyncHttpClient Header(string name, string value)
        {
            if (_headers == null)
            {
                _headers = new Dictionary<string, string>();
            }
            _headers[name] = value;

            return this;
        }

        public AsyncHttpClient Cookies(string cookies)
        {
            if (cookies != null)
            {
                Header("Cookie", cookies);
            }
            return this;
        }

        public AsyncHttpClient Referer(string referer)
        {
            if (referer != null)
            {
                Header("Referer", referer);
            }
            return this;
        }

        public AsyncHttpClient UserAgent(string userAgent)
        {
            if (userAgent != null)
            {
                Header("User-Agent", userAgent);
            }
            return this;
        }

        public AsyncHttpClient ContentType(string contentType)
        {
            if (contentType != null)
            {
                Header("Content-Type", contentType);
            }
            return this;
        }

        public AsyncHttpClient Accept(string accept)
        {
            if (accept != null)
            {
                Header("Accept", accept);
            }
            return this;
        }


        public async Task<AsyncHttpResponse> Get()
        {
            var client = DoBuildHttpClient();

            try
            {
                using (var rsp = await client.GetAsync(_uri))
                {
                    return new AsyncHttpResponse(rsp, _encoding);
                }
            }
            catch (Exception ex)
            {
                return new AsyncHttpResponse(ex, _encoding);
            }
        }

        public async Task<AsyncHttpResponse> Post(Dictionary<string, string> args)
        {
            var client = DoBuildHttpClient();

            var postData = new FormUrlEncodedContent(args);

            try
            {
                using (var rsp = await client.PostAsync(_uri, postData))
                {
                    return new AsyncHttpResponse(rsp, _encoding);
                }
            }
            catch (Exception ex)
            {
                return new AsyncHttpResponse(ex, _encoding);
            }
        }

        private HttpClient DoBuildHttpClient()
        {

            HttpClient client = new HttpClient();

            if (_headers != null)
            {
                foreach (var kv in _headers)
                {
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                }
            }

            return client;
        }
    }

    public class AsyncHttpResponse : IDisposable
    {
        protected byte[] Data { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }
        public string Cookies { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public Encoding Encoding { get; private set; }
        public Exception Exception { get; private set; }

        internal AsyncHttpResponse(HttpResponseMessage rsp, string encoding)
        {
            this.StatusCode = rsp.StatusCode;
            this.Encoding = Encoding.GetEncoding(encoding ?? "UTF-8");
            this.Headers = new Dictionary<string, string>();
            this.Cookies = null;
            this.Exception = null;
            Init(rsp);
        }

        internal AsyncHttpResponse(Exception exp, string encoding)
        {
            this.StatusCode = 0;
            this.Encoding = Encoding.GetEncoding(encoding ?? "UTF-8");
            this.Headers = new Dictionary<string, string>();
            this.Cookies = null;
            this.Exception = exp;
        }

        protected async void Init(HttpResponseMessage rsp)
        {
            if (rsp.StatusCode == HttpStatusCode.OK)
            {
                this.Data = await rsp.Content.ReadAsByteArrayAsync();
            }

            if (rsp.Headers != null)
            {
                foreach (var kv in rsp.Headers)
                {
                    if ("Set-Cookie".Equals(kv.Key))
                    {
                        Cookies = string.Join(";", kv.Value);
                    }

                    Headers[kv.Key] = string.Join(";", kv.Value);
                }
            }
        }

        //public async Task<IRandomAccessStream> GetRandomStream()
        //{
        //    if (Data == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        var buffer = this.GetBuffer();
        //        InMemoryRandomAccessStream inStream = new InMemoryRandomAccessStream();
        //        DataWriter datawriter = new DataWriter(inStream.GetOutputStreamAt(0));
        //        datawriter.WriteBuffer(buffer, 0, buffer.Length);
        //        await datawriter.StoreAsync();
        //        return inStream;
        //    }

        //}

        //public IBuffer GetBuffer()
        //{
        //    if (Data == null)
        //        return null;
        //    else
        //        return WindowsRuntimeBufferExtensions.AsBuffer(Data);
        //}

        public byte[] GetBytes()
        {
            if (Data == null)
                return null;
            else
                return Data;
        }

        public string GetString()
        {
            if (Data == null)
                return null;
            else
                return Encoding.GetString(Data);
        }

        public void Dispose()
        {
            Data = null;
            Headers = null;
            Cookies = null;
            Encoding = null;
            Exception = null;
        }
    }
}
