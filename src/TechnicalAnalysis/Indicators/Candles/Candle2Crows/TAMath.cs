using System.Numerics;
using TechnicalAnalysis.Candles.Candle2Crows;
// ReSharper disable once CheckNamespace

namespace TechnicalAnalysis;

public static partial class TAMath
{
    public static Candle2CrowsResult Cdl2Crows<T>(
        int startIdx, int endIdx, T[] open, T[] high, T[] low, T[] close)
        where T : IFloatingPoint<T>
    {
        return new Candle2Crows<T>(open, high, low, close)
            .Compute(startIdx, endIdx);
    }
}
