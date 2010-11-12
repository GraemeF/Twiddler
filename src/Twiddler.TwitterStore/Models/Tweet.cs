using System;
using System.Collections.Generic;
using System.ComponentModel;
using Twiddler.Core;
using Twiddler.Core.Models;

namespace Twiddler.TwitterStore.Models
{
    public class Tweet : ITweet
    {
        private bool _isArchived;
        private bool _isRead;

        public Tweet()
        {
            Links = new List<Uri>();
        }

        #region ITweet Members

        public string Id { get; set; }
        public User User { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string InReplyToStatusId { get; set; }
        public List<Uri> Links { get; set; }
        public List<string> Mentions { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

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