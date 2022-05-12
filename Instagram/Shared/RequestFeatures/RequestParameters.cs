namespace Shared.RequestFeatures
{
    public abstract record RequestParameters
    {
        public enum SelectedShows
        {
            Latest = 1,
            Suitable = 2,
            Popular = 3,
        }
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }
    }
}
