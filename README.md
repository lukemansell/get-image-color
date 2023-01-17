# Get Average Image Color

## Summary
This NuGet package contains methods allowing you to get the average color of an image by either passing through the bytes of the image, or passing through the URL of an image.

The rgba hex value is then returned (eg: #ff000088).

This is a work in progress and I'm open to ideas on how to increase the speed of this and also how well it calculates the average color.

## Why I have written this
I needed to get the average color of images for [Musicstax](https://musicstax.com).

## How to use

Once installing this package, there are two methods you can use:

`GetImageColor.AverageFromUrl(string url, string defaultColor = DefaultColor)`

`GetImageColor.AverageFromBytes(byte[] imageBytes, string defaultColor = DefaultColor)`

An example might be:

`var averageColor = await GetImageColor.AverageFromUrl(url);`

which will return a string, eg: #ffffffff

### Exception handling

As you could pass through an invalid URL (or perhaps you don't have permission to view it), there is the possibility that an average color can not be calculated. In this case, by default `#000000FF` will be returned if the `FromUrl` method can not fetch the image. This is the color black.

You can override this default color by:

`var averageColor = await GetImageColor.AverageFromUrl(url, "#FFFFFFFF);`

## Expanding

I am keen to expand this to get the most dominant color from an image, which will come soon.