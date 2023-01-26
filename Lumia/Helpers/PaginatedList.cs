namespace Lumia.Helpers
{
    public class PaginatedList<T>: List<T>
    {

        public PaginatedList(List<T> values,int totalPageCount, int activePage, int elementCount)
        {
            this.AddRange(values);
            TotalPageCount = (int)Math.Ceiling(totalPageCount/(double)elementCount);
            ActivePage = activePage;
            ElementCount = elementCount;
        }

        public int TotalPageCount { get; set; }
        public int ActivePage { get; set; }
        public int ElementCount { get; set; }
        public bool HasNext { get => ActivePage == TotalPageCount; }
        public bool HasPrevious { get => ActivePage > 1; }


        public static PaginatedList<T> Create(IQueryable<T> query, int elementcount, int activepage)
        {
            query = query.Skip((activepage - 1) * elementcount).Take(elementcount);
            return new PaginatedList<T>(query.ToList(), query.Count(), activepage, elementcount);
        }
    }
}
