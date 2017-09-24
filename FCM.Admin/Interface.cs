using System;

namespace FCM.Admin
{

    public interface SingleTokenListener
    {
        void OnSuccess();
        void OnSuccess(string RegToken, string error);
        void OnError(Exception e);
    }
    public interface MultiTokenListener
    {
        void OnSuccess();
        void OnSuccess(string[] RegTokens, string[] error);
        void OnError(Exception e);
    }
    public interface ImportIosTokenListener
    {
        void OnSuccess(ImportTokenResponse importTokenResponse);
        void OnError(Exception e);
    }
}
