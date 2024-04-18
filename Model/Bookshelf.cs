using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ArcHive.Model;

/// <summary>
///     The DTO representing the data sent by the OpenLibrary API for a
///     bookshelf endpoint.
/// </summary>
internal sealed class BookshelfDto
{
    /// <summary>
    ///     The number of people who want to read this book in the future.
    /// </summary>
    [JsonPropertyName("want_to_read")]
    public int WantToRead { get; set; } = 0;

    /// <summary>
    ///     The number of people currently reading this book.
    /// </summary>
    [JsonPropertyName("currently_reading")]
    public int Current { get; set; } = 0;

    /// <summary>
    ///     The number of people who have already read this book.
    /// </summary>
    [JsonPropertyName("already_read")]
    public int AlreadyRead { get; set; } = 0;
}

/// <summary>
///     A model class representing a bookshelf entity on the OpenLibrary API.
///     It stores data about the number of people who are reading, already have
///     read or just want to read a given book.
/// </summary>
public partial class Bookshelf : ObservableObject
{
    /// <summary>
    ///     The number of people who want to read this book in the future.
    /// </summary>
    [ObservableProperty]
    private int _wantToRead;

    /// <summary>
    ///     The number of people currently reading this book.
    /// </summary>
    [ObservableProperty]
    private int _current;

    /// <summary>
    ///     The number of people who have already read this book.
    /// </summary>
    [ObservableProperty]
    private int _alreadyRead;

    /// <summary>
    ///     A constructor that creates a client-side model object from the
    ///     API endpoint's DTO class.
    /// </summary>
    /// <param name="dto">
    ///     The DTO class to use for the initialization of the model class.
    /// </param>
    internal Bookshelf(BookshelfDto dto)
    {
        WantToRead = dto.WantToRead;
        Current = dto.Current;
        AlreadyRead = dto.AlreadyRead;
    }
}
