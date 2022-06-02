using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Sqlsugar.Domain;
using SqlSugar.Api.Response;
using SqlSugar.Service.IService;
using Sqlsugar.Domain.Model;
using MySqlX.XDevAPI.Common;
using Microsoft.AspNetCore.Authorization;

namespace SqlSugar.Api.Controllers
{
   /// <summary>
   /// 示例接口
   /// </summary>
    public class BaseCoderuleController : BaseApiController
    {

        private readonly IBaseCoderuleService _studentService;
        public BaseCoderuleController(IBaseCoderuleService studentService) {

            _studentService=studentService;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<IEnumerable<BaseCoderule>>), 200)]
        public async Task<IActionResult> GetBaseCoderuleList()
        {
            var result = new ResponseResult<IEnumerable<BaseCoderule>>();
            var res =await _studentService.GetBaseCoderuleListAsync();
            result.Success(res);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add(StudentModel model)
        {
            int i = 0;
            var result = new ResponseResult<bool>();
            if (i == 0) {
                throw new Exception("sddasd");
            }
            result.Success(true);
            return Ok(result);

        }
    }
}
