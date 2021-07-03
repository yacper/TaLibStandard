using System;
using static TechnicalAnalysis.TACore.CandleSettingType;

namespace TechnicalAnalysis
{
    internal static partial class TACore
    {
        public static RetCode CdlBeltHold(
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
            // Local variables
            double num5;
            double num10;
            double num15;
            double num20;
            int num21;
            double num28;
            double num44;
            
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
            int lookbackTotal = CdlBeltHoldLookback();

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
            double bodyLongPeriodTotal = 0.0;
            int bodyLongTrailingIdx = startIdx - GetCandleAvgPeriod(BodyLong);
            double shadowVeryShortPeriodTotal = 0.0;
            int shadowVeryShortTrailingIdx = startIdx - GetCandleAvgPeriod(ShadowVeryShort);
            
            int i = bodyLongTrailingIdx;
            while (i < startIdx)
            {
                bodyLongPeriodTotal += GetCandleRange(BodyLong, i, inOpen, inHigh, inLow, inClose);
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (i < startIdx)
            {
                shadowVeryShortPeriodTotal += GetCandleRange(ShadowVeryShort, i, inOpen, inHigh, inLow, inClose);
                i++;
            }

            /* Proceed with the calculation for the requested range.
             * Must have:
             * - long white (black) real body
             * - no or very short lower (upper) shadow
             * The meaning of "long" and "very short" is specified with TA_SetCandleSettings
             * outInteger is positive (1 to 100) when white (bullish), negative (-1 to -100) when black (bearish)
             */
            int outIdx = 0;
            Label_022E:
            if (GetCandleAvgPeriod(BodyLong) != 0.0)
            {
                num44 = bodyLongPeriodTotal / GetCandleAvgPeriod(BodyLong);
            }
            else
            {
                double num43;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num43 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num42;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num42 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num39;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num41 = inClose[i] >= inOpen[i] ? inClose[i] : inOpen[i];
                            double num40 = inClose[i] >= inOpen[i] ? inOpen[i] : inClose[i];
                            num39 = inHigh[i] - num41 + (num40 - inLow[i]);
                        }
                        else
                        {
                            num39 = 0.0;
                        }

                        num42 = num39;
                    }

                    num43 = num42;
                }

                num44 = num43;
            }

            double num38 = Globals.candleSettings[0].rangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i] - inOpen[i]) <= Globals.candleSettings[0].factor * num44 / num38)
            {
                goto Label_064A;
            }

            if (inClose[i] >= inOpen[i])
            {
                double num36;
                double num37 = inClose[i] >= inOpen[i] ? inOpen[i] : inClose[i];

                if (GetCandleAvgPeriod(ShadowVeryShort) != 0.0)
                {
                    num36 = shadowVeryShortPeriodTotal / GetCandleAvgPeriod(ShadowVeryShort);
                }
                else
                {
                    double num35;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        double num34;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num31;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                double num33 = inClose[i] >= inOpen[i] ? inClose[i] : inOpen[i];
                                double num32 = inClose[i] >= inOpen[i] ? inOpen[i] : inClose[i];
                                num31 = inHigh[i] - num33 + (num32 - inLow[i]);
                            }
                            else
                            {
                                num31 = 0.0;
                            }

                            num34 = num31;
                        }

                        num35 = num34;
                    }

                    num36 = num35;
                }

                double num30 = Globals.candleSettings[7].rangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (num37 - inLow[i] < Globals.candleSettings[7].factor * num36 / num30)
                {
                    goto Label_062A;
                }
            }

            if ((inClose[i] < inOpen[i] ? -1 : 1) != -1)
            {
                goto Label_064A;
            }

            double num29 = inClose[i] >= inOpen[i] ? inClose[i] : inOpen[i];

            if (GetCandleAvgPeriod(ShadowVeryShort) != 0.0)
            {
                num28 = shadowVeryShortPeriodTotal / GetCandleAvgPeriod(ShadowVeryShort);
            }
            else
            {
                double num27;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num27 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num26;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num26 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num23;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num25 = inClose[i] >= inOpen[i] ? inClose[i] : inOpen[i];
                            double num24 = inClose[i] >= inOpen[i] ? inOpen[i] : inClose[i];
                            num23 = inHigh[i] - num25 + (num24 - inLow[i]);
                        }
                        else
                        {
                            num23 = 0.0;
                        }

                        num26 = num23;
                    }

                    num27 = num26;
                }

                num28 = num27;
            }

            double num22 = Globals.candleSettings[7].rangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (inHigh[i] - num29 >= Globals.candleSettings[7].factor * num28 / num22)
            {
                goto Label_064A;
            }

            Label_062A:
            if (inClose[i] >= inOpen[i])
            {
                num21 = 1;
            }
            else
            {
                num21 = -1;
            }

            outInteger[outIdx] = num21 * 100;
            outIdx++;
            goto Label_0653;
            Label_064A:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_0653:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num19;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num18 = inClose[i] >= inOpen[i] ? inClose[i] : inOpen[i];
                        double num17 = inClose[i] >= inOpen[i] ? inOpen[i] : inClose[i];
                        num16 = inHigh[i] - num18 + (num17 - inLow[i]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[bodyLongTrailingIdx] - inOpen[bodyLongTrailingIdx]);
            }
            else
            {
                double num14;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[bodyLongTrailingIdx] - inLow[bodyLongTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num13 = inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx] ? inClose[bodyLongTrailingIdx] : inOpen[bodyLongTrailingIdx];
                        double num12 = inClose[bodyLongTrailingIdx] >= inOpen[bodyLongTrailingIdx] ? inOpen[bodyLongTrailingIdx] : inClose[bodyLongTrailingIdx];
                        num11 = inHigh[bodyLongTrailingIdx] - num13 + (num12 - inLow[bodyLongTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            bodyLongPeriodTotal += num20 - num15;
            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num9;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        double num8 = inClose[i] >= inOpen[i] ? inClose[i] : inOpen[i];
                        double num7 = inClose[i] >= inOpen[i] ? inOpen[i] : inClose[i];
                        num6 = inHigh[i] - num8 + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowVeryShortTrailingIdx] - inOpen[shadowVeryShortTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowVeryShortTrailingIdx] - inLow[shadowVeryShortTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        double num3 = inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx] ? inClose[shadowVeryShortTrailingIdx] : inOpen[shadowVeryShortTrailingIdx];
                        double num2 = inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx] ? inOpen[shadowVeryShortTrailingIdx] : inClose[shadowVeryShortTrailingIdx];
                        num = inHigh[shadowVeryShortTrailingIdx] - num3 + (num2 - inLow[shadowVeryShortTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowVeryShortPeriodTotal += num10 - num5;
            i++;
            bodyLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_022E;
            }

            // All done. Indicate the output limits and return.
            outNBElement = outIdx;
            outBegIdx = startIdx;
            
            return RetCode.Success;
        }

        public static int CdlBeltHoldLookback()
        {
            return GetCandleAvgPeriod(BodyLong) <= GetCandleAvgPeriod(ShadowVeryShort)
                        ? GetCandleAvgPeriod(ShadowVeryShort)
                        : GetCandleAvgPeriod(BodyLong);
        }
    }
}
