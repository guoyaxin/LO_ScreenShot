using UnityEngine;
using System.Collections;
using UnityEditor;
public enum CaptureType
{
	CaptureType_Default = 0,
	CaptureType_MainCamera,
	CaptureType_CutomCamera
}

public class ScreenShot : EditorWindow
{
	//定义图片保存路径
	private string mPath1;
	private string mPath2;
	private string mPath3;

	//相机
	public Camera CameraTrans;
	// Use this for initialization
	void Start ()
	{
		//初始化路径
		mPath1 = Application.dataPath + "/ScreenShot/ScreenShot1.png";
		mPath2 = Application.dataPath + "/ScreenShot/ScreenShot2.png";
		mPath3 = Application.dataPath + "/ScreenShot/ScreenShot3.png";
	}

//	private IEnumerator  CaptureByCamera (Camera mCamera, Rect mRect, string mFileName)
//	{
//		//等待渲染线程结束
//		yield return new WaitForEndOfFrame ();
//		
//		//初始化RenderTexture
//		RenderTexture mRender = new RenderTexture ((int)mRect.width, (int)mRect.height, 0);
//		//设置相机的渲染目标
//		mCamera.targetTexture = mRender;
//		//开始渲染
//		mCamera.Render ();
//		
//		//激活渲染贴图读取信息
//		RenderTexture.active = mRender;
//		
//		Texture2D mTexture = new Texture2D ((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
//		//读取屏幕像素信息并存储为纹理数据
//		mTexture.ReadPixels (mRect, 0, 0);
//		//应用
//		mTexture.Apply ();
//		
//		//释放相机，销毁渲染贴图
//		mCamera.targetTexture = null;   
//		RenderTexture.active = null; 
//		GameObject.Destroy (mRender);  
//		
//		//将图片信息编码为字节信息
//		byte[] bytes = mTexture.EncodeToPNG ();  
//		//保存
//		System.IO.File.WriteAllBytes (mFileName, bytes);
//		//如果需要可以返回截图
//		//return mTexture;
//	}


	/*下面的插件的截图方法*/

	/// <summary>
	/// Captures the by defaut.
	/// </summary>
	[MenuItem("Capture/FullScreen")]
	public static void CaptureByDefaut ()
	{
		Debug.Log ("today ==" + System.DateTime.Now.ToLongTimeString ());
		string savePath = Application.dataPath + "/ScreenShot/" + System.DateTime.Now.ToLongTimeString () + ".png";
		Application.CaptureScreenshot (savePath, 0);
	}
	[MenuItem("Capture/FullScreen", true)]
	public static bool CaptureByDefaultVaild ()
	{
		return Application.isPlaying;
	}
	
	[MenuItem("Capture/BySize")]
	public static void CaptureBySize ()
	{
//		Rect wr = new Rect (0, 0, 500, 500);
		ScreenShot ss = EditorWindow.GetWindow (typeof(ScreenShot), true, "CaptureBySize") as ScreenShot;
//		ScreenShot ss = EditorWindow.GetWindowWithRect (typeof(ScreenShot), wr, true, "CaptureBySize") as ScreenShot;
		ss.Show ();
	}
//	[MenuItem("Capture/BySize", true)]
//	public static bool CaptureBySizeVaild ()
//	{
//		return Application.isPlaying;
//	}


	float originX;
	float originY;
	float sizeWidth;
	float sizeHeight;
	string[] cameraNames;
	bool isCapture;
	void OnGUI ()
	{	
		if (isByCamera) {
			EditorGUILayout.LabelField ("Camera");
			index = EditorGUILayout.Popup (index, cameraNames);
		}
		EditorGUILayout.LabelField ("origin");
		originX = EditorGUILayout.Slider ("x", originX, 0, Screen.width);
		originY = EditorGUILayout.Slider ("y", originY, 0, Screen.height);
		EditorGUILayout.LabelField ("size");
		sizeWidth = EditorGUILayout.FloatField ("width", sizeWidth);
		sizeHeight = EditorGUILayout.FloatField ("heigth", sizeHeight);
		
		if (GUILayout.Button ("Captureaa")) {
			isCapture = true;

		}

	}	

