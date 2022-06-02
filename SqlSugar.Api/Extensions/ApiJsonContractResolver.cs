using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace SqlSugar.Api.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiJsonContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// 解析属性名
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (propertyName.Length is 0)
            {
                return string.Empty;
            }

            if (propertyName.Contains("_"))
            {// 名称中带有下划线的转成全小写
                return propertyName.ToLower();
            }
            else if (propertyName.All(char.IsUpper))
            {// 名称全大写的转成全小写
                return propertyName.ToLower();
            }
            else
            {// PascalCasing 风格的只把第一个字符转成小写
                return $"{char.ToLower(propertyName[0])}{string.Join(string.Empty, propertyName.Skip(1))}";
            }
        }
    }
}
