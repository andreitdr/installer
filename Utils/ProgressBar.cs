public class ProgressBar
{
    public ProgressBar(ProgressBarType type)
    {
        this.type = type;
    }

    public float Max { get; init; }
    public ConsoleColor Color { get; init; }
    public bool NoColor { get; init; }
    public ProgressBarType type { get; set; }

    private int BarLength = 32;
    private int position = 1;
    private bool positive = true;

    private bool isRunning;

    public async void Start()
    {
        if (type != ProgressBarType.NO_END)
            throw new Exception("Only NO_END progress bar can use this method");
        if (isRunning)
            throw new Exception("This progress bar is already running");

        isRunning = true;
        while (isRunning)
        {
            UpdateNoEnd();
            await System.Threading.Tasks.Task.Delay(100);
        }
    }

    public void Stop()
    {
        if (type != ProgressBarType.NO_END)
            throw new Exception("Only NO_END progress bar can use this method");
        if (!isRunning)
            throw new Exception("Can not stop a progressbar that did not start");
        isRunning = false;
    }

    public void Update(float progress)
    {
        switch (type)
        {
            case ProgressBarType.NORMAL:
                UpdateNormal(progress);
                return;
            case ProgressBarType.NO_END:
                if (progress <= 99.9f)
                    UpdateNoEnd();
                return;
            default:
                return;
        }
    }

    private void UpdateNoEnd()
    {
        Console.CursorLeft = 0;
        Console.Write("[");
        for (int i = 1; i <= position; i++)
            Console.Write(" ");
        Console.Write("<==()==>");
        position += positive ? 1 : -1;
        for (int i = position; i <= BarLength - 1 - (positive ? 0 : 2); i++)
            Console.Write(" ");
        Console.Write("]");


        if (position == BarLength - 1 || position == 1)
            positive = !positive;
    }

    private void UpdateNormal(float progress)
    {
        Console.CursorLeft = 0;
        Console.Write("[");
        Console.CursorLeft = BarLength;
        Console.Write("]");
        Console.CursorLeft = 1;
        float onechunk = 30.0f / Max;

        int position = 1;

        for (int i = 0; i < onechunk * progress; i++)
        {
            Console.BackgroundColor = NoColor ? ConsoleColor.Black : this.Color;
            Console.CursorLeft = position++;
            Console.Write("#");
        }

        for (int i = position; i < BarLength; i++)
        {
            Console.BackgroundColor = NoColor ? ConsoleColor.Black : ConsoleColor.DarkGray;
            Console.CursorLeft = position++;
            Console.Write(" ");
        }

        Console.CursorLeft = BarLength + 4;
        Console.BackgroundColor = ConsoleColor.Black;
        if (CanAproximateTo(progress, Max))
            Console.Write(progress + " %      ✓");
        else
            Console.Write(MathF.Round(progress, 2) + " %       ");
    }

    private bool CanAproximateTo(float f, float y) => (MathF.Abs(f - y) < 0.000001);


}

public enum ProgressBarType { NORMAL, NO_END }
