using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MaterialChanger : EditorWindow
{

    private GameObject[] _targetObjects;
    private Material[] _materials;
    private Vector2 _scrollPosition;

    [MenuItem("W0NYV_Tool/MaterialChanger")]
    private static void Create() {
        GetWindow<MaterialChanger>("MaterialChanger");
        
    }

    private void Awake() {
        _targetObjects = GetTargetObjects();
        _materials = new Material[_targetObjects.Length];
    }

    private void OnGUI() 
    {

        Color defaultColor = GUI.backgroundColor;
        using(new GUILayout.VerticalScope(GUI.skin.box))
        {
            using(var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPosition))
            {

                _scrollPosition = scrollView.scrollPosition;

                for(int i = 0; i < _targetObjects.Length; i++) {

                    GUI.backgroundColor = defaultColor;
                    using(new GUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        GUI.backgroundColor = Color.gray;
                        using(new GUILayout.HorizontalScope(EditorStyles.toolbar))
                        {
                            GUILayout.Label(_targetObjects[i].name);
                        }
                        
                        Material mat = _targetObjects[i].GetComponent<MeshRenderer>().sharedMaterial;

                        GUI.backgroundColor = Color.green;
                        mat = (Material)EditorGUILayout.ObjectField("適用前のマテリアル", mat, typeof(Material), false);
                        
                        GUI.backgroundColor = Color.magenta;
                        _materials[i] = (Material)EditorGUILayout.ObjectField("適用後のマテリアル", _materials[i], typeof(Material), false);
                    }
                }
            }
        }

        GUI.backgroundColor = defaultColor;
        if(GUILayout.Button("適用"))
        {
            Apply();
        }
    }

    private void Apply() {

        //実行中は適用させない
        if(!EditorApplication.isPlaying) {
            for(int i = 0; i < _targetObjects.Length; i++) {
                if(_materials[i] != null) _targetObjects[i].GetComponent<MeshRenderer>().material = _materials[i];
            }
        }

        AssetDatabase.Refresh();

    }

    private GameObject[] GetTargetObjects() {
        var objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        return objects.Where(obj => obj.GetComponent<MeshRenderer>() != null && !EditorUtility.IsPersistent(obj.transform.root.gameObject) && !(obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave)).ToArray();
    }
}
