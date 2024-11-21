namespace writings_backend_dotnet.Models.Util
{
    public enum FollowStatus
    {
        Pending = 0,
        Accepted = 1
    }

    public enum FollowRStatus
    {
        AutomaticallyAccepted = 0,
        Accepted = 1,
        Pending = 2,
        Retrieved = 3,
        Rejected = 4,
        Unfollowed = 5,
        Removed = 6
    }

}
