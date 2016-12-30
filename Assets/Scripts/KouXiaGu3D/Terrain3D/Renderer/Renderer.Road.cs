using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public sealed partial class Renderer : UnitySington<Renderer>
    {

        [Serializable]
        class RoadDecorate : RenderBase<RoadRes>
        {
            RoadDecorate() { }

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

                foreach (var coord in coords)
                {
                    try
                    {
                        var road = new RoadNode(map, coord);
                        foreach (var roadAngle in road.RoadAngles)
                        {
                            CubicHexCoord crood = road.Position;
                            var mesh = DequeueMesh(crood, roadAngle);
                            this.meshs.Add(new KeyValuePair<RoadRes, MeshRenderer>(road.Road, mesh));
                        }
                    }
                    catch (ObjectNotExistedException)
                    {
                        continue;
                    }
                }
                return this.meshs;
            }

            protected override void SetDiffuseParameter(Material material, RoadRes res)
            {
                return;
            }

            protected override void SetHeightParameter(Material material, RoadRes res)
            {
                return;
            }

        }

    }

}
