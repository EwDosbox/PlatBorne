using System;
using UnityEngine;
public class BetterRandom
{
    private readonly System.Random rnd;

    public BetterRandom(object seed)
    {
        string seedString = seed.ToString();
        int hash = 0;

        foreach (char c in seedString)
        {
            hash = (hash * 31) + c;
        }

        rnd = new System.Random(hash);
    }

    public int Range(int min, int max) => rnd.Next(min, max + 1);
    public float Range(float min, float max) => (float)(min + rnd.NextDouble() * (max - min));
    public double Range(double min, double max) => min + rnd.NextDouble() * (max - min);
    public Vector3 Range(Vector3 min, Vector3 max) => new(
        Range(min.x, max.x),
        Range(min.y, max.y),
        Range(min.z, max.z)
    );

    public int Random(int max) => Range(0, max);
    public float Random(float max) => Range(0f, max);
    public double Random(double max) => Range(0d, max);

    public T RandomEnum<T>() where T : Enum
    {
        T[] values = (T[])Enum.GetValues(typeof(T));
        return values[rnd.Next(values.Length)];
    }

    public Color Color() => new(
        (float)rnd.NextDouble(),
        (float)rnd.NextDouble(),
        (float)rnd.NextDouble()
    );
    public Color Color(float min, float max) => new(
        Range(min, max),
        Range(min, max),
        Range(min, max)
    );
}
