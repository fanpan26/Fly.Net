using Fly.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fly.Model.Article;
using Fly.Model.Common;

namespace Fly.Service.Dapper
{
    public class CategoryService : ICategoryService
    {
        public bool Add(Category model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> Query(string sql)
        {
            sql = sql ?? "SELECT [id],[name] FROM [dbo].[fly_article_category]";
            return DB.DBHelper.Query<Category>(sql);
        }

        public PageView<Category> QueryPageList(PageSearchOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
