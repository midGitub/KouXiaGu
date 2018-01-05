using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源读取顺序,不包括核心资源;
    /// </summary>
    public class LoadOrder : IEnumerable<LoadableContent>
    {
        public LinkedList<LoadableContent> List { get; private set; }
        public int Count => List.Count;

        public LoadOrder(IReadOnlyCollection<LoadableContent> contents, LoadOrderDefinition orderDefinition) : base()
        {
            List = new LinkedList<LoadableContent>();
            foreach (var definition in orderDefinition)
            {
                LoadableContent content = contents.FirstOrDefault(item => item.Description.ID == definition);
                if (content != null)
                {
                    List.AddLast(content);
                }
            }
        }

        public LoadOrderDefinition ToDefinition()
        {
            LoadOrderDefinition definition = new LoadOrderDefinition(List.Select(content => content.OriginalDescription.ID));
            return definition;
        }

        public IEnumerator<LoadableContent> GetEnumerator()
        {
            return ((IEnumerable<LoadableContent>)List).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<LoadableContent>)List).GetEnumerator();
        }
    }
}
