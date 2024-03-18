using Microsoft.Extensions.Caching.Memory;

namespace LCPC.Share;

public class CacherHelper
{
    private MemoryCacheEntryOptions _options = new MemoryCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromSeconds(10));
    private readonly IMemoryCache _memoryCache;
    public CacherHelper(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="key">缓存 key</param>
    /// <param name="value">缓存值</param>
    /// <typeparam name="T">缓存类型</typeparam>
    public void Set<T>(T value)
    {
        var name = typeof(T).Name;
        _memoryCache.Set<T>(name, value,_options);
    }

    public T Get<T>(Type key)
    {
        return  _memoryCache.Get<T>(key);
    }

    public IQueryable<T> GetQuery<T>()
    {
        var name = typeof(T).Name;
        var obj =_memoryCache.Get<T>(name);
        if (obj != null)
            return (IQueryable<T>)obj;
        return default(IQueryable<T>);
    }
}