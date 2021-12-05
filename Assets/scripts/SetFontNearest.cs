using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SetFontNearest))]
public class SetFontNearesteditor : Editor {

    public override void OnInspectorGUI() {
        SetFontNearest a = (SetFontNearest)target;
        DrawDefaultInspector();

        if (GUILayout.Button("sharpen Fonts")) {
            a.Start();
        }
    }
}
#endif
public class SetFontNearest : MonoBehaviour {

    [SerializeField] Font[] fonts = null;

    public void Start() {
        foreach (Font font in fonts) {
            var mat = font.material;
            var txtr = mat.mainTexture;
            txtr.filterMode = FilterMode.Point;
        }
    }
}