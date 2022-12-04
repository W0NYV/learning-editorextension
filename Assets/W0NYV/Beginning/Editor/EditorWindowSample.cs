using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Beginning
{
    public class EditorWindowSample : EditorWindow {

        private ScriptableObjectSample _sample;
        private const string ASSET_PATH = "Assets/W0NYV/Beginning/Data.asset";

        [MenuItem("Editor/Sample")]
        private static void Create() 
        {
            GetWindow<EditorWindowSample>("サンプル");
        }

        private void OnGUI()
        {
            if(_sample == null) 
            {
                Import();
            }

            Color defaultColor = GUI.backgroundColor;
            using(new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUI.backgroundColor = Color.gray;
                using(new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    GUILayout.Label("設定");
                }
                GUI.backgroundColor = defaultColor;

                _sample.SampleIntValue = EditorGUILayout.IntField("サンプル", _sample.SampleIntValue);

            }

            using(new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUI.backgroundColor = Color.gray;
                using(new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    GUILayout.Label("ファイル操作");
                }
                GUI.backgroundColor = defaultColor;

                GUILayout.Label("パス" + ASSET_PATH);

                using(new GUILayout.HorizontalScope(GUI.skin.box))
                {
                    GUI.backgroundColor = Color.green;
                    using(new GUILayout.HorizontalScope()) 
                    {
                        if(GUILayout.Button("読み込み"))
                        {
                            Import();
                        }
                    }

                    GUI.backgroundColor = Color.magenta;
                    using(new GUILayout.HorizontalScope()) 
                    {

                        if(GUILayout.Button("書き込み"))
                        {
                            Export();
                        }  
                        GUI.backgroundColor = defaultColor;

                    }

                }
            }
        }      

        private void Import()
        {

            if(_sample == null) _sample = ScriptableObject.CreateInstance<ScriptableObjectSample>();

            ScriptableObjectSample sample = AssetDatabase.LoadAssetAtPath<ScriptableObjectSample>(ASSET_PATH);
            if(sample == null) return;

            EditorUtility.CopySerialized(sample, _sample);
        }

        private void Export() 
        {

            ScriptableObjectSample sample = AssetDatabase.LoadAssetAtPath<ScriptableObjectSample>(ASSET_PATH);
            if(sample == null) sample = ScriptableObject.CreateInstance<ScriptableObjectSample>();

            //新規の場合は作成
            if(!AssetDatabase.Contains(sample as UnityEngine.Object))
            {
                string directory = Path.GetDirectoryName(ASSET_PATH);
                if(!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                //アセット作成
                AssetDatabase.CreateAsset(sample, ASSET_PATH);
            }

            EditorUtility.CopySerialized(_sample, sample);

            //インスペクターから設定できないようにする
            sample.hideFlags = HideFlags.NotEditable;

            //更新通知
            EditorUtility.SetDirty(sample);

            //保存
            AssetDatabase.SaveAssets();

            //エディタを最新の状態にする
            AssetDatabase.Refresh();

        }  

    }
}

