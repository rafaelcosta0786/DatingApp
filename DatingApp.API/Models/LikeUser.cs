namespace DatingApp.API.Models
{
    public class LikeUser
    {
        public int LikeOriginUserId { get; set; }
        public int LikeDestinyUserId { get; set; }

        public Users LikeOriginUser { get; set; }
        public Users LikeDestinyUser { get; set; }
    }
}