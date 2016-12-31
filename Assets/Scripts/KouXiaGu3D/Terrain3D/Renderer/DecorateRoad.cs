using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路信息烘焙;
    /// </summary>
    [Serializable]
    public class DecorateRoad : RenderBase<RoadRes>
    {
        DecorateRoad() { }

        List<KeyValuePair<RoadRes, MeshRenderer>> meshs;

        public override void Awake()
        {
            base.Awake();
            meshs = new List<KeyValuePair<RoadRes, MeshRenderer>>();
        }

        protected override List<KeyValuePair<RoadRes, MeshRenderer>> InitMeshs(IDictionary<CubicHexCoord, TerrainNode> map, IEnumerable<CubicHexCoord> coords)
        {
            this.meshs.Clear();
            RecoveryActive();

            TerrainNode node;
            foreach (var coord in coords)
            {
                if (map.TryGetValue(coord, out node))
                {
                    if (node.ExistRoad)
                    {
                        var res = GetRoadRes(node.Road);
                        IEnumerable<float> roadAngles = GetRoadAngles(map, coord);
                        foreach (var roadAngle in roadAngles)
                        {
                            var mesh = DequeueMesh(coord, roadAngle);
                            this.meshs.Add(new KeyValuePair<RoadRes, MeshRenderer>(res, mesh));
                        }
                    }
                }
            }

            return this.meshs;
        }

        protected override void SetDiffuseParameter(Material material, RoadRes res)
        {
            material.SetTexture("_MainTex", res.DiffuseTex);
            material.SetTexture("_BlendTex", res.DiffuseBlendTex);
            return;
        }

        protected override void CameraRenderDiffuse(RenderTexture rt, MeshDisplay display)
        {
            Renderer.CameraRender(rt, display, new Color(0, 0, 0, 0));
            //base.CameraRenderDiffuse(rt, display);
        }

        protected override void SetHeightParameter(Material material, RoadRes res)
        {
            material.SetTexture("_MainTex", res.HeightAdjustTex);
            return;
        }


        /// <summary>
        /// 获取到道路资源信息;
        /// </summary>
        RoadRes GetRoadRes(int id)
        {
            try
            {
                return RoadRes.initializedInstances[id];
            }
            catch (KeyNotFoundException ex)
            {
                throw new LackOfResourcesException("缺少材质资源;", ex);
            }
        }

        IEnumerable<float> GetRoadAngles(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            HexDirections Directions = GetRoadDirections(map, coord);
            return GetRoadAngles(Directions);
        }

        /// <summary>
        /// 获取到存在道路的方向;
        /// </summary>
        HexDirections GetRoadDirections(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            HexDirections roadDirections = 0;
            TerrainNode node;
            foreach (var dir in CubicHexCoord.Directions)
            {
                CubicHexCoord dirCoord = coord.GetDirection(dir);
                if (map.TryGetValue(dirCoord, out node))
                {
                    if (node.ExistRoad)
                    {
                        roadDirections |= dir;
                    }
                }
            }
            return roadDirections;
        }

        /// <summary>
        /// 获取到方向对应的角度;
        /// </summary>
        List<float> GetRoadAngles(HexDirections roadDirections)
        {
            List<float> angles = new List<float>();
            foreach (var dir in CubicHexCoord.GetDirections(roadDirections))
            {
                float angle = GridConvert.GetAngle(dir);
                angles.Add(angle);
            }
            return angles;
        }


    }

}
