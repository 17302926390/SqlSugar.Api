using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar.Api.Response;

namespace SqlSugar.Api.Controllers
{
    /// <summary>
    /// 公用api
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ResponseResult<object>), 200)]
   
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// 构建操作结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private ResponseResult<T> BuildResult<T>(bool isSuccess, T data = default, int total = 0, string msg = null)
        {
            var result = new ResponseResult<T>();
            if (isSuccess)
                result.Success(data, total, msg);
            else
                result.Fail(string.IsNullOrWhiteSpace(msg) ? "操作失败" : msg);
            return result;
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <param name="msg"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IActionResult Success<T>(T data, int total = 0, string msg = null)
        {
            return Ok(BuildResult(true, data, total, msg));
        }
        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected IActionResult Fail(string msg = null)
        {
            return Ok(BuildResult<object>(false, msg: msg));
        }
    }
}