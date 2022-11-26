﻿// Copyright (c) 2022, LuiCat (as MaTech)
// 
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using UnityEngine;

namespace MaTech.Common.Utils {
    public static class MathUtil {
        /// <summary> 辗转相除法算最大公约数 </summary>
        public static int GCD(int x, int y) {
            x = Math.Abs(x);
            y = Math.Abs(y);
            int t = y;
            while (t != 0) {
                t = x % y;
                x = y;
                y = t;
            }
            return x;
        }

        public enum RoundingMode { Round, Floor, Ceiling }

        public static int RoundToInt(double value, RoundingMode mode = RoundingMode.Round) {
            switch (mode) {
            case RoundingMode.Round: return (int)Math.Round(value);
            case RoundingMode.Floor: return (int)Math.Floor(value);
            case RoundingMode.Ceiling: return (int)Math.Ceiling(value);
            }
            return 0;
        }

        public static int RoundToInt(float value, RoundingMode mode = RoundingMode.Round) {
            switch (mode) {
            case RoundingMode.Round: return (int)Math.Round(value);
            case RoundingMode.Floor: return (int)Math.Floor(value);
            case RoundingMode.Ceiling: return (int)Math.Ceiling(value);
            }
            return 0;
        }

        public static int Clamp(int value, int min, int max) => value < min ? min : value > max ? max : value;
        public static long Clamp(long value, long min, long max) => value < min ? min : value > max ? max : value;
        public static double Clamp(double value, double min, double max) => value < min ? min : value > max ? max : value;

        /// <summary>
        /// 两个无符号整数间的的有符号差距，返回数学意义上的(to-from)。（函数实现仅在参数不超有符号整数范围时有效）
        /// </summary>
        public static long OffsetBetween(ulong from, ulong to) {
            return (long)to - (long)from;
        }

        /// <summary>
        /// 两个无符号整数间的的有符号差距，返回数学意义上的(to-from)。（函数实现仅在参数不超有符号整数范围时有效）
        /// </summary>
        public static int OffsetBetween(uint from, uint to) {
            return (int)to - (int)from;
        }

        /// <summary> 用法同UnityEngine.Mathf.Lerp </summary>
        public static double Lerp(double a, double b, double k) => LerpUnclamped(a, b, Clamp(k, 0, 1));
        /// <summary> 用法同UnityEngine.Mathf.LerpUnclamped </summary>
        public static double LerpUnclamped(double a, double b, double k) => (1 - k) * a + k * b;
        /// <summary> 用法同UnityEngine.Mathf.InverseLerp </summary>
        public static double InverseLerp(double a, double b, double value) => Clamp(InverseLerpUnclamped(a, b, value), 0, 1);

        public static float InverseLerpUnclamped(float a, float b, float value) => (value - a) / (b - a);
        public static double InverseLerpUnclamped(double a, double b, double value) => (value - a) / (b - a);

        public static float LinearMap(float sourceA, float sourceB, float destA, float destB, float value) {
            return Mathf.Lerp(destA, destB, InverseLerpUnclamped(sourceA, sourceB, value));
        }
        public static double LinearMap(double sourceA, double sourceB, double destA, double destB, double value) {
            return Lerp(destA, destB, InverseLerpUnclamped(sourceA, sourceB, value));
        }

        /// <summary> 用法同cg的step函数 </summary>
        public static float Step(float a, float x) {
            return x >= a ? 1 : 0;
        }

        // https://web.archive.org/web/20100613230051/http://www.devmaster.net/forums/showthread.php?t=5784
        // x in [-pi, pi], max error 0.001
        // !! correctness untested !!
        private static float FastSinRaw(float x) {
            const float a = 0.225f;
            const float b = 4 / Mathf.PI;
            const float c = -4 / (Mathf.PI * Mathf.PI);
            float t = b * x + c * x * Mathf.Abs(x);
            return a * t * (Mathf.Abs(t) - 1) + t;
        }

        public static float FastSin(float x) => FastSinRaw(Mathf.Repeat(x + Mathf.PI, Mathf.PI) - Mathf.PI);
        public static float FastCos(float x) => FastSinRaw(Mathf.Repeat(x + Mathf.PI, Mathf.PI) - Mathf.PI / 2);
        public static float FastTan(float x) => FastSin(x) / FastCos(x);

        // https://www.dsprelated.com/showarticle/1052.php
        // z in [-1, 1], max error 0.005
        // !! correctness untested !!
        private static float FastAtanRaw(float z) {
            const float n1 = 0.97239411f;
            const float n2 = -0.19194795f;
            return (n1 + n2 * z * z) * z;
        }

        public static float FastAtan(float z) => Mathf.Abs(z) < 1 ? FastAtanRaw(z) : Mathf.PI / 2 - FastAtanRaw(1 / z);
        public static float FastAtan2(float y, float x) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (x == 0 && y == 0) return 0;
            // ReSharper restore CompareOfFloatsByEqualityOperator
            return Mathf.Abs(x) > Mathf.Abs(y) ? FastAtanRaw(y / x) + Step(x, 0) * Mathf.Sign(y) * Mathf.PI
                : Mathf.Sign(y) * Mathf.PI / 2 - FastAtanRaw(x / y);
        }

        public static float RatioFromSinDegrees(float angleNumer, float angleDenom) {
            return Mathf.Sin(Mathf.Deg2Rad * angleNumer) / Mathf.Sin(Mathf.Deg2Rad * angleDenom);
        }
        public static float RatioFromSinRadians(float angleNumer, float angleDenom) {
            return Mathf.Sin(angleNumer) / Mathf.Sin(angleDenom);
        }
        
        /// 将角度转换为>=lowerBound的最小等价角度（角度制）
        public static float LowerBoundAngle(float degrees, float lowerBound) {
            float delta = Mathf.DeltaAngle(lowerBound, degrees); // delta in [-180, 180]
            return lowerBound + delta + (delta < 0 ? 360 : 0);
        }
        
    }
}