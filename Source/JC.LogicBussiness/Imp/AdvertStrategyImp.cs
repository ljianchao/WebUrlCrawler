using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JC.Model;
using JC.LogicBussiness.Facade;
using log4net;
using System.IO;
using System.Net;

namespace JC.LogicBussiness.Imp
{
    /// <summary>
    /// 广告策略下载类
    /// </summary>
    public class AdvertStrategyImp : IAdvertStrategyProxy
    {
        /// <summary>
        /// 创建日志对象
        /// </summary>
        private readonly ILog logInfo = LogManager.GetLogger(typeof(AdvertStrategyImp));

        /// <summary>
        /// 获取网络广告策略
        /// </summary>
        /// <param name="restUrl">rest接口完整url</param>
        /// <param name="localStrategyFilePath">广告策略保存到本地的路径</param>
        /// <returns></returns>
        //public ResultQueryEntity<AdvertStrategy> GetAdvertStrategy(string restUrl, string localStrategyFilePath)
        //{
        //    ResultQueryEntity<AdvertStrategy> result = new ResultQueryEntity<AdvertStrategy>();

        //    //请求rest接口获取的数据
        //    string advertJson = string.Empty;
        //    //执行步骤
        //    int executionStep = 0;
        //    try
        //    {
        //        //1. 根据rest接口获取策略信息
        //        advertJson = HttpUtils.GetReponseString(restUrl);
        //        executionStep++;
        //        logInfo.InfoFormat("时间：{0}请求rest策略接口的返回值为：{1}", DateTime.Now.ToString(), advertJson);

        //        //2. 转换请求的数据成策略对象
        //        if (!string.IsNullOrEmpty(advertJson))
        //        {
        //            result.QueryResult = Newtonsoft.Json.JsonConvert.DeserializeObject<AdvertStrategy>(advertJson);
        //            executionStep++;
        //        }

        //        //3. 保存rest数据到本地
        //        FileUtils.CreateOrOverrideFile(localStrategyFilePath, advertJson);
        //        executionStep++;
        //    }
        //    catch (Exception ex)
        //    {
        //        logInfo.ErrorFormat("获取网络广告策略信息出错，原因：{0}", ex.Message);
        //        executionStep = -1;
        //    }
        //    finally
        //    {
        //        switch (executionStep)
        //        {
        //            case -1:
        //                {
        //                    result.StrErrCode = "100";
        //                    result.StrErrMsg = "获取网络广告策略信息抛出异常，请查看日志";
        //                }
        //                break;
        //            case 0:
        //                {
        //                    result.StrErrCode = "500";
        //                    result.StrErrMsg = "请求rest接口获取网络广告策略信息出错";
        //                }
        //                break;
        //            case 1:
        //                {
        //                    result.StrErrCode = "501";
        //                    result.StrErrMsg = "网络信息转换成策略对象出错";
        //                }
        //                break;
        //            case 2:
        //                {
        //                    result.StrErrCode = "502";
        //                    result.StrErrMsg = "保存策略对象到本地出错";
        //                }
        //                break;
        //            default:
        //                break;
        //        }

        //        if (!string.IsNullOrEmpty(result.StrErrMsg))
        //        {
        //            logInfo.Error(result.StrErrMsg);
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// 系统启动时获取本地广告策略
        /// </summary>
        /// <param name="localStrategyFilePath">广告策略保存到本地的路径</param>
        /// <returns></returns>
        //public ResultQueryEntity<AdvertStrategy> GetLocalAdvertStrategy(string localStrategyFilePath)
        //{
        //    ResultQueryEntity<AdvertStrategy> result = new ResultQueryEntity<AdvertStrategy>();

        //    //本地策略文件获取的数据
        //    string advertJson = string.Empty;
        //    //执行步骤
        //    int executionStep = 0;
        //    try
        //    {
        //        if (File.Exists(localStrategyFilePath))
        //        {
        //            //1. 获取本地策略信息
        //            advertJson = File.ReadAllText(localStrategyFilePath, Encoding.UTF8).Trim();
        //            executionStep++;
        //            logInfo.InfoFormat("时间：{0}本地广告策略的内容为：{1}", DateTime.Now.ToString(), advertJson);

        //            //2. 转换请求的数据成策略对象
        //            if (!string.IsNullOrEmpty(advertJson))
        //            {
        //                result.QueryResult = Newtonsoft.Json.JsonConvert.DeserializeObject<AdvertStrategy>(advertJson);
        //                executionStep++;
        //            }
        //        }
        //        else
        //        {
        //            logInfo.InfoFormat("时间：{0}没有找到本地广告策略文件，地址：{1}",
        //                DateTime.Now.ToString(), localStrategyFilePath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logInfo.ErrorFormat("获取本地广告策略信息出错，原因：{0}", ex.Message);
        //        executionStep = -1;
        //    }
        //    finally
        //    {
        //        switch (executionStep)
        //        {
        //            case -1:
        //                {
        //                    result.StrErrCode = "100";
        //                    result.StrErrMsg = "获取本地广告策略信息抛出异常，请查看日志";
        //                }
        //                break;
        //            case 0:
        //                {
        //                    result.StrErrCode = "500";
        //                    result.StrErrMsg = "读取本地广告策略信息出错";
        //                }
        //                break;
        //            case 1:
        //                {
        //                    result.StrErrCode = "501";
        //                    result.StrErrMsg = "本地广告策略内容转换成策略对象出错";
        //                }
        //                break;
        //            default:
        //                break;
        //        }

        //        if (!string.IsNullOrEmpty(result.StrErrMsg))
        //        {
        //            logInfo.Error(result.StrErrMsg);
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// 下载广告图片
        /// </summary>
        /// <param name="advert">广告实体</param>
        /// <param name="DefaultAdvertImagasPath">广告图片默认保存路径</param>
        //public void DownAdvertsImages(Advert advert, string defaultAdvertImagasPath)
        //{
        //    try
        //    {
        //        if (!Directory.Exists(defaultAdvertImagasPath))
        //        {
        //            Directory.CreateDirectory(defaultAdvertImagasPath);
        //        }
        //        WebClient wc = new WebClient();
        //        wc.DownloadFileAsync(new Uri(advert.UrlPath), Path.Combine(defaultAdvertImagasPath, advert.AdvertName));

        //    }
        //    catch (Exception ex)
        //    {
        //        logInfo.ErrorFormat("时间：{0}下载广播图片：{1} 失败，原因：{2}", DateTime.Now.ToString(), advert.AdvertName, ex.Message);
        //    }
        //}
    }
}
