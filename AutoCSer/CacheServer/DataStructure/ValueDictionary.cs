﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="nodeType">数据节点类型</typeparam>
    public sealed class ValueDictionary<keyType, nodeType> : Abstract.ValueDictionary<keyType, nodeType>
        where keyType : IEquatable<keyType>
        where nodeType : Abstract.Node, Abstract.IValue
    {
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private ValueDictionary(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.ValueDictionary);
            stream.Write((byte)ValueData.Data<keyType>.DataType);
            serializeParentDataStructure(stream);
        }

#if NOJIT
        /// <summary>
        /// 创建字典节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static ValueDictionary<keyType, nodeType> create(Abstract.Node parent)
        {
            return new ValueDictionary<keyType, nodeType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, ValueDictionary<keyType, nodeType>> constructor;
        static ValueDictionary()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, ValueDictionary<keyType, nodeType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, ValueDictionary<keyType, nodeType>>), typeof(ValueDictionary<keyType, nodeType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, ValueDictionary<keyType, nodeType>>)AutoCSer.Emit.Constructor.Create(typeof(ValueDictionary<keyType, nodeType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
