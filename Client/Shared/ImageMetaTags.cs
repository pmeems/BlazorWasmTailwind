namespace BlazorApp.Client.Shared
{
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
    public readonly record struct ImageMetaTag(string Name, int Width, int Heigth, string Alt);
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
}
