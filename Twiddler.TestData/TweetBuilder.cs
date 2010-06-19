using System;
using Twiddler.Core.Models;

namespace Twiddler.TestData
{
    public class TweetBuilder
    {
        private readonly DateTime _createdDate = DateTime.Now.AddMinutes(-5.0);
        private readonly string _inReplyToStatusId;
        private readonly bool _isRead;
        private readonly User _user = New.User;
        private string _id = "1";
        private bool _isArchived;
        private string _status = "Unspecified Status";

        internal TweetBuilder(TweetBuilder basedOn)
        {
            _createdDate = basedOn._createdDate;
            _id = basedOn._id;
            _inReplyToStatusId = basedOn._inReplyToStatusId;
            _isArchived = basedOn._isArchived;
            _isRead = basedOn._isRead;
            _status = basedOn._status;
            _user = basedOn._user;
        }

        public TweetBuilder()
        {
        }

        public static implicit operator Tweet(TweetBuilder builder)
        {
            return new Tweet
                       {
                           CreatedDate = builder._createdDate,
                           Id = builder._id,
                           InReplyToStatusId = builder._inReplyToStatusId,
                           IsArchived = builder._isArchived,
                           IsRead = builder._isRead,
                           Status = builder._status,
                           User = builder._user
                       };
        }

        public TweetBuilder IdentifiedBy(string id)
        {
            return new TweetBuilder(this) {_id = id};
        }

        public TweetBuilder WithStatus(string status)
        {
            return new TweetBuilder(this) {_status = status};
        }

        public TweetBuilder Archived()
        {
            return new TweetBuilder(this) {_isArchived = true};
        }

        public TweetBuilder NotArchived()
        {
            return new TweetBuilder(this) {_isArchived = false};
        }
    }
}