using System;

namespace TechnicalAnalysis
{
    internal static partial class TACore
    {
        public static RetCode CdlThrusting(
            int startIdx,
            int endIdx,
            double[] inOpen,
            double[] inHigh,
            double[] inLow,
            double[] inClose,
            ref int outBegIdx,
            ref int outNBElement,
            int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlThrustingLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            double equalPeriodTotal = 0.0;
            int equalTrailingIdx = startIdx - Globals.candleSettings[10].avgPeriod;
            double bodyLongPeriodTotal = 0.0;
            int bodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = equalTrailingIdx;
            while (true)
            {
                double num44;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num43;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num40;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            double num41;
                            double num42;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num42 = inClose[i - 1];
                            }
                            else
                            {
                                num42 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num41 = inOpen[i - 1];
                            }
                            else
                            {
                                num41 = inClose[i - 1];
                            }

                            num40 = inHigh[i - 1] - num42 + (num41 - inLow[i - 1]);
                        }
                        else
                        {
                            num40 = 0.0;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                equalPeriodTotal += num44;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                double num39;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num39 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num38;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num38 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num35;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num36;
                            double num37;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num37 = inClose[i - 1];
                            }
                            else
                            {
                                num37 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num36 = inOpen[i - 1];
                            }
                            else
                            {
                                num36 = inClose[i - 1];
                            }

                            num35 = inHigh[i - 1] - num37 + (num36 - inLow[i - 1]);
                        }
                        else
                        {
                            num35 = 0.0;
                        }

                        num38 = num35;
                    }

                    num39 = num38;
                }

                bodyLongPeriodTotal += num39;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0272:
            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) == -1)
            {
                double num28;
                double num34;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num34 = bodyLongPeriodTotal / Globals.candleSettings[0].avgPeriod;
                }
                else
                {
                    double num33;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num33 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num32;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num32 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num29;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                double num30;
                                double num31;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num31 = inClose[i - 1];
                                }
                                else
                                {
                                    num31 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num30 = inOpen[i - 1];
                                }
                                else
                                {
                                    num30 = inClose[i - 1];
                                }

                                num29 = inHigh[i - 1] - num31 + (num30 - inLow[i - 1]);
                            }
                            else
                            {
                                num29 = 0.0;
                            }

                            num32 = num29;
                        }

                        num33 = num32;
                    }

                    num34 = num33;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num28 = 2.0;
                }
                else
                {
                    num28 = 1.0;
                }

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1])
                    > Globals.candleSettings[0].factor * num34 / num28 && inClose[i] >= inOpen[i]
                                                                       && inOpen[i] < inLow[i - 1])
                {
                    double num21;
                    double num27;
                    if (Globals.candleSettings[10].avgPeriod != 0.0)
                    {
                        num27 = equalPeriodTotal / Globals.candleSettings[10].avgPeriod;
                    }
                    else
                    {
                        double num26;
                        if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                        {
                            num26 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                        }
                        else
                        {
                            double num25;
                            if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                            {
                                num25 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num22;
                                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                {
                                    double num23;
                                    double num24;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num24 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num24 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num23 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num23 = inClose[i - 1];
                                    }

                                    num22 = inHigh[i - 1] - num24 + (num23 - inLow[i - 1]);
                                }
                                else
                                {
                                    num22 = 0.0;
                                }

                                num25 = num22;
                            }

                            num26 = num25;
                        }

                        num27 = num26;
                    }

                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        num21 = 2.0;
                    }
                    else
                    {
                        num21 = 1.0;
                    }

                    if (inClose[i] > inClose[i - 1] + Globals.candleSettings[10].factor * num27 / num21
                        && inClose[i] <= inClose[i - 1]
                        + Math.Abs(inClose[i - 1] - inOpen[i - 1]) * 0.5)
                    {
                        outInteger[outIdx] = -100;
                        outIdx++;
                        goto Label_05A7;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_05A7:
            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num19;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num18 = inClose[i - 1];
                        }
                        else
                        {
                            num18 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num17 = inOpen[i - 1];
                        }
                        else
                        {
                            num17 = inClose[i - 1];
                        }

                        num16 = inHigh[i - 1] - num18 + (num17 - inLow[i - 1]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[equalTrailingIdx - 1] - inOpen[equalTrailingIdx - 1]);
            }
            else
            {
                double num14;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[equalTrailingIdx - 1] - inLow[equalTrailingIdx - 1];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num13 = inClose[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[equalTrailingIdx - 1];
                        }

                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num12 = inOpen[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[equalTrailingIdx - 1];
                        }

                        num11 = inHigh[equalTrailingIdx - 1] - num13 + (num12 - inLow[equalTrailingIdx - 1]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            equalPeriodTotal += num20 - num15;
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num9;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num8 = inClose[i - 1];
                        }
                        else
                        {
                            num8 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num7 = inOpen[i - 1];
                        }
                        else
                        {
                            num7 = inClose[i - 1];
                        }

                        num6 = inHigh[i - 1] - num8 + (num7 - inLow[i - 1]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyLongTrailingIdx - 1] - inOpen[bodyLongTrailingIdx - 1]);
            }
            else
            {
                double num4;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyLongTrailingIdx - 1] - inLow[bodyLongTrailingIdx - 1];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[bodyLongTrailingIdx - 1] >= inOpen[bodyLongTrailingIdx - 1])
                        {
                            num3 = inClose[bodyLongTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[bodyLongTrailingIdx - 1];
                        }

                        if (inClose[bodyLongTrailingIdx - 1] >= inOpen[bodyLongTrailingIdx - 1])
                        {
                            num2 = inOpen[bodyLongTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[bodyLongTrailingIdx - 1];
                        }

                        num = inHigh[bodyLongTrailingIdx - 1] - num3 + (num2 - inLow[bodyLongTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyLongPeriodTotal += num10 - num5;
            i++;
            equalTrailingIdx++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0272;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlThrustingLookback()
        {
            return (Globals.candleSettings[10].avgPeriod <= Globals.candleSettings[0].avgPeriod
                         ? Globals.candleSettings[0].avgPeriod
                         : Globals.candleSettings[10].avgPeriod) + 1;
        }
    }
}
