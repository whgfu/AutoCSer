﻿using System;
using System.Reflection;
using AutoCSer.Net.TcpServer.Emit;

namespace AutoCSer.Net.TcpInternalServer.Emit
{
    /// <summary>
    /// TCP 服务
    /// </summary>
    internal static class Server
    {
        /// <summary>
        /// TCP 服务端元数据
        /// </summary>
        internal static readonly ServerMetadata Metadata = new ServerMetadata(typeof(TcpInternalServer.Server), typeof(ServerAttribute), typeof(ServerSocketSender), typeof(ServerCall)
            , ParameterGenericType.Get, ReturnParameterGenericType.Get
            , ((Func<bool>)ParameterGenericType.ServerSocketSender.Push).Method
            , ((Func<AutoCSer.Net.TcpServer.ReturnType, bool>)ParameterGenericType.ServerSocketSender.Push).Method
            , ((Func<uint, AutoCSer.Net.TcpServer.ReturnType, bool>)ParameterGenericType.ServerSocketSender.Push).Method
            , ((Func<uint, AutoCSer.Net.TcpServer.ReturnType, bool>)ParameterGenericType.ServerSocketSender.PushNoThread).Method
            , ((Func<TcpServer.OutputInfo, TcpServer.ServerCallback>)ParameterGenericType.ServerSocketSender.GetCallback).Method
            , ((Func<TcpServer.OutputInfo, Func<TcpServer.ReturnValue, bool>>)ParameterGenericType.ServerSocketSender.GetCallbackEmit).Method
            , ((Action<Exception>)ParameterGenericType.ServerSocketSender.AddLog).Method
            , ((Action<ServerSocketSender, object, TcpServer.ServerTaskType>)ServerCall.Null.Default.Call).Method
            , ((Action<ServerSocketSender, object, TcpServer.ServerTaskType, byte>)ServerCall.Null.Default.Call).Method
            );
    }
    /// <summary>
    /// TCP 服务
    /// </summary>
    /// <typeparam name="interfaceType">接口类型</typeparam>
    [AutoCSer.IOS.Preserve(AllMembers = false)]
    public static class Server<interfaceType>
    {
        /// <summary>
        /// 默认 TCP 服务配置
        /// </summary>
        private static readonly ServerAttribute defaultServerAttribute;
        /// <summary>
        /// 错误字符串提示信息
        /// </summary>
        private static readonly string errorString;
        /// <summary>
        /// TCP 函数集合
        /// </summary>
        private static readonly Method<ServerAttribute, TcpServer.MethodAttribute, ServerSocketSender>[] methods;
        /// <summary>
        /// TCP 服务类型
        /// </summary>
        private static readonly Type serverType;
        /// <summary>
        /// TCP 服务构造函数
        /// </summary>
        private static readonly ConstructorInfo serverConstructorInfo;
        /// <summary>
        /// 自定义队列构造函数
        /// </summary>
        private static readonly ConstructorInfo serverCallQueueConstructorInfo;
        /// <summary>
        /// 服务端输出信息集合
        /// </summary>
        [AutoCSer.IOS.Preserve]
        internal static readonly TcpServer.OutputInfo[] Outputs;
        /// <summary>
        /// 创建 TCP 服务端对象
        /// </summary>
        /// <param name="value">接口对象</param>
        /// <param name="attribute">TCP 调用服务器端配置信息</param>
        /// <param name="verify">套接字验证委托</param>
        /// <param name="serverCallQueue">自定义队列</param>
        /// <param name="extendCommandBits">扩展服务命令二进制位数</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        /// <returns>TCP 服务</returns>
        public static TcpInternalServer.Server Create(interfaceType value, TcpInternalServer.ServerAttribute attribute = null, Func<System.Net.Sockets.Socket, bool> verify = null, AutoCSer.Net.TcpServer.IServerCallQueueSet serverCallQueue = null, byte extendCommandBits = 0, Action<SubArray<byte>> onCustomData = null, AutoCSer.ILog log = null)
        {
            if (serverConstructorInfo == null) throw new InvalidOperationException();
            if (errorString != null) throw new Exception(errorString);
            if (value == null) throw new ArgumentNullException();
            if (serverCallQueue == null && serverCallQueueConstructorInfo != null)
            {
                serverCallQueue = (AutoCSer.Net.TcpServer.IServerCallQueueSet)serverCallQueueConstructorInfo.Invoke(EmptyArray<object>.Array);
            }
            TcpInternalServer.Server server = (TcpInternalServer.Server)serverConstructorInfo.Invoke(new object[] { attribute = attribute ?? defaultServerAttribute, verify, value, serverCallQueue, extendCommandBits, onCustomData, log });
            server.setCommandData(methods.Length);
            foreach (Method<ServerAttribute, TcpServer.MethodAttribute, ServerSocketSender> method in methods)
            {
                if (method != null)
                {
                    if (method.Attribute.IsVerifyMethod) server.setVerifyCommand(method.Attribute.CommandIdentity);
                    else server.setCommand(method.Attribute.CommandIdentity);
                }
            }
            TcpServer.ISetTcpServer<TcpInternalServer.Server> setTcpServer = value as TcpServer.ISetTcpServer<TcpInternalServer.Server>;
            if (setTcpServer != null) setTcpServer.SetTcpServer(server);
            if (attribute.IsAutoServer) server.Start();
            return server;
        }

        static Server()
        {
            Type type = typeof(interfaceType);
            MethodGetter builder = new MethodGetter { IsClient = false };
            bool isMethod = builder.Get(type);
            errorString = builder.ErrorString;
            if (isMethod)
            {
                defaultServerAttribute = builder.DefaultServerAttribute;
                methods = builder.Methods;
                if (defaultServerAttribute.ServerCallQueueType != null)
                {
                    serverCallQueueConstructorInfo = defaultServerAttribute.ServerCallQueueType.GetConstructor(EmptyArray<Type>.Array);
                }

                Type[] constructorParameterTypes = new Type[] { typeof(ServerAttribute), typeof(Func<System.Net.Sockets.Socket, bool>), type, typeof(AutoCSer.Net.TcpServer.IServerCallQueueSet), typeof(byte), typeof(Action<SubArray<byte>>), typeof(AutoCSer.ILog) };
                Method<ServerAttribute, TcpServer.MethodAttribute, ServerSocketSender>.ServerBuilder serverBuilder = new Method<ServerAttribute, TcpServer.MethodAttribute, ServerSocketSender>.ServerBuilder { Metadata = Server.Metadata };
                serverType = serverBuilder.Build(type, defaultServerAttribute, typeof(Server<interfaceType>), typeof(ServerCall<>), constructorParameterTypes, methods, builder.Queues);
                Outputs = serverBuilder.Outputs;
                serverConstructorInfo = serverType.GetConstructor(constructorParameterTypes);
            }
        }
    }
}
