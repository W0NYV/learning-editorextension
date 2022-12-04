using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TransformCleaner
{
    public class TransformCleaner
    {

        [MenuItem("W0NYV_Tool/Clean Transform")]
        public static void CleanTransform()
        {
            GameObject[] gameObjects = Selection.gameObjects;

            foreach (var obj in gameObjects)
            {
            
                if(obj.GetComponent<Transform>() != null) {

                    Undo.RecordObject(obj.transform, "Clean Transform");

                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localEulerAngles = Vector3.zero;
                    obj.transform.localScale = new Vector3(1f, 1f, 1f);
                }

            }
        }
    }
}
