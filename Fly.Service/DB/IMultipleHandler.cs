using Fly.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using Dapper;

namespace Fly.Service.DB
{
    public interface IMultipleHandler<T>
    {
        T Handle(GridReader reader);
    }



}
