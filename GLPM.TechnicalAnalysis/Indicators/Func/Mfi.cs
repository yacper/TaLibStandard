﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mfi.cs" company="GLPM">
//   Copyright (c) GLPM. All rights reserved.
// </copyright>
// <summary>
//   Defines Mfi.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GLPM.TechnicalAnalysis
{
    public partial class TAMath
    {
        public static Mfi Mfi(
            int startIdx,
            int endIdx,
            double[] high,
            double[] low,
            double[] close,
            double[] volume,
            int timePeriod = 14)
        {
            int outBegIdx = default;
            int outNBElement = default;
            double[] outReal = new double[endIdx - startIdx + 1];

            var retCode = TACore.Mfi(
                startIdx,
                endIdx,
                high,
                low,
                close,
                volume,
                timePeriod,
                ref outBegIdx,
                ref outNBElement,
                outReal);
            return new Mfi(retCode, outBegIdx, outNBElement, outReal);
        }

        public static Mfi Mfi(
            int startIdx,
            int endIdx,
            float[] high,
            float[] low,
            float[] close,
            float[] volume,
            int timePeriod = 14)
        {
            int outBegIdx = default;
            int outNBElement = default;
            double[] outReal = new double[endIdx - startIdx + 1];

            var retCode = TACore.Mfi(
                startIdx,
                endIdx,
                high,
                low,
                close,
                volume,
                timePeriod,
                ref outBegIdx,
                ref outNBElement,
                outReal);
            return new Mfi(retCode, outBegIdx, outNBElement, outReal);
        }
    }

    public class Mfi : IndicatorBase
    {
        public Mfi(RetCode retCode, int begIdx, int nbElement, double[] real)
            : base(retCode, begIdx, nbElement)
        {
            this.Real = real;
        }

        public double[] Real { get; }
    }
}