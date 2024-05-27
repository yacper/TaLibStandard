// Copyright (c) 2023 Philippe Matray. All rights reserved.
// This file is part of TaLibStandard.
// TaLibStandard is licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for the full license text.
// For more information, visit https://github.com/phmatray/TaLibStandard.

using FluentAssertions;
using TechnicalAnalysis.Candles;
using TechnicalAnalysis.Common;

namespace MyTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        {
            // 测试孕线， 生成2根孕线形态，测试必须通过
            // Arrange
            const int StartIdx = 1;
            const int EndIdx   = 1;
            double[]  open     = new Double[] { 100, 125 };
            double[]  high     = new Double[] { 130, 128 };
            double[]  low      = new Double[] { 90, 100 };
            double[]  close    = new Double[] { 125, 110 };

            // Act
            TACandle.ha
            CandleIndicatorResult result = TACandle.CdlHarami(
                                                              StartIdx, EndIdx, open, high, low, close);

            // Assert
            result.Should().NotBeNull();
            result.RetCode.Should().Be(RetCode.Success);
        }

    }
}
