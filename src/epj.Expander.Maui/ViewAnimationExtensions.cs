namespace epj.Expander.Maui;

public static class ViewAnimationExtensions
{
    public static Task AnimateHeightAsync(this View view, double start, double end, Easing easing, uint duration)
    {
        var tcs = new TaskCompletionSource();

        var animation = new Animation(h => view.HeightRequest = h, start, end, easing);
        animation.Commit(view, "Height", length: duration, finished: (_, _) => tcs.SetResult());

        return tcs.Task;
    }

    public static Task AnimateTranslationYAsync(this View view, double start, double end, Easing easing, uint duration)
    {
        var tcs = new TaskCompletionSource();

        var animation = new Animation(y => view.TranslationY = y, start, end, easing);
        animation.Commit(view, "Translation", length: duration, finished: (_, _) => tcs.SetResult());

        return tcs.Task;
    }
}