﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rsi.cs" company="GLPM">
//   Copyright (c) GLPM. All rights reserved.
// </copyright>
// <summary>
//   Defines Rsi.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TechnicalAnalysis
{
    public partial class TAMath
    {
        public static Rsi Rsi(int startIdx, int endIdx, double[] real, int timePeriod = 14)
        {
            int outBegIdx = default;
            int outNBElement = default;
            double[] outReal = new double[endIdx - startIdx + 1];

            RetCode retCode = TACore.Rsi(startIdx, endIdx, real, timePeriod, ref outBegIdx, ref outNBElement, outReal);
            return new Rsi(retCode, outBegIdx, outNBElement, outReal);
        }

        public static Rsi Rsi(int startIdx, int endIdx, float[] real, int timePeriod = 14)
        {
            int outBegIdx = default;
            int outNBElement = default;
            double[] outReal = new double[endIdx - startIdx + 1];

            RetCode retCode = TACore.Rsi(startIdx, endIdx, real, timePeriod, ref outBegIdx, ref outNBElement, outReal);
            return new Rsi(retCode, outBegIdx, outNBElement, outReal);
        }
    }

    public class Rsi : IndicatorBase
    {
        public Rsi(RetCode retCode, int begIdx, int nbElement, double[] real)
            : base(retCode, begIdx, nbElement)
        {
            this.Real = real;
        }

        public double[] Real { get; }
    }
}
