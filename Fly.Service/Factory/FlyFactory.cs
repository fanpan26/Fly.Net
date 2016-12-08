using Fly.IService;
using Fly.IService.MessageTemplate;
using Fly.Service.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Service.Factory
{
    public class FlyFactory
    {
        public static IArticleService CreateArticleService()
        {
            IArticleService service = new ArticleService();
            return service;
        }

        public static ICategoryService CreateCategoryService()
        {
            ICategoryService service = new CategoryService();
            return service;
        }

        public static IUserService CreateUserService()
        {
            IUserService service = new UserService();
            return service;
        }

        public static IFlyUserProvider CreateUserProvider()
        {
            IFlyUserProvider provider = new FlyUserProvider();
            return provider;
        }

        public static IVisitorService CreateVisitorService()
        {
            IVisitorService service = new VisitorService();
            return service;
        }

        public static ICommentService CreateCommentService()
        {
            ICommentService service = new CommentService();
            return service;
        }

        public static IUserMessageService CreateUserMessageService()
        {
            IUserMessageService service = new UserMessageService();
            return service;
        }

        public static IMsgTemplate CreateMessageTemplate()
        {
            return new Message.MessageTemplate();
        }
    }
}
