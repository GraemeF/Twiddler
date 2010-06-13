using System;
using Twiddler.Core.Models;

namespace Twiddler.TestData
{
    public class TweetBuilder
    {
        private readonly DateTime _createdDate = DateTime.Now.AddMinutes(-5.0);
        private readonly string _inReplyToStatusId;
        private readonly User _user = New.User;
        private string _id = "1";
        private bool _isArchived;
        private bool _isRead;
        private string _status = "Unspecified Status";

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
    }
}