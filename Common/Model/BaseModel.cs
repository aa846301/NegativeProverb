using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    /// <summary>
    /// API統一格式
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 狀態碼
        /// </summary>
        public string Code { get; set; } = "000";

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? Exception { get; set; }


    }

    /// <summary>
    /// API 回傳 泛型 包住資料結構
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseModel<T>
    {
        /// <summary>
        /// 輸出資料
        /// </summary>
        public T? Data { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 狀態碼
        /// </summary>
        public string Code { get; set; } = "000";

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? Exception { get; set; }




    }

}
