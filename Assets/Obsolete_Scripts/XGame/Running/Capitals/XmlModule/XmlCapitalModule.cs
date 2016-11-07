using System.Xml.Linq;
using UnityEngine;
using XGame.XmlModule;

namespace XGame.Running.Capitals.XmlModule
{

    /// <summary>
    /// 帮助组件读取XML文件;
    /// </summary>
    public class XmlCapitalModule : CapitalModule, IXmlModule
    {

        [Header("XML读取;")]

        /// <summary>
        /// 组件节点名称;
        /// </summary>
        [SerializeField]
        private string xElementName;

        /// <summary>
        /// 是否允许重复放置;
        /// </summary>
        [SerializeField]
        private bool isDisallowMultiple;

        string IXmlModule.XElementName
        {
            get { return xElementName; }
        }

        bool IXmlModule.IsDisallowMultiple
        {
            get { return isDisallowMultiple; }
        }

        void IXmlModule.Add(XElement module, XGameObject insObject, ModInfo modInfo)
        {
            CapitalModule capitalModule = insObject.GetComponent<CapitalModule>();
            if (capitalModule == null)
                capitalModule = insObject.gameObject.AddComponent<CapitalModule>();

            LoadCosts(module, ref capitalModule, this);
        }

        /// <summary>
        /// 根据XML节点子元素信息对 CapitalScript 进行变更;
        /// </summary>
        /// <param name="xCosts"></param>
        /// <param name="capital"></param>
        /// <param name="defaultCapitalScript"></param>
        public void LoadCosts(XElement module, ref CapitalModule capital, CapitalModule defaultCapitalScript)
        {
            XElement xCosts = module.Element("Costs");

            if (xCosts == null)
                return;

            XAttribute FacilityType = xCosts.Attribute("FacilityType");
            capital.Facility = XmlHelper.LoadEnum(FacilityType, defaultCapitalScript.Facility);

            XAttribute Value = xCosts.Attribute("Value");
            capital.ObjectValue = XmlHelper.Load(Value, defaultCapitalScript.ObjectValue);

            XAttribute Construction = xCosts.Attribute("Construction");
            capital.ConstructionCosts = XmlHelper.Load(Construction, defaultCapitalScript.ConstructionCosts);

            XAttribute Maintenance = xCosts.Attribute("Maintenance");
            capital.MaintenanceCosts = XmlHelper.Load(Maintenance, defaultCapitalScript.MaintenanceCosts);

            XAttribute Demolition = xCosts.Attribute("Demolition");
            capital.DemolitionCosts = XmlHelper.Load(Demolition, defaultCapitalScript.DemolitionCosts);
        }


    }

}
