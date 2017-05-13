using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Wall
{

    [Serializable]
    public class DynamicWall
    {
        DynamicWall()
        {
        }

        public DynamicWall(JointInfo joint, Vector3[] vertices)
        {
            this.joint = joint;
            pointCollection = Build(joint, vertices);
        }

        [SerializeField]
        JointInfo joint;
        [SerializeField]
        LocalVertice[] pointCollection;

        public JointInfo JointInfo
        {
            get { return joint; }
        }

        public IList<LocalVertice> Points
        {
            get { return pointCollection; }
        }

        /// <summary>
        /// 根据节点信息生成点合集;
        /// </summary>
        LocalVertice[] Build(JointInfo joint, Vector3[] vertices)
        {
            LocalVertice[] pointCollection = new LocalVertice[vertices.Length];
            foreach (var section in joint.JointPoints)
            {
                foreach (var childIndex in section.Children)
                {
                    Vector3 childPosition = vertices[childIndex];
                    Vector3 localPosition = childPosition - section.Position;
                    float localAngle = AngleY(section.Position, childPosition);
                    LocalVertice pointInfo = new LocalVertice(localPosition, localAngle);
                    pointCollection[childIndex] = pointInfo;
                }
            }
            return pointCollection;
        }

        /// <summary>
        /// 返回弧度;
        /// </summary>
        float AngleY(Vector3 from, Vector3 to)
        {
            return Mathf.Atan2((to.x - from.x), (to.z - from.z));
        }
    }

}
