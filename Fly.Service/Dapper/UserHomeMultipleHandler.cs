using Fly.Model;
using Fly.Model.ViewModel;
using Fly.Service.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Fly.Service.Dapper
{
    public class UserHomeMultipleHandler : IMultipleHandler<UserHomeViewModel>
    {
        public UserHomeViewModel Handle(GridReader reader)
        {
            var model = new UserHomeViewModel
            {
                user = reader.Read<User>().FirstOrDefault(),
                articleRecords = reader.Read<ArticleView>(),
                commentRecords = reader.Read<ArticleCommentViewModel>()
            };
            return model;
        }
    }
}
