#region License

// TweetSharp
// Copyright (c) 2010 Daniel Crenna and Jason Diller
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

namespace Twiddler.TweetSharp
{
    #region Using Directives

    using System;
    using System.Diagnostics;
    using System.Windows;

    using global::TweetSharp;

    using Twiddler.Core;
    using Twiddler.Core.Models;
    using Twiddler.Core.Services;

    #endregion

    [NoCoverage]
    public partial class OAuthDialog
    {
        private readonly IAccessTokenStore _accessTokenStore;

        private readonly ITwitterApplicationCredentials _applicationCredentials;

        private readonly TwitterService _service;

        private OAuthRequestToken _requestToken;

        public OAuthDialog(IAccessTokenStore accessTokenStore, ITwitterApplicationCredentials applicationCredentials)
        {
            _accessTokenStore = accessTokenStore;
            _applicationCredentials = applicationCredentials;
            InitializeComponent();

            pinTextBox.Visibility = Visibility.Hidden;
            pinLbl.Visibility = Visibility.Hidden;
            pinInstruction.Visibility = Visibility.Hidden;

            _service = new TwitterService(_applicationCredentials.ConsumerKey, 
                                          _applicationCredentials.ConsumerSecret);
        }

        private void AuthorizeDesktopBtn_Click(object sender, RoutedEventArgs e)
        {
            // Step 1 - Retrieve an OAuth Request Token
            _requestToken = _service.GetRequestToken();

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = _service.GetAuthorizationUri(_requestToken);
            Process.Start(uri.ToString());

            AuthorizeDesktopBtn.IsEnabled = false;
            pinTextBox.Visibility = Visibility.Visible;
            pinLbl.Visibility = Visibility.Visible;
            pinInstruction.Visibility = Visibility.Visible;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pinTextBox.Text))
            {
                MessageBox.Show("Enter the PIN provided by twitter.com", 
                                "Can't complete Authorization", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
                return;
            }

            string verifier = pinTextBox.Text;
            OAuthAccessToken access = _service.GetAccessToken(_requestToken, verifier);

            // Step 4 - User authenticates using the Access Token
            _service.AuthenticateWith(access.Token, access.TokenSecret);

            var credentials = new AccessToken(AccessToken.DefaultCredentialsId, 
                                              access.Token, 
                                              access.TokenSecret);
            _accessTokenStore.Save(credentials);

            DialogResult = true;
            Close();
        }
    }
}