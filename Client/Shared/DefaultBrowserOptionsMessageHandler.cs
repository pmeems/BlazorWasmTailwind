﻿using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace BlazorApp.Client.Shared;

/// <inheritdoc />
/// <seealso cref="https://www.meziantou.net/bypass-browser-cache-using-httpclient-in-blazor-webassembly.htm" />
public sealed class DefaultBrowserOptionsMessageHandler : DelegatingHandler
{
    private static readonly HttpRequestOptionsKey<IDictionary<string, object>> FetchRequestOptionsKey = new ("WebAssemblyFetchOptions");

    public DefaultBrowserOptionsMessageHandler()
    {
    }

    public DefaultBrowserOptionsMessageHandler(HttpMessageHandler innerHandler)
    {
        InnerHandler = innerHandler;
    }

    public BrowserRequestCache DefaultBrowserRequestCache { get; set; }

    public BrowserRequestCredentials DefaultBrowserRequestCredentials { get; set; }

    public BrowserRequestMode DefaultBrowserRequestMode { get; set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Get the existing options to not override them if set explicitly
        if (!request.Options.TryGetValue(FetchRequestOptionsKey, out var fetchOptions))
        {
            fetchOptions = null;
        }

        if (fetchOptions?.ContainsKey("cache") != true)
        {
            request.SetBrowserRequestCache(DefaultBrowserRequestCache);
        }

        if (fetchOptions?.ContainsKey("credentials") != true)
        {
            request.SetBrowserRequestCredentials(DefaultBrowserRequestCredentials);
        }

        if (fetchOptions?.ContainsKey("mode") != true)
        {
            request.SetBrowserRequestMode(DefaultBrowserRequestMode);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
