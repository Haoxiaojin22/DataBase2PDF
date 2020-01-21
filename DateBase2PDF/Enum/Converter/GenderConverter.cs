using System;
using System.Collections.Generic;
using System.Text;

namespace DateBase2PDF
{
    public class GenderConverter : Converter<Gender>
    {
        public static GenderConverter Instance { get; set; }

        static GenderConverter()
        {
            Instance = new GenderConverter();
        }

        public override Gender GetEnum(string s)
        {
            throw new NotImplementedException();
        }

        public override string GetString(Gender t)
        {
            switch(t)
            {
                case Gender.Female:
                    {
                        return "女";
                    }

                case Gender.Male:
                    {
                        return "男";
                    }
                default:
                    {
                        return "未知";
                    }
            }
        }
    }
}
