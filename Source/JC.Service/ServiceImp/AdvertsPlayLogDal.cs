using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JC.Service.ServiceFacade;
using JC.Model;
using log4net;
using JC.DataAccess;
using System.Data;
using System.Data.SQLite;
using JC.Model.Custom;

namespace JC.Service.ServiceImp
{
    public class AdvertsPlayLogDal : IAdvertsPlayLogDal
    {
        /// <summary>
        /// 创建日志对象
        /// </summary>
        private readonly ILog logInfo = LogManager.GetLogger(typeof(AdvertsPlayLogDal));

//        public ResultEntity AddOrUpdateAdvertPlayLog(AdvertPlayLog advertPlayLog)
//        {
//            ResultEntity result = new ResultEntity();
//            result.ExcutRetStatus = false;

//            if (advertPlayLog == null)
//            {
//                result.StrErrMsg = "调用接口【AddOrUpdateAdvertPlayLog】更新播放日志错误，传入对象参数为null";
//                logInfo.Info(result.StrErrMsg);
//                return result;

//            }

//            logInfo.InfoFormat("广告ID：{0} 调用接口【AddOrUpdateAdvertPlayLog】更新播放日志", advertPlayLog.AdvertId);

//            //执行脚本
//            string strSql = string.Empty;
//            //参数数组
//            SQLiteParameter[] paras = null;
            
//            try
//            {
//                using (IDAO dao = new SQLiteDAO(DefaultDBConn.DefaultDbConnectionString))
//                {
//                    if (AdvertPlayLogIsExist(advertPlayLog.AdvertId, advertPlayLog.CreateTime, dao))
//                    {
//                        //已存在记录，更新记录次数
//                        strSql = @"UPDATE advert_play_logs
//                                    SET play_count=play_count + 1,
//                                    lasttime=@lasttime
//                                    WHERE advert_id = @advert_id
//                                    AND date(createtime) = @createtime";
//                        paras = new SQLiteParameter[]
//                        {
//                            new SQLiteParameter("@lasttime", advertPlayLog.LastTime),
//                            new SQLiteParameter("@advert_id", advertPlayLog.AdvertId),
//                            new SQLiteParameter("@createtime", advertPlayLog.CreateTime.ToString("yyyy-MM-dd"))
//                        };    
//                    }
//                    else
//                    {
//                        //不存在记录，添加记录
//                        strSql = @"INSERT INTO advert_play_logs(self_service_machine_id,strategy_id,advert_id,advert_name,createtime,lasttime,play_count,state)
//                                   VALUES(@self_service_machine_id,@strategy_id,@advert_id,@advert_name,@createtime,@lasttime,@play_count,@state)";
                        
//                        paras = new SQLiteParameter[]
//                        {
//                            new SQLiteParameter("@self_service_machine_id", advertPlayLog.SelfServiceMachineId),
//                            new SQLiteParameter("@strategy_id", advertPlayLog.StrategyId),
//                            new SQLiteParameter("@advert_id", advertPlayLog.AdvertId),
//                            new SQLiteParameter("@advert_name", advertPlayLog.AdvertName),
//                            new SQLiteParameter("@createtime", advertPlayLog.CreateTime),
//                            new SQLiteParameter("@lasttime", advertPlayLog.LastTime),
//                            new SQLiteParameter("@play_count", 1),
//                            new SQLiteParameter("@state", Convert.ToInt16(EnumAdvertPlayLogState.有效))
//                        };                        
//                    }

//                    if (dao.ExecuteNonQuery(CommandType.Text, strSql, paras) > 0)
//                    {
//                        result.ExcutRetStatus = true;
//                    }
//                    else
//                    {
//                        result.StrErrMsg = string.Format("广告ID：{0}调用接口【AddOrUpdateAdvertPlayLog】更新播放日志失败", advertPlayLog.AdvertId);
//                        logInfo.Error(result.StrErrMsg);
//                    }
//                }

                
//            }
//            catch (Exception ex)
//            {
//                result.StrErrMsg = string.Format("广告ID：{0}调用接口【AddOrUpdateAdvertPlayLog】更新播放日志失败，原因：{1}", advertPlayLog.AdvertId, ex.Message);
//                logInfo.Error(result.StrErrMsg);
//            }

//            return result;
//        }

