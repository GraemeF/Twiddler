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
using TweetSharp.Model;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using Twiddler.Core;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler
{
    [NoCoverage]
    public partial class OAuthDialog
    {
        private readonly ITwitterApplicationCredentials _applicationCredentials;
        private readonly IAccessTokenStore _accessTokenStore;
        private readonly OAuthToken _requestToken;

        public OAuthDialog(IAccessTokenStore accessTokenStore, ITwitterApplicationCredentials applicationCredentials)
        {
            _accessTokenStore = accessTokenStore;
            _applicationCredentials = applicationCredentials;
            InitializeComponent();

            pinTextBox.Visibility = Visibility.Hidden;
            pinLbl.Visibility = Visibility.Hidden;
            pinInstruction.Visibility = Visibility.Hidden;

            AccessToken creds = _accessTokenStore.Load(AccessToken.DefaultCredentialsId);

            //get a request token.  this is only used during 
            //this process. 
            IFluentTwitter getRequestTokenRequest = FluentTwitter.CreateRequest()
                .Authentication.GetRequestToken(_applicationCredentials.ConsumerKey,
                                                _applicationCredentials.ConsumerSecret);

            TwitterResult response = getRequestTokenRequest.Request();
            _requestToken = response.AsToken();


            //TODO: handle the case where the token is NULL because 
            //twitter is down or broken in a manner suitable to your app

            //wait for the user to click the "Authorize button" 
        }

        private void AuthorizeDesktopBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthorizeDesktopBtn.IsEnabled = false;
            pinTextBox.Visibility = Visibility.Visible;
            pinLbl.Visibility = Visibility.Visible;
            pinInstruction.Visibility = Visibility.Visible;
            IFluentTwitter twitter = FluentTwitter.CreateRequest()
                .Authentication
                .AuthorizeDesktop(_applicationCredentials.ConsumerKey,
                                  _applicationCredentials.ConsumerSecret,
                                  _requestToken.Token);

            twitter.Request();

            //wait again until the user has authorized the desktop app
            //entered the PIN, and clicked "OK"
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pinTextBox.Text))
            {
                MessageBox.Show("Enter the PIN provided by twitter.com", "Can't complete Authorization",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string verifier = pinTextBox.Text;
            IFluentTwitter twitter =
                FluentTwitter.CreateRequest().Authentication.GetAccessToken(_applicationCredentials.ConsumerKey,
                                                                            _applicationCredentials.ConsumerSecret,
                                                                            _requestToken.Token,
                                                                            verifier);
            TwitterResult response = twitter.Request();
            OAuthToken result = response.AsToken();

            if (result == null || string.IsNullOrEmpty(result.Token))
            {
                MessageBox.Show(response.AsError().ErrorMessage, "Error", MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                //TODO: handle this error condition. 
                //the user may have incorrectly entered the PIN or twitter 
                //may be down. Handle this in a way that makes sense for your
                //application.
                DialogResult = false;
                return;
            }

            var credentials = new AccessToken(AccessToken.DefaultCredentialsId,
                                                     result.Token, result.TokenSecret);
            _accessTokenStore.Save(credentials);

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}