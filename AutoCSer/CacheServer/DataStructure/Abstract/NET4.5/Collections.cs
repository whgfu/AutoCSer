﻿using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 集合节点
    /// </summary>
    public abstract partial class Collections
    {
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<int>> GetCountTask()
        {
            return Client.GetInt(await ClientDataStructure.Client.QueryTask(GetCountNode()));
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> RemoveTask()
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationTask(GetClearNode()));
        }
    }
}
