using Sino.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace DateBase2PDF
{
    public class People : FullAuditedEntity<int>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
    }
}
