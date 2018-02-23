using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.ScenarioController
{
    public struct ScenarioInfo
    {
        public IContentInfo ContentInfo { get; private set; }
        public ScenarioDescription Description { get; private set; }

        public ScenarioInfo(IContentInfo contentInfo, ScenarioDescription description)
        {
            ContentInfo = contentInfo;
            Description = description;
        }
    }
}
