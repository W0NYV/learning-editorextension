using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TextureProcessor {

    public class TextureProcessor : EditorWindow
    {

        private Texture2D _tex;
        private Texture2D _destTex;
        private string _filePath;
        private string _fileName;

        [MenuItem("W0NYV_Tool/TextureProcessor")]
        private static void Create()
        {
            GetWindow<TextureProcessor>("TextureProcessor");
        }

        private void OnGUI()
        {
            Color defaultColor = GUI.backgroundColor;
            using(new GUILayout.VerticalScope(GUI.skin.box))
            {
                GUI.backgroundColor = Color.green;
                using(new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label("編集するテクスチャを選んでください:");

                    GUI.backgroundColor = defaultColor;
                    _tex = (Texture2D)EditorGUILayout.ObjectField("Texture", _tex, typeof(Texture2D), false);
                }

                GUI.backgroundColor = defaultColor;
                using(new GUILayout.VerticalScope(EditorStyles.helpBox))
                {

                    GUI.backgroundColor = defaultColor;
                    GUILayout.Label("編集:");

                    if(GUILayout.Button("X軸反転"))
                    {
                        FlipX();
                    }

                    if(GUILayout.Button("Y軸反転"))
                    {
                        FlipY();
                    }


                    if(GUILayout.Button("色反転"))
                    {
                        Invert();
                    }
            
                    GUILayout.Space(20);
                    GUILayout.Label("プレビュー:");
                    EditorGUILayout.LabelField(new GUIContent(_destTex), GUILayout.Height(256), GUILayout.Width(256));

                }
            
                GUI.backgroundColor = Color.magenta;
                using(new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label("保存:");

                    GUI.backgroundColor = defaultColor;
                    _fileName = EditorGUILayout.TextField("ファイル名", _fileName);

                    if(GUILayout.Button("保存"))
                    {
                        SaveTex(_destTex);
                    }
                }
            }

        }

        private void SaveTex(Texture2D tex)
        {
            if(_fileName != "") 
            {
                string filePath = AssetDatabase.GetAssetPath(_tex);
                string[] strArr = filePath.Split('/');
                strArr[strArr.Length - 1] = _fileName + ".png";
                string newFilePath = "";
                foreach(var value in strArr)
                {
                    newFilePath += value + "/";
                }
                newFilePath = newFilePath.Remove(newFilePath.Length - 1);

                var bytes = tex.EncodeToPNG();
                System.IO.File.WriteAllBytes(newFilePath, bytes);

                AssetDatabase.Refresh();
            } 
            else 
            {
                Debug.LogError("TextureProcessor: ファイル名を入力してください");
            }
        }

        private void FlipX()
        {
            Texture2D tex;
            tex = CreateTex4Calc();

            for(var x = 0; x < _destTex.width; x++) 
            {
                for(var y = 0; y < _destTex.height; y++)
                {
                    var color = tex.GetPixel(x, y);
                    _destTex.SetPixel((_destTex.width-1)-x, y, color);
                }  
            } 

            _destTex.Apply();
        }

        private void FlipY()
        {
            Texture2D tex;
            tex = CreateTex4Calc();

            for(var x = 0; x < _destTex.width; x++) 
            {
                for(var y = 0; y < _destTex.height; y++)
                {
                    var color = tex.GetPixel(x, y);
                    _destTex.SetPixel(x, (_destTex.height-1)-y, color);
                }  
            } 

            _destTex.Apply();
        }

        private void Invert()
        {   

            Texture2D tex;
            tex = CreateTex4Calc();

            for(var x = 0; x < _destTex.width; x++) 
            {
                for(var y = 0; y < _destTex.height; y++)
                {
                    var color = tex.GetPixel(x, y);
                    color[0] = 1 - color[0];
                    color[1] = 1 - color[1];
                    color[2] = 1 - color[2];
                    _destTex.SetPixel(x, y, color);
                } 
            }

            _destTex.Apply();
        }

        private Texture2D CreateTex4Calc() {

            if(!_tex.isReadable) throw new System.Exception("TextureProcessor: テクスチャを読めるように設定してください");

            Texture2D tex4calc;

            if(_filePath == null) 
            {
                _filePath = AssetDatabase.GetAssetPath(_tex);
                tex4calc = CopyTexture(_tex);
                _destTex = new Texture2D(_tex.width, _tex.height, TextureFormat.RGBA32, false);
            } 
            else 
            {
                if(_filePath != AssetDatabase.GetAssetPath(_tex))
                {
                    _filePath = AssetDatabase.GetAssetPath(_tex);
                    tex4calc = CopyTexture(_tex);
                    _destTex = new Texture2D(_tex.width, _tex.height, TextureFormat.RGBA32, false);
                } 
                else 
                {
                    tex4calc = CopyTexture(_destTex);
                }
            }

            return tex4calc;

        }

        private Texture2D CopyTexture(Texture2D refTex) {

            Texture2D tex = new Texture2D(refTex.width, refTex.height, TextureFormat.RGBA32, false);

            for(var x = 0; x < refTex.width; x++) 
            {
                for(var y = 0; y < refTex.height; y++)
                {
                    var color = refTex.GetPixel(x, y);
                    tex.SetPixel(x, y, color);
                }     
            } 
            tex.Apply();
            return tex;

        }

    }
}
