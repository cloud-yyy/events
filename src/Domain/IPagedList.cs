namespace Domain;

public interface IPagedList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public bool HasNextPage { get; }
    public bool HasPreviousPage { get; }
}
