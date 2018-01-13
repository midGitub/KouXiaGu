using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源读取顺序(包括核心资源);
    /// </summary>
    public interface ILoadOrder : IEnumerable<ModificationContent>
    {
        int Count { get; }
    }

    /// <summary>
    /// 资源读取顺序,不包括核心资源;
    /// </summary>
    public class ModificationOrder : ILoadOrder, IEnumerable<ModificationContent>
    {
        public LinkedList<ModificationContent> List { get; private set; }
        public int Count => List.Count;

        public ModificationOrder(IReadOnlyCollection<ModificationContent> contents, LoadOrderDefinition orderDefinition) : base()
        {
            List = new LinkedList<ModificationContent>();
            foreach (var definition in orderDefinition)
            {
                ModificationContent content = contents.FirstOrDefault(item => item.Description.ID == definition);
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

        public IEnumerator<ModificationContent> GetEnumerator()
        {
            return ((IEnumerable<ModificationContent>)List).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ModificationContent>)List).GetEnumerator();
        }
    }
}
