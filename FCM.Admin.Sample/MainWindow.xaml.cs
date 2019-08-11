using System;
using System.Windows;

namespace FCM.Admin.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, SingleTokenListener, MultiTokenListener, ImportIosTokenListener
    {
        Admin admin;
        public MainWindow()
        {
            InitializeComponent();
            admin = new Admin(ServerKeyBox.Text);
            admin.SingleTokenListener(this);
            admin.MultiTokenListener(this);
            admin.ImportIosTokenListener(this);
        }

        #region Click Events
        public void SubscribeSingleClickEvent(object sender, EventArgs eventArgs)
        {
            admin.SubscribeToTopic("fcm_device_token", "test");
        }

        public void UnsubscribeSingleClickEvent(object sender, EventArgs eventArgs)
        {
            admin.UnSubscribeFromTopic("dsdsscsc", "test");
        }

        public void SubscribeMultipleClickEvent(object sender, EventArgs eventArgs)
        {
            admin.SubscribeToTopic(new string[]{"fcm_device_token1", "fcm_device_token2" }, "test");
        }

        public void UnsubscribeMultipleClickEvent(object sender, EventArgs eventArgs)
        {
            admin.UnSubscribeFromTopic(new string[]{"fcm_device_token1", "fcm_device_token2"}, "test");
        }

        private void ImportiOSTokenClickEvent(object sender, RoutedEventArgs e)
        {
            admin.ImportToken("com.google.FCMTestApp", true, new string[] { "apns_tokens" });
        }
        #endregion

        public void OnSuccess()
        {
            MessageBox.Show("Successfully Subscribed/Unsubscribed");
        }

        public void OnSuccess(string RegToken, string error)
        {
            MessageBox.Show("Error Occurred for " + RegToken);
        }

        public void OnSuccess(string[] RegTokens, string[] error)
        {
            MessageBox.Show("Error Occurred");
        }

        public void OnSuccess(ImportTokenResponse importTokenResponse)
        {

        }

        public void OnError(Exception e)
        {

        }
    }
}
