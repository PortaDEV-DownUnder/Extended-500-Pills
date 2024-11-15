using System;

namespace ExtendedPills;

public static class Util
{
    public static double NDC(Random random, double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }
}