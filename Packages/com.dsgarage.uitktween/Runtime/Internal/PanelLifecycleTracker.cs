using UnityEngine.UIElements;

namespace DSGarage.UITKTween.Internal
{
    /// <summary>
    /// VisualElement の DetachFromPanelEvent を監視し、
    /// パネル離脱時に関連する Tween を自動的に Kill する。
    /// </summary>
    internal static class PanelLifecycleTracker
    {
        /// <summary>
        /// VisualElement にライフサイクル追跡を設定する。
        /// パネルから離脱した際に、該当 target の全 Tween を Kill する。
        /// </summary>
        internal static void Track(VisualElement element)
        {
            // 重複登録を防止
            element.UnregisterCallback<DetachFromPanelEvent>(OnDetach);
            element.RegisterCallback<DetachFromPanelEvent>(OnDetach);
        }

        private static void OnDetach(DetachFromPanelEvent evt)
        {
            if (evt.target is VisualElement element)
            {
                TweenManager.KillByTarget(element);
                element.UnregisterCallback<DetachFromPanelEvent>(OnDetach);
            }
        }
    }
}
