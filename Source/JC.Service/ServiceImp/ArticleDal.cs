using JC.DataAccess;
using JC.DataAccess.Sqlite;
using JC.Model;
using JC.Service.ServiceFacade;
using log4net;
using System;
using System.Data;
using System.Data.Common;

namespace JC.Service.ServiceImp
{
    public class ArticleDal : IArticleDal
    {
        /// <summary>
        /// 创建日志对象
        /// </summary>
        private readonly ILog logInfo = LogManager.GetLogger(typeof(ArticleDal));

        public ResultEntity AddArticle(Article article)
        {
            ResultEntity result = new ResultEntity();
            result.ExcutRetStatus = false;

            if (article == null)
            {
                result.StrErrMsg = "调用接口【AddArticle】新增文章错误，传入对象参数为null";
                logInfo.Info(result.StrErrMsg);
                return result;

            }

            //执行脚本
            string strSql = @"INSERT INTO article(article_category_id,article_name,article_url,article_publishtime)
                                          VALUES(@article_category_id,@article_name,@article_url,@article_publishtime)";
            //参数数组
            //SQLiteParameter[] paras = null;

            try
            {
                Database database = new SqliteDatabase(DefaultDBConn.DefaultDbConnectionString);
                DbCommand command = database.GetSqlStringCommand(strSql);
                database.AddInParameter(command, "@article_category_id", DbType.Int32, article.ArticleCategoryId);
                database.AddInParameter(command, "@article_name", DbType.String, article.ArticleName);
                database.AddInParameter(command, "@article_url", DbType.String, article.ArticleUrl);
                database.AddInParameter(command, "@article_publishtime", DbType.DateTime, article.ArticlePublishtime);

                if (database.ExecuteNonQuery(command) > 0)
                {
                    result.ExcutRetStatus = true;
                }

//                using (IDAO dao = new SQLiteDAO(DefaultDBConn.DefaultDbConnectionString))
//                {
//                    //不存在记录，添加记录
//                    strSql = @"INSERT INTO article(article_category_id,article_name,article_url,article_publishtime)
//                                   VALUES(@article_category_id,@article_name,@article_url,@article_publishtime)";

//                    paras = new SQLiteParameter[]
//                        {
//                            new SQLiteParameter("@article_category_id", article.ArticleCategoryId),
//                            new SQLiteParameter("@article_name", article.ArticleName),
//                            new SQLiteParameter("@article_url", article.ArticleUrl),
//                            new SQLiteParameter("@article_publishtime", DateTime.Now)
//                        };
                    
//                    if (dao.ExecuteNonQuery(CommandType.Text, strSql, paras) > 0)
//                    {
//                        result.ExcutRetStatus = true;
//                    }
//                }

            }
            catch (Exception ex)
            {
                result.StrErrMsg = string.Format("调用接口【AddArticle】新增文章异常，url:{0}，原因：{1}",
                    article.ArticleUrl, ex.Message);
                logInfo.Error(result.StrErrMsg);
            }

            return result;
        }
    }
}
