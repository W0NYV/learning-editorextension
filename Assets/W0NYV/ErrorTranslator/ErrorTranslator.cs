using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ErrorTranslator {

    class ErrorLog
    {
        public string logText;
        public string stackTrace;
        public LogType logType;

        public ErrorLog(string _logText, string _stackTrace, LogType _logType)
        {
            logText = _logText;
            stackTrace = _stackTrace;
            logType = _logType;
        }
    }

    public class ErrorTranslator : EditorWindow
    {

        private List<ErrorLog> _errors = new List<ErrorLog>();
        private Vector2 _scrollPosition;

        [MenuItem("W0NYV_Tool/ErrorTranslator")]
        private static void Create() {
            GetWindow<ErrorTranslator>("ErrorTranslator");
        }

        private void OnEnable() {
            Application.logMessageReceived += OnReceiveLog;
        }

        private void OnGUI() {
    
            Color defaultColor = GUI.backgroundColor;
            using(new GUILayout.VerticalScope(GUI.skin.box))
            {

                using(var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPosition))
                {   

                    _scrollPosition = scrollView.scrollPosition;

                    for(int i = 0; i < _errors.Count; i++) 
                    {

                        if(_errors[i].logType == LogType.Warning) 
                        {
                            GUI.backgroundColor = new Color(1f, 1f, 0.2f,1f);
                        } 
                        else 
                        {
                            GUI.backgroundColor = new Color(1f, 0.2f, 0.2f,1f);
                        }

                        using(new GUILayout.VerticalScope(EditorStyles.helpBox))
                        {

                            GUILayout.Label("==========================================");

                            string[] strArr = _errors[i].logText.Split(':');
                            foreach (var value in strArr)
                            {
                                GUILayout.Label(value);
                            }

                            GUILayout.Label("==========================================");
                            
                            GUI.backgroundColor = defaultColor;
                            using(new GUILayout.HorizontalScope(GUI.skin.box))
                            {

                                GUI.backgroundColor = new Color(0.2f,0.2f,1f,1f);
                                using(new GUILayout.HorizontalScope())
                                {
                                    if(GUILayout.Button("翻訳"))
                                    {
                                        OpenURL("https://translate.google.co.jp/?hl=ja&sl=en&tl=ja&text=" + _errors[i].logText + "&op=translate");
                                    }
                                }

                                // GUI.backgroundColor = new Color(0.2f,0.7f,0.95f,1f);
                                // using(new GUILayout.HorizontalScope())
                                // {
                                //     if(GUILayout.Button("検索"))
                                //     {

                                //         if(_errors[i].logType == LogType.Assert) {
                                //             OpenURL("https://www.google.com/search?q=" + _errors[i].logText);
                                //         } else {
                                //             OpenURL("https://www.google.com/search?q=" + strArr[1] + " " + strArr[2]);
                                //         }
                                //     }
                                // }
                            }

                        }
                    }
                }
            }

        }

        private void OnReceiveLog(string logText, string stackTrace, LogType logType) 
        {

            if(logType != LogType.Log) 
            {

                bool isSameTest = false;
                for(int i = 0; i < _errors.Count; i++) 
                {
                    if(logText == _errors[i].logText) isSameTest = true;
                }

                if(!isSameTest)
                {
                    var err = new ErrorLog(logText, stackTrace, logType);
                    _errors.Add(err);
                }
            }
        }

        private void OpenURL(string uriStr) 
        {
            var uri = new System.Uri(uriStr);
            Application.OpenURL(uri.AbsoluteUri);
        }
    }
}