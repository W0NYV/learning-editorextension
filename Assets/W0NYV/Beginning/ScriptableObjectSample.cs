using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateSampleAsset")]
public class ScriptableObjectSample : ScriptableObject 
{
    [SerializeField] private int _sampleIntValue;

    public int SampleIntValue
    {
        get => _sampleIntValue;

#if UNITY_EDITOR
        set => _sampleIntValue = Mathf.Clamp(value, 0, int.MaxValue);
#endif

    }
}

