namespace epj.Expander.Maui;

public static class ViewAnimationExtensions
{
    public static Animation AnimateHeightRequest(this View view, double start, double end, Easing easing)
    {
        return new Animation(h => view.HeightRequest = h, start, end, easing);
    }

    public static Animation AnimateTranslationY(this View view, double start, double end, Easing easing)
    {
        return new Animation(y => view.TranslationY = y, start, end, easing);
    }

    public static Animation Add(this Animation parent, Animation animation)
    {
        parent.Add(0, 1, animation);
        return parent;
    }

    public static Task AnimateAsync(this View view, Animation animation, uint duration)
    {
        var tcs = new TaskCompletionSource();

        animation.Commit(view, "ExpanderAnimation", length: duration, finished: (v, c) =>
        {
            tcs.SetResult();
        });

        return tcs.Task;
    }
}