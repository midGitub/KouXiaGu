using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源读取顺序(不包括核心资源);
    /// </summary>
    public interface IReadOnlyModificationOrder
    {
        int Count { get; }
        IEnumerable<ModificationInfo> EnumerateModification();
    }

    /// <summary>
    /// 资源读取顺序(不包括核心资源);
    /// </summary>
    public class ModificationOrder : IReadOnlyModificationOrder, IEnumerable<string>
    {
        public LinkedList<ModificationInfo> List { get; private set; }
        public int Count => List.Count;

        public ModificationOrder()
        {
            List = new LinkedList<ModificationInfo>();
        }

        public ModificationOrder(IReadOnlyCollection<ModificationInfo> contents)
        {
            List = new LinkedList<ModificationInfo>(contents);
        }

        public void Add(string id)
        {
            foreach (var info in Modification.All)
            {
                if (info.Description.ID == id)
                {
                    List.AddLast(info);
                    break;
                }
            }
        }

        public IEnumerable<ModificationInfo> EnumerateModification()
        {
            return List;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return List.Select(item => item.Description.ID).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ModificationOrderSerializer : ContentSerializer<ModificationOrder>
    {
        [PathDefinition(PathDefinitionType.Config, "资源读取顺序定义")]
        public override string RelativePath
        {
            get { return "ModificationOrder.xml"; }
        }

        private readonly XmlSerializer<ModificationOrder> xmlSerializer = new XmlSerializer<ModificationOrder>();

        public override ISerializer<ModificationOrder> Serializer
        {
            get { return xmlSerializer; }
        }
    }
}
