using System;
using System.IO;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// I/O操作封装类
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// 创建或者更新本地文件的数据
        /// </summary>
        /// <param name="localFilePath"></param>
        /// <param name="txtStr"></param>
        public static void CreateOrOverrideFile(string localFilePath, string txtStr)
        {
            //创建并保存数据
            using (FileStream fs = File.Create(localFilePath))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(txtStr);
                fs.Write(info, 0, info.Length);
            }            
        }        
    }
}
