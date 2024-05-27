// Copyright (c) 2023 Philippe Matray. All rights reserved.
// This file is part of TaLibStandard.
// TaLibStandard is licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for the full license text.
// For more information, visit https://github.com/phmatray/TaLibStandard.

namespace TechnicalAnalysis.Functions.UnitTests.Func;

public class SmaTests
{
    [Fact]
    public void SmaDouble()
    {
        {
            // Arrange
            const int StartIdx = 1;
            const int EndIdx   = 1;
            double[]  open     = new Double[] { 100, 125 };
            double[]  high     = new Double[] { 130, 128 };
            double[]  low      = new Double[] { 90, 100 };
            double[]  close    = new Double[] { 125, 110 };

            // Act
            SmaResult actualResult = TAMath.Sma(
                                                StartIdx,
                                                EndIdx,
                                                close, 2);
            // Assert
            actualResult.Should().NotBeNull();
            actualResult.RetCode.Should().Be(RetCode.Success);
        }



        {

        // Arrange
        Fixture fixture = new();
        const int StartIdx = 0;
        const int EndIdx = 99;
        double[] real = fixture.CreateMany<double>(100).ToArray();
            
        // Act
        SmaResult actualResult = TAMath.Sma(
            StartIdx,
            EndIdx,
            real);

        // Assert
        actualResult.Should().NotBeNull();
        actualResult.RetCode.Should().Be(RetCode.Success);

        }
    }
        
    [Fact]
    public void SmaFloat()
    {
        // Arrange
        Fixture fixture = new();
        const int StartIdx = 0;
        const int EndIdx = 99;
        float[] real = fixture.CreateMany<float>(100).ToArray();
            
        // Act
        SmaResult actualResult = TAMath.Sma(
            StartIdx,
            EndIdx,
            real);

        // Assert
        actualResult.Should().NotBeNull();
        actualResult.RetCode.Should().Be(RetCode.Success);
    }
}
