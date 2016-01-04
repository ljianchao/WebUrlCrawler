using JC.DataAccess;
using JC.DataAccess.Sqlite;
using JC.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace JC.Service.ServiceImp
{
    public class ArticleCategoryDal : ServiceFacade.IArticleCategoryDal
    {
        /// <summary>
        /// 创建日志对象
        /// </summary>
        private readonly ILog logInfo = LogManager.GetLogger(typeof(ArticleCategoryDal));

        public Model.ResultEntity AddArticleCategory(Model.ArticleCategory articleCategory)
        {
            ResultEntity result = new ResultEntity();
            result.ExcutRetStatus = false;

            if (articleCategory == null)
            {
                result.StrErrMsg = "调用接口【AddArticleCategory】新增文章分类错误，传入对象参数为null";
                logInfo.Info(result.StrErrMsg);
                return result;

            }

            //执行插入脚本
            string strSql = @"INSERT INTO article_category(parent_id,article_category_name)
                                                    VALUES(@parent_id,@article_category_name)";
            //参数数组
            //SQLiteParameter[] paras = null;
            try
            {
                
                Database database = new SqliteDatabase(DefaultDBConn.DefaultDbConnectionString);
                DbCommand command = database.GetSqlStringCommand(strSql);
                database.AddInParameter(command, "@parent_id", DbType.Int32, articleCategory.ParentId);
                database.AddInParameter(command, "@article_category_name", DbType.String, articleCategory.ArticleCategoryName);

                if (database.ExecuteNonQuery(command) > 0)
                {
                    result.ExcutRetStatus = true;
                }

            }
            catch (Exception ex)
            {
                result.StrErrMsg = string.Format("调用接口【AddArticleCategory】新增文章分类异常，原因：{0}", ex.Message);
                logInfo.Error(result.StrErrMsg);
            }

            return result;
        }

        public IDictionary<int, string> GetAllArticleCategories()
        {
            logInfo.Info("调用接口【GetAllAricleCategories】查询所有的文章分类");

            IDictionary<int, string> articleCategoryDic = null;

            string strSql = @"SELECT article_category_id,article_category_name
                             FROM article_category";
            try
            {
                articleCategoryDic = new Dictionary<int, string>();

                Database database = new SqliteDatabase(DefaultDBConn.DefaultDbConnectionString);
                DbCommand command = database.GetSqlStringCommand(strSql);

                using (IDataReader reader = database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        articleCategoryDic.Add(reader.GetInt32(0), reader.GetString(1));
                    }
                }             
            }
            catch (Exception ex)
            {
                logInfo.ErrorFormat("调用接口【GetAllAricleCategories】查询所有的文章分类异常，原因：{0}", ex.Message);
            }

            return articleCategoryDic;
        }
    }
}
