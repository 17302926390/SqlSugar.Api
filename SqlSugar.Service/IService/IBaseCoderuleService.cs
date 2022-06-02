using Sqlsugar.Domain;
using Sqlsugar.Domain.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.Service.IService
{
    public interface IBaseCoderuleService
    {
        Task<IEnumerable<BaseCoderule>> GetBaseCoderuleListAsync();
    }
}
