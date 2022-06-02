using Microsoft.AspNetCore.Mvc;
using Serilog;
using Sqlsugar.Infrastructure.Helpers;
using SqlSugar.Api.Response;
using System.Threading.Tasks;
namespace SqlSugar.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginController : BaseApiController
    {
        private IJWTService _iJWTService = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iJWTService"></param>
        public LoginController(IJWTService iJWTService) {
            _iJWTService = iJWTService;
        }
        /// <summary>
        ///  登录获取Token
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login(string name, string pwd)
        {
            var result = new ResponseResult<string>();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pwd))
            {
                string token = this._iJWTService.GetToken(name);
                Log.Logger.Information($"Token获取:{token}");
                result.Success(token,"查询成功");
            }
            else
            {
                result.Fail("请输入用户名或密码");
            }
            return Ok(result);
        }
    }
}
