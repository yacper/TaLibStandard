// Copyright (c) 2023 Philippe Matray. All rights reserved.
// This file is part of TaLibStandard.
// TaLibStandard is licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for the full license text.
// For more information, visit https://github.com/phmatray/TaLibStandard.

namespace TechnicalAnalysis.Candles;

/// <summary>
/// 
/// </summary>
/// <param name="open">An array of open prices.</param>
/// <param name="high">An array of high prices.</param>
/// <param name="low">An array of low prices.</param>
/// <param name="close">An array of close prices.</param>
/// <typeparam name="T"></typeparam>
public class CandleTristar<T>(in T[] open, in T[] high, in T[] low, in T[] close)
    : CandleIndicator<T>(open, high, low, close)
    where T : IFloatingPoint<T>
{
    private T _bodyPeriodTotal = T.Zero;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startIdx"></param>
    /// <param name="endIdx"></param>
    /// <returns></returns>
    public CandleIndicatorResult Compute(int startIdx, int endIdx)
    {
        // Initialize output variables 
        int outBegIdx = default;
        int outNBElement = default;
        int[] outInteger = new int[int.Max(0, endIdx - startIdx + 1)];
            
        // Validate the requested output range.
        if (startIdx < 0)
        {
            return new CandleIndicatorResult(OutOfRangeStartIndex, outBegIdx, outNBElement, outInteger);
        }

        if (endIdx < 0 || endIdx < startIdx)
        {
            return new CandleIndicatorResult(OutOfRangeEndIndex, outBegIdx, outNBElement, outInteger);
        }

        // Verify required price component.
        if (Open == null! || High == null! || Low == null! || Close == null!)
        {
            return new CandleIndicatorResult(BadParam, outBegIdx, outNBElement, outInteger);
        }

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
        int bodyTrailingIdx = startIdx - 2 - GetCandleAvgPeriod(BodyDoji);
            
        int i = bodyTrailingIdx;
        while (i < startIdx - 2)
        {
            _bodyPeriodTotal += GetCandleRange(BodyDoji, i);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         * - 3 consecutive doji days
         * - the second doji is a star
         * The meaning of "doji" is specified with TA_SetCandleSettings
         * outInteger is positive (1 to 100) when bullish or negative (-1 to -100) when bearish
         */
        int outIdx = 0;
        do
        {
            if (GetPatternRecognition(i))
            {
                outInteger[outIdx] = 0;

                if ( // 2nd gaps up
                    GetRealBodyGapUp(i - 1, i - 2) &&
                    // 3rd is not higher than 2nd
                    T.Max(Open[i], Close[i]) < T.Max(Open[i - 1], Close[i - 1]))
                {
                    outInteger[outIdx] = -100;
                }

                if ( // 2nd gaps down 
                    GetRealBodyGapDown(i - 1, i - 2) &&
                    // 3rd is not lower than 2nd
                    T.Min(Open[i], Close[i]) > T.Min(Open[i - 1], Close[i - 1]))
                {
                    outInteger[outIdx] = +100;
                }

                outIdx++;
            }
            else
            {
                outInteger[outIdx++] = 0;
            }

            /* add the current range and subtract the first range: this is done after the pattern recognition 
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            _bodyPeriodTotal +=
                GetCandleRange(BodyDoji, i - 2) -
                GetCandleRange(BodyDoji, bodyTrailingIdx);

            i++;
            bodyTrailingIdx++;
        } while (i <= endIdx);

        // All done. Indicate the output limits and return.
        outNBElement = outIdx;
        outBegIdx = startIdx;
            
        return new CandleIndicatorResult(Success, outBegIdx, outNBElement, outInteger);
    }

    /// <inheritdoc />
    public override bool GetPatternRecognition(int i)
    {
        bool isCdlTristar =
            // 1st: doji
            GetRealBody(i - 2) <= GetCandleAverage(BodyDoji, _bodyPeriodTotal, i - 2) &&
            // 2nd: doji
            GetRealBody(i - 1) <= GetCandleAverage(BodyDoji, _bodyPeriodTotal, i - 2) &&
            // 3rd: doji
            GetRealBody(i) <= GetCandleAverage(BodyDoji, _bodyPeriodTotal, i - 2);
            
        return isCdlTristar;
    }

    /// <inheritdoc />
    public override int GetLookback()
    {
        return GetCandleAvgPeriod(BodyDoji) + 2;
    }
}
