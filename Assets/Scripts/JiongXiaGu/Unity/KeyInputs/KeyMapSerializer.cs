using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.KeyInputs
{
    public class KeyMapSerializer : ContentSerializer<KeyMap>
    {
        public override string RelativePath { get; }
        public override ISerializer<KeyMap> Serializer { get; }
    }
}
