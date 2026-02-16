using UnityEngine;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// MonoBehaviour Singleton。Update/LateUpdate から TweenManager を駆動する。
    /// </summary>
    internal class TweenUpdateRunner : MonoBehaviour
    {
        private static TweenUpdateRunner instance;
        private static bool isQuitting;

        internal static void EnsureInitialized()
        {
            if (instance != null || isQuitting) return;
            Create();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Create()
        {
            if (instance != null) return;

            var go = new GameObject("[UITKTween]");
            go.hideFlags = HideFlags.HideInHierarchy;
            DontDestroyOnLoad(go);
            instance = go.AddComponent<TweenUpdateRunner>();
        }

        private void Update()
        {
            TweenManager.Update(Time.deltaTime, UpdateType.Normal);
        }

        private void LateUpdate()
        {
            TweenManager.Update(Time.deltaTime, UpdateType.Late);
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
        }

        private void OnDestroy()
        {
            if (instance == this) instance = null;
        }
    }
}
