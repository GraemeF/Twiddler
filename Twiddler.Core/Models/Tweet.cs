using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Twiddler.Core.Models
{
    public class Tweet : INotifyPropertyChanged
    {
        private bool _isArchived;
        private bool _isRead;

        public Tweet()
        {
            Links = new List<Uri>();
        }

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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void Archive()
        {
            IsArchived = true;
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}