namespace Backend
{
    public interface IPrayerTiming
    {
        Task<string> CreateNewTiming(int category, int module, CancellationToken cancellationToken);

    }
}
