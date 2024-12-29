using System.Linq.Expressions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class PagedList<T> : IPagedList<T>
{
    private PagedList(List<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public List<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }

    public bool HasNextPage => PageNumber * PageSize < TotalCount;
    public bool HasPreviousPage => PageNumber > 1;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public async static Task<PagedList<T>> CreateAsync
        (IQueryable<T> query, int page, int pageSize, CancellationToken token = default)
    {
        var totalCount = await query.CountAsync(token);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);
        
        return new PagedList<T>(items, page, pageSize, totalCount);
    }

    public IPagedList<K> ConvertTo<K>(Expression<Func<T, K>> selector)
    {
        return new PagedList<K>(
            Items.Select(i => selector.Compile()(i)).ToList(), 
            PageNumber, 
            PageSize, 
            TotalCount
        );
    }
}
