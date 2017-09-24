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
            admin.SubscribeToTopic("doHfsf52kZk:APA91bHMC__u-VxBjiApy1VibveHz0ubsBT1Z1tqPvUeBhYpwUCgQc0bH3ubbJnJ8VwvDk2cwqNc4IlNmjAPX25Te56X7VyJepLZ2N9gujE1NEy5Py0JIkb8M6rXosR0ZXNkT_YqYK7x",
                "test");
        }

        public void UnsubscribeSingleClickEvent(object sender, EventArgs eventArgs)
        {
            admin.UnSubscribeFromTopic("dsdsscsc", "test");
        }

        public void SubscribeMultipleClickEvent(object sender, EventArgs eventArgs)
        {
            admin.SubscribeToTopic(new string[]{"doHfsf52kZk:APA91bHMC__u-VxBjiApy1VibveHz0ubsBT1Z1tqPvUeBhYpwUCgQc0bH3ubbJnJ8VwvDk2cwqNc4IlNmjAPX25Te56X7VyJepLZ2N9gujE1NEy5Py0JIkb8M6rXosR0ZXNkT_YqYK7x",
            "doHfsf52kZk:APA91bHMC__u-VxBjiApy1VibveHz0ubsBT1Z1tqPvUeBhYpwUCgQc0bH3ubbJnJ8VwvDk2cwqNc4IlNmjAPX25Te56X7VyJepLZ2N9gujE1NEy5Py0JIkb8M6rXosR0ZXNkT_YqYK7x" },
            "test");
        }

        public void UnsubscribeMultipleClickEvent(object sender, EventArgs eventArgs)
        {
            admin.UnSubscribeFromTopic(new string[]{"",
            "doHfsf52kZk:APA91bHMC__1VibveHz0ubsBT1Z1tqPveeeeYpwUCgQc0bH3ubbJnJ8VwvDk2cwqNc4IlNmjAPX25Te56X7VyJepLZ2N9gujE1NEy5Py0JIkb8M6rXosR0ZXNkT_YqYK7x",
            ""},
           "test");
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
