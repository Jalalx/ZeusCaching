namespace ZeusCaching.Services
{
    public interface ICachingAdapterFactory
    {
        ICachingAdapter Create(CachingAdapterMode mode);
    }
}
