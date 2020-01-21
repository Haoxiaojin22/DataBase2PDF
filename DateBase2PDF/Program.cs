using iTextSharp.text.pdf;
using log4net;
using log4net.Config;
using log4net.Repository;
using Sino.Dapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace DateBase2PDF
{
    class Program
    {
		public static IDapperConfiguration dapperConfiguration = new DapperConfiguration()
		{
			WriteConnectionString = "Data Source=localhost;port=3306;user id=root;password=123456;Initial Catalog=test;convertzerodatetime=True;Charset=utf8;",
			ReadConnectionString = "Data Source=localhost;port=3306;user id=root;password=123456;Initial Catalog=test;convertzerodatetime=True;Charset=utf8;"
		};

		public static IPeopleReporitory PeopleReporitory = new PeopleReporitory(dapperConfiguration);
		static void Main(string[] args)
        {
			//中文字体包iTextAsianCmaps.dll与iTextAsian.dll
			BaseFont.AddToResourceSearch(Path.Combine(AppContext.BaseDirectory, "iTextAsian4Core.dll"));
			BaseFont.AddToResourceSearch(Path.Combine(AppContext.BaseDirectory, "iTextAsianCmaps4Core.dll"));

			//取全部列表
			List<People> peopleList = PeopleReporitory.GetPeoples().Result;
			foreach(var item in peopleList)
			{
				//读取和解析PDF文档
				PdfReader reader = new PdfReader("people.pdf");

				//创建文件流用来保存填充模板后的文件,为系统内存提供流式的读写操作。常作为其他流数据交换时的中间对象操作
				MemoryStream ms = new MemoryStream();

				//向现有PDF文档添加额外内容
				PdfStamper stamp = new PdfStamper(reader, ms);

				//获取AcroFields对象，该对象允许获取和设置字段值并合并FDF表单
				AcroFields form = stamp.AcroFields;

				//表单文本框是否锁定
				stamp.FormFlattening = true;

				Dictionary<string, string> keyValues = new Dictionary<string, string>();
				keyValues.Add("1", item.Id.ToString());
				keyValues.Add("2", item.Name);
				keyValues.Add("3", GenderConverter.Instance.GetString(item.Gender));
				keyValues.Add("4", item.Age.ToString());

				//填充表单,para为表单的一个（属性-值）字典
				foreach (KeyValuePair<string, string> parameter in keyValues)
				{
					//要输入中文就要设置域的字体;
					//form.SetFieldProperty(parameter.Key, "textfont", baseFont, null);
					//为需要赋值的域设置值;
					form.SetField(parameter.Key, parameter.Value);
				}
				
				stamp.Close();
				reader.Close();
				var now = DateTime.Now;
				string path = string.Format("\\ID{0}", item.Id);
				FileStream fs = new FileStream(@"E:\hxj\PDF" + path + "myData.pdf", FileMode.Create);
				fs.Write(ms.GetBuffer(), 0,(int)ms.Position);

				ms.Close();
				fs.Close();
				
			}

			Console.WriteLine("Hello World!");
        }
    }
}
