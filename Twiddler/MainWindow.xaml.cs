#region License

// TweetSharp
// Copyright (c) 2010 Daniel Crenna and Jason Diller
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System.Windows;
using System.Windows.Media;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using Twiddler.Properties;

namespace Twiddler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: Make sure you get a consumer key and secret from twitter
            //and put it in the settings file. Then you can remove this check.
            ////////////////////////////////////////////////////////////
            if (string.IsNullOrEmpty(Settings.Default.ConsumerKey)
                || string.IsNullOrEmpty(Settings.Default.ConsumerSecret))
            {
                MessageBox.Show(
                                   "You need to obtain a consumer key and consumer secret from Twitter and add them to the Settings.Settings file.",
                                   "Can't continue", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            ///////////////////////////////////////////////////////////

            if (!CheckHasAuthorization())
            {
                PerformOAuthAuthorization();
            }
            else
            {
                AuthStatusLabel.Content = "Authorized";
                AuthStatusLabel.Foreground = Brushes.Green;
            }
        }

        private void PerformOAuthAuthorization()
        {
            var dlg = new OAuthDialog();
            var result = dlg.ShowDialog();
            if (result.HasValue == result.Value)
            {
                if (VerifyOAuthCredentials())
                {
                    AuthStatusLabel.Content = "Authorized";
                    AuthStatusLabel.Foreground = Brushes.Green;
                }
            }
            else
            {
                AuthStatusLabel.Content = "Authorization cancelled.";
                AuthStatusLabel.Foreground = Brushes.Red;
                tryAgainButton.Visibility = Visibility.Visible;
            }
        }

        private bool CheckHasAuthorization()
        {
            var authorized = false;
            if (!string.IsNullOrEmpty(Settings.Default.AccessToken)
                && !string.IsNullOrEmpty(Settings.Default.AccessTokenSecret))
            {
                authorized = VerifyOAuthCredentials();
            }
            else
            {
                AuthStatusLabel.Content = "Auth tokens not found.";
                AuthStatusLabel.Foreground = Brushes.Red;
            }
            return authorized;
        }

        private static bool VerifyOAuthCredentials()
        {
            var authorized = false;
            var twitter = FluentTwitter.CreateRequest()
                .AuthenticateWith(Settings.Default.ConsumerKey, Settings.Default.ConsumerSecret,
                                  Settings.Default.AccessToken, Settings.Default.AccessTokenSecret)
                .Account().VerifyCredentials();
            var response = twitter.Request();
            var profile = response.AsUser();
            if (profile != null)
            {
                authorized = true;
            }
            return authorized;
        }

        private void tryAgainBtn_Click(object sender, RoutedEventArgs e)
        {
            PerformOAuthAuthorization();
        }
    }
}