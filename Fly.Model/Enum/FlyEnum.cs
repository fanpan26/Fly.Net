using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.Enum
{
    /// <summary>
    /// 访客类型
    /// </summary>
    public enum VisitorType
    {
        User = 1,
        Article = 2
    }

    public enum UpdateUserType
    {
        Password = 1,
        Photo = 2,
        Info = 3
    }

    public enum MsgTemplateType
    {
        /// <summary>
        /// 新的解答
        /// </summary>
        NewComment = 0,
        /// <summary>
        /// 新的评论
        /// </summary>
        NewReply = 1,
        /// <summary>
        /// 管理员删贴
        /// </summary>
        AdminDelete = 2,
        /// <summary>
        /// 管理员加精
        /// </summary>
        AdminAddCream = 3,
        /// <summary>
        /// 管理员置顶
        /// </summary>
        AdminAddTop = 4,
        /// <summary>
        /// 管理员恢复帖子
        /// </summary>
        AdminCancelDelete = 5,
        /// <summary>
        /// 管理员撤销精帖
        /// </summary>
        AdminCancelCream = 6,
        /// <summary>
        /// 管理员撤销置顶
        /// </summary>
        AdminCancelTop = 7,
        /// <summary>
        /// 管理员删除解答
        /// </summary>
        AdminDeleteComment = 8,
        /// <summary>
        /// 解答被采纳
        /// </summary>
        CommentAccepted = 9,
        /// <summary>
        /// 解答被点赞
        /// </summary>
        CommentRecommand = 10

    }


}
