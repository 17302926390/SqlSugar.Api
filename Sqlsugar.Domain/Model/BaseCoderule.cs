using SqlSugar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlsugar.Domain.Model
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("BASE_CODERULE")]
    public class BaseCoderule
    {
        /// <summary>
        /// 唯一编号 
        ///</summary>
        [SugarColumn(ColumnName = "CODENUMBER", IsPrimaryKey = true, IsIdentity = true)]
        public string CODENUMBER { get; set; }
        /// <summary>
        /// 表名 
        ///</summary>
        [SugarColumn(ColumnName = "TABLENAME")]
        public string TABLENAME { get; set; }
        /// <summary>
        /// 首位字符（2位字符）【录入】 
        ///</summary>
        [SugarColumn(ColumnName = "FRISTPLACE")]
        public string FRISTPLACE { get; set; }
        /// <summary>
        /// 中间日期格式 yyMMdd 【公用方法】 
        ///</summary>
        [SugarColumn(ColumnName = "CENTRALDATA")]
        public string CENTRALDATA { get; set; }
        /// <summary>
        /// 中间流水号占位长度（数字+字母）【公用方法】 
        ///</summary>
        [SugarColumn(ColumnName = "CENTRALNUMBER")]
        public string CENTRALNUMBER { get; set; }
    }
}
