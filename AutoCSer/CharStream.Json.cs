﻿using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 内存字符流
    /// </summary>
    //public sealed unsafe partial class CharStream
    {
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        public void WriteJson(byte value, bool isNumberToHex = true)
        {
            if (isNumberToHex)
            {
                byte* chars = (byte*)GetPrepCharSizeCurrent(4);
                *(int*)chars = '0' + ('x' << 16);
                *(char*)(chars + sizeof(char) * 2) = (char)AutoCSer.Extension.Number.ToHex((uint)value >> 4);
                *(char*)(chars + sizeof(char) * 3) = (char)AutoCSer.Extension.Number.ToHex((uint)value & 15);
                ByteSize += 4 * sizeof(char);
            }
            else AutoCSer.Extension.Number.ToString(value, this);
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        public void WriteJson(sbyte value, bool isNumberToHex = true)
        {
            if (isNumberToHex)
            {
                if (value < 0)
                {
                    char* chars = GetPrepCharSizeCurrent(5);
                    uint value32 = (uint)-(int)value;
                    *(int*)chars = '-' + ('0' << 16);
                    *(chars + 2) = 'x';
                    *(chars + 3) = (char)((value32 >> 4) + '0');
                    *(chars + 4) = (char)AutoCSer.Extension.Number.ToHex(value32 & 15);
                    ByteSize += 5 * sizeof(char);
                }
                else
                {
                    char* chars = GetPrepCharSizeCurrent(4);
                    uint value32 = (uint)(int)value;
                    *(int*)chars = '0' + ('x' << 16);
                    *(chars + 2) = (char)((value32 >> 4) + '0');
                    *(chars + 3) = (char)AutoCSer.Extension.Number.ToHex(value32 & 15);
                    ByteSize += 4 * sizeof(char);
                }
            }
            else AutoCSer.Extension.Number.ToString(value, this);
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        public void WriteJson(short value, bool isNumberToHex = true)
        {
            if (value >= 0) WriteJson((ushort)value, isNumberToHex);
            else
            {
                PrepCharSize(7);
                UnsafeWrite('-');
                WriteJson((ushort)-value, isNumberToHex);
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        public void WriteJson(ushort value, bool isNumberToHex = true)
        {
            if (isNumberToHex && value >= 10000)
            {
                char* chars;
                //if (value < 10000)
                //{
                //    if (value < 10)
                //    {
                //        Write((char)(value + '0'));
                //        return;
                //    }
                //    int div10 = (value * (int)AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                //    if (div10 < 10)
                //    {
                //        *(chars = GetPrepSizeCurrent(2)) = (char)(div10 + '0');
                //        *(chars + 1) = (char)((value - div10 * 10) + '0');
                //        ByteSize += 2 * sizeof(char);
                //        return;
                //    }
                //    int div100 = (div10 * (int)AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                //    if (div100 < 10)
                //    {
                //        *(chars = GetPrepSizeCurrent(3)) = (char)(div100 + '0');
                //        *(chars + 1) = (char)((div10 - div100 * 10) + '0');
                //        *(chars + 2) = (char)((value - div10 * 10) + '0');
                //        ByteSize += 3 * sizeof(char);
                //        return;
                //    }
                //    int div1000 = (div100 * (int)AutoCSer.Extension.Number.Div10_16Mul) >> AutoCSer.Extension.Number.Div10_16Shift;
                //    *(chars = GetPrepSizeCurrent(4)) = (char)(div1000 + '0');
                //    *(chars + 1) = (char)((div100 - div1000 * 10) + '0');
                //    *(chars + 2) = (char)((div10 - div100 * 10) + '0');
                //    *(chars + 3) = (char)((value - div10 * 10) + '0');
                //    ByteSize += 4 * sizeof(char);
                //    return;
                //}
                *(int*)(chars = GetPrepCharSizeCurrent(6)) = '0' + ('x' << 16);
                AutoCSer.Extension.Number.ToHex4(value, chars + 2);
                ByteSize += 6 * sizeof(char);
            }
            else AutoCSer.Extension.Number.ToString(value, this);
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        public void WriteJson(int value, bool isNumberToHex = true)
        {
            if (value >= 0) WriteJson((uint)value, isNumberToHex);
            else
            {
                PrepCharSize(11);
                UnsafeWrite('-');
                WriteJson((uint)-value, isNumberToHex);
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        public void WriteJson(uint value, bool isNumberToHex = true)
        {
            if (value <= ushort.MaxValue) WriteJson((ushort)value, isNumberToHex);
            else if (isNumberToHex)
            {
                char* chars = GetPrepCharSizeCurrent(10);
                *(int*)chars = '0' + ('x' << 16);
                char* next = AutoCSer.Extension.Number.GetToHex(value >> 16, chars + 2);
                AutoCSer.Extension.Number.ToHex4(value & 0xffff, next);
                ByteSize += ((int)(next - chars) + 4) * sizeof(char);
            }
            else AutoCSer.Extension.Number.ToString(value, this);
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="isMaxToString">超出最大有效精度是否转换成字符串</param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        public void WriteJson(long value, bool isNumberToHex = true, bool isMaxToString = true)
        {
            if ((ulong)(value + AutoCSer.Json.Serializer.MaxInt) <= (ulong)(AutoCSer.Json.Serializer.MaxInt << 1) || !isMaxToString) writeJson(value, isNumberToHex);
            else
            {
                PrepCharSize(24 + 2);
                UnsafeWrite('"');
                AutoCSer.Extension.Number.UnsafeToString(value, this);
                UnsafeWrite('"');
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        private void writeJson(long value, bool isNumberToHex)
        {
            if (value >= 0) writeJson((ulong)value, isNumberToHex);
            else
            {
                PrepCharSize(19);
                UnsafeWrite('-');
                writeJson((ulong)-value, isNumberToHex);
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="isMaxToString">超出最大有效精度是否转换成字符串</param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        public void WriteJson(ulong value, bool isNumberToHex = true, bool isMaxToString = true)
        {
            if (value <= AutoCSer.Json.Serializer.MaxInt || !isMaxToString) writeJson(value, isNumberToHex);
            else
            {
                PrepCharSize(22 + 2);
                UnsafeWrite('"');
                AutoCSer.Extension.Number.UnsafeToString(value, this);
                UnsafeWrite('"');
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        private void writeJson(ulong value, bool isNumberToHex)
        {
            if (value <= uint.MaxValue) WriteJson((uint)value, isNumberToHex);
            else if (isNumberToHex)
            {
                char* chars = GetPrepCharSizeCurrent(18), next;
                uint value32 = (uint)(value >> 32);
                *(int*)chars = '0' + ('x' << 16);
                if (value32 >= 0x10000)
                {
                    next = AutoCSer.Extension.Number.GetToHex(value32 >> 16, chars + 2);
                    AutoCSer.Extension.Number.ToHex4(value32 & 0xffff, next);
                    next += 4;
                }
                else next = AutoCSer.Extension.Number.GetToHex(value32, chars + 2);
                AutoCSer.Extension.Number.ToHex4((value32 = (uint)value) >> 16, next);
                AutoCSer.Extension.Number.ToHex4(value32 & 0xffff, next + 4);
                ByteSize += ((int)(next - chars) + 8) * sizeof(char);
            }
            else AutoCSer.Extension.Number.ToString(value, this);
        }
        /// <summary>
        /// 输出 double 值
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteJson(float value)
        {
            if (!float.IsNaN(value) && !float.IsInfinity(value)) SimpleWriteNotNull(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            else WriteJsonNaN();
        }
        /// <summary>
        /// 输出 double 值
        /// </summary>
        /// <param name="value"></param>
        public void WriteJson(double value)
        {
            if (!double.IsNaN(value) && !double.IsInfinity(value))
            {
                if (value <= 1.7976931348623150E+308)
                {
                    if (value >= -1.7976931348623150E+308) SimpleWriteNotNull(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    else writeDoubleMinValue(value);
                }
                else writeDoubleMaxValue(value);
            }
            else WriteJsonNaN();
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="nullChar">空字符替换</param>
        public void WriteJson(char value, char nullChar = ' ')
        {
            if (((AutoCSer.Json.Parser.ParseBits.Byte[(byte)value] & AutoCSer.Json.Parser.EscapeBit) | (value >> 8)) == 0)
            {
                byte* data = (byte*)GetPrepCharSizeCurrent(4);
                *(char*)data = '"';
                switch ((value ^ (value >> 3)) & 7)
                {
                    case ('\b' ^ ('\b' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('b' << 16); break;
                    case ('\t' ^ ('\t' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('t' << 16); break;
                    case ('\n' ^ ('\n' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('n' << 16); break;
                    case ('\f' ^ ('\f' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('f' << 16); break;
                    case ('\r' ^ ('\r' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('r' << 16); break;
                    case ('\\' ^ ('\\' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('\\' << 16); break;
                    case ('\"' ^ ('\"' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('"' << 16); break;
                }
                *(char*)(data + sizeof(char) * 3) = '"';
                ByteSize += 4 * sizeof(char);
            }
            else
            {
                byte* data = (byte*)GetPrepCharSizeCurrent(3);
                *(char*)data = '"';
                *(char*)(data + sizeof(char)) = value == 0 ? nullChar : value;
                *(char*)(data + sizeof(char) * 2) = '"';
                ByteSize += 3 * sizeof(char);
            }
        }
        /// <summary>
        /// 写入 JSON 字符串
        /// </summary>
        /// <param name="stringStart">起始位置</param>
        /// <param name="stringLength">字符串长度</param>
        /// <param name="nullChar">空字符替换</param>
        internal void WriteJson(char* stringStart, int stringLength, char nullChar)
        {
            if (stringLength == 0)
            {
                char* data = GetPrepCharSizeCurrent(2);
                *(int*)data = '"' + ('"' << 16);
                ByteSize += 2 * sizeof(char);
                return;
            }
            char* start = stringStart, end = stringStart + stringLength;
            byte* bits = AutoCSer.Json.Parser.ParseBits.Byte;
            int length = 0;
            do
            {
                if (((bits[*(byte*)start] & AutoCSer.Json.Parser.EscapeBit) | *(((byte*)start) + 1)) == 0) ++length;
            }
            while (++start != end);
            if (length == 0)
            {
                if (nullChar == 0)
                {
                    char* write = GetPrepCharSizeCurrent(stringLength + 1 + 3);
                    *write = '"';
                    AutoCSer.Extension.StringExtension.SimpleCopyNotNull64(stringStart, ++write, stringLength);
                    *(write + stringLength) = '"';
                }
                else
                {
                    char* write = GetPrepCharSizeCurrent(stringLength + 2);
                    *write = '"';
                    start = stringStart;
                    do
                    {
                        *++write = *start == 0 ? nullChar : *start;
                    }
                    while (++start != end);
                    *++write = '"';
                }
                ByteSize += (stringLength + 2) << 1;
            }
            else
            {
                char* write = GetPrepCharSizeCurrent(length += stringLength + 2);
                *write++ = '"';
                start = stringStart;
                do
                {
                    if (((bits[*(byte*)start] & AutoCSer.Json.Parser.EscapeBit) | *(((byte*)start) + 1)) == 0)
                    {
                        switch ((*start ^ (*start >> 3)) & 7)
                        {
                            case ('\b' ^ ('\b' >> 3)) & 7: *(int*)write = '\\' + ('b' << 16); break;
                            case ('\t' ^ ('\t' >> 3)) & 7: *(int*)write = '\\' + ('t' << 16); break;
                            case ('\n' ^ ('\n' >> 3)) & 7: *(int*)write = '\\' + ('n' << 16); break;
                            case ('\f' ^ ('\f' >> 3)) & 7: *(int*)write = '\\' + ('f' << 16); break;
                            case ('\r' ^ ('\r' >> 3)) & 7: *(int*)write = '\\' + ('r' << 16); break;
                            case ('\\' ^ ('\\' >> 3)) & 7: *(int*)write = '\\' + ('\\' << 16); break;
                            case ('\"' ^ ('\"' >> 3)) & 7: *(int*)write = '\\' + ('"' << 16); break;
                        }
                        write += 2;
                    }
                    else *write++ = *start == 0 ? nullChar : *start;
                }
                while (++start != end);
                *write = '"';
                ByteSize += length << 1;
            }
        }
        /// <summary>
        /// 时间转字符串 第三方格式 /Date(xxx)/
        /// </summary>
        /// <param name="time">时间</param>
        internal void WriteJsonOther(DateTime time)
        {
            byte* data = (byte*)GetPrepCharSizeCurrent(7 + 19 + 4);
            *(long*)data = '"' + ('/' << 16) + ((long)'D' << 32) + ((long)'a' << 48);
            *(long*)(data + sizeof(long)) = 't' + ('e' << 16) + ((long)'(' << 32);
            ByteSize += 7 * sizeof(char);
            writeJson((long)(((time.Kind == DateTimeKind.Utc ? time.Ticks + Date.LocalTimeTicks : time.Ticks) - AutoCSer.Json.Parser.JavascriptLocalMinTimeTicks) / TimeSpan.TicksPerMillisecond), false);
            *(long*)CurrentChar = ')' + ('/' << 16) + ((long)'"' << 32);
            ByteSize += 3 * sizeof(char);
        }
        /// <summary>
        /// 时间转字符串
        /// </summary>
        /// <param name="time">时间</param>
        internal void WriteJsonString(DateTime time)
        {
            switch(time.Kind)
            {
                case DateTimeKind.Utc:
                    char* utcFixed = GetPrepCharSizeCurrent(19 + 8 + 3);
                    *utcFixed = '"';
                    int utcSize = AutoCSer.Date.ToString(time, utcFixed + 1);
                    *(int*)(utcFixed + (utcSize + 1)) = 'Z' + ('"' << 16);
                    ByteSize += (utcSize + 3) << 1;
                    return;
                case DateTimeKind.Local:
                    char* localFixed = GetPrepCharSizeCurrent(20 + 8 + 8);
                    *localFixed = '"';
                    int localSize = AutoCSer.Date.ToString(time, localFixed + 1);
                    *(long*)(localFixed + (localSize + 1)) = Date.ZoneHourString;
                    *(long*)(localFixed + (localSize + 5)) = Date.ZoneMinuteString;
                    ByteSize += (localSize + 6 + 2) << 1;
                    return;
                default:
                    char* timeFixed = GetPrepCharSizeCurrent(19 + 8 + 2);
                    *timeFixed = '"';
                    int size = AutoCSer.Date.ToString(time, timeFixed + 1);
                    *(timeFixed + (size + 1)) = '"';
                    ByteSize += (size + 2) << 1;
                    return;
            }
        }
        /// <summary>
        /// 时间转字符串
        /// </summary>
        /// <param name="time">时间</param>
        internal void WriteJsonSqlString(DateTime time)
        {
            PrepCharSize(AutoCSer.Date.MillisecondStringSize + 2);
            UnsafeWrite('"');
            AutoCSer.Date.ToMillisecondString(time, this);
            UnsafeWrite('"');
        }
        /// <summary>
        /// 时间转字符串
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="isNumberToHex">数字是否允许转换为 16 进制字符串</param>
        public void WriteJson(DateTime time, bool isNumberToHex = true)
        {
            byte* data = (byte*)GetPrepCharSizeCurrent(9 + 19 + 1);
            *(long*)data = 'n' + ('e' << 16) + ((long)'w' << 32) + ((long)' ' << 48);
            *(long*)(data + sizeof(long)) = 'D' + ('a' << 16) + ((long)'t' << 32) + ((long)'e' << 48);
            *(char*)(data + sizeof(long) * 2) = '(';
            ByteSize += 9 * sizeof(char);
            writeJson((long)(((time.Kind == DateTimeKind.Utc ? time.Ticks + Date.LocalTimeTicks : time.Ticks) - AutoCSer.Json.Parser.JavascriptLocalMinTimeTicks) / TimeSpan.TicksPerMillisecond), isNumberToHex);
            UnsafeWrite(')');
        }
        /// <summary>
        /// Guid转换成字符串
        /// </summary>
        /// <param name="value">Guid</param>
        public void WriteJson(ref System.Guid value)
        {
            byte* data = (byte*)GetPrepCharSizeCurrent(38);
            *(char*)data = '"';
            new GuidCreator { Value = value }.ToString((char*)(data + sizeof(char)));
            *(char*)(data + sizeof(char) * 37) = '"';
            ByteSize += 38 * sizeof(char);
        }
    }
}
