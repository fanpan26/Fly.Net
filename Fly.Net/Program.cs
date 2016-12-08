using Fly.IService;
using Fly.Service;
using Fly.Service.Dapper;
using Fly.Service.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Net
{
    class Program
    {

        static void Main(string[] args)
        {
            IUserService userService = FlyFactory.CreateUserService();
            //登录测试
            userService.Login("zhangsan", "123123");
            //注册测试
            //userService.Add(new Model.User
            //{
            //    account = "zhangsan",
            //    nickname = "小盘子",
            //    pwd = "123123"
            //});
            Console.WriteLine("over");
            Console.Read();
        }
    }
}
