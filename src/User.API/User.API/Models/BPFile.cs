using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class BPFile//商业计划书，会单独进行查询
    {
        /// <summary>
        /// BP Id 
        /// </summary>
        public int Id { get; set; }//这里的Id使用自增的方式，这样在同步的时候会有问题，并且也容易被人猜测出来
        /// <summary>
        /// 用户Id
        /// </summary>
        public int  UserId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public String FileName { get; set; }
        /// <summary>
        /// 上传元文件的地址
        /// </summary>
        public string OriginFilePath{ get; set; }
        /// <summary>
        /// 格式转化后的文件地址
        /// </summary>
        public string FormateFilePath { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}
