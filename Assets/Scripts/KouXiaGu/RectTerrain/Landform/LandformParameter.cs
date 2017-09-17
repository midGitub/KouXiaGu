//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace KouXiaGu.RectTerrain
//{

//    [Serializable]
//    public class LandformParameter
//    {

//        [SerializeField, Range(0, 64)]
//        float tessellation = 48f;

//        /// <summary>
//        /// 地形细分程度;
//        /// </summary>
//        public float Tessellation
//        {
//            get { return tessellation; }
//        }

//        const string TessellationName = "_LandformTess";

//        public static float GetTessellation()
//        {
//            float value = Shader.GetGlobalFloat(TessellationName);
//            return value;
//        }

//        public static void SetTessellation(float value)
//        {
//            Shader.SetGlobalFloat(TessellationName, value);
//        }



//        [SerializeField, Range(0, 5)]
//        float displacement = 1.5f;

//        /// <summary>
//        /// 地形高度缩放;
//        /// </summary>
//        public float Displacement
//        {
//            get { return displacement; }
//        }

//        const string DisplacementName = "_LandformDisplacement";

//        public static float GetDisplacement()
//        {
//            float value = Shader.GetGlobalFloat(DisplacementName);
//            return value;
//        }

//        public static void SetDisplacement(float value)
//        {
//            Shader.SetGlobalFloat(DisplacementName, value);
//        }



//        public void Awake()
//        {
//            OnValidate();
//        }

//        /// <summary>
//        /// 由Unity调用;
//        /// </summary>
//        public void OnValidate()
//        {
//            SetTessellation(tessellation);
//            SetDisplacement(displacement);
//        }
//    }
//}
