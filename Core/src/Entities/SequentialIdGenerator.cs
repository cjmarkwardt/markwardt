namespace Markwardt;

public class SequentialIdGenerator : IIdGenerator
{
    public class Options
    {
        public bool Rollover { get; set; } = false;
        public long Min { get; set; } = 1;
        public long Max { get; set; } = long.MaxValue;
        public Func<long, string>? Convert { get; set; } = null;
    }

    public SequentialIdGenerator(Options? options = null)
    {
        this.options = options ?? new();

        Current = Min;
    }

    private readonly Options options;

    public long Min => options.Min;
    public long Max => options.Max;

    private long current;
    public long Current
    {
        get => current;
        set
        {
            if (value < Min || value > Max)
            {
                throw new ArgumentOutOfRangeException();
            }

            current = value;
        }
    }

    public string GenerateId()
    {
        if (Current == Max)
        {
            if (options.Rollover)
            {
                Current = Min;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        else
        {
            Current++;
        }

        return options.Convert != null ? options.Convert(Current) : Current.ToString();
    }
}