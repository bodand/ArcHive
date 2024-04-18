namespace ArcHive.Model;

/// <summary>
///     A special URL class, that stores a string and whether the URL it holds
///     has returned an error when asked about the resource it points to.
/// </summary>
/// <param name="Url">
///     The URL representing a valid resource. If null, the URL has not yet
///     responded, or returned an error. See <see cref="Errored"/>.
/// </param>
/// <param name="Errored">
///     If the URL is null, this field specifies whether the URL just has not
///     responded yet, or has actively failed.
///     If the URL is not null, this field's value is false.
/// </param>
public readonly record struct LoadingUrl(string? Url, bool Errored = false);
