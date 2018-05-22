﻿using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache
{
    /// <summary>
    /// 32768 基分段 数组节点
    /// </summary>
    internal static class FragmentArray
    {
        /// <summary>
        /// 数组长度 2 进制位长度
        /// </summary>
        internal const int ArrayShift = 15;
        /// <summary>
        /// 数组长度
        /// </summary>
        internal const int ArraySize = 1 << ArrayShift;
        /// <summary>
        /// 数组位置 and 值
        /// </summary>
        internal const int ArraySizeAnd = ArraySize - 1;
    }
    /// <summary>
    /// 32768 基分段 数组节点
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    internal sealed class FragmentArray<nodeType> : Node where nodeType : Node
    {
        /// <summary>
        /// 数组
        /// </summary>
        private nodeType[][] arrays = NullValue<nodeType[]>.Array;
        /// <summary>
        /// 有效数据数量
        /// </summary>
        private int count;
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        private Node getNext(ref OperationParameter.NodeParser parser)
        {
            int index = parser.GetValueData(-1);
            if ((uint)index < count)
            {
                nodeType node = arrays[index >> FragmentArray.ArrayShift][index & FragmentArray.ArraySizeAnd];
                if (node != null) return node;
                parser.ReturnParameter.Type = ReturnType.NullArrayNode;
            }
            else parser.ReturnParameter.Type = ReturnType.ArrayIndexOutOfRange;
            return null;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            return getNext(ref parser);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetOrCreateNode: getOrCreateNode(ref parser); return;
                case OperationParameter.OperationType.Remove: remove(ref parser); return;
                case OperationParameter.OperationType.Clear:
                    if (arrays.Length != 0)
                    {
                        arrays = NullValue<nodeType[]>.Array;
                        count = 0;
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.Set(true);
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 获取或者创建节点
        /// </summary>
        /// <param name="parser"></param>
        private void getOrCreateNode(ref OperationParameter.NodeParser parser)
        {
            int indexParameter = parser.GetValueData(-1);
            if (indexParameter >= 0)
            {
                int arrayIndex = indexParameter >> FragmentArray.ArrayShift;
                if (arrayIndex >= arrays.Length)
                {
                    nodeType[][] newArrays = arrays.copyNew(arrayIndex + 1, arrays.Length);
                    for (int newIndex = arrays.Length; newIndex != newArrays.Length; newArrays[newIndex++] = new nodeType[FragmentArray.ArraySize]) ;
                    arrays = newArrays;
                }
                nodeType[] array = arrays[arrayIndex];
                int index = indexParameter & FragmentArray.ArraySizeAnd;
                if (array[index] == null)
                {
                    array[index] = nodeConstructor();
                    parser.IsOperation = true;
                    if (indexParameter >= count) count = indexParameter + 1;
                }
                parser.ReturnParameter.Set(true);
            }
            else parser.ReturnParameter.Type = ReturnType.ArrayIndexOutOfRange;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="parser"></param>
        private void remove(ref OperationParameter.NodeParser parser)
        {
            int index = parser.GetValueData(-1);
            if ((uint)index < count)
            {
                nodeType[] array = arrays[index >> FragmentArray.ArrayShift];
                if (array[index &= FragmentArray.ArraySizeAnd] != null)
                {
                    parser.IsOperation = true;
                    array[index] = null;
                }
                parser.ReturnParameter.Set(true);
            }
            else parser.ReturnParameter.Type = ReturnType.ArrayIndexOutOfRange;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            return getNext(ref parser);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void QueryEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.Set(count); return;
                case OperationParameter.OperationType.ContainsKey:
                    int index = parser.GetValueData(-1);
                    if ((uint)index < count)
                    {
                        nodeType[] array = arrays[index >> FragmentArray.ArrayShift];
                        parser.ReturnParameter.Set(array[index & FragmentArray.ArraySizeAnd] != null);
                    }
                    else parser.ReturnParameter.Set(false);
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
        }

        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal override Snapshot.Node CreateSnapshot()
        {
            Snapshot.Node[] array = new Snapshot.Node[count];
            int index = 0;
            foreach (nodeType[] nodeArray in arrays)
            {
                if (nodeArray != null)
                {
                    foreach (nodeType node in nodeArray)
                    {
                        if (node != null) array[index] = node.CreateSnapshot();
                        if (++index >= count) break;
                    }
                }
                else index += FragmentArray.ArraySize;
                if (index >= count) break;
            }
            return new Snapshot.Array(array);
        }

#if NOJIT
        /// <summary>
        /// 创建数组节点
        /// </summary>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static FragmentArray<nodeType> create()
        {
            return new FragmentArray<nodeType>();
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<FragmentArray<nodeType>> constructor;
        /// <summary>
        /// 子节点构造函数
        /// </summary>
        private static readonly Func<nodeType> nodeConstructor;
        static FragmentArray()
        {
#if NOJIT
            constructor = (Func<FragmentArray<nodeType>>)Delegate.CreateDelegate(typeof(Func<FragmentArray<nodeType>>), typeof(FragmentArray<nodeType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NullValue<Type>.Array, null));
#else
            constructor = (Func<FragmentArray<nodeType>>)AutoCSer.Emit.Constructor.Create(typeof(FragmentArray<nodeType>));
#endif
            nodeConstructor = (Func<nodeType>)typeof(nodeType).GetField(ConstructorFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null);
        }
    }
}
