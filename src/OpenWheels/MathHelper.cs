﻿using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OpenWheels
{
    public static class MathHelper
    {
        /// <summary>
        /// Two times <see cref="System.Math.PI"/>.
        /// </summary>
        public const float TwoPi = (float) (2 * System.Math.PI);

        /// <summary>
        /// <code>1 / <see cref="System.Math.PI"/></code>
        /// </summary>
        public const float InvPi = (float) (1 / System.Math.PI);

        /// <summary>
        /// <code>1f / <see cref="TwoPi"/></code>
        /// </summary>
        public const float InvTwoPi = 1f / TwoPi;

        /// <summary>
        ///   Convert the given angle in radians to degrees.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToDegrees(float r)
        {
             return r * 57.295779513082320876798154814105f;
        }

        /// <summary>
        ///   Convert the given angle in degrees to radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToRadians(float d)
        {
            return d * 0.017453292519943295769236907684886f;
        }

        /// <summary>
        ///   Clamp <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <returns>
        ///   <paramref name="min"/> if <code>value &lt; min</code>, max if <code>value &gt; max</code>,
        ///   <paramref name="value"/> otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : (value > max ? max : value);
        }

        /// <summary>
        ///   Clamp <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <returns>
        ///   <paramref name="min"/> if <code>value &lt; min</code>, max if <code>value &gt; max</code>,
        ///   <paramref name="value"/> otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max)
        {
            return value < min ? min : (value > max ? max : value);
        }

        /// <summary>
        ///   Clamp <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <returns>
        ///   <paramref name="min"/> if <code>value < min</code>, max if <code>value > max</code>,
        ///   <paramref name="value"/> otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) < 0 ? min : (value.CompareTo(max) > 0 ? max : min);
        }

        #region Mapping and Lerping

        /// <summary>
        ///   Linearly interpolate between a and b by t.
        ///   t is not clamped, so if it is not in the range [0, 1] the return
        ///   value will be outside the [a, b] range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        /// <summary>
        ///   Get the value for which linear interpolation between a and b would give v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InvLerp(float a, float b, float v)
        {
            return (v - a) / (b - a);
        }

        /// <summary>
        /// Performs component-wise linear interpolation.
        /// </summary>
        /// <seealso cref="Lerp(float,float,float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Lerp(in Vector2 a, in Vector2 b, in Vector2 v)
        {
            return new Vector2(Lerp(a.X, b.X, v.X), Lerp(a.Y, b.Y, v.Y));
        }

        /// <summary>
        /// Performs component-wise inverted linear interpolation.
        /// </summary>
        /// <seealso cref="Lerp(float,float,float)"/>
        /// <seealso cref="InvLerp(float,float,float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 InvLerp(in Vector2 a, in Vector2 b, in Vector2 v)
        {
            return new Vector2(InvLerp(a.X, b.X, v.X), InvLerp(a.Y, b.Y, v.Y));
        }

        /// <summary>
        /// Compute the so called 2D cross product. Computed as <c>a.X * b.Y - b.X * a.Y</c>.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        public static float Cross2D(in Vector2 a, in Vector2 b)
        {
            return a.X * b.Y - b.X * a.Y;
        }

        /// <summary>
        /// Linearly map a value from one range to another.
        /// Equivalent to <code>Lerp(aTo, bTo, InvLerp(aFrom, bFrom, value))</code>.
        /// <seealso cref="Lerp(float,float,float)"/>
        /// <seealso cref="InvLerp(float,float,float)"/>
        /// </summary>
        public static float LinearMap(float value, float aFrom, float bFrom, float aTo, float bTo)
        {
            return Lerp(aTo, bTo, InvLerp(aFrom, bFrom, value));
        }

        /// <summary>
        /// Performs component-wise linear mapping. <seealso cref="LinearMap(float,float,float,float,float)"/>
        /// </summary>
        public static Vector2 LinearMap(in Vector2 value, in RectangleF from, in RectangleF to)
        {
            var normalized = (value.X - from.Left) / from.Width;
            var normalized1 = (value.Y - from.Top) / from.Height;
            return new Vector2(
                normalized * to.Width + to.Left,
                normalized1 * to.Height + to.Top);
        }

        /// <summary>
        /// Performs component-wise linear mapping on all 4 points of the rectangle. <seealso cref="LinearMap(float,float,float,float,float)"/>
        /// </summary>
        public static RectangleF LinearMap(in RectangleF value, in RectangleF from, in RectangleF to)
        {
            return RectangleF.FromExtremes(
                LinearMap(value.TopLeft, from, to),
                LinearMap(value.BottomRight, from, to));
        }
        
        #endregion

    }
}