using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Stax.GetAverageImageColor.GetAverageImageColor;

public class GetAverageImageColor
{
    private const string DefaultColor = "#00000000";
    private static readonly HttpClient _httpClient;
    
    static GetAverageImageColor() 
    {
        _httpClient = new HttpClient();
    }
    
    /// <summary>
    /// Method which allows you to pass through a URL, which will then be fetched and the average color of that image
    /// will be returned. If for any reason the image cannot be loaded or the color cannot be calculated #000000 by
    /// default will return, but you can override this with defaultColor.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="defaultColor">Defaults to #000000, but you can optionally override this if you wish for a
    /// different color to return if for any reason the color of the provided image cannot be calculated.</param>
    /// <returns></returns>
    public static async Task<string> FromUrl(string url, string defaultColor = DefaultColor)
    {
        try
        {
            using var response = await _httpClient.GetAsync(url);
            var imageBytes =
                await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            var hexColor = FromBytes(imageBytes);

            return hexColor;
        }
        catch (Exception)
        {
            return defaultColor;
        }
    }

    /// <summary>
    /// Method which allows you to pass through bytes which will then be transformed into an image, then the average
    /// color will be calculated and returned. If for any reason the color of the provided bytes cannot be calculated
    /// then #000000 by default will be returned, which can be overridden through defaultColor.
    /// </summary>
    /// <param name="imageBytes"></param>
    /// <param name="defaultColor"></param>
    /// <returns></returns>
    public static string FromBytes(byte[] imageBytes, string defaultColor = DefaultColor)
    {
        try
        {
            using var image = Image.Load<Rgba32>(imageBytes);
            image.Mutate(x => x
                .Resize(new ResizeOptions
                {
                    Sampler = KnownResamplers.NearestNeighbor,
                    Size = new Size(100, 0)
                }));

            var r = 0;
            var g = 0;
            var b = 0;
            var a = 0;
            var pixelCount = 0;

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var pixel = image[x, y];

                    r += Convert.ToInt32(pixel.R);
                    g += Convert.ToInt32(pixel.G);
                    b += Convert.ToInt32(pixel.B);
                    a += Convert.ToInt32(pixel.A);

                    pixelCount++;
                }
            }

            r /= pixelCount;
            g /= pixelCount;
            b /= pixelCount;
            a /= pixelCount;

            return "#" + new Rgba32((byte)r, (byte)g, (byte)b, (byte)a).ToHex();
        }
        catch (Exception)
        {
            return defaultColor;
        }
    }
}