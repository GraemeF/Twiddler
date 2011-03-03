namespace Twiddler.TestData
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NSubstitute;

    using ReactiveUI.Testing;

    using Twiddler.Core.Models;

    #endregion

    public class TweetBuilder
    {
        private readonly DateTime _createdDate = DateTime.Now.AddMinutes(-5.0);

        private readonly User _user = A.User;

        private string _id = "1";

        private string _inReplyToStatusId;

        private bool _isArchived;

        private bool _isRead;

        private List<Uri> _links = new List<Uri>();

        private List<string> _mentions = new List<string>();

        private string _status = "Unspecified Status";

        public TweetBuilder()
        {
        }

        internal TweetBuilder(TweetBuilder basedOn)
        {
            _createdDate = basedOn._createdDate;
            _id = basedOn._id;
            _inReplyToStatusId = basedOn._inReplyToStatusId;
            _isArchived = basedOn._isArchived;
            _isRead = basedOn._isRead;
            _status = basedOn._status;
            _user = basedOn._user;
            _links = basedOn._links.ToList();
            _mentions = basedOn._mentions.ToList();
        }

        public TweetBuilder Archived()
        {
            return new TweetBuilder(this)
                       {
                           _isArchived = true
                       };
        }

        public ITweet Build()
        {
            ITweet tweet = Substitute.For<ITweet>().WithReactiveProperties();

            tweet.CreatedDate.Returns(_createdDate);
            tweet.Id.Returns(_id);
            tweet.InReplyToStatusId.Returns(_inReplyToStatusId);
            tweet.IsArchived.Returns(_isArchived);
            tweet.IsRead.Returns(_isRead);
            tweet.Status.Returns(_status);
            tweet.User.Returns(_user);
            tweet.Links.Returns(new List<Uri>(_links));
            tweet.Mentions.Returns(new List<string>(_mentions));

            return tweet;
        }

        public TweetBuilder IdentifiedBy(string id)
        {
            return new TweetBuilder(this)
                       {
                           _id = id
                       };
        }

        public TweetBuilder InReplyTo(string statusId)
        {
            return new TweetBuilder(this)
                       {
                           _inReplyToStatusId = statusId
                       };
        }

        public TweetBuilder LinkingTo(Uri link)
        {
            return new TweetBuilder(this)
                       {
                           _links = new List<Uri>(_links)
                                        {
                                            link
                                        }
                       };
        }

        public TweetBuilder Mentioning(string screenName)
        {
            return new TweetBuilder(this)
                       {
                           _mentions = new List<string>(_mentions)
                                           {
                                               screenName
                                           }
                       };
        }

        public TweetBuilder NotArchived()
        {
            return new TweetBuilder(this)
                       {
                           _isArchived = false
                       };
        }

        public TweetBuilder WhichHasBeenRead()
        {
            return new TweetBuilder(this)
                       {
                           _isRead = true
                       };
        }

        public TweetBuilder WhichIsUnread()
        {
            return new TweetBuilder(this)
                       {
                           _isRead = false
                       };
        }

        public TweetBuilder WithStatus(string status)
        {
            return new TweetBuilder(this)
                       {
                           _status = status
                       };
        }
    }
}