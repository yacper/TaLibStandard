using System;

namespace TechnicalAnalysis.Common;

/// <summary>
/// Provides a set of extension methods for arrays.
/// </summary>
internal static class ArrayExtension
{
    /// <summary>
    /// Converts an array of floats to an array of doubles.
    /// </summary>
    /// <param name="arr">The float array to be converted.</param>
    /// <returns>A new array of doubles with the same elements as the input float array.</returns>
    internal static double[] ToDouble(this float[] arr)
        => Array.ConvertAll(arr, x => (double)x);
}
