using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.Models
{
    public class ProjectRecommend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserAvatar { get; set; }


        public ERecommendType RecommendType { get; set; }

        public int ProjectId { get; set; }
        public string ProjectAvatar { get; set; }
        public string Company { get; set; }
        /// <summary>
        /// 项目介绍
        /// </summary>
        public string Introduction { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinState { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime RecommendTime { get; set; }
    }

    public enum ERecommendType:int
    {
        /// <summary>
        /// 系统推荐
        /// </summary>
        Platform = 1,
        /// <summary>
        /// 好友推荐
        /// </summary>
        Friend = 2,
        /// <summary>
        /// 二度好友推荐
        /// </summary>
        FriendOfAFriend = 3
    }
}
