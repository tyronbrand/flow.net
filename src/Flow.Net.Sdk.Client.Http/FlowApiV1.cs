using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Net.Sdk.Client.Http
{
    public partial class FlowApiV1
    {
        private string _baseUrl = "";
        private HttpClient _httpClient;
        private Lazy<JsonSerializerSettings> _settings;

        public FlowApiV1(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _settings = new Lazy<JsonSerializerSettings>(() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new ScriptValueConverter());
                UpdateJsonSerializerSettings(settings);
                return settings;
            });
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }

        protected JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }

        partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings);
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url);
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder);
        partial void ProcessResponse(HttpClient client, HttpResponseMessage response);

        /// <summary>Gets Blocks by Height</summary>
        /// <param name="height">A comma-separated list of block heights to get. This parameter is incompatible with `start_height` and `end_height`.</param>
        /// <param name="start_height">The start height of the block range to get. Must be used together with `end_height`. This parameter is incompatible with `height`.</param>
        /// <param name="end_height">The ending height of the block range to get. Must be used together with `start_height`. This parameter is incompatible with `height`.</param>
        /// <param name="expand">A comma-separated list indicating which properties of the content to expand.</param>
        /// <param name="select">A comma-separated list indicating which properties of the content to return.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<ICollection<Block>> BlocksAllAsync(IEnumerable<string> height, string start_height, string end_height, IEnumerable<string> expand, IEnumerable<string> select)
        {
            return BlocksAllAsync(height, start_height, end_height, expand, select, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Gets Blocks by Height</summary>
        /// <param name="height">A comma-separated list of block heights to get. This parameter is incompatible with `start_height` and `end_height`.</param>
        /// <param name="start_height">The start height of the block range to get. Must be used together with `end_height`. This parameter is incompatible with `height`.</param>
        /// <param name="end_height">The ending height of the block range to get. Must be used together with `start_height`. This parameter is incompatible with `height`.</param>
        /// <param name="expand">A comma-separated list indicating which properties of the content to expand.</param>
        /// <param name="select">A comma-separated list indicating which properties of the content to return.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<ICollection<Block>> BlocksAllAsync(IEnumerable<string> height, string start_height, string end_height, IEnumerable<string> expand, IEnumerable<string> select, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/blocks?");
            if (height != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("height") + "=");
                foreach (var item_ in height)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            if (start_height != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("start_height") + "=").Append(Uri.EscapeDataString(ConvertToString(start_height, CultureInfo.InvariantCulture))).Append("&");
            }
            if (end_height != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("end_height") + "=").Append(Uri.EscapeDataString(ConvertToString(end_height, CultureInfo.InvariantCulture))).Append("&");
            }
            if (expand != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("expand") + "=");
                foreach (var item_ in expand)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            if (select != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("select") + "=");
                foreach (var item_ in select)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ICollection<Block>>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "404")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Not Found", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>Get Blocks by ID.</summary>
        /// <param name="id">A block ID or comma-separated list of block IDs.</param>
        /// <param name="expand">A comma-separated list indicating which properties of the content to expand.</param>
        /// <param name="select">A comma-separated list indicating which properties of the content to return.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<ICollection<Block>> BlocksAsync(IEnumerable<string> id, IEnumerable<string> expand, IEnumerable<string> select)
        {
            return BlocksAsync(id, expand, select, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get Blocks by ID.</summary>
        /// <param name="id">A block ID or comma-separated list of block IDs.</param>
        /// <param name="expand">A comma-separated list indicating which properties of the content to expand.</param>
        /// <param name="select">A comma-separated list indicating which properties of the content to return.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<ICollection<Block>> BlocksAsync(IEnumerable<string> id, IEnumerable<string> expand, IEnumerable<string> select, System.Threading.CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/blocks/{id}?");
            urlBuilder_.Replace("{id}", Uri.EscapeDataString(string.Join(",", id.Select(s_ => ConvertToString(s_, CultureInfo.InvariantCulture)))));
            if (expand != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("expand") + "=");
                foreach (var item_ in expand)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            if (select != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("select") + "=");
                foreach (var item_ in select)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ICollection<Block>>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "404")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Not Found", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>Get a Transaction by ID.</summary>
        /// <param name="id">The ID of the transaction to get.</param>
        /// <param name="expand">A comma-separated list indicating which properties of the content to expand.</param>
        /// <param name="select">A comma-separated list indicating which properties of the content to return.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<Transaction> TransactionsAsync(string id, IEnumerable<string> expand, IEnumerable<string> select)
        {
            return TransactionsAsync(id, expand, select, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get a Transaction by ID.</summary>
        /// <param name="id">The ID of the transaction to get.</param>
        /// <param name="expand">A comma-separated list indicating which properties of the content to expand.</param>
        /// <param name="select">A comma-separated list indicating which properties of the content to return.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<Transaction> TransactionsAsync(string id, IEnumerable<string> expand, IEnumerable<string> select, System.Threading.CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/transactions/{id}?");
            urlBuilder_.Replace("{id}", Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
            if (expand != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("expand") + "=");
                foreach (var item_ in expand)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            if (select != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("select") + "=");
                foreach (var item_ in select)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Transaction>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "404")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Not Found", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>Submit a Transaction</summary>
        /// <param name="body">The transaction to submit.</param>
        /// <returns>Created</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<Transaction> SendTransactionAsync(TransactionBody body)
        {
            return SendTransactionAsync(body, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Submit a Transaction</summary>
        /// <param name="body">The transaction to submit.</param>
        /// <returns>Created</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<Transaction> SendTransactionAsync(TransactionBody body, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/transactions");

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    var content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
                    content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    request_.Content = content_;
                    request_.Method = new HttpMethod("POST");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200" || status_ == "201")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Transaction>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>Gets a Collection by ID</summary>
        /// <param name="id">The collection ID.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<Collection> CollectionsAsync(string id, IEnumerable<string> expand)
        {
            return CollectionsAsync(id, expand, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Gets a Collection by ID</summary>
        /// <param name="id">The collection ID.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<Collection> CollectionsAsync(string id, IEnumerable<string> expand, System.Threading.CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/collections/{id}?");
            urlBuilder_.Replace("{id}", Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));

            if (expand != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("expand") + "=");
                foreach (var item_ in expand)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Collection>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "404")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Not Found", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>Get an Account By Address</summary>
        /// <param name="address">The address of the account.</param>
        /// <param name="block_height">The block height to query for the account details at the "sealed" is used by default.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<Account> AccountsAsync(string address, string block_height, IEnumerable<string> expand)
        {
            return AccountsAsync(address, block_height, expand, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get an Account By Address</summary>
        /// <param name="address">The address of the account.</param>
        /// <param name="block_height">The block height to query for the account details at the "sealed" is used by default.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<Account> AccountsAsync(string address, string block_height, IEnumerable<string> expand, System.Threading.CancellationToken cancellationToken)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/accounts/{address}?");
            urlBuilder_.Replace("{address}", Uri.EscapeDataString(ConvertToString(address, CultureInfo.InvariantCulture)));
            if (block_height != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("block_height") + "=").Append(Uri.EscapeDataString(ConvertToString(block_height, CultureInfo.InvariantCulture))).Append("&");
            }
            if (expand != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("expand") + "=");
                foreach (var item_ in expand)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Account>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "404")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Not Found", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>Execute a Cadence Script</summary>
        /// <param name="block_id">The ID of the block to execute the script against. For a specific block height, use `block_height` instead.</param>
        /// <param name="block_height">The height of the block to execute the script against. This parameter is incompatible with `block_id`.</param>
        /// <param name="body">The script to execute.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<Response> ScriptsAsync(string block_id, string block_height, ScriptBody body)
        {
            return ScriptsAsync(block_id, block_height, body, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Execute a Cadence Script</summary>
        /// <param name="block_id">The ID of the block to execute the script against. For a specific block height, use `block_height` instead.</param>
        /// <param name="block_height">The height of the block to execute the script against. This parameter is incompatible with `block_id`.</param>
        /// <param name="body">The script to execute.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<Response> ScriptsAsync(string block_id, string block_height, ScriptBody body, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/scripts?");
            if (block_id != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("block_id") + "=").Append(Uri.EscapeDataString(ConvertToString(block_id, CultureInfo.InvariantCulture))).Append("&");
            }
            if (block_height != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("block_height") + "=").Append(Uri.EscapeDataString(ConvertToString(block_height, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    var content_ = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
                    content_.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    request_.Content = content_;
                    request_.Method = new HttpMethod("POST");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Response>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>Get Events</summary>
        /// <param name="type">The event type is [identifier of the event as defined here](https://docs.onflow.org/core-contracts/flow-token/#events).</param>
        /// <param name="start_height">The start height of the block range for events. Must be used together with `end_height`. This parameter is incompatible with `block_ids`.</param>
        /// <param name="end_height">The end height of the block range for events. Must be used together with `start_height`. This parameter is incompatible with `block_ids`.</param>
        /// <param name="block_ids">List of block IDs. Either provide this parameter or both height parameters. This parameter is incompatible with heights parameters.</param>
        /// <param name="select">A comma-separated list indicating which properties of the content to return.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<ICollection<BlockEvents>> EventsAsync(string type, string start_height, string end_height, IEnumerable<string> block_ids, IEnumerable<string> select)
        {
            return EventsAsync(type, start_height, end_height, block_ids, select, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get Events</summary>
        /// <param name="type">The event type is [identifier of the event as defined here](https://docs.onflow.org/core-contracts/flow-token/#events).</param>
        /// <param name="start_height">The start height of the block range for events. Must be used together with `end_height`. This parameter is incompatible with `block_ids`.</param>
        /// <param name="end_height">The end height of the block range for events. Must be used together with `start_height`. This parameter is incompatible with `block_ids`.</param>
        /// <param name="block_ids">List of block IDs. Either provide this parameter or both height parameters. This parameter is incompatible with heights parameters.</param>
        /// <param name="select">A comma-separated list indicating which properties of the content to return.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<ICollection<BlockEvents>> EventsAsync(string type, string start_height, string end_height, IEnumerable<string> block_ids, IEnumerable<string> select, System.Threading.CancellationToken cancellationToken)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/events?");
            urlBuilder_.Append(Uri.EscapeDataString("type") + "=").Append(Uri.EscapeDataString(ConvertToString(type, CultureInfo.InvariantCulture))).Append("&");
            if (start_height != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("start_height") + "=").Append(Uri.EscapeDataString(ConvertToString(start_height, CultureInfo.InvariantCulture))).Append("&");
            }
            if (end_height != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("end_height") + "=").Append(Uri.EscapeDataString(ConvertToString(end_height, CultureInfo.InvariantCulture))).Append("&");
            }
            if (block_ids != null)
            {
                foreach (var item_ in block_ids) { urlBuilder_.Append(Uri.EscapeDataString("block_ids") + "=").Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append("&"); }
            }
            if (select != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("select") + "=");
                foreach (var item_ in select)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ICollection<BlockEvents>>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "404")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Not Found", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        protected struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                Object = responseObject;
                Text = responseText;
            }

            public T Object { get; }

            public string Text { get; }
        }

        public bool ReadResponseAsString { get; set; }

        protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default, string.Empty);
            }

            if (ReadResponseAsString)
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    var typedBody = JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                    return new ObjectResponseResult<T>(typedBody, responseText);
                }
                catch (JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var streamReader = new System.IO.StreamReader(responseStream))
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var serializer = JsonSerializer.Create(JsonSerializerSettings);
                        var typedBody = serializer.Deserialize<T>(jsonTextReader);
                        return new ObjectResponseResult<T>(typedBody, string.Empty);
                    }
                }
                catch (JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }

        /// <summary>Get a Transaction Result by ID.</summary>
        /// <param name="transaction_id">The transaction ID of the transaction result.</param>
        /// <param name="expand">A comma-separated list indicating which properties of the content to expand.</param>
        /// <param name="select">A comma-separated list indicating which properties of the content to return.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<TransactionResult> ResultsAsync(string transaction_id, IEnumerable<string> expand, IEnumerable<string> select)
        {
            return ResultsAsync(transaction_id, expand, select, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get a Transaction Result by ID.</summary>
        /// <param name="transaction_id">The transaction ID of the transaction result.</param>
        /// <param name="expand">A comma-separated list indicating which properties of the content to expand.</param>
        /// <param name="select">A comma-separated list indicating which properties of the content to return.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<TransactionResult> ResultsAsync(string transaction_id, IEnumerable<string> expand, IEnumerable<string> select, System.Threading.CancellationToken cancellationToken)
        {
            if (transaction_id == null)
                throw new ArgumentNullException("transaction_id");

            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/transaction_results/{transaction_id}?");
            urlBuilder_.Replace("{transaction_id}", Uri.EscapeDataString(ConvertToString(transaction_id, CultureInfo.InvariantCulture)));
            if (expand != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("expand") + "=");
                foreach (var item_ in expand)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            if (select != null)
            {
                urlBuilder_.Append(Uri.EscapeDataString("select") + "=");
                foreach (var item_ in select)
                {
                    urlBuilder_.Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append(",");
                }
                urlBuilder_.Length--;
                urlBuilder_.Append("&");
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<TransactionResult>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "404")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Not Found", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }
        private string ConvertToString(object value, CultureInfo cultureInfo)
        {
            if (value is Enum)
            {
                string name = Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(EnumMemberAttribute))
                            as EnumMemberAttribute;
                        if (attribute != null)
                        {
                            return attribute.Value ?? name;
                        }
                    }
                }
            }
            else if (value is bool)
            {
                return Convert.ToString(value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[])
            {
                return Convert.ToBase64String((byte[])value);
            }
            else if (value != null && value.GetType().IsArray)
            {
                var array = ((Array)value).OfType<object>();
                return string.Join(",", array.Select(o => ConvertToString(o, cultureInfo)));
            }

            return Convert.ToString(value, cultureInfo);
        }


        /// <summary>Get Execution Results by Block ID</summary>
        /// <param name="block_id">Single ID or comma-separated list of block IDs.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<ICollection<ExecutionResult>> ResultsAllAsync(IEnumerable<string> block_id)
        {
            return ResultsAllAsync(block_id, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get Execution Results by Block ID</summary>
        /// <param name="block_id">Single ID or comma-separated list of block IDs.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<ICollection<ExecutionResult>> ResultsAllAsync(IEnumerable<string> block_id, System.Threading.CancellationToken cancellationToken)
        {
            if (block_id == null)
                throw new ArgumentNullException("block_id");

            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/execution_results?");
            foreach (var item_ in block_id) { urlBuilder_.Append(Uri.EscapeDataString("block_id") + "=").Append(Uri.EscapeDataString(ConvertToString(item_, CultureInfo.InvariantCulture))).Append("&"); }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ICollection<ExecutionResult>>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "404")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Not Found", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>Get Execution Result by ID</summary>
        /// <param name="id">The ID of the execution result.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<ExecutionResult> ResultsAsync(string id)
        {
            return ResultsAsync(id, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get Execution Result by ID</summary>
        /// <param name="id">The ID of the execution result.</param>
        /// <returns>OK</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<ExecutionResult> ResultsAsync(string id, System.Threading.CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/execution_results/{id}");
            urlBuilder_.Replace("{id}", Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<ExecutionResult>(response_, headers_).ConfigureAwait(false);
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == "400")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Bad Request", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "404")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Not Found", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == "500")
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Error>(response_, headers_).ConfigureAwait(false);
                            throw new ApiException<Error>("Internal Server Error", (int)response_.StatusCode, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }
    }

    public partial class Account
    {
        [JsonProperty("address", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Address { get; set; }

        /// <summary>Flow balance of the account.</summary>
        [JsonProperty("balance", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Balance { get; set; }

        [JsonProperty("keys", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        [MinLength(1)]
        public ICollection<AccountPublicKey> Keys { get; set; }

        [JsonProperty("contracts", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, byte[]> Contracts { get; set; }

        [JsonProperty("_expandable", Required = Required.Always)]
        [Required]
        public _expandable _expandable { get; set; } = new _expandable();

        [JsonProperty("_links", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Links _links { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }
    }

    public partial class AccountPublicKey
    {
        /// <summary>Index of the public key.</summary>
        [JsonProperty("index", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Index { get; set; }

        /// <summary>Hex encoded public key.</summary>
        [JsonProperty("public_key", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Public_key { get; set; }

        [JsonProperty("signing_algorithm", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public SigningAlgorithm Signing_algorithm { get; set; }

        [JsonProperty("hashing_algorithm", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public HashingAlgorithm Hashing_algorithm { get; set; }

        /// <summary>Current account sequence number.</summary>
        [JsonProperty("sequence_number", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Sequence_number { get; set; }

        /// <summary>Weight of the key.</summary>
        [JsonProperty("weight", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Weight { get; set; }

        /// <summary>Flag indicating whether the key is active or not.</summary>
        [JsonProperty("revoked", Required = Required.Always)]
        public bool Revoked { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public enum SigningAlgorithm
    {
        [EnumMember(Value = @"BLSBLS12381")]
        BLSBLS12381 = 0,

        [EnumMember(Value = @"ECDSA_P256")]
        ECDSA_P256 = 1,

        [EnumMember(Value = @"ECDSA_secp256k1")]
        ECDSA_secp256k1 = 2,

    }

    public enum HashingAlgorithm
    {
        [EnumMember(Value = @"SHA2_256")]
        SHA2_256 = 0,

        [EnumMember(Value = @"SHA2_384")]
        SHA2_384 = 1,

        [EnumMember(Value = @"SHA3_256")]
        SHA3_256 = 2,

        [EnumMember(Value = @"SHA3_384")]
        SHA3_384 = 3,

        [EnumMember(Value = @"KMAC128")]
        KMAC128 = 4,

    }

    public partial class Collection
    {
        [JsonProperty("id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Id { get; set; }

        [JsonProperty("transactions", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Transaction> Transactions { get; set; }

        [JsonProperty("_expandable", Required = Required.Always)]
        [Required]
        public _expandable2 _expandable { get; set; } = new _expandable2();

        [JsonProperty("_links", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Links _links { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }
    }

    public partial class Transaction
    {
        [JsonProperty("id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Id { get; set; }

        /// <summary>Base64 encoded Cadence script.</summary>
        [JsonProperty("script", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public byte[] Script { get; set; }

        /// <summary>Array of Base64 encoded arguments with in [JSON-Cadence interchange format](https://docs.onflow.org/cadence/json-cadence-spec/).</summary>
        [JsonProperty("arguments", Required = Required.Always)]
        [Required]
        public ICollection<byte[]> Arguments { get; set; } = new System.Collections.ObjectModel.Collection<byte[]>();

        [JsonProperty("reference_block_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Reference_block_id { get; set; }

        /// <summary>The limit on the amount of computation a transaction is allowed to preform.</summary>
        [JsonProperty("gas_limit", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Gas_limit { get; set; }

        [JsonProperty("payer", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Payer { get; set; }

        [JsonProperty("proposal_key", Required = Required.Always)]
        [Required]
        public ProposalKey Proposal_key { get; set; } = new ProposalKey();

        [JsonProperty("authorizers", Required = Required.Always)]
        [Required]
        public ICollection<string> Authorizers { get; set; } = new System.Collections.ObjectModel.Collection<string>();

        [JsonProperty("payload_signatures", Required = Required.Always)]
        [Required]
        public ICollection<TransactionSignature> Payload_signatures { get; set; } = new System.Collections.ObjectModel.Collection<TransactionSignature>();

        [JsonProperty("envelope_signatures", Required = Required.Always)]
        [Required]
        public ICollection<TransactionSignature> Envelope_signatures { get; set; } = new System.Collections.ObjectModel.Collection<TransactionSignature>();

        [JsonProperty("result", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public TransactionResult Result { get; set; }

        [JsonProperty("_expandable", Required = Required.Always)]
        [Required]
        public _expandable3 _expandable { get; set; } = new _expandable3();

        [JsonProperty("_links", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Links _links { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class ProposalKey
    {
        [JsonProperty("address", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Address { get; set; }

        [JsonProperty("key_index", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Key_index { get; set; }

        [JsonProperty("sequence_number", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Sequence_number { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    /// <summary>Base64 encoded signature.</summary>
    public partial class TransactionSignature
    {
        [JsonProperty("address", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Address { get; set; }

        [JsonProperty("key_index", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Key_index { get; set; }

        [JsonProperty("signature", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public byte[] Signature { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class TransactionResult
    {
        [JsonProperty("block_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Block_id { get; set; }

        [JsonProperty("execution", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public TransactionExecution Execution { get; set; }

        [JsonProperty("status", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public TransactionStatus Status { get; set; }

        [JsonProperty("status_code", Required = Required.Always)]
        public int Status_code { get; set; }

        /// <summary>Provided transaction error in case the transaction wasn't successful.</summary>
        [JsonProperty("error_message", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Error_message { get; set; }

        [JsonProperty("computation_used", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Computation_used { get; set; }

        [JsonProperty("events", Required = Required.Always)]
        [Required]
        public ICollection<Event> Events { get; set; } = new System.Collections.ObjectModel.Collection<Event>();

        [JsonProperty("_links", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Links _links { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    /// <summary>This value indicates whether the transaction execution succeded or not, this value should be checked when determining transaction success.</summary>
    public enum TransactionExecution
    {
        [EnumMember(Value = @"Pending")]
        Pending = 0,

        [EnumMember(Value = @"Success")]
        Success = 1,

        [EnumMember(Value = @"Failure")]
        Failure = 2,

    }

    /// <summary>This value indicates the state of the transaction execution. Only sealed and expired are final and immutable states.</summary>
    public enum TransactionStatus
    {
        [EnumMember(Value = @"Pending")]
        Pending = 0,

        [EnumMember(Value = @"Finalized")]
        Finalized = 1,

        [EnumMember(Value = @"Executed")]
        Executed = 2,

        [EnumMember(Value = @"Sealed")]
        Sealed = 3,

        [EnumMember(Value = @"Expired")]
        Expired = 4,

    }

    public partial class Block
    {
        [JsonProperty("header", Required = Required.Always)]
        [Required]
        public BlockHeader Header { get; set; } = new BlockHeader();

        [JsonProperty("payload", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public BlockPayload Payload { get; set; }

        [JsonProperty("execution_result", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ExecutionResult Execution_result { get; set; }

        [JsonProperty("_expandable", Required = Required.Always)]
        [Required]
        public _expandable4 _expandable { get; set; } = new _expandable4();

        [JsonProperty("_links", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Links _links { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class BlockHeader
    {
        [JsonProperty("id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Id { get; set; }

        [JsonProperty("parent_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Parent_id { get; set; }

        [JsonProperty("height", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Height { get; set; }

        [JsonProperty("timestamp", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("parent_voter_signature", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public byte[] Parent_voter_signature { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class BlockPayload
    {
        [JsonProperty("collection_guarantees", Required = Required.Always)]
        [Required]
        public ICollection<CollectionGuarantee> Collection_guarantees { get; set; } = new System.Collections.ObjectModel.Collection<CollectionGuarantee>();

        [JsonProperty("block_seals", Required = Required.Always)]
        [Required]
        public ICollection<BlockSeal> Block_seals { get; set; } = new System.Collections.ObjectModel.Collection<BlockSeal>();

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class CollectionGuarantee
    {
        [JsonProperty("collection_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Collection_id { get; set; }

        [JsonProperty("signer_ids")]
        [Required]
        [MinLength(1)]
        public ICollection<string> Signer_ids { get; set; } = new System.Collections.ObjectModel.Collection<string>();

        [JsonProperty("signature")]
        [Required(AllowEmptyStrings = true)]
        public byte[] Signature { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class BlockSeal
    {
        [JsonProperty("block_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Block_id { get; set; }

        [JsonProperty("result_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Result_id { get; set; }

        [JsonProperty("final_state", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Final_state { get; set; }

        [JsonProperty("aggregated_approval_signatures", Required = Required.Always)]
        [Required]
        [MinLength(1)]
        public ICollection<AggregatedSignature> Aggregated_approval_signatures { get; set; } = new System.Collections.ObjectModel.Collection<AggregatedSignature>();

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class AggregatedSignature
    {
        [JsonProperty("verifier_signatures", Required = Required.Always)]
        [Required]
        [MinLength(1)]
        public ICollection<byte[]> Verifier_signatures { get; set; } = new System.Collections.ObjectModel.Collection<byte[]>();

        [JsonProperty("signer_ids", Required = Required.Always)]
        [Required]
        [MinLength(1)]
        public ICollection<string> Signer_ids { get; set; } = new System.Collections.ObjectModel.Collection<string>();

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class ExecutionResult
    {
        [JsonProperty("id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Id { get; set; }

        [JsonProperty("block_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Block_id { get; set; }

        [JsonProperty("events", Required = Required.Always)]
        [Required]
        public ICollection<Event> Events { get; set; } = new System.Collections.ObjectModel.Collection<Event>();

        [JsonProperty("chunks", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Chunk> Chunks { get; set; }

        [JsonProperty("previous_result_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Previous_result_id { get; set; }

        [JsonProperty("_links", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Links _links { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class BlockEvents
    {
        [JsonProperty("block_id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Block_id { get; set; }

        [JsonProperty("block_height", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Block_height { get; set; }

        [JsonProperty("block_timestamp", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset Block_timestamp { get; set; }

        [JsonProperty("events", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Event> Events { get; set; }

        [JsonProperty("_links", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Links _links { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class Event
    {
        [JsonProperty("type", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Type { get; set; }

        [JsonProperty("transaction_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Transaction_id { get; set; }

        [JsonProperty("transaction_index", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Transaction_index { get; set; }

        [JsonProperty("event_index", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Event_index { get; set; }

        [JsonProperty("payload", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public byte[] Payload { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class BlockHeight
    {
        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class Error
    {
        [JsonProperty("code", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }

        [JsonProperty("message", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class Chunk
    {
        [JsonProperty("block_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Block_id { get; set; }

        [JsonProperty("collection_index", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Collection_index { get; set; }

        [JsonProperty("start_state", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public byte[] Start_state { get; set; }

        [JsonProperty("end_state", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public byte[] End_state { get; set; }

        [JsonProperty("event_collection", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public byte[] Event_collection { get; set; }

        [JsonProperty("index", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Index { get; set; }

        [JsonProperty("number_of_transactions", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Number_of_transactions { get; set; }

        [JsonProperty("total_computation_used", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Total_computation_used { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class Links
    {
        [JsonProperty("_self", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string _self { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.0.22.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class TransactionBody
    {
        /// <summary>Base64 encoded content of the Cadence script.</summary>
        [JsonProperty("script", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public byte[] Script { get; set; }

        /// <summary>A list of arguments each encoded as Base64 passed in the [JSON-Cadence interchange format](https://docs.onflow.org/cadence/json-cadence-spec/).</summary>
        [JsonProperty("arguments", Required = Required.Always)]
        [Required]
        public ICollection<byte[]> Arguments { get; set; } = new System.Collections.ObjectModel.Collection<byte[]>();

        [JsonProperty("reference_block_id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Reference_block_id { get; set; }

        /// <summary>The limit on the amount of computation a transaction is allowed to preform.</summary>
        [JsonProperty("gas_limit", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Gas_limit { get; set; }

        [JsonProperty("payer", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Payer { get; set; }

        [JsonProperty("proposal_key", Required = Required.Always)]
        [Required]
        public ProposalKey Proposal_key { get; set; } = new ProposalKey();

        [JsonProperty("authorizers", Required = Required.Always)]
        [Required]
        public ICollection<string> Authorizers { get; set; } = new System.Collections.ObjectModel.Collection<string>();

        /// <summary>A list of Base64 encoded signatures.</summary>
        [JsonProperty("payload_signatures", Required = Required.Always)]
        [Required]
        public ICollection<TransactionSignature> Payload_signatures { get; set; } = new System.Collections.ObjectModel.Collection<TransactionSignature>();

        /// <summary>A list of Base64 encoded signatures.</summary>
        [JsonProperty("envelope_signatures", Required = Required.Always)]
        [Required]
        public ICollection<TransactionSignature> Envelope_signatures { get; set; } = new System.Collections.ObjectModel.Collection<TransactionSignature>();

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class ScriptBody
    {
        /// <summary>Base64 encoded content of the Cadence script.</summary>
        [JsonProperty("script", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public byte[] Script { get; set; }

        /// <summary>An list of arguments each encoded as Base64 passed in the [JSON-Cadence interchange format](https://docs.onflow.org/cadence/json-cadence-spec/).</summary>
        [JsonProperty("arguments", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<byte[]> Arguments { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class Response
    {
        [JsonProperty("value", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class _expandable
    {
        [JsonProperty("keys", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Keys { get; set; }

        [JsonProperty("contracts", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Contracts { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class _expandable2
    {
        [JsonProperty("transactions", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Uri> Transactions { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class _expandable3
    {
        [JsonProperty("result", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Uri Result { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class _expandable4
    {
        [JsonProperty("payload", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Payload { get; set; }

        [JsonProperty("execution_result", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Uri Execution_result { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    public partial class ApiException : Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + response.Substring(0, response.Length >= 512 ? 512 : response.Length), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    public partial class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }

    public class ScriptValueConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Response);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                var bytes = Convert.FromBase64String(reader.Value.ToString());
                var value = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                return new Response
                {
                    Value = value
                };
            }

            return null;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
