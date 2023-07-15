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
    [SerializeField]
    private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setFaceShot()
    {

        StartCoroutine("DoShot");
    }
    IEnumerator DoShot() // 바로 찍으면 눈이 잘림 ( 생성되면서 중력땜시 조금 떨어져서 그런가? )
    {
        GameObject temporaryLoc = GameObject.Find("forScreenshot");
        GameObject Scaler = Instantiate(playerPrefab, temporaryLoc.transform.position, temporaryLoc.transform.rotation);
        Scaler.transform.localScale = new Vector3(10f, 10f, 1);
        temporaryLoc.SetActive(false);
        yield return new WaitForSeconds(Time.deltaTime);
        ScreenShot();
        Destroy(Scaler);        
    }
    private void ScreenShot()
    {
        Texture2D texture = new Texture2D((int)screenshotArea.width, (int)screenshotArea.height, TextureFormat.RGB24, false);

        // 카메라 설정
        screenshotCamera.targetTexture = RenderTexture.GetTemporary((int)screenshotArea.width, (int)screenshotArea.height, 16);
        screenshotCamera.Render();
        // 화면 텍스처를 픽셀로 읽어오기
        RenderTexture.active = screenshotCamera.targetTexture;
        texture.ReadPixels(screenshotArea, 0, 0);
        texture.Apply();
        face.texture = texture;
    }
}
