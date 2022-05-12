namespace Service.Contracts
{
    public interface IPostSlugService
    {
        int Decode(string slug);
        string Encode(int postId);
    }
}
