namespace Twiddler.TwitterStore.Models
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Twiddler.Core;
    using Twiddler.Core.Models;

    #endregion

    public class Tweet : ITweet
    {
        private bool _isArchived;

        private bool _isRead;

        public Tweet()
        {
            Links = new List<Uri>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime CreatedDate { get; set; }

        public string Id { get; set; }

        public string InReplyToStatusId { get; set; }

        public bool IsArchived
        {
            get { return _isArchived; }
            set
            {
                if (_isArchived != value)
                {
                    _isArchived = value;
                    PropertyChanged.Raise(x => IsArchived);
                }
            }
        }

        public bool IsRead
        {
            get { return _isRead; }
            set
            {
                if (_isRead != value)
                {
                    _isRead = value;
                    PropertyChanged.Raise(x => IsRead);
                }
            }
        }

        public List<Uri> Links { get; set; }

        public List<string> Mentions { get; set; }

        public string Status { get; set; }

        public User User { get; set; }

        #region ITweet members

        public void Archive()
        {
            IsArchived = true;
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }

        #endregion
    }
}