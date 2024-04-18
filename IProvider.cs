using System;

namespace ArcHive;

/// <summary>
///     Interface hiding the concrete DI container. Used to ease the problems of
///     the Service Locator pattern, which we cannot get around, as pages and
///     Windows are not created by a DI system.
/// </summary>
public interface IProvider : IAsyncDisposable
{
    /// <summary>
    ///     Asks the DI subsystem go create a object of type T. Throws an
    ///     exception if it does not exist in the IoC container.
    /// </summary>
    /// <typeparam name="T">The type to request from DI.</typeparam>
    /// <returns>An object as the given type.</returns>
    T Get<T>() where T : notnull;
}
