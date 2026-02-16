using UnityEditor;
using UnityEngine;

namespace DSGarage.UITKTween.Editor
{
    /// <summary>
    /// Project Settings に UITKTween の設定パネルを追加する。
    /// </summary>
    public class UITKTweenSettingsProvider : SettingsProvider
    {
        private const string SettingsPath = "Project/UITKTween";

        // EditorPrefs キー
        private const string PrefDefaultEase = "UITKTween_DefaultEase";
        private const string PrefDefaultAutoKill = "UITKTween_DefaultAutoKill";

        public UITKTweenSettingsProvider()
            : base(SettingsPath, SettingsScope.Project)
        {
            label = "UITKTween";
            keywords = new System.Collections.Generic.HashSet<string> { "tween", "animation", "uitoolkit", "ease" };
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new UITKTweenSettingsProvider();
        }

        public override void OnActivate(string searchContext, UnityEngine.UIElements.VisualElement rootElement)
        {
            // 保存された設定を読み込み
            LoadSettings();
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("UITKTween Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUILayout.HelpBox(
                "UITKTween のグローバルデフォルト設定です。\n" +
                "ランタイムで UITKTween.defaultEase / UITKTween.defaultAutoKill を変更することもできます。",
                MessageType.Info);

            EditorGUILayout.Space(10);

            // デフォルトイージング
            EditorGUI.BeginChangeCheck();
            var ease = (EaseType)EditorGUILayout.EnumPopup("Default Ease", UITKTween.defaultEase);
            if (EditorGUI.EndChangeCheck())
            {
                UITKTween.defaultEase = ease;
                EditorPrefs.SetInt(PrefDefaultEase, (int)ease);
            }

            // デフォルト AutoKill
            EditorGUI.BeginChangeCheck();
            var autoKill = EditorGUILayout.Toggle("Default Auto Kill", UITKTween.defaultAutoKill);
            if (EditorGUI.EndChangeCheck())
            {
                UITKTween.defaultAutoKill = autoKill;
                EditorPrefs.SetBool(PrefDefaultAutoKill, autoKill);
            }

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Version", "0.1.0");
            EditorGUILayout.LabelField("Active Tweens", Application.isPlaying ? UITKTween.ActiveTweenCount.ToString() : "N/A (Not Playing)");

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Open Debug Window"))
            {
                UITKTweenDebugWindow.ShowWindow();
            }
        }

        private static void LoadSettings()
        {
            if (EditorPrefs.HasKey(PrefDefaultEase))
                UITKTween.defaultEase = (EaseType)EditorPrefs.GetInt(PrefDefaultEase);
            if (EditorPrefs.HasKey(PrefDefaultAutoKill))
                UITKTween.defaultAutoKill = EditorPrefs.GetBool(PrefDefaultAutoKill);
        }

        /// <summary>プロジェクト起動時に設定を自動読み込み</summary>
        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            LoadSettings();
        }
    }
}