	void Update ()
	{
		if (isCapture) {
			string savePath = Application.dataPath + "/ScreenShot/" + System.DateTime.Now.ToLongTimeString () + ".png";
			if (isByCamera) {
				Camera cmra = cameras [index] as Camera;
				CaptureByCamera (cmra, new Rect (originX, originY, sizeWidth, sizeHeight), savePath);
			} else {
				
				CaptureBySize (new Rect (originX, originY, sizeWidth, sizeHeight), savePath);
			}
			this.Close ();
			isCapture = false;
		}
	}


	void CaptureBySize (Rect rect, string mFileName)
	{
		//初始化Texture2D
		Texture2D mTexture = new Texture2D ((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
		//读取屏幕像素信息并存储为纹理数据
		mTexture.ReadPixels (rect, 0, 0);
		//应用
		mTexture.Apply ();

		//将图片信息编码为字节信息
		byte[] bytes = mTexture.EncodeToPNG ();  
		//保存
		System.IO.File.WriteAllBytes (mFileName, bytes);

	}

	private bool isByCamera = false;
	private int index;
	Object[] cameras;
	[MenuItem("Capture/ByCamera")]
	public static void CaptureByCamera ()
	{
//		Camera c = Transform.FindObjectOfType (typeof(Camera)) as Camera;
//
//		CaptureByCamera (Camera.main, new Rect (0, 0, 1000, 1000), Application.dataPath + "/ScreenShot/aaaaaa");
		ScreenShot ss = EditorWindow.GetWindow (typeof(ScreenShot), true, "CaptureByCamera") as ScreenShot;
		ss.Show ();
		ss.isByCamera = true;
		ss.cameras = Transform.FindObjectsOfType (typeof(Camera));
		ss.cameraNames = new string[ss.cameras.Length];
		if (ss.cameras.Length > 0) {
			for (int i = 0; i< ss.cameras.Length; i++) {
				string name = ss.cameras [i].name;
				ss.cameraNames [i] = name;
				Debug.Log (ss.cameraNames [i]);
			}
		}
	}

	[MenuItem("Capture/ByCamera", true)]
	public static bool CaptureByCameraVaild ()
	{
		return Application.isPlaying;
	}

	public static void CaptureByCamera (Camera mCamera, Rect mRect, string mFileName)
	{
//		//初始化RenderTexture
//		RenderTexture mRender = new RenderTexture ((int)mRect.width, (int)mRect.height, 0);
//		//设置相机的渲染目标
//		mCamera.targetTexture = mRender;
//		//开始渲染
//		mCamera.Render ();
//		
//		//激活渲染贴图读取信息
//		RenderTexture.active = mRender;
//		
//		Texture2D mTexture = new Texture2D ((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
//		//读取屏幕像素信息并存储为纹理数据
//		mTexture.ReadPixels (mRect, 0, 0);
//		//应用
//		mTexture.Apply ();
//		
//		//释放相机，销毁渲染贴图
//		mCamera.targetTexture = null;   
//		RenderTexture.active = null; 
//		GameObject.Destroy (mRender);  
//		
//		//将图片信息编码为字节信息
//		byte[] bytes = mTexture.EncodeToPNG ();  
//		//保存
//		System.IO.File.WriteAllBytes (mFileName, bytes);


		// 创建一个RenderTexture对象  
		RenderTexture rt = new RenderTexture ((int)mRect.width, (int)mRect.height, 0);  
		// 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
		mCamera.targetTexture = rt;  
		mCamera.Render ();  
		//ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
		//ps: camera2.targetTexture = rt;  
		//ps: camera2.Render();  
		//ps: -------------------------------------------------------------------  
		
		// 激活这个rt, 并从中中读取像素。  
		RenderTexture.active = rt;  
		Texture2D screenShot = new Texture2D ((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);  
		screenShot.ReadPixels (mRect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
		screenShot.Apply ();  
		
		// 重置相关参数，以使用camera继续在屏幕上显示  
		mCamera.targetTexture = null;  
		//ps: camera2.targetTexture = null;  
		RenderTexture.active = null; // JC: added to avoid errors  
		GameObject.Destroy (rt);  

		// 最后将这些纹理数据，成一个png图片文件  
		byte[] bytes = screenShot.EncodeToPNG ();    
		System.IO.File.WriteAllBytes (mFileName, bytes);  
	}
}























