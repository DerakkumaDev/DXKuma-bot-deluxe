namespace DXKumaBot.Utils;

public sealed class WeightsRandom
{
    private readonly int[] _alias;
    private readonly float[] _probability;

    public WeightsRandom(IList<int> weights)
    {
        if (weights.Count <= 0)
        {
            throw new ArgumentException("Weights must have at least one weight", nameof(weights));
        }

        int count = weights.Count;
        float scale = Convert.ToSingle(count) / weights.Sum();
        List<float> scaledWeights = [..weights.Select(weight => weight * scale)];
        Queue<int> small = [];
        Queue<int> large = [];
        for (int i = 0; i < count; ++i)
        {
            if (scaledWeights[i] < 1.0f)
            {
                small.Enqueue(i);
            }
            else
            {
                large.Enqueue(i);
            }
        }

        _alias = new int[count];
        _probability = new float[count];
        while (small.TryPeek(out int less) && large.TryPeek(out int more))
        {
            _alias[less] = more;
            _probability[less] = scaledWeights[less];
            scaledWeights[more] += scaledWeights[less] - 1.0f;
            small.Dequeue();
            large.Dequeue();
            if (scaledWeights[more] < 1.0f)
            {
                small.Enqueue(more);
            }
            else
            {
                large.Enqueue(more);
            }
        }

        while (small.TryDequeue(out int result))
        {
            _probability[result] = 1.0f;
        }

        while (large.TryDequeue(out int result))
        {
            _probability[result] = 1.0f;
        }
    }

    public int Next()
    {
        int column = Random.Shared.Next(_alias.Length);
        bool coinToss = Random.Shared.NextSingle() < _probability[column];
        return coinToss ? column : _alias[column];
    }
}