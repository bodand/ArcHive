using System;
using System.Threading.Tasks;
using Autofac;

namespace ArcHive;

/// <summary>
///     The AutoFac backed <see cref="ArcHive.IProvider"/> implementation.
/// </summary>
/// <param name="container">
///     The AutoFac backend to use as the IoC container for type lookups.
/// </param>
public class AutofacProvider(ILifetimeScope container) : IProvider
{
    /// <inheritdoc/>
    public T Get<T>() where T : notnull => container.Resolve<T>();

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await container.DisposeAsync();
    }
}
