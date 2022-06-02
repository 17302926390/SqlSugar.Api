using System.Runtime.Serialization;

namespace SqlSugar.Api.Response
{
    /// <summary>
    /// 接口输出结果实体
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [DataContract]
    public class ResponseResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember]
        public int Code { get; private set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [DataMember]
        public string Message { get; private set; }

        /// <summary>
        /// 数据
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public T Data { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int? Total { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        public ResponseResult()
        {

        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public ResponseResult(int code, string message, T data)
            : this()
        {
            Code = code;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        public void Success()
        {
            Code = 0;
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="message">消息</param>
        public void Success(string message)
        {
            Message = message;
            Success();
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="message">消息</param>
        public void Success(T data, string message = null)
        {
            Data = data;
            Message = message ?? string.Empty;
            Success();
        }

        /// <summary>
        /// 操作成功，分页数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <param name="message"></param>
        public void Success(T data, int total, string message = null)
        {
            Data = data;
            Total = total;
            Message = message ?? string.Empty;
            Success();
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        public void Fail()
        {
            Code = -1;
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="message">消息</param>
        public void Fail(string message)
        {
            Message = message;
            Fail();
        }
    }
}
