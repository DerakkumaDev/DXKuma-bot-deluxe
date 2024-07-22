namespace DXKumaBot.Utils;

public static class RandomExtensions
{
    public static int Choose(this Random random, IList<int> weights)
    {
        int totalWeight = weights.Sum();
        int randomResult = random.Next(totalWeight);
        for (int index = 0; index < weights.Count; index++)
        {
            randomResult -= weights[index];
            if (randomResult < 0)
            {
                return index;
            }
        }

        return -1;
    }
}