using Service.Contracts;

namespace Service
{

    public class PostSlugService : IPostSlugService
    {
        public int Decode(string slug)
        {
            return new Shared.Base53(slug).Value;
        }
        public string Encode(int postId)
        {
            return new Shared.Base53(postId).Slug;
        }
    }
}
