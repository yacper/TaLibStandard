using System;
using static TechnicalAnalysis.TACore.CandleSettingType;

namespace TechnicalAnalysis
{
    internal static partial class TACore
    {
        public static RetCode CdlThrusting(
            int startIdx,
            int endIdx,
            in double[] inOpen,
            in double[] inHigh,
            in double[] inLow,
            in double[] inClose,
            ref int outBegIdx,
            ref int outNBElement,
            ref int[] outInteger)
        {
            // Validate the requested output range.
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            // Verify required price component.
            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            // Identify the minimum number of price bar needed to calculate at least one output.
            int lookbackTotal = CdlThrustingLookback();

            // Move up the start index if there is not enough initial data.
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            // Make sure there is still something to evaluate.
            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            // Do the calculation using tight loops.
            // Add-up the initial period, except for the last value.
            double equalPeriodTotal = 0.0;
            int equalTrailingIdx = startIdx - GetCandleAvgPeriod(Equal);
            double bodyLongPeriodTotal = 0.0;
            int bodyLongTrailingIdx = startIdx - GetCandleAvgPeriod(BodyLong);
            
            int i = equalTrailingIdx;
            while (i < startIdx)
            {
                equalPeriodTotal += GetCandleRange(Equal, i - 1, inOpen, inHigh, inLow, inClose);
                i++;
            }

            i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal += GetCandleRange(BodyLong, i - 1, inOpen, inHigh, inLow, inClose);
                i++;
            }

            i = startIdx;

            /* Proceed with the calculation for the requested range.
             * Must have:
             * - first candle: long black candle
             * - second candle: white candle with open below previous day low and close into previous day body under the midpoint;
             * to differentiate it from in-neck the close should not be equal to the black candle's close
             * The meaning of "equal" is specified with TA_SetCandleSettings
             * outInteger is negative (-1 to -100): thrusting pattern is always bearish
             * the user should consider that the thrusting pattern is significant when it appears in a downtrend and it could be 
             * even bullish "when coming in an uptrend or occurring twice within several days" (Steve Nison says), while this 
             * function does not consider the trend
             */
            int outIdx = 0;
            do
            {
                bool isThrusting =
                    // 1st: black
                    GetCandleColor(i - 1, inOpen, inClose) == -1 &&
                    // long
                    GetRealBody(i - 1, inOpen, inClose) >
                    GetCandleAverage(BodyLong, bodyLongPeriodTotal, i - 1, inOpen, inHigh, inLow, inClose) &&
                    // 2nd: white
                    GetCandleColor(i, inOpen, inClose) == 1 &&
                    // open below prior low
                    inOpen[i] < inLow[i - 1] &&
                    // close into prior body
                    inClose[i] > inClose[i - 1] +
                    GetCandleAverage(Equal, equalPeriodTotal, i - 1, inOpen, inHigh, inLow, inClose) &&
                    // under the midpoint
                    inClose[i] <= inClose[i - 1] + GetRealBody(i - 1, inOpen, inClose) * 0.5;

                outInteger[outIdx++] = isThrusting ? -100 : 0;

                /* add the current range and subtract the first range: this is done after the pattern recognition 
                 * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
                 */
                equalPeriodTotal +=
                    GetCandleRange(Equal, i - 1, inOpen, inHigh, inLow, inClose) -
                    GetCandleRange(Equal, equalTrailingIdx - 1, inOpen, inHigh, inLow, inClose);

                bodyLongPeriodTotal +=
                    GetCandleRange(BodyLong, i - 1, inOpen, inHigh, inLow, inClose) -
                    GetCandleRange(BodyLong, bodyLongTrailingIdx - 1, inOpen, inHigh, inLow, inClose);

                i++;
                equalTrailingIdx++;
                bodyLongTrailingIdx++;
            } while (i <= endIdx);

            // All done. Indicate the output limits and return.
            outNBElement = outIdx;
            outBegIdx = startIdx;
            
            return RetCode.Success;
        }

        public static int CdlThrustingLookback()
        {
            return Math.Max(GetCandleAvgPeriod(Equal), GetCandleAvgPeriod(BodyLong)) + 1;
        }
    }
}
