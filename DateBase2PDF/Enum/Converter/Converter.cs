using System;
using System.Collections.Generic;
using System.Text;

namespace DateBase2PDF
{
    /// <summary>
    /// 转换器
    /// </summary>
    public abstract class Converter<T>
    {
        /// <summary>
        /// 根据枚举获取对应的字符串表达方式
        /// </summary>
        public abstract string GetString(T t);

        /// <summary>
        /// 根据字符串得到对应的枚举表达方式
        /// </summary>
        public abstract T GetEnum(string s);
    }
}
