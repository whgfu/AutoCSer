﻿using System;
using System.Reflection;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// 输出参数泛型类型元数据
    /// </summary>
    internal abstract partial class ReturnParameterGenericType : AutoCSer.Metadata.GenericTypeBase
    {
        /// <summary>
        /// 获取异步回调
        /// </summary>
        internal abstract MethodInfo ClientGetCallbackMethod { get; }

        /// <summary>
        /// 异步回调
        /// </summary>
        internal abstract MethodInfo ServerSocketSenderGetCallbackMethod { get; }
        /// <summary>
        /// 异步回调
        /// </summary>
        internal abstract MethodInfo ServerSocketSenderGetCallbackEmitMethod { get; }
        /// <summary>
        /// 异步回调
        /// </summary>
        internal abstract MethodInfo ServerSocketSenderGetCallbackReturnMethod { get; }
    }
}
