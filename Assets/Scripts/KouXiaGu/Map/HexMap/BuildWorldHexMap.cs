using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Map.HexMap;
using UniRx;
using System.Collections;

namespace KouXiaGu.Map
{

    [DisallowMultipleComponent]
    public class BuildWorldHexMap : MonoBehaviour
    {

        [SerializeField, Tooltip("地图六边形的外径")]
        private float hexOuterDiameter = 2;

        [SerializeField]
        private Transform target;

        [SerializeField]
        private MapBlockIOInfo mapBlockIOInfo;


        private DynaBlocksArchiver dynaBlocksArchiver;
        private Hexagon mapHexagon;

        /// <summary>
        /// 当前地图所用的六边形尺寸;
        /// </summary>
        public Hexagon MapHexagon
        {
            get { return mapHexagon; }
        }

        private void Awake()
        {
            mapHexagon = new Hexagon() { OuterDiameter = hexOuterDiameter };
            dynaBlocksArchiver = new DynaBlocksArchiver(mapBlockIOInfo);

            AddToInit();
        }

        private void AddToInit()
        {
            IBuildGameData buildGame = GameData.BuildGameData;

            buildGame.AppendBuildGame.Add(dynaBlocksArchiver);
            buildGame.AppendArchiveGame.Add(dynaBlocksArchiver);
        }


    }

}
