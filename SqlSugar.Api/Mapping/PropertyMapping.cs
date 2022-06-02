using AutoMapper;

namespace SqlSugar.Api.Mapping
{
    /// <summary>
    ///  配置构造函数，用来创建关系映射
    /// </summary>
    public class PropertyMapping : Profile
    {
        public PropertyMapping() {
            CreateMap<object, object>();

        }
      
    }
}
