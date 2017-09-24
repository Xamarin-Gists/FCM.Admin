namespace FCM.Admin
{
    public class FcmResponse
    {
        public Error[] results { get; set; }
    }

    public class Error
    {
        public string error { get; set; }
    }

    public class ImportTokenResponse
    {
        public ImportSingleTokenResponse[] results { get; set; }
    }

    public class ImportSingleTokenResponse
    {
        public string apns_token { get; set; }
        public string status { get; set; }
        public string registration_token { get; set; }
    }

}
