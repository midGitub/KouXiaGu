using System.Collections;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 地形编辑模式;
    /// </summary>
    public class TerrainEdit
    {
        static TerrainEdit()
        {
            IsRunning = false;
            IsSaving = false;
            IsPause = false;
        }

        public static bool IsRunning { get; private set; }
        public static bool IsSaving { get; private set; }
        public static bool IsPause { get; private set; }

        public static IEnumerator Begin()
        {
            if (IsRunning)
                throw new PremiseNotInvalidException();

            yield return TerrainInitializer.Begin();

            IsRunning = true;
            yield break;
        }

        public static IEnumerator Save(Archive archive)
        {
            IsSaving = true;

            MapFiler.Write();

            IsSaving = false;
            yield break;
        }

        public static IEnumerator Finish()
        {
            if (!IsRunning)
                throw new PremiseNotInvalidException();

            yield return TerrainInitializer.Finish();

            IsRunning = false;
            yield break;
        }

    }

}
