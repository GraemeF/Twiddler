namespace Twiddler.Core.Models
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using ReactiveUI;

    #endregion

    public interface ITweet : IReactiveNotifyPropertyChanged
    {
        DateTime CreatedDate { get; set; }

        string Id { get; set; }

        string InReplyToStatusId { get; set; }

        bool IsArchived { get; set; }

        bool IsRead { get; set; }

        List<Uri> Links { get; set; }

        List<string> Mentions { get; set; }

        string Status { get; set; }

        User User { get; set; }

        void Archive();

        void MarkAsRead();
    }
}