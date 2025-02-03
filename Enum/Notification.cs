namespace scriptium_backend_dotnet.Models.Util
{
    public enum EntityType
    {
        Comment = 0,
        Note = 1,
        User = 2
    }

    public enum NotificationType
    {
        Like = 0,
        Follow = 1,
        FollowPending = 2,
        Comment = 3,
        Reply = 4,
        Mention = 5
    }

}