using Sqlsugar.Domain;
using Sqlsugar.Domain.Model;
using Sqlsugar.Repositories.IRepositories;
using SqlSugar.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.Service.Service
{
    public class BaseCoderuleService: IBaseCoderuleService
    {
        private IBaseCoderuleRepositories _student;
        public BaseCoderuleService(IBaseCoderuleRepositories student) {

            _student = student;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BaseCoderule>> GetBaseCoderuleListAsync()
        {
            var entity = await _student.Query();
            return entity;
        }
    }
}
