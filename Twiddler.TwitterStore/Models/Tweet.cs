namespace Twiddler.TwitterStore.Models
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using ReactiveUI;

    using Twiddler.Core.Models;

    #endregion

    public class Tweet : ReactiveObject, 
                         ITweet
    {
        private bool _IsArchived;

        private bool _IsRead;

        public Tweet()
        {
            Links = new List<Uri>();
        }

        public DateTime CreatedDate { get; set; }

        public string Id { get; set; }

        public string InReplyToStatusId { get; set; }

        public bool IsArchived
        {
            get { return _IsArchived; }
            set { this.RaiseAndSetIfChanged(x => x.IsArchived, value); }
        }

        public bool IsRead
        {
            get { return _IsRead; }
            set { this.RaiseAndSetIfChanged(x => x.IsRead, value); }
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