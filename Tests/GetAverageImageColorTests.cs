using Stax.GetAverageImageColor.GetAverageImageColor;

namespace Tests;

public class GetAverageImageColorTests
{
    [Theory]
    [InlineData("https://htmlcolorcodes.com/assets/images/colors/red-color-solid-background-1920x1080.png", "#FF0000FF")]
    [InlineData("https://htmlcolorcodes.com/assets/images/colors/green-color-solid-background-1920x1080.png", "#008000FF")]
    [InlineData("https://i.ibb.co/pyGzSBW/red-domination.png", "#CC3300FF")]
    public async void GetAverageColour_FromUrl__Returns__ExpectedColor(string url, string expected)
    {
        // Arrange
        
        // Act
        var averageColor = await GetImageColor.AverageFromUrl(url);
        
        // Assert
        Assert.Equal(expected, averageColor);
    }
}