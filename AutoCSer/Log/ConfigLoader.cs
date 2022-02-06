﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Log
{
    /// <summary>
    /// 配置加载
    /// </summary>
    //internal static class ConfigLoader
    {
        /// <summary>
        /// 获取配置数据
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <param name="name">配置名称</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static UnionType GetUnion(System.Type type, string name = "")
        {
            return new UnionType { Object = AutoCSer.Config.Loader.GetObject(type, name) };
        }
    }
}
