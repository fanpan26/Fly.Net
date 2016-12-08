using Fly.Model;
using Fly.Model.Article;
using Fly.Model.Common;
using Fly.Model.Enum;
using Fly.Model.Msg;
using Fly.Model.Result;
using Fly.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.IService
{
    /// <summary>
    /// 基础接口，增删改查
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDaoService<T> where T:class,new()
    {
        /// <summary>
        /// 添加一个问题
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        bool Add(T model);

        PageView<T> QueryPageList(PageSearchOptions options);

        IEnumerable<T> Query(string sql);
    }

    /// <summary>
    /// 查询多表集合，返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDaoMultipleService<T> where T : class, new()
    {
        T QueryMultiple(string sql, object param);
    }


    /// <summary>
    /// 文章
    /// </summary>
    public interface IArticleService : IDaoService<ArticleView>
    {
        IEnumerable<ArticleDisplayViewModel> QueryTop10(int count);

        ArticleView QueryOne(int artid);

        bool UpdateArticleTop(int artid,int toUserId, bool isTop);
        bool UpdateArticleCream(int artid,int toUserId, bool isCream);
    }
    /// <summary>
    /// 分类
    /// </summary>
    public interface ICategoryService : IDaoService<Category>
    {

    }
    /// <summary>
    /// 用户
    /// </summary>
    public interface IUserService : IDaoService<User>, IDaoMultipleService<UserHomeViewModel>
    {
        User Login(string account, string pwd);

        void CacheTokenAfterLogin(User user);

        bool AccountExist(string account);

        RegisterResult Register(User user);

        PageView<ArticleView> QueryUserArticles(int pageIndex,int userid, int pageSize = 20);

        User QueryOne(int userid);

        IEnumerable<UserVisitor> GetVisitors(int userid);

        bool Update(User user,UpdateUserType type);

        bool UpdatePwd(string oldPwd, string newPwd,int userid);
    }

    public interface IVisitorService : IDaoService<Visitor>
    {

    }

    public interface ICommentService : IDaoService<Comment>
    {
        /// <summary>
        /// 查询某个用户在某个文章里的对底部的答案的点赞
        /// </summary>
        /// <param name="artid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        IEnumerable<CommentRelation> QueryUserCommentRecommend(int artid, int userid);

        IEnumerable<ArticleCommentViewModel> QueryArticleComment(int artid,int userid);

        bool AddCommentRecomand(int commentid, int userid,int artid,int touserid);
    }

    public interface IUserMessageService : IDaoService<UserMessage>
    {
        IEnumerable<UserMessageViewModel> QueryMessages(int userid);

        bool DeleteMessage(int userid, int id);
    } 
}
