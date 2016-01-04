using JC.Model;
using System;
using System.Collections.Generic;

namespace JC.Service.ServiceFacade
{
    /// <summary>
    /// 广告日志访问接口
    /// </summary>
    public interface IAdvertsPlayLogDal
    {
        /// <summary>
        /// 新增或修改播放日志
        /// 如果日志记录已存在，更新播放次数，否则新增播放记录
        /// </summary>
        /// <param name="advertPlayLog">播放日志实体</param>
        /// <returns></returns>
        //ResultEntity AddOrUpdateAdvertPlayLog(AdvertPlayLog advertPlayLog);

        /// <summary>
        /// 查询某日的广告是否有记录
        /// </summary>
        /// <param name="advertId">广告ID</param>
        /// <param name="containDate">要查询的日期</param>
        /// <param name="dao">数据查询对象</param>
        /// <returns></returns>
        ResultEntity AdvertPlayLogIsExist(long advertId, DateTime containDate);

        /// <summary>
        /// 获取某个时间段内的广告日志播放记录
        /// </summary>
        /// <param name="startTime">时间段开始时间</param>
        /// <param name="endTime">时间段结束时间</param>
        /// <returns>日志记录的列表</returns>
        //List<AdvertPlayLog> GetAdvertPlayLogListByTimeSpan(DateTime startTime, DateTime endTime);

        
    }
}
