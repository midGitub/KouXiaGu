using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectTerrain
{

    public struct Description<T>
    {
        public LoadableContent Content { get; private set; }
        public T Value { get; private set; }

        public Description(LoadableContent content, T value)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            Content = content;
            Value = value;
        }
    }
}
