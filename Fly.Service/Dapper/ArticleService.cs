using Fly.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fly.Model.Article;
using Fly.Service.DB;
using Fly.Model.Common;
using Fly.Service.Page;
using Fly.Model.ViewModel;
using Dapper;
using Fly.Service.Factory;
using Fly.Model.Enum;

namespace Fly.Service.Dapper
{
    public class ArticleService : IArticleService
    {
        /// <summary>
        /// 添加一条新闻
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool Add(ArticleView article)
        {
           
            Dictionary<String, Object> param = new Dictionary<string, object>();
            //添加文章
            param.Add("INSERT INTO dbo.fly_article (userid,category,title,artcontent) VALUES  (@userid ,@category ,@title  ,@artcontent)", article);
            //添加文章设置
            param.Add("INSERT INTO dbo.fly_article_setting (artid,istop,iscream,isover,reward) VALUES  (@@identity  ,0 ,0 ,0 ,@reward )", new { reward = article.reward });
            //执行带事物的SQL
            bool result = DBHelper.Execute(param);

            return result;
        }

        public IEnumerable<ArticleView> Query(string sql)
        {
            throw new NotImplementedException();
        }

        public ArticleView QueryOne(int artid)
        {
            string sql = "select * from [dbo].[V_Fly_Article] where id=@artid";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@artid", artid, System.Data.DbType.Int32);
            var result = DBHelper.Query<ArticleView>(sql, parameter, System.Data.CommandType.Text);
            if (result.Count() > 0) {
                return result.First();
            }
            return new ArticleView();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public PageView<ArticleView> QueryPageList(PageSearchOptions options)
        {
            options.PrimaryKey = "id";
            options.TableName = "V_Fly_Article";
            options.Fields = "*";
            options.Condition = "1=1";
            options.Sort = "istop desc,comment_count desc,addtime desc";
            var result = PageHelper.GetPageList<ArticleView>(options);
            return result;
        }

        /// <summary>
        /// 查询查看量和评论量最多的问题
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<ArticleDisplayViewModel> QueryTop10(int count=10)
        {
            string sql = $@"
SELECT id AS artid,A.artcount,t AS arttype,A.title FROM (
SELECT TOP {count}
        A.id,A.comment_count AS artcount,1 AS t,B.title
FROM    dbo.V_Fly_Article_CommentSummary A
LEFT JOIN dbo.fly_article B ON A.id=B.id
UNION ALL
SELECT TOP {count}
        A.id,A.visitor_count AS artcount,2 AS t,B.title
FROM    dbo.V_Fly_Visitor_Summary A
LEFT JOIN dbo.fly_article B ON A.id=B.id where A.vtype=2
) A ORDER BY t,A.artcount desc";
            return DBHelper.Query<ArticleDisplayViewModel>(sql);
        }

        public bool UpdateArticleCream(int artid, int toUserId, bool isCream)
        {
            bool result = UpdateArticle(artid, toUserId, isCream, 2);
            return result;
        }

        public bool UpdateArticleTop(int artid, int toUserId, bool isTop)
        {
            bool result = UpdateArticle(artid, toUserId, isTop, 1);
            return result;
        }

        private bool UpdateArticle(int artid, int toUserId, bool value, int type)
        {
            const int TopType = 1;
            string val = type == TopType ? "istop=@value" : "iscream=@value";
            string sql = "UPDATE dbo.fly_article_setting SET " + val + " WHERE artid=@artid";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@value", value);
            parameter.Add("@artid", artid);
            bool result = DBHelper.Execute(sql, parameter);

            if (result)
            {
                MsgTemplateType tempType = MsgTemplateType.AdminAddCream;
                if (type == 1)
                {
                    tempType = value ? MsgTemplateType.AdminAddTop : MsgTemplateType.AdminCancelTop;
                }
                else
                {
                    tempType = value ? MsgTemplateType.AdminAddCream : MsgTemplateType.AdminCancelCream;
                }
                IUserMessageService msgService = FlyFactory.CreateUserMessageService();
                msgService.Add(new Model.Msg.UserMessage()
                {
                    artid = artid,
                    fromuser = 0,
                    touser = toUserId,
                    msgtype = tempType
                });
            }

            return result;
        }
    }
}
