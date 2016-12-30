﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public sealed partial class Renderer : UnitySington<Renderer>
    {

        /// <summary>
        /// 地形烘焙;
        /// </summary>
        [Serializable]
        class TerrainRender : RenderBase<LandformRes>
        {
            TerrainRender() { }

            List<KeyValuePair<LandformRes, MeshRenderer>> meshs;

            public override void Awake()
            {
                base.Awake();
                meshs = new List<KeyValuePair<LandformRes, MeshRenderer>>();
            }

            protected override List<KeyValuePair<LandformRes, MeshRenderer>> InitMeshs(IDictionary<CubicHexCoord, TerrainNode> map, IEnumerable<CubicHexCoord> coords)
            {
                this.meshs.Clear();
                RecoveryActive();

                TerrainNode node;
                KeyValuePair<LandformRes, MeshRenderer> mesh;

                foreach (var coord in coords)
                {
                    if (map.TryGetValue(coord, out node))
                        if (TryGetMeshWith(coord, node, out mesh))
                            this.meshs.Add(mesh);
                }

                return this.meshs;
            }

            bool TryGetMeshWith(CubicHexCoord coord, TerrainNode node, out KeyValuePair<LandformRes, MeshRenderer> landformMesh)
            {
                if (node.ExistLandform)
                {
                    var res = GetLandform(node.Landform);
                    var mesh = DequeueMesh(coord, node.LandformAngle);
                    landformMesh = new KeyValuePair<LandformRes, MeshRenderer>(res, mesh);
                    return true;
                }
                landformMesh = default(KeyValuePair<LandformRes, MeshRenderer>);
                return false;
            }

            protected override void SetDiffuseParameter(Material material, LandformRes res)
            {
                return;
            }

            protected override void SetHeightParameter(Material material, LandformRes res)
            {
                return;
            }

            /// <summary>
            /// 获取到地貌信息;
            /// </summary>
            LandformRes GetLandform(int id)
            {
                try
                {
                    return LandformRes.initializedInstances[id];
                }
                catch (KeyNotFoundException ex)
                {
                    throw new LackOfResourcesException("缺少材质资源;", ex);
                }
            }

        }

    }

}