        private bool AdvertPlayLogIsExist(long advertId, DateTime containDate, IDAO dao)
        {
            bool result= false;

            string strSql = string.Format(@"SELECT count(apl.advert_play_logs_id) FROM advert_play_logs apl
                            WHERE state = {0}
                            and apl.advert_id = @advert_id 
                            and date(apl.createtime) = @containDate", (Int16)EnumAdvertPlayLogState.有效);

            SQLiteParameter[] paras = 
            { 
                new SQLiteParameter("@advert_id", advertId),
                new SQLiteParameter("@containDate", containDate.ToString("yyyy-MM-dd"))
            };

            if (Convert.ToInt32(dao.ExecuteScalar(CommandType.Text, strSql, paras)) > 0)
            {
                result = true;
            }

            return result;
        }

        public ResultEntity AdvertPlayLogIsExist(long advertId, DateTime containDate)
        {
            logInfo.InfoFormat("广告ID：{0} 调用接口【AdvertPlayLogIsExist】查询某日：{1} 的广告是否有记录", advertId, containDate);

            ResultEntity result = new ResultEntity();
            result.ExcutRetStatus = false;

            try
            {
                using (IDAO dao = new SQLiteDAO(DefaultDBConn.DefaultDbConnectionString))
                {
                    result.ExcutRetStatus = AdvertPlayLogIsExist(advertId, containDate, dao);
                }

            }
            catch (Exception ex)
            {
                result.StrErrMsg = string.Format("广告ID：{0}调用接口【AdvertPlayLogIsExist】查询某日：{1} 的广告是否有记录，原因：{2}", 
                    advertId, containDate, ex.Message);
                logInfo.Error(result.StrErrMsg);
                
            }

            return result;
        }

//        public List<AdvertPlayLog> GetAdvertPlayLogListByTimeSpan(DateTime startTime, DateTime endTime)
//        {
//            logInfo.InfoFormat("调用接口【GetAdvertPlayLogListByTimeSpan】查询时间段：【{0}】到【{1}】 的广告记录", startTime, endTime);

//            List<AdvertPlayLog> advertPlayLoglist = null;

//            string strSql = string.Format(@"SELECT apl.advert_play_logs_id,apl.advert_id,apl.advert_name,apl.self_service_machine_id, 
//                            apl.createtime,apl.lasttime,apl.play_count,apl.state,strategy_id
//                             FROM advert_play_logs apl
//                            WHERE state = {0}
//                            and apl.createtime BETWEEN @startTime AND @endTime", Convert.ToInt16(EnumAdvertPlayLogState.有效));
//            try
//            {
//                SQLiteParameter[] paras = 
//                {
//                    new SQLiteParameter("@startTime", startTime.ToString("yyyy-MM-dd 00:00:00")),
//                    new SQLiteParameter("@endTime", endTime.ToString("yyyy-MM-dd 00:00:00"))
//                };

//                advertPlayLoglist = new List<AdvertPlayLog>();
//                AdvertPlayLog advertPlayLog = null;
//                using (IDAO dao = new SQLiteDAO(DefaultDBConn.DefaultDbConnectionString))
//                {
//                    using (var reader = dao.ExecuteReader(CommandType.Text, strSql, paras))
//                    {
//                        while (reader.Read())
//                        {
//                            advertPlayLog = new AdvertPlayLog();
//                            advertPlayLog.AdvertsPlayLogId = reader.GetInt64(0);
//                            advertPlayLog.AdvertId = reader.GetInt64(1);
//                            advertPlayLog.AdvertName = reader.GetString(2);
//                            advertPlayLog.SelfServiceMachineId = reader.GetInt64(3);
//                            advertPlayLog.CreateTime = reader.GetDateTime(4);
//                            advertPlayLog.LastTime = reader.GetDateTime(5);
//                            advertPlayLog.PlayCount = reader.GetInt32(6);
//                            advertPlayLog.State = reader.GetInt16(7);
//                            advertPlayLog.StrategyId = reader.GetInt64(8);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                logInfo.ErrorFormat("调用接口【GetAdvertPlayLogListByTimeSpan】查询时间段：【{0}】到【{1}】 的广告记录失败，原因：{2}", 
//                    startTime, endTime, ex.Message);
//            }

//            return advertPlayLoglist;
//        }
    }
}
