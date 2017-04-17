using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    class AttributeExtensions
    {


        public static IEnumerable<T> GetCustomAttributes<T>(IEnumerable<Type> types, bool inherit)
            where T : Attribute
        {
            throw new NotImplementedException();
        }

    }

}
