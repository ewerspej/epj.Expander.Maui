namespace epj.Expander.Maui;

public static class ViewAnimationExtensions
{
    public static Task AnimateHeightAsync(this View view, double start, double end)
    {
        var tcs = new TaskCompletionSource();

        var animation = new Animation(h => view.HeightRequest = h, start, end);
        animation.Commit(view, "Height", finished: (_, _) => tcs.SetResult());

        return tcs.Task;
    }

    public static Task AnimateTranslationYAsync(this View view, double start, double end)
    {
        var tcs = new TaskCompletionSource();

        var animation = new Animation(y => view.TranslationY = y, start, end);
        animation.Commit(view, "Translation", finished: (_, _) => tcs.SetResult());

        return tcs.Task;
    }
}