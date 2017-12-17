using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{


    public class LoadOrder : LinkedList<LoadableContent>
    {
        public LoadOrder(LoadOrderDefinition orderDefinition) : base()
        {
            foreach (var definition in orderDefinition)
            {
                LoadableContent content = LoadableResource.All.FirstOrDefault(item => item.Description.ID == definition);
                if (content != null)
                {
                    AddLast(content);
                }
            }
        }

        public LoadOrderDefinition ToDefinition()
        {
            LoadOrderDefinition definition = new LoadOrderDefinition(this.Select(content => content.OriginalDescription.ID));
            return definition;
        }
    }
}
