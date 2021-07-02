using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using static UnityEngine.Networking.UnityWebRequest;
using System.IO;

public class PhotosFetcher: MonoBehaviour{
	private const string GDrivePreamble = "https://drive.google.com/uc?export=download&id=";
	public GameObject photoPrefab;
	public string[] photoGDriveIDs;

	void Start(){
		var toggleGroup = GetComponent< ToggleGroup >();
		foreach( var photoGDriveID in photoGDriveIDs ){
			var photo = Instantiate( photoPrefab, transform );
			photo.GetComponent< Toggle >().group = toggleGroup;
			StartCoroutine( LoadFromWeb( photo, GDrivePreamble+photoGDriveID ) );
		}
	}

	IEnumerator LoadFromWeb( GameObject photo, string url ){
		var webRequest = new UnityWebRequest( url );
		var texDownloadHandler = new DownloadHandlerTexture( true );
		webRequest.downloadHandler = texDownloadHandler;
		yield return webRequest.SendWebRequest();
		if( webRequest.result==Result.Success ){
			Texture2D tex = texDownloadHandler.texture;
			photo.GetComponent< Image >().sprite = Sprite.Create( tex, new Rect( 0, 0, tex.width, tex.height ), Vector2.zero, 1f );
			float height = 124;
			( photo.transform as RectTransform ).SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, height );
		}
	}
}