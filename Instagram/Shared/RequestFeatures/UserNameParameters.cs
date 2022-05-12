namespace Shared.RequestFeatures
{
    public class UserNameParameters
    {
        public string? Search { get; set; }
        public IEnumerable<string>? UserIds { get; set; }
    }
}
