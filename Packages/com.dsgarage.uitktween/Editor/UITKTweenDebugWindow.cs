using UnityEditor;
using UnityEngine;

namespace DSGarage.UITKTween.Editor
{
    /// <summary>
    /// UITKTween デバッグウィンドウ。
    /// アクティブな Tween の一覧と状態をリアルタイム表示する。
    /// </summary>
    public class UITKTweenDebugWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private bool autoRepaint = true;

        [MenuItem("Window/UITKTween/Debug Window")]
        public static void ShowWindow()
        {
            GetWindow<UITKTweenDebugWindow>("UITKTween Debug");
        }

        private void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            if (autoRepaint && Application.isPlaying)
                Repaint();
        }

        private void OnGUI()
        {
            // ヘッダー
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label($"Active Tweens: {UITKTween.ActiveTweenCount}", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            autoRepaint = GUILayout.Toggle(autoRepaint, "Auto Repaint", EditorStyles.toolbarButton);

            if (GUILayout.Button("Kill All", EditorStyles.toolbarButton))
            {
                UITKTween.KillAll();
            }
            EditorGUILayout.EndHorizontal();

            // デフォルト設定
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Default Settings", EditorStyles.boldLabel);
            UITKTween.defaultEase = (EaseType)EditorGUILayout.EnumPopup("Default Ease", UITKTween.defaultEase);
            UITKTween.defaultAutoKill = EditorGUILayout.Toggle("Default Auto Kill", UITKTween.defaultAutoKill);

            EditorGUILayout.Space(8);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Play モードで Tween 情報が表示されます。", MessageType.Info);
                return;
            }

            // Tween リスト
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawTweenList();
            EditorGUILayout.EndScrollView();
        }

        private void DrawTweenList()
        {
            var tweens = TweenManager.ActiveTweens;
            if (tweens == null || tweens.Count == 0)
            {
                EditorGUILayout.LabelField("アクティブな Tween はありません。");
                return;
            }

            for (int i = 0; i < tweens.Count; i++)
            {
                var tween = tweens[i];
                if (tween == null) continue;

                EditorGUILayout.BeginVertical("box");

                // 1行目: 型名 + 状態
                EditorGUILayout.BeginHorizontal();
                string typeName = tween.GetType().Name;
                if (typeName.Contains("`"))
                    typeName = typeName.Substring(0, typeName.IndexOf('`')) + "<" + tween.GetType().GetGenericArguments()[0].Name + ">";

                string state = tween.IsPlaying ? "Playing" :
                               tween.IsPaused ? "Paused" :
                               tween.IsComplete ? "Complete" : "Idle";
                Color stateColor = tween.IsPlaying ? Color.green :
                                   tween.IsPaused ? Color.yellow :
                                   tween.IsComplete ? Color.gray : Color.white;

                GUILayout.Label($"#{i} {typeName}", EditorStyles.boldLabel, GUILayout.Width(200));

                var oldColor = GUI.color;
                GUI.color = stateColor;
                GUILayout.Label(state, GUILayout.Width(70));
                GUI.color = oldColor;

                if (tween.id != null)
                    GUILayout.Label($"ID: {tween.id}", GUILayout.Width(100));
                if (tween.target != null)
                    GUILayout.Label($"Target: {tween.target.GetType().Name}", GUILayout.Width(150));

                GUILayout.FlexibleSpace();

                // 制御ボタン
                if (tween.IsPlaying && GUILayout.Button("Pause", GUILayout.Width(50)))
                    tween.Pause();
                else if (tween.IsPaused && GUILayout.Button("Play", GUILayout.Width(50)))
                    tween.Play();
                if (GUILayout.Button("Kill", GUILayout.Width(40)))
                    tween.Kill();

                EditorGUILayout.EndHorizontal();

                // 2行目: プログレスバー
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"Ease: {tween.easeType}", GUILayout.Width(120));
                GUILayout.Label($"Loop: {tween.completedLoops}/{(tween.loops <= 0 ? "∞" : tween.loops.ToString())}", GUILayout.Width(80));

                var rect = EditorGUILayout.GetControlRect(false, 16);
                float progress = tween.ElapsedPercentage;
                EditorGUI.ProgressBar(rect, progress, $"{progress * 100f:F0}% ({tween.elapsedTime:F2}s / {tween.duration:F2}s)");

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }
        }
    }
}
