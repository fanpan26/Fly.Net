using Fly.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fly.Model.Article;
using Fly.Model.Common;
using Fly.Service.DB;
using Dapper;
using Fly.Model.ViewModel;
using Fly.Service.Message;

namespace Fly.Service.Dapper
{
    public class CommentService : ICommentService
    {
        /// <summary>
        /// 用户添加一条评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(Comment model)
        {
            string sql = "INSERT INTO dbo.fly_article_comment (userid,artid,toid,comment) VALUES (@userid,@artid,@toid ,@comment)";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add(sql, new { userid = model.UserId, artid = model.ArtId, toid = model.ToCommentId, comment = model.Content });
            var result = DBHelper.Execute(param);

            AfterAdd(model, result);
            return result;
        }

        /// <summary>
        /// 发送消息后的操作
        /// </summary>
        /// <param name="model"></param>
        /// <param name="result"></param>
        private void AfterAdd(Comment model,bool result)
        {
            if (result)
            {
                new MessageSender().SendMessage(new Model.Msg.UserMessage
                {
                    addtime = DateTime.Now,
                    artid = model.ArtId,
                    fromuser = model.UserId,
                    touser = model.ToUserId,
                    msg = null,
                    //如果有回复id，则为回复解答，否则解答文章，收消息人的对象不同
                    msgtype = model.ToCommentId > 0 ? Model.Enum.MsgTemplateType.NewReply : Model.Enum.MsgTemplateType.NewComment
                });
            }
        }

        /// <summary>
        /// 用户点赞
        /// </summary>
        /// <param name="commentid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool AddCommentRecomand(int commentid, int userid,int artid,int touserid)
        {
            string sql = @"IF EXISTS (SELECT id FROM dbo.fly_comment_recommand WHERE commentid=@commentid AND userid=@userid)
 BEGIN
	DELETE FROM dbo.fly_comment_recommand WHERE commentid=@commentid AND userid=@userid
  END
  ELSE
  BEGIN
	INSERT INTO dbo.fly_comment_recommand (commentid,userid,addtime) VALUES  (@commentid ,@userid ,GETDATE())
  end";
            DynamicParameters param = new DynamicParameters();
            param.Add("@commentid", commentid, System.Data.DbType.Int32);
            param.Add("@userid", userid, System.Data.DbType.Int32);
            var result = DBHelper.Execute(sql, param);
            AfterAddCommentRecomand(result, userid, artid, touserid);
            return result;
        }

        private void AfterAddCommentRecomand(bool result,int userid, int artid, int touserid)
        {
            if (result) {
                //发送消息（这么设计有点不合理）
                new MessageSender().SendMessage(new Model.Msg.UserMessage
                {
                    artid = artid,
                    fromuser = userid,
                    touser = touserid,
                    msgtype = Model.Enum.MsgTemplateType.CommentRecommand
                });
            }
        }

        public IEnumerable<Comment> Query(string sql)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ArticleCommentViewModel> QueryArticleComment(int artid, int userid)
        {
            string sql = "SELECT * FROM dbo.V_Fly_Article_Comment WHERE artid =@artid ORDER BY addtime asc";
            DynamicParameters param = new DynamicParameters();
            param.Add("@artid", artid);
            IEnumerable<ArticleCommentViewModel> result = DBHelper.Query<ArticleCommentViewModel>(sql, param, System.Data.CommandType.Text);
            //
            if (userid == 0)
            {
                return result;
            }
            IEnumerable<CommentRelation> relation = QueryUserCommentRecommend(artid, userid);

            List<ArticleCommentViewModel> finalResult = new List<ArticleCommentViewModel>();
            foreach (ArticleCommentViewModel model in result)
            {
                //该条自己有没有推荐过（点赞）
                model.selfrecommend = relation.Any(x => x.commentid == model.id && x.userid == userid);
                //该条是不是自己发布的
                model.isself = model.userid == userid;
                model.isadmin = model.publisher == userid;//是否是本人查看，如果是admin=true
                finalResult.Add(model);
            }

            return finalResult;

        }

        public PageView<Comment> QueryPageList(PageSearchOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询某个用户在某个文章下的已经点赞的答案
        /// </summary>
        /// <param name="artid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IEnumerable<CommentRelation> QueryUserCommentRecommend(int artid, int userid)
        {
            string sql = "SELECT A.userid,A.commentid FROM dbo.fly_comment_recommand A LEFT JOIN dbo.fly_article_comment B ON A.commentid=B.id WHERE A.userid=@userid AND B.artid=@artid";
            DynamicParameters param = new DynamicParameters();
            param.Add("@userid", userid);
            param.Add("@artid", artid);
            return DBHelper.Query<CommentRelation>(sql, param, System.Data.CommandType.Text);
        }
    }
}
