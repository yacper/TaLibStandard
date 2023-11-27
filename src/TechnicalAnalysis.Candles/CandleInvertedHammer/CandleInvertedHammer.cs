// Copyright (c) 2023 Philippe Matray. All rights reserved.
// This file is part of TaLibStandard.
// TaLibStandard is licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for the full license text.
// For more information, visit https://github.com/phmatray/TaLibStandard.

namespace TechnicalAnalysis.Candles;

/// <summary>
/// Inverted Hammer (Pattern Recognition)
/// </summary>
/// <param name="open">An array of open prices.</param>
/// <param name="high">An array of high prices.</param>
/// <param name="low">An array of low prices.</param>
/// <param name="close">An array of close prices.</param>
/// <typeparam name="T">The type of the array elements.</typeparam>
public class CandleInvertedHammer<T>(in T[] open, in T[] high, in T[] low, in T[] close)
    : CandleIndicator<T>(open, high, low, close)
    where T : IFloatingPoint<T>
{
    private T _bodyPeriodTotal = T.Zero;
    private T _shadowLongPeriodTotal = T.Zero;
    private T _shadowVeryShortPeriodTotal = T.Zero;

    /// <summary>
    /// Computes the <see cref="CandleInvertedHammer{T}"/> indicator.
    /// </summary>
    /// <param name="startIdx">The start index.</param>
    /// <param name="endIdx">The end index.</param>
    /// <returns></returns>
    public CandleIndicatorResult Compute(int startIdx, int endIdx)
    {
        // Initialize output variables 
        int outBegIdx = default;
        int outNBElement = default;
        int[] outInteger = new int[int.Max(0, endIdx - startIdx + 1)];
            
        // Validate the requested output range.
        ArgumentOutOfRangeException.ThrowIfNegative(startIdx);
        ArgumentOutOfRangeException.ThrowIfNegative(endIdx - startIdx);

        // Verify required price component.
        ArgumentNullException.ThrowIfNull(Open);
        ArgumentNullException.ThrowIfNull(High);
        ArgumentNullException.ThrowIfNull(Low);
        ArgumentNullException.ThrowIfNull(Close);

        // Identify the minimum number of price bar needed to calculate at least one output.
        int lookbackTotal = GetLookback();

        // Move up the start index if there is not enough initial data.
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        // Make sure there is still something to evaluate.
        if (startIdx > endIdx)
        {
            return new CandleIndicatorResult(Success, outBegIdx, outNBElement, outInteger);
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        int bodyTrailingIdx = startIdx - GetCandleAvgPeriod(BodyShort);
        int shadowLongTrailingIdx = startIdx - GetCandleAvgPeriod(ShadowLong);
        int shadowVeryShortTrailingIdx = startIdx - GetCandleAvgPeriod(ShadowVeryShort);
            
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            _bodyPeriodTotal += GetCandleRange(BodyShort, i);
            i++;
        }

        i = shadowLongTrailingIdx;
        while (i < startIdx)
        {
            _shadowLongPeriodTotal += GetCandleRange(ShadowLong, i);
            i++;
        }

        i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            _shadowVeryShortPeriodTotal += GetCandleRange(ShadowVeryShort, i);
            i++;
        }

        /* Proceed with the calculation for the requested range.
         * Must have:
         * - small real body
         * - long upper shadow
         * - no, or very short, lower shadow
         * - gap down
         * The meaning of "short", "very short" and "long" is specified with TA_SetCandleSettings;
         * outInteger is positive (1 to 100): inverted hammer is always bullish;
         * the user should consider that an inverted hammer must appear in a downtrend, while this function does not consider it
         */
        int outIdx = 0;
        do
        {
            outInteger[outIdx++] = GetPatternRecognition(i) ? 100 : 0;

            /* add the current range and subtract the first range: this is done after the pattern recognition 
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            _bodyPeriodTotal +=
                GetCandleRange(BodyShort, i) -
                GetCandleRange(BodyShort, bodyTrailingIdx);

            _shadowLongPeriodTotal +=
                GetCandleRange(ShadowLong, i) -
                GetCandleRange(ShadowLong, shadowLongTrailingIdx);

            _shadowVeryShortPeriodTotal +=
                GetCandleRange(ShadowVeryShort, i) -
                GetCandleRange(ShadowVeryShort, shadowVeryShortTrailingIdx);

            i++;
            bodyTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
        } while (i <= endIdx);
            
        // All done. Indicate the output limits and return.
        outNBElement = outIdx;
        outBegIdx = startIdx;
            
        return new CandleIndicatorResult(Success, outBegIdx, outNBElement, outInteger);
    }

    /// <inheritdoc />
    public override bool GetPatternRecognition(int i)
    {
        bool isInvertedHammer =
            // small rb
            GetRealBody(i) < GetCandleAverage(BodyShort, _bodyPeriodTotal, i) &&
            // long upper shadow
            GetUpperShadow(i) > GetCandleAverage(ShadowLong, _shadowLongPeriodTotal, i) &&
            // very short lower shadow
            GetLowerShadow(i) < GetCandleAverage(ShadowVeryShort, _shadowVeryShortPeriodTotal, i) &&
            // gap down
            GetRealBodyGapDown(i, i - 1);
            
        return isInvertedHammer;
    }

    /// <inheritdoc />
    public override int GetLookback()
    {
        return GetCandleMaxAvgPeriod(BodyShort, ShadowLong, ShadowVeryShort) + 1;
    }
}
