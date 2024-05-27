// Copyright (c) 2023 Philippe Matray. All rights reserved.
// This file is part of TaLibStandard.
// TaLibStandard is licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for the full license text.
// For more information, visit https://github.com/phmatray/TaLibStandard.

namespace TechnicalAnalysis.Candles.UnitTests.Cdl;

public class CdlHaramiTests : CdlTestsBase
{
    protected override Func<int, int, float[], float[], float[], float[], IndicatorResult> SUT { get; }
        = TACandle.CdlHarami;

    [Theory]
    [InlineData(typeof(float))]
    [InlineData(typeof(double))]
    [InlineData(typeof(decimal))]
    [InlineData(typeof(Half))]
    public void CdlHaramiFloatingPoint(Type floatingPointType)
    {
        InvokeGeneric(nameof(CdlHarami), floatingPointType);
    }
    
    private static void CdlHarami<T>()
        where T : IFloatingPoint<T>
    {

        {
            // 判断harami，至少需要11根前置k线
            // Arrange
            const int StartIdx = 11;
            const int EndIdx   = 11;
            // 10根前置k线，使用同样的开盘价、最高价、最低价、收盘价，方便测试
            // doji 的body长度小于10%的前10根k线的高低差的平均值
            double   o     = 100;
            double   h     = 110;
            double   l     = 90;
            double   c     = 110;
            double[] open  = new Double[] { o,o,o,o,o, o,o,o,o,o, 100, 110};
            double[] high  = new Double[] { h,h,h,h,h, h,h,h,h,h, 130,  110, };
            double[] low   = new Double[] { l,l,l,l,l, l,l,l,l,l, 90,  105, };
            double[] close = new Double[] { c,c,c,c,c, c,c,c,c,c, 111,  105, };

            // Act
            CandleIndicatorResult result = TACandle.CdlHarami(
                                                              StartIdx, EndIdx, open, high, low, close);

            // Assert
            result.Should().NotBeNull();
            result.RetCode.Should().Be(RetCode.Success);
            // 找到了doji
            result.Integers[0] = -100;
        }

        {
            // Arrange
            Fixture   fixture  = new();
            const int StartIdx = 0;
            const int EndIdx   = 99;
            T[]       open     = fixture.CreateMany<T>(100).ToArray();
            T[]       high     = fixture.CreateMany<T>(100).ToArray();
            T[]       low      = fixture.CreateMany<T>(100).ToArray();
            T[]       close    = fixture.CreateMany<T>(100).ToArray();

            // Act
            CandleIndicatorResult result = TACandle.CdlHarami(
                                                              StartIdx, EndIdx, open, high, low, close);

            // Assert
            result.Should().NotBeNull();
            result.RetCode.Should().Be(RetCode.Success);
        }


    }
}
