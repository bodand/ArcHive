using Microsoft.UI.Xaml;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ArcHive.Services.Covers;
using ArcHive.Services.Details;
using ArcHive.Services.Search;
using ArcHive.View;
using ArcHive.ViewModel;
using ArcHive.ViewModel.Pages;
using Autofac;
using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.DependencyInjection;

namespace ArcHive;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application, IAsyncDisposable
{
    /// <summary>
    ///     Special member to get the current App instance in the application.
    ///     A specialized case of the <see cref="Application.Current"/> member,
    ///     but with a correct type for this application.
    /// </summary>
    public new static readonly App Current = (Application.Current as App)!;

    /// <summary>
    ///     Service locator, used to enable DI capabilities where there are no
    ///     DI contexts otherwise available. (UI elements, instantiated by
    ///     WinUI 3.)
    /// </summary>
    public readonly IProvider Provider;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();

        // hack to get IHttpClientFactory into Autofac
        IServiceProvider httpProvider = new ServiceCollection()
            .AddHttpClient()
            .BuildServiceProvider();

        var builder = new ContainerBuilder();
        builder.Register(c => httpProvider.GetRequiredService<IHttpClientFactory>().CreateClient()).As<HttpClient>();
        builder.RegisterType<OlSearchService>().As<ISearchService>();
        builder.RegisterType<OlDetailsService>().As<IDetailsService>();
        builder.RegisterType<OlCoverService>().Keyed<ICoverService>("raw");
        builder.RegisterType<CachingCoverService>().Keyed<ICoverService>("cached").WithAttributeFiltering();

        builder.RegisterType<DetailPageViewModel>().WithAttributeFiltering();
        builder.RegisterType<MainWindowViewModel>().WithAttributeFiltering();
        builder.RegisterType<ListPageViewModel>().WithAttributeFiltering();

        Provider = new AutofacProvider(builder.Build().BeginLifetimeScope());
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();
    }

    private Window _window = default!;

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await Provider.DisposeAsync();
    }
}