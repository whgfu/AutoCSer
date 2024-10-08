﻿using System;
using AutoCSer.Log;
using AutoCSer.Extensions;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.CompilerServices;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 服务端
    /// </summary>
    public abstract unsafe class Server : TcpServer.Server<Server, ServerSocketSender>
    {
        /// <summary>
        /// 套接字等待事件
        /// </summary>
        private AutoCSer.Threading.OnceAutoWaitHandle socketHandle;
        /// <summary>
        /// 套接字链表头部
        /// </summary>
        private SocketLink socketHead;
        /// <summary>
        /// TCP 服务端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">同步验证接口</param>
        /// <param name="serverCallQueue">自定义队列</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        /// <param name="callQueueCount">独占的 TCP 服务器端同步调用队列数量</param>
        /// <param name="isCallQueueLink">是否提供独占的 TCP 服务器端同步调用队列（低优先级）</param>
        /// <param name="isSynchronousVerifyMethod">验证函数是否同步调用</param>
        /// <param name="extendCommandBits">扩展服务命令二进制位数</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public Server(ServerAttribute attribute, Func<System.Net.Sockets.Socket, bool> verify, AutoCSer.Net.TcpServer.IServerCallQueueSet serverCallQueue, byte extendCommandBits, Action<SubArray<byte>> onCustomData, ILog log, int callQueueCount, bool isCallQueueLink, bool isSynchronousVerifyMethod)
            : base(attribute, verify, serverCallQueue, extendCommandBits, onCustomData, log, AutoCSer.Threading.ThreadTaskType.TcpOpenServerGetSocket, callQueueCount, isCallQueueLink, isSynchronousVerifyMethod)
        {
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        internal void GetSocket()
        {
            //ThreadPriority priority = Thread.CurrentThread.Priority;
            ReceiveVerifyCommandTimeout = new SocketTimeoutLink(ServerAttribute.ReceiveVerifyCommandSeconds > 0 ? ServerAttribute.ReceiveVerifyCommandSeconds : TcpOpenServer.ServerAttribute.DefaultReceiveVerifyCommandSeconds);
            try
            {
                socketHandle.Set(0);
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(this, AutoCSer.Threading.ThreadTaskType.TcpOpenServerOnSocket);
                //Thread.CurrentThread.Priority = ThreadPriority.Highest;
                if (verify == null) getSocket();
                else getSocketVerify();
                //Thread.CurrentThread.Priority = priority;
                socketHandle.Set();
            }
            finally { SocketTimeoutLink.Free(ref ReceiveVerifyCommandTimeout); }
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        private void getSocket()
        {
            Socket listenSocket = this.Socket;
            SocketLink head, newSocketLink;
            while (isListen != 0)
            {
                try
                {
                    NEXT:
                    newSocketLink = new SocketLink();
                    newSocketLink.Socket = listenSocket.Accept();
                    if (isListen == 0)
                    {
                        if (this.Socket != null)
                        {
                            this.Socket = null;
                            ShutdownServer(listenSocket);
                        }
                        return;
                    }
                    do
                    {
                        if ((head = socketHead) == null)
                        {
                            newSocketLink.LinkNext = null;
                            if (Interlocked.CompareExchange(ref socketHead, newSocketLink, null) == null)
                            {
                                socketHandle.Set();
                                newSocketLink = null;
                                goto NEXT;
                            }
                        }
                        else
                        {
                            newSocketLink.LinkNext = head;
                            if (Interlocked.CompareExchange(ref socketHead, newSocketLink, head) == head)
                            {
                                newSocketLink = null;
                                goto NEXT;
                            }
                        }
                        AutoCSer.Threading.ThreadYield.Yield();
                    }
                    while (true);
                }
                catch (Exception error)
                {
                    if (isListen == 0) return;
                    Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    Thread.Sleep(1);
                }
            }
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        private void getSocketVerify()
        {
            Socket listenSocket = this.Socket;
            SocketLink head, newSocketLink;
            while (isListen != 0)
            {
                try
                {
                    NEXT:
                    newSocketLink = new SocketLink();
                    ACCEPT:
                    newSocketLink.Socket = listenSocket.Accept();
                    if (isListen == 0)
                    {
                        if (this.Socket != null)
                        {
                            this.Socket = null;
                            ShutdownServer(listenSocket);
                        }
                        return;
                    }
                    if (verify(newSocketLink.Socket))
                    {
                        do
                        {
                            if ((head = socketHead) == null)
                            {
                                newSocketLink.LinkNext = null;
                                if (Interlocked.CompareExchange(ref socketHead, newSocketLink, null) == null)
                                {
                                    socketHandle.Set();
                                    newSocketLink = null;
                                    goto NEXT;
                                }
                            }
                            else
                            {
                                newSocketLink.LinkNext = head;
                                if (Interlocked.CompareExchange(ref socketHead, newSocketLink, head) == head)
                                {
                                    newSocketLink = null;
                                    goto NEXT;
                                }
                            }
                            AutoCSer.Threading.ThreadYield.Yield();
                        }
                        while (true);
                    }
                    newSocketLink.DisposeSocket();
                    goto ACCEPT;
                }
                catch (Exception error)
                {
                    if (isListen == 0) return;
                    Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    Thread.Sleep(1);
                }
            }
        }
        /// <summary>
        /// 套接字处理
        /// </summary>
        internal void OnSocket()
        {
            ServerSocket serverSocket = null;
            while (isListen != 0)
            {
                socketHandle.Wait();
                SocketLink socket = Interlocked.Exchange(ref socketHead, null);
                do
                {
                    try
                    {
                        while (socket != null) socket = socket.Start(this, ref serverSocket);
                        break;
                    }
                    catch (Exception error)
                    {
                        Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                    if (serverSocket != null)
                    {
                        serverSocket.Close();
                        serverSocket = null;
                    }
                    socket = socket.Cancel();
                }
                while (true);
            }
            socketHead = null;
        }
    }
}
