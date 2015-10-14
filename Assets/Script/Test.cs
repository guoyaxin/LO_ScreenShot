using UnityEngine;
using System.Collections;
 
public class Test : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnGUI ()
	{
		if (GUILayout.Button ("cut screen")) {
			string savePath = Application.dataPath + "/ScreenShot/haha.png";
			StartCoroutine (CaptureByCamera (Camera.main, new Rect (0, 0, 500, 500), savePath));
		}
	}

	IEnumerator CaptureByCamera (Camera mCamera, Rect mRect, string mFileName)
	{	
		yield return new WaitForEndOfFrame ();
		//初始化RenderTexture
		RenderTexture mRender = new RenderTexture ((int)mRect.width, (int)mRect.height, 0);
		//设置相机的渲染目标
		mCamera.targetTexture = mRender;
		//开始渲染
		mCamera.Render ();
		
		//激活渲染贴图读取信息
		RenderTexture.active = mRender;
		
		Texture2D mTexture = new Texture2D ((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
		//读取屏幕像素信息并存储为纹理数据
		mTexture.ReadPixels (mRect, 0, 0);
		//应用
		mTexture.Apply ();
		//释放相机，销毁渲染贴图
		mCamera.targetTexture = null;   
		RenderTexture.active = null; 
		GameObject.Destroy (mRender);  
		
		//将图片信息编码为字节信息
		byte[] bytes = mTexture.EncodeToPNG ();  
		//保存
		System.IO.File.WriteAllBytes (mFileName, bytes);
	}
}
