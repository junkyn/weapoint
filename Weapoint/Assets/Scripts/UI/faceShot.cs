using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class faceShot : MonoBehaviour
{
    [SerializeField]
    private Camera screenshotCamera;
    [SerializeField]
    private Rect screenshotArea;
    [SerializeField]
    private RawImage face;
    // Start is called before the first frame update
    void Start()
    {
        ScreenShot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ScreenShot()
    {
        Texture2D texture = new Texture2D((int)screenshotArea.width, (int)screenshotArea.height, TextureFormat.RGB24, false);

        // ī�޶� ����
        screenshotCamera.targetTexture = RenderTexture.GetTemporary((int)screenshotArea.width, (int)screenshotArea.height, 16);
        screenshotCamera.Render();
        // ȭ�� �ؽ�ó�� �ȼ��� �о����
        RenderTexture.active = screenshotCamera.targetTexture;
        texture.ReadPixels(screenshotArea, 0, 0);
        texture.Apply();
        face.texture = texture;
    }
}
