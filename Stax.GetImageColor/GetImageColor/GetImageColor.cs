using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace Stax.GetAverageImageColor.GetAverageImageColor;

public class GetImageColor
{
    private const string DefaultColor = "#000000FF";
    private static readonly HttpClient _httpClient;
    
    static GetImageColor() 
    {
        _httpClient = new HttpClient();
    }
    
    /// <summary>
    /// Method which allows you to pass through a URL, which will then be fetched and the average color of that image
    /// will be returned. If for any reason the image cannot be loaded or the color cannot be calculated #000000 by
    /// default will return, but you can override this with defaultColor.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="defaultColor">Defaults to #000000FF, but you can optionally override this if you wish for a
    /// different color to return if for any reason the color of the provided image cannot be calculated.</param>
    /// <returns></returns>
    public static async Task<string> AverageFromUrl(string url, string defaultColor = DefaultColor)
    {
        try
        {
            var imageBytes = await RetrieveImage(url);

            var hexColor = AverageFromBytes(imageBytes);

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
    /// then #000000FF by default will be returned, which can be overridden through defaultColor.
    /// </summary>
    /// <param name="imageBytes"></param>
    /// <param name="defaultColor"></param>
    /// <returns></returns>
    public static string AverageFromBytes(byte[] imageBytes, string defaultColor = DefaultColor)
    {
        try
        {
            using var image = Image.Load<Rgba32>(imageBytes);
            
            image.Mutate(
                x => x
                    .Resize(new ResizeOptions
                    {
                        Sampler = KnownResamplers.NearestNeighbor,
                        Size = new Size(100, 0)
                    })
                    .Quantize(new OctreeQuantizer(
                        new QuantizerOptions
                        {
                                Dither = null,
                                MaxColors = 1
                            }
                        )
                    )
                );

            var average = image[0, 0];
            
            return "#" + average.ToHex();
        }
        catch (Exception)
        {
            return defaultColor;
        }
    }

    private static async Task<byte[]> RetrieveImage(string url)
    {
        using var response = await _httpClient.GetAsync(url);
        return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
    }
}