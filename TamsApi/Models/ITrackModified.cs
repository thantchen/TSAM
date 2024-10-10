using System;

namespace TamsApi.Models
{
    public interface ITrackModified
    {
        DateTime LastModifiedDate { get; set; }
        long LastModifiedUserId { get; set; }
    }

    public interface ITrackCreated
    {
        DateTime CreatedDate { get; set; }
        long CreatedUserId { get; set; }
    }
}
