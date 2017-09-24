using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace FCM.Admin
{
    public class Configuration
    {
        public static string FCM_SERVER_KEY { get; set; }
        public static string BASE_URL = "https://iid.googleapis.com/iid/v1:";
        public static string BATCH_ADD = BASE_URL + "batchAdd";
        public static string BATCH_REMOVE = BASE_URL + "batchRemove";
        public static string BATCH_IMPORT = BASE_URL + "batchImport";
    }

    public class Admin
    {
        string[] RegistrationToken;
        string Topic = string.Empty;
        public SingleTokenListener sListener;
        public MultiTokenListener mListener;
        public ImportIosTokenListener iListener;

        public Admin(string FcmServerKey)
        {
            Configuration.FCM_SERVER_KEY = FcmServerKey;
        }

        #region Listener
        public void SingleTokenListener(SingleTokenListener listener)
        {
            sListener = listener;
        }

        public void MultiTokenListener(MultiTokenListener listener)
        {
            mListener = listener;
        }

        public void ImportIosTokenListener(ImportIosTokenListener listener)
        {
            iListener = listener;
        }
        #endregion

        #region Subscribe & Unsubscribe Hanler

        /// <summary>
        /// Subscribe a Single Token
        /// </summary>
        /// <param name="RegistrationToken"></param>
        /// <param name="Topic"></param>
        public async void SubscribeToTopic(string RegistrationToken, string Topic)
        {
            if (string.IsNullOrEmpty(Configuration.FCM_SERVER_KEY))
                sListener.OnError(new Exception("Server Key should not be empty"));
            if (string.IsNullOrEmpty(RegistrationToken))
                sListener.OnError(new Exception("Registration Token should not be empty"));
            if (string.IsNullOrEmpty(Topic))
                sListener.OnError(new Exception("Topic should not be empty"));

            this.RegistrationToken = new string[] { RegistrationToken };
            this.Topic = Topic;

            string Subscription = await TopicClient(Configuration.BATCH_ADD);
            FcmResponse fcmResponse = DeSerializeContent(typeof(FcmResponse), Subscription) as FcmResponse;

            if (fcmResponse.results.Length > 0 && !string.IsNullOrEmpty(fcmResponse.results[0].error))
            {
                if (sListener != null)
                    sListener.OnSuccess(RegistrationToken, fcmResponse.results[0].error);
            }
            else
            {
                if (sListener != null)
                    sListener.OnSuccess();
            }
        }

        /// <summary>
        /// Subcribe Multiple Tokens
        /// </summary>
        /// <param name="RegistrationToken"></param>
        /// <param name="Topic"></param>
        public async void SubscribeToTopic(string[] RegistrationToken, string Topic)
        {
            if (string.IsNullOrEmpty(Configuration.FCM_SERVER_KEY))
                mListener.OnError(new Exception("Server Key should not be empty"));
            if (RegistrationToken.Length > 0)
                mListener.OnError(new Exception("Should use this for multiple registration tokens"));
            if (string.IsNullOrEmpty(Topic))
                mListener.OnError(new Exception("Topic should not be empty"));

            this.RegistrationToken = RegistrationToken;
            this.Topic = Topic;

            string Subscription = await TopicClient(Configuration.BATCH_ADD);
            FcmResponse fcmResponse = DeSerializeContent(typeof(FcmResponse), Subscription) as FcmResponse;

            if (fcmResponse.results.Length > 0 && !string.IsNullOrEmpty(fcmResponse.results[0].error))
            {
                if (mListener != null)
                {
                    string[] errors = fcmResponse.results.Where(x => x.error != null).Select(x => x.error).ToArray();
                    List<string> RegTokens = new List<string>();
                    for (int i = 0; i < RegistrationToken.Length; i++)
                    {
                        if (fcmResponse.results[i].error != null)
                            RegTokens.Add(RegistrationToken[i]);
                    }
                    string[] RegistrationTokens = RegTokens.ToArray();
                    mListener.OnSuccess(RegistrationTokens, errors);
                }
            }
            else
            {
                if (mListener != null)
                    mListener.OnSuccess();
            }
        }

        /// <summary>
        /// Unsubscribe Single Token
        /// </summary>
        /// <param name="RegistrationToken"></param>
        /// <param name="Topic"></param>
        public async void UnSubscribeFromTopic(string RegistrationToken, string Topic)
        {
            if (string.IsNullOrEmpty(Configuration.FCM_SERVER_KEY))
                sListener.OnError(new Exception("Server Key should not be empty"));
            if (string.IsNullOrEmpty(RegistrationToken))
                sListener.OnError(new Exception("Registration Token should not be empty"));
            if (string.IsNullOrEmpty(Topic))
                sListener.OnError(new Exception("Topic should not be empty"));

            this.RegistrationToken = new string[] { RegistrationToken };
            this.Topic = Topic;

            string Subscription = await TopicClient(Configuration.BATCH_REMOVE);
            FcmResponse fcmResponse = DeSerializeContent(typeof(FcmResponse), Subscription) as FcmResponse;

            if (fcmResponse.results.Length > 0 && !string.IsNullOrEmpty(fcmResponse.results[0].error))
            {
                if (sListener != null)
                    sListener.OnSuccess(RegistrationToken, fcmResponse.results[0].error);
            }
            else
            {
                if (sListener != null)
                    sListener.OnSuccess();
            }
        }

        /// <summary>
        /// Unsubcribe Multiple Tokens
        /// </summary>
        /// <param name="RegistrationToken"></param>
        /// <param name="Topic"></param>
        public async void UnSubscribeFromTopic(string[] RegistrationToken, string Topic)
        {
            if (string.IsNullOrEmpty(Configuration.FCM_SERVER_KEY))
                mListener.OnError(new Exception("Server Key should not be empty"));
            if (RegistrationToken.Length > 0)
                mListener.OnError(new Exception("Should use this for multiple registration tokens"));
            if (string.IsNullOrEmpty(Topic))
                mListener.OnError(new Exception("Topic should not be empty"));

            this.RegistrationToken = RegistrationToken;
            this.Topic = Topic;

            string Subscription = await TopicClient(Configuration.BATCH_REMOVE);
            FcmResponse fcmResponse = DeSerializeContent(typeof(FcmResponse), Subscription) as FcmResponse;

            if (fcmResponse.results.Length > 0 && !string.IsNullOrEmpty(fcmResponse.results[0].error))
            {
                if (mListener != null)
                {
                    string[] errors = fcmResponse.results.Where(x => x.error != null).Select(x => x.error).ToArray();
                    List<string> RegTokens = new List<string>();
                    for (int i = 0; i < RegistrationToken.Length; i++)
                    {
                        if (fcmResponse.results[i].error != null)
                            RegTokens.Add(RegistrationToken[i]);
                    }
                    string[] RegistrationTokens = RegTokens.ToArray();
                    mListener.OnSuccess(RegistrationTokens, errors);
                }
            }
            else
            {
                if (mListener != null)
                    mListener.OnSuccess();
            }
        }

        /// <summary>
        /// Http Service for Subscription & Unsubscription
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> TopicClient(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = "key=" + Configuration.FCM_SERVER_KEY;

                var RequestData = new
                {
                    to = "/topics/" + Topic,
                    registration_tokens = RegistrationToken
                };

                string SerializedRequest = SerializeContent(RequestData);

                var stream = await request.GetRequestStreamAsync();
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(SerializedRequest);
                    writer.Flush();
                    writer.Dispose();
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            var content = reader.ReadToEnd();
                            if (string.IsNullOrWhiteSpace(content))
                            {
                                return response.StatusCode + "Response contained an empty body...";
                            }
                            else
                            {
                                return content;
                            }
                        }
                    }
                    else
                    {
                        return response.StatusCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        #region Import iOS Tokens
        public async void ImportToken(string application, bool sandbox, string[] apns_tokens)
        {
            if (string.IsNullOrEmpty(Configuration.FCM_SERVER_KEY))
                iListener.OnError(new Exception("Server Key should not be empty"));

            string Subscription = await ImportTokenClient(Configuration.BATCH_IMPORT, application, sandbox, apns_tokens);
            ImportTokenResponse importTokenResponse = DeSerializeContent(typeof(ImportTokenResponse), Subscription) as ImportTokenResponse;
            if (iListener != null)
                iListener.OnSuccess(importTokenResponse);
        }

        private async Task<string> ImportTokenClient(string url, string application, bool sandbox, string[] apns_tokens)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = "key=" + Configuration.FCM_SERVER_KEY;

                var RequestData = new
                {
                    application = application,
                    sandbox = sandbox,
                    apns_tokens = apns_tokens
                };

                string SerializedRequest = SerializeContent(RequestData);

                var stream = await request.GetRequestStreamAsync();
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(SerializedRequest);
                    writer.Flush();
                    writer.Dispose();
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            var content = reader.ReadToEnd();
                            if (string.IsNullOrWhiteSpace(content))
                            {
                                return response.StatusCode + "Response contained an empty body...";
                            }
                            else
                            {
                                return content;
                            }
                        }
                    }
                    else
                    {
                        return response.StatusCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        } 
        #endregion

        #region Serializer & Deserializer
        private string SerializeContent(object ObjectToSerialize)
        {
            return JsonConvert.SerializeObject(ObjectToSerialize);
        }

        private object DeSerializeContent(Type Response, string ObjectToDeSerialize)
        {
            if (Response == typeof(FcmResponse))
                return JsonConvert.DeserializeObject<FcmResponse>(ObjectToDeSerialize);
            else
                return JsonConvert.DeserializeObject<ImportTokenResponse>(ObjectToDeSerialize);
        }
        #endregion
        
    }
}
