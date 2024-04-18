using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ArcHive.Model.Search;

/// <summary>
///     A search field type that allows querying for multiple fields.
///     The generated query explicitly states the provided fields, to search
///     the value for.
/// </summary>
public partial class ComplexSearchField : ObservableObject, ISearchFields
{
    /// <summary>
    ///     The title to search for.
    /// </summary>
    [ObservableProperty]
    private string? _title;

    /// <summary>
    ///     The author to search for.
    /// </summary>
    [ObservableProperty]
    private string? _author;

    /// <summary>
    ///     A subject/theme/keyword to search for.
    ///     Depending on the <see cref="SubjectTotal"/> field, this field is
    ///     matched totally, without fuzziness.
    /// </summary>
    ///
    /// <seealso cref="SubjectTotal"/>
    [ObservableProperty]
    private string? _subject;

    /// <summary>
    ///     Specifies whether the value provided in <see cref="Subject"/> is
    ///     matched with fuzzy search semantics. If this is true, then no fuzzy
    ///     finding is performed.
    /// </summary>
    ///
    /// <seealso cref="Subject"/>
    [ObservableProperty]
    private bool _subjectTotal = false;

    /// <summary>
    ///     A place to search for. Could be fictional, or real, that is related
    ///     to the book.
    /// </summary>
    [ObservableProperty]
    private string? _place;

    /// <summary>
    ///     A person to search for, either real, or fictional that is related to
    ///     the book or its story.
    /// </summary>
    [ObservableProperty]
    private string? _person;

    /// <summary>
    ///     A language to search for that is related to the book.
    ///     Either the language it is written, or a language mentioned in the
    ///     story.
    /// </summary>
    [ObservableProperty]
    private string? _language;

    /// <summary>
    ///     The publisher that published the book.
    /// </summary>
    [ObservableProperty]
    private string? _publisher;

    /// <summary>
    ///     Only find books published after this date.
    /// </summary>
    [ObservableProperty]
    private string? _publishAfter;

    /// <summary>
    ///     Only find books published before this date.
    /// </summary>
    [ObservableProperty]
    private string? _publishBefore;

    /// <inheritdoc/>
    public string ToQueryString()
    {
        var builder = new List<string>();
        AddField(builder, "title", Title);
        AddField(builder, "author", Author);

        var subjKey = SubjectTotal ? "subject_key" : "subject";
        AddField(builder, subjKey, Subject);

        AddField(builder, "place", Place);
        AddField(builder, "person", Person);
        AddField(builder, "language", Language);
        AddField(builder, "publisher", Publisher);

        if (this is { PublishBefore: null, PublishAfter: null }) return string.Join(",", builder);

        var after = PublishAfter?.ToString() ?? "*";
        var before = PublishBefore?.ToString() ?? "*";
        AddField(builder, "publish_year", $"[{after} TO {before}]");

        return string.Join(",", builder);
    }

    private static void AddField(ICollection<string> builder, string name, string? value)
    {
        if (value is null) return;

        builder.Add($"{name}:{value.Replace(' ', '+')}");
    }
}
