﻿using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Metadata.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct DictionaryGenericType3
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public Metadata.DictionaryGenericType3 Value;
    }
}
