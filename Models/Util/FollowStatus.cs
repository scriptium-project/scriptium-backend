using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace writings_backend_dotnet.Models
{
    public enum FollowStatus
    {
        Pending,
        Accepted
    }
    
    public enum FollowRStatus
    {
        AutomaticallyAccepted,
        Accepted,
        Pending,
        Retrieved,
        Rejected,
        Unfollowed,
        Removed
    }

}
