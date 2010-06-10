using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Twiddler.Models
{
    public class Tweet : INotifyPropertyChanged
    {
        private bool _isRead;

        public Tweet()
        {
            Links = new Uri[] {};
        }

        public string Id { get; set; }
        public User User { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string InReplyToStatusId { get; set; }
        public IEnumerable<Uri> Links { get; set; }

        public bool IsRead
        {
            get { return _isRead; }
            private set
            {
                if (_isRead != value)
                {
                    _isRead = value;
                    PropertyChanged.Raise(x => IsRead);
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}