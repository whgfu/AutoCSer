﻿using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 服务配置
    /// </summary>
    public unsafe class ServerAttribute : TcpServer.ServerAttribute
    {
        /// <summary>
        /// 默认二进制反序列化配置参数名称
        /// </summary>
        public const string BinaryDeSerializeConfigName = "TcpOpenServer";
        /// <summary>
        /// 默认二进制反序列化配置参数
        /// </summary>
        internal static readonly AutoCSer.BinarySerialize.DeSerializeConfig DefaultBinaryDeSerializeConfig = (AutoCSer.BinarySerialize.DeSerializeConfig)AutoCSer.Configuration.Common.Get(typeof(AutoCSer.BinarySerialize.DeSerializeConfig), BinaryDeSerializeConfigName) ?? new AutoCSer.BinarySerialize.DeSerializeConfig { IsDisposeMemberMap = true, MaxArraySize = 1024 };
        /// <summary>
        /// 获取二进制反序列化配置参数
        /// </summary>
        /// <param name="maxArraySize"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static AutoCSer.BinarySerialize.DeSerializeConfig GetBinaryDeSerializeConfig(int maxArraySize)
        {
            if (maxArraySize == AutoCSer.BinaryDeSerializer.DefaultConfig.MaxArraySize) return AutoCSer.BinaryDeSerializer.DefaultConfig;
            if (maxArraySize == DefaultBinaryDeSerializeConfig.MaxArraySize) return DefaultBinaryDeSerializeConfig;
            return new AutoCSer.BinarySerialize.DeSerializeConfig { IsDisposeMemberMap = true, MaxArraySize = maxArraySize };
        }

        /// <summary>
        /// 成员选择类型
        /// </summary>
        public MemberFilters MemberFilters = MemberFilters.Instance;
        /// <summary>
        /// 成员选择类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override MemberFilters GetMemberFilters { get { return MemberFilters; } }
        /// <summary>
        /// 服务器端发送数据（包括客户端接受数据）缓冲区初始化字节数，默认为 8KB。
        /// </summary>
        public AutoCSer.Memory.BufferSize SendBufferSize = AutoCSer.Memory.BufferSize.Kilobyte8;
        /// <summary>
        /// 服务器端发送数据（包括客户端接受数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override AutoCSer.Memory.BufferSize GetSendBufferSize { get { return SendBufferSize; } }
        /// <summary>
        /// 服务器端发送数据缓冲区最大字节数
        /// </summary>
        public int ServerSendBufferMaxSize;
        /// <summary>
        /// 服务器端发送数据缓冲区最大字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetServerSendBufferMaxSize { get { return ServerSendBufferMaxSize; } }
        /// <summary>
        /// 服务器端接受数据（包括客户端发送数据）缓冲区初始化字节数，默认为 8KB。
        /// </summary>
        public AutoCSer.Memory.BufferSize ReceiveBufferSize = AutoCSer.Memory.BufferSize.Kilobyte8;
        /// <summary>
        /// 服务器端接受数据（包括客户端发送数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override AutoCSer.Memory.BufferSize GetReceiveBufferSize { get { return ReceiveBufferSize; } }
        /// <summary>
        /// 最大输入数据字节数，默认为 16 KB，小于等于 0 表示不限
        /// </summary>
        public int MaxInputSize = DefaultMaxInputSize;
        /// <summary>
        /// 最大输入数据字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetMaxInputSize { get { return MaxInputSize; } }
        ///// <summary>
        ///// 压缩启用最低字节数量，默认为 512 字节（压缩数据需要消耗一定的 CPU 资源降低带宽使用，在简单测试中可能降低 60% 的性能），设置为 0 表示不压缩数据（适合内网服务）。
        ///// </summary>
        //public int MinCompressSize = 512;
        ///// <summary>
        ///// 压缩启用最低字节数量
        ///// </summary>
        //[AutoCSer.Metadata.Ignore]
        //internal override int GetMinCompressSize { get { return MinCompressSize; } }
        /// <summary>
        /// 客户端保持连接心跳包间隔时间默认为 60 秒，对于频率稳定可靠的服务类型可以设置为 0 禁用心跳包。
        /// </summary>
        public int CheckSeconds = 60;
        /// <summary>
        /// 客户端保持连接心跳包间隔时间
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetCheckSeconds { get { return CheckSeconds; } }
        /// <summary>
        /// 客户端接收命令默认超时为 9 秒，超时客户端将被当作攻击者被抛弃。
        /// </summary>
        internal const int DefaultReceiveVerifyCommandSeconds = 9;
        /// <summary>
        /// 客户端接收命令超时为 9 秒，超时客户端将被当作攻击者被抛弃。
        /// </summary>
        public int ReceiveVerifyCommandSeconds = DefaultReceiveVerifyCommandSeconds;
        /// <summary>
        /// 客户端接收命令超时
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetReceiveVerifyCommandSeconds { get { return ReceiveVerifyCommandSeconds; } }
        /// <summary>
        /// 客户端等待连接毫秒数，默认为 0 表示等待直到成功或者失败
        /// </summary>
        public uint ClientWaitConnectedMilliseconds;
        /// <summary>
        /// 客户端等待连接毫秒数，默认为 0 表示等待直到成功或者失败
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override uint GetClientWaitConnectedMilliseconds { get { return ClientWaitConnectedMilliseconds; } }
        /// <summary>
        /// 客户端最大自定义数据包字节大小，默认为 16KB，设置为 0 表示不限
        /// </summary>
        public int MaxCustomDataSize = (16 << 10) - (sizeof(uint) + sizeof(int) * 2);
        /// <summary>
        /// 客户端最大自定义数据包字节大小
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetMaxCustomDataSize { get { return MaxCustomDataSize; } }
        /// <summary>
        /// 客户端重建连接休眠毫秒数，默认为 1000
        /// </summary>
        public int ClientTryCreateSleep = 1000;
        /// <summary>
        /// 客户端重建连接休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetClientTryCreateSleep { get { return ClientTryCreateSleep; } }
        /// <summary>
        /// 客户端第一次重建连接休眠毫秒数，默认为 1000
        /// </summary>
        public int ClientFirstTryCreateSleep = 1000;
        /// <summary>
        /// 客户端第一次重建连接休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetClientFirstTryCreateSleep { get { return ClientFirstTryCreateSleep; } }
        /// <summary>
        /// 客户端批量处理等待类型，默认为不等待
        /// </summary>
        public TcpServer.OutputWaitType ClientOutputWaitType = TcpServer.OutputWaitType.DontWait;
        /// <summary>
        /// 客户端批量处理等待类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override TcpServer.OutputWaitType GetClientOutputWaitType { get { return ClientOutputWaitType; } }
        /// <summary>
        /// 服务端批量处理等待类型，默认为不等待
        /// </summary>
        public TcpServer.OutputWaitType ServerOutputWaitType = TcpServer.OutputWaitType.DontWait;
        /// <summary>
        /// 服务端批量处理等待类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override TcpServer.OutputWaitType GetServerOutputWaitType { get { return ServerOutputWaitType; } }
        /// <summary>
        /// 命令池初始化二进制大小，默认为 3
        /// </summary>
        public byte CommandPoolBitSize = 3;
        /// <summary>
        /// 命令池初始化二进制大小 2^n
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override byte GetCommandPoolBitSize { get { return CommandPoolBitSize; } }
        /// <summary>
        /// 客户端最大未处理命令数量，默认为 1024
        /// </summary>
        public int QueueCommandSize = 1 << 10;
        /// <summary>
        /// 客户端最大未处理命令数量
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetQueueCommandSize { get { return QueueCommandSize; } }
        /// <summary>
        /// 当需要将客户端提供给第三方使用的时候，可能不希望 dll 中同时包含服务端，默认为 true 会将客户端代码单独剥离出来生成一个代码文件 {项目名称}.tcpServer.服务名称.client.cs，当然你需要将服务中所有参数与返回值及其依赖的数据类型剥离出来。
        /// </summary>
        public bool IsSegmentation = true;
        /// <summary>
        /// 当需要将客户端提供给第三方使用的时候，可能不希望 dll 中同时包含服务端，设置为 true 会将客户端代码单独剥离出来生成一个代码文件 {项目名称}.tcpServer.服务名称.client.cs，当然你需要将服务中所有参数与返回值及其依赖的数据类型剥离出来。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsSegmentation { get { return IsSegmentation; } }
        /// <summary>
        /// 默认使用 JSON 序列化，适合数据类型不稳定的互联网服务。对于参数数据类型稳定的服务，或者可以同步部署服务器端与客户端的内部服务，可以考虑使用二进制序列化以提升性能（对于简单测试可能提升 100+% 性能）。
        /// </summary>
        public bool IsJsonSerialize = true;
        /// <summary>
        /// 是否使用 JSON 序列化
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsJsonSerialize { get { return IsJsonSerialize; } }
        /// <summary>
        /// 服务端创建输出是否开启线程，默认为 false
        /// </summary>
        public bool IsServerBuildOutputThread;
        /// <summary>
        /// 服务端创建输出是否开启线程
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsServerBuildOutputThread { get { return IsServerBuildOutputThread; } }
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        public int BinaryDeSerializeMaxArraySize = DefaultBinaryDeSerializeConfig.MaxArraySize;
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetBinaryDeSerializeMaxArraySize { get { return BinaryDeSerializeMaxArraySize; } }
        /// <summary>
        /// 默认为 false 需要第一次调用触发，否则在创建客户端对象的时候自动启动连接
        /// </summary>
        public bool IsAutoClient;
        /// <summary>
        /// 命令映射枚举类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public Type CommandIdentityEnmuType;
        /// <summary>
        /// 是否生成记忆数字编号标识与长字符串名称标识之间对应关系的代码
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsRememberCommand
        {
            get { return IsRememberCommand && CommandIdentityEnmuType == null; }
        }
        /// <summary>
        /// 服务端自定义队列类型，需要继承自 AutoCSer.Net.TcpServer.IServerCallQueueSet
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public Type ServerCallQueueType;
        /// <summary>
        /// 服务端自定义队列类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override Type GetServerCallQueueType { get { return ServerCallQueueType; } }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="serviceName">TCP 调用服务名称</param>
        /// <param name="type">TCP 服务器类型</param>
        /// <returns>TCP 调用服务器端配置信息</returns>
        public static ServerAttribute GetConfig(string serviceName, Type type = null)
        {
            return GetConfig(serviceName, type, (ServerAttribute)AutoCSer.Configuration.Common.Get(typeof(ServerAttribute), serviceName));
        }
    }
}
