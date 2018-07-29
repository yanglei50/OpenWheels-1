﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using OpenWheels;
using OpenWheels.Fonts;
using OpenWheels.Fonts.ImageSharp;
using OpenWheels.Rendering;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace OpenWheels.Rendering.ImageSharp
{
    /// <summary>
    /// Extension methods for <see cref="Batcher" /> to load and register textures and fonts from a stream or path with a single method call.
    /// </summary>
    public static class BatcherExtensions
    {

#if !NETSTANDARD1_1

        /// <summary>
        /// Load an image from a path and register it in the batcher.
        /// </summary>
        /// <param name="batcher">The batcher to register the texture in.</param>
        /// <param name="path">Path to the image.</param>
        public static int LoadTexture(this Batcher batcher, string path)
        {
            using (var img = Image.Load<Rgba32>(path))
                return RegisterImage(batcher, img);
        }

        /// <summary>
        /// Load a font and register it in the batcher.
        /// </summary>
        /// <param name="path">Path to the font file.</param>
        /// <param name="size">Size to render the font at.</param>
        /// <param name="fallbackCharacter">Optional fallback character for the font. Defaults to <c>null</c> (no fallback).</param>
        public static TextureFont LoadFont(this Batcher batcher, string path, float size, int? fallbackCharacter = null)
        {
            FontAtlasHelpers.CreateFont(path, size, out var glyphMap, out var image);
            var texId = RegisterImage(batcher, image);
            image.Dispose();
            return new TextureFont(glyphMap, texId, fallbackCharacter);
        }

        /// <summary>
        /// Load a font and register it in the batcher.
        /// </summary>
        /// <param name="path">Path to the font file.</param>
        /// <param name="size">Size to render the font at.</param>
        /// <param name="ranges">The character ranges to render to the font atlas.</param>
        /// <param name="fallbackCharacter">Optional fallback character for the font. Defaults to <c>null</c> (no fallback).</param>
        public static TextureFont LoadFont(this Batcher batcher, string path, float size, IEnumerable<Range<int>> ranges, int? fallbackCharacter = null)
        {
            FontAtlasHelpers.CreateFont(path, size, ranges, out var glyphMap, out var image);
            var texId = RegisterImage(batcher, image);
            image.Dispose();
            return new TextureFont(glyphMap, texId, fallbackCharacter);
        }

        /// <summary>
        /// Load a system font and register it in the batcher.
        /// </summary>
        /// <param name="name">Name of the system font.</param>
        /// <param name="size">Size to render the font at.</param>
        /// <param name="fallbackCharacter">Optional fallback character for the font. Defaults to <c>null</c> (no fallback).</param>
        public static TextureFont LoadSystemFont(this Batcher batcher, string name, float size, int? fallbackCharacter = null)
        {
            FontAtlasHelpers.CreateSystemFont(name, size, out var glyphMap, out var image);
            var texId = RegisterImage(batcher, image);
            image.Dispose();
            return new TextureFont(glyphMap, texId, fallbackCharacter);
        }

        /// <summary>
        /// Load a system font and register it in the batcher.
        /// </summary>
        /// <param name="name">Name of the system font.</param>
        /// <param name="size">Size to render the font at.</param>
        /// <param name="ranges">The character ranges to render to the font atlas.</param>
        /// <param name="fallbackCharacter">Optional fallback character for the font. Defaults to <c>null</c> (no fallback).</param>
        public static TextureFont LoadSystemFont(this Batcher batcher, string name, float size, IEnumerable<Range<int>> ranges, int? fallbackCharacter = null)
        {
            FontAtlasHelpers.CreateSystemFont(name, size, ranges, out var glyphMap, out var image);
            var texId = RegisterImage(batcher, image);
            image.Dispose();
            return new TextureFont(glyphMap, texId, fallbackCharacter);
        }

#endif

        /// <summary>
        /// Load an image from a stream and register it in the batcher.
        /// </summary>
        /// <param name="batcher">The batcher to register the texture in.</param>
        /// <param name="imageStream">Stream of the image data.</param>
        public static int LoadTexture(this Batcher batcher, Stream imageStream)
        {
            using (var img = Image.Load<Rgba32>(imageStream))
                return RegisterImage(batcher, img);
        }

        /// <summary>
        /// Load a font from a stream and register it in the batcher. Includes Unicode latin characters.
        /// </summary>
        /// <param name="batcher">The batcher to register the texture in.</param>
        /// <param name="fontStream">Stream of the font data.</param>
        /// <param name="size">Size to render the font at.</param>
        /// <param name="fallbackCharacter">Optional fallback character for the font. Defaults to <c>null</c> (no fallback).</param>
        public static TextureFont LoadFont(this Batcher batcher, Stream fontStream, float size, int? fallbackCharacter = null)
        {
            FontAtlasHelpers.CreateFont(fontStream, size, out var glyphMap, out var image);
            var texId = RegisterImage(batcher, image);
            image.Dispose();
            return new TextureFont(glyphMap, texId, fallbackCharacter);
        }

        /// <summary>
        /// Load a font from a stream and register it in the batcher.
        /// </summary>
        /// <param name="batcher">The batcher to register the texture in.</param>
        /// <param name="fontStream">Stream of the font data.</param>
        /// <param name="size">Size to render the font at.</param>
        /// <param name="ranges">The character ranges to render to the font atlas.</param>
        /// <param name="fallbackCharacter">Optional fallback character for the font. Defaults to <c>null</c> (no fallback).</param>
        public static TextureFont LoadFont(this Batcher batcher, Stream fontStream, float size, IEnumerable<Range<int>> ranges, int? fallbackCharacter = null)
        {
            FontAtlasHelpers.CreateFont(fontStream, size, ranges, out var glyphMap, out var image);
            var texId = RegisterImage(batcher, image);
            image.Dispose();
            return new TextureFont(glyphMap, texId, fallbackCharacter);
        }

        private static int RegisterImage(Batcher batcher, Image<Rgba32> img)
        {
            var pixelSpan = MemoryMarshal.Cast<Rgba32, Color>(img.GetPixelSpan());
            return batcher.RegisterTexture(pixelSpan, img.Width, img.Height);
        }
    }
}
