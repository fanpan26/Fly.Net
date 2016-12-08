using Fly.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fly.Model;
using Fly.Model.Common;
using Fly.Service.DB;
using Fly.Util.Config;
using Fly.Util.Encrypt;
using Dapper;
using Fly.Util.Token;
using Fly.Util.Cookie;
using Fly.Util;
using Fly.Model.Result;
using Fly.Model.ViewModel;
using Fly.Model.Enum;

namespace Fly.Service.Dapper
{
    public class UserService : IUserService
    {
        /// <summary>
        /// 判断用户账号是否已经存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool AccountExist(string account)
        {
            string sql = "select count(1) from dbo.fly_user_account where account=@account";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@account", account, System.Data.DbType.String);

            int count = DBHelper.ExecuteScalar<int>(sql, parameters);
            return count > 0;
        }

        /// <summary>
        /// 添加一个用户，注册使用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(User model)
        {
            Dictionary<String, Object> param = new Dictionary<string, object>();
            //添加用户
            string defaultPhoto = ConfigHelper.GetDefaultPhoto();
            string pwd = EncryptHelper.StringToMD5(model.pwd);
            //做一下密码处理
            param.Add("INSERT INTO dbo.fly_user (nickname,photo) VALUES (@nickname  ,'" + defaultPhoto + "')", new { nickname = model.nickname });
            //添加用户账户
            param.Add("INSERT INTO dbo.fly_user_account (userid,account,pwd) VALUES  (@@IDENTITY,@account,@pwd)", new { account = model.account, pwd = pwd });
            //执行带事物的SQL
            bool result = DBHelper.Execute(param);

            return result;
        }

        public void CacheTokenAfterLogin(User user)
        {
            var tokenUser = new User
            {
                userid = user.userid,
                account = user.account,
                pwd = user.pwd
            };
            string token = TokenHelper.CreateToken(tokenUser);

            CookieHelper.WriteCookie(FlyConst.FLY_USER_TOKEN_KEY, token);
        }

        public IEnumerable<UserVisitor> GetVisitors(int userid)
        {
            string sql = @"
SELECT  userid,A.nickname,A.photo,addtime
FROM    (SELECT ROW_NUMBER() OVER (PARTITION BY visitor ORDER BY addtime DESC) AS rowid,*
         FROM   [Fly].[dbo].[V_Fly_UserVisitor]
         WHERE  mainid=@userid
        ) A
WHERE   A.rowid=1
ORDER BY A.addtime DESC;";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@userid", userid);
            return DBHelper.Query<UserVisitor>(sql, parameter, System.Data.CommandType.Text);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public User Login(string account, string pwd)
        {
            string md5Pwd = EncryptHelper.StringToMD5(pwd);
            string sql = "SELECT A.*,B.nickname,B.photo FROM dbo.fly_user_account A LEFT JOIN dbo.fly_user B ON A.userid = B.id WHERE account=@account AND pwd =@pwd";
            DynamicParameters param = new DynamicParameters();
            param.Add("@account", account,System.Data.DbType.String);
            param.Add("@pwd", md5Pwd,System.Data.DbType.String);
            var user = DBHelper.Query<User>(sql, param, System.Data.CommandType.Text);
            if (user.Count() == 0) {
                return null;
            }
            User u = user.First();
            CacheTokenAfterLogin(u);
            u.pwd = "";
            return u;
        }

        public IEnumerable<User> Query(string sql)
        {
            throw new NotImplementedException();
        }

        public UserHomeViewModel QueryMultiple(string sql, object param)
        {
            sql = @"SELECT A.*,A.id as userid,B.score FROM dbo.fly_user A LEFT JOIN fly_user_score B ON A.id=B.userid WHERE A.id=@userid

                    SELECT TOP 10 id,title,istop,isover,iscream,visitor_count,comment_count,addtime FROM dbo.V_Fly_Article WHERE userid=@userid ORDER BY addtime DESC

                    SELECT TOP 10 artid,title,comment,toid,touserid,tonickname,tocomment,addtime FROM dbo.V_Fly_Article_Comment WHERE userid=@userid ORDER BY addtime desc";

            IMultipleHandler<UserHomeViewModel> handler = new UserHomeMultipleHandler();

            return DBHelper.QueryMultiple(sql, param, handler);
        }

        public User QueryOne(int userid)
        {
            string sql = @"SELECT A.*,A.id as userid,B.score,c.account FROM dbo.fly_user A LEFT JOIN fly_user_score B ON A.id=B.userid
LEFT JOIN dbo.fly_user_account C ON a.id = c.userid
 WHERE A.id = @userid";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@userid", userid);
            return DBHelper.Query<User>(sql, parameter, System.Data.CommandType.Text).FirstOrDefault();
        }

        public PageView<User> QueryPageList(PageSearchOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 分页查询用户的问题
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageView<ArticleView> QueryUserArticles(int pageIndex,int userid, int pageSize = 20)
        {
            PageSearchOptions options = new PageSearchOptions()
            {
                Condition = "userid=" + userid,
                Fields = "id,title,istop,isover,iscream,visitor_count,comment_count,addtime",
                PageIndex = pageIndex,
                PageSize = pageSize,
                PrimaryKey = "id",
                Sort = "addtime desc",
                TableName = "V_Fly_Article"
            };
            return Page.PageHelper.GetPageList<ArticleView>(options);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public RegisterResult Register(User user)
        {
            bool exist = AccountExist(user.account);
            if (exist) {
                return new RegisterResult
                {
                    msg = "账号已经存在",
                    success = false
                };
            }
            bool reg = Add(user);
            User userResult = null;
            if (reg)
            {
                //自动登录
                userResult = Login(user.account, user.pwd);
            }

            return new RegisterResult
            {
                success = reg,
                msg = reg ? "注册成功" : "注册失败",
                user = userResult
            };

        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Update(User user, UpdateUserType type)
        {
            string sql = string.Empty;
            var parameter = new DynamicParameters();
            switch (type)
            {
                case UpdateUserType.Password:
                    sql = "UPDATE dbo.fly_user_account SET pwd = @pwd WHERE userid =(SELECT userid FROM dbo.fly_user_account WHERE userid=@userid AND pwd=@pwd)";
                    //这里pwd需要转为md5
                    string pwd = EncryptHelper.StringToMD5(user.pwd);
                    parameter.Add("@pwd",pwd);
                    break;
                case UpdateUserType.Photo:
                    sql = "UPDATE dbo.fly_user SET photo=@photo WHERE id=@userid";
                    parameter.Add("@photo", user.photo);
                    break;
                case UpdateUserType.Info:
                    sql = "UPDATE dbo.fly_user SET city=@city,sex=@sex,nickname=@nickname, sign=@sign WHERE id=@userid";
                    parameter.Add("@city", user.city);
                    parameter.Add("@nickname", user.nickname);
                    parameter.Add("@sign", user.sign);
                    parameter.Add("@sex", user.sex);
                    break;
                default:
                    throw new Exception("wrong type");
            }
            parameter.Add("@userid", user.userid);

            return DBHelper.Execute(sql, parameter);
        }

        public bool UpdatePwd(string oldPwd, string newPwd, int userid)
        {
            string sql = " UPDATE dbo.fly_user_account SET pwd = @newPwd WHERE userid = (SELECT userid FROM dbo.fly_user_account WHERE userid = @userid AND pwd = @oldPwd)";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@newPwd",EncryptHelper.StringToMD5(newPwd));
            parameter.Add("@oldPwd", EncryptHelper.StringToMD5(oldPwd));
            parameter.Add("@userid", userid);
            return DBHelper.Execute(sql, parameter);
        }
    }
}
