using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图保存和读取;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MapBlockEditIO<T> : DynaBlocksMap<MapBlockEdit<T>, T>, IMapBlockIO<MapBlockEdit<T>, T>
    {
        protected MapBlockEditIO(BlocksMapInfo info) :
            base(info)
        {
            base.DynamicMapIO = this;
        }

        public MapBlockEditIO(BlocksMapInfo info, IMapBlockEditIOInfo mapBlockIOInfo) :
            base(info)
        {
            base.DynamicMapIO = this;
            this.MapBlockIOInfo = mapBlockIOInfo;
        }

        public IMapBlockEditIOInfo MapBlockIOInfo { get; protected set; }


        public MapBlockEdit<T> Load(ShortVector2 address)
        {
            Dictionary<ShortVector2, T> map;

            throw new NotImplementedException();
        }

        public bool LoadAsyn(ShortVector2 address, Action<MapBlockEdit<T>> onComplete, Action<Exception> onFail)
        {
            throw new NotImplementedException();
        }

        public void Save(ShortVector2 address, MapBlockEdit<T> mapBlock)
        {
            throw new NotImplementedException();
        }

        public void SaveAsyn(ShortVector2 address, MapBlockEdit<T> mapBlock, Action onComplete, Action<Exception> onFail)
        {
            throw new NotImplementedException();
        }
    }

}
