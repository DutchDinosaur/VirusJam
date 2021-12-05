using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostprossesingOutline : MonoBehaviour {

    [SerializeField] bool active = false;
    [SerializeField] float outlineSize = 1;
    [SerializeField] float PixelSize = 1;
    [SerializeField] Color outlineColor = Color.white;
    public Shader outlineMask;
    public Shader Outline;
    Material _outlineMaterial;
    Camera TempCam;
    RenderTexture maskRT;

    private void Start() {
        _outlineMaterial = new Material(Outline);
        TempCam = new GameObject().AddComponent<Camera>();
        TempCam.name = "outline Cam";
        maskRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.R8);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        if (maskRT.width != Screen.width || maskRT.height != Screen.height) {
            maskRT.Release();
            maskRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.R8); 
        }

        if (!active) {
            TempCam.enabled = false;
            Camera.current.targetTexture = null;
            Graphics.Blit(src, dst);
            return;
        }
        TempCam.enabled = true;
        TempCam.CopyFrom(Camera.current);
        TempCam.backgroundColor = Color.black;
        TempCam.clearFlags = CameraClearFlags.Color;

        TempCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");

        TempCam.targetTexture = maskRT;
        maskRT.filterMode = FilterMode.Point;
        src.filterMode = FilterMode.Point;
        TempCam.RenderWithShader(outlineMask, "RenderType");
        _outlineMaterial.SetFloat("_PixelDensity", PixelSize);
        _outlineMaterial.SetVector("_AspectRatioMultiplier", new Vector2(Screen.width, Screen.height));
        _outlineMaterial.SetColor("_Color",outlineColor);
        _outlineMaterial.SetFloat("_Size", outlineSize);
        _outlineMaterial.SetTexture("_SceneTex", src);
        Graphics.Blit(maskRT, dst, _outlineMaterial);
        maskRT.Release();
    }

    static public Texture2D GetRTPixels(RenderTexture rt) {
        RenderTexture currentActiveRT = RenderTexture.active;

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();
        RenderTexture.active = currentActiveRT;
        return tex;
    }
}