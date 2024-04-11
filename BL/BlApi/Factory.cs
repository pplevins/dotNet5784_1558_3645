namespace BlApi;

public static class Factory
{
    private static readonly Lazy<IBl> lazyInstance = new Lazy<IBl>(() => new BlImplementation.Bl(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static IBl Get() => lazyInstance.Value;
};
