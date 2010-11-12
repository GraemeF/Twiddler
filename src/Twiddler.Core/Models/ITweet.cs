using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Twiddler.Core.Models
{
    public interface ITweet : INotifyPropertyChanged
    {
        string Id { get; set; }
        User User { get; set; }
        string Status { get; set; }
        DateTime CreatedDate { get; set; }
        string InReplyToStatusId { get; set; }
        List<Uri> Links { get; set; }
        List<string> Mentions { get; set; }
        bool IsRead { get; set; }
        bool IsArchived { get; set; }
        void Archive();
        void MarkAsRead();
    }
}