using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using static UnityEngine.Networking.UnityWebRequest;

public class PhotosFetcher: MonoBehaviour{
	private const string GitHubPhotoPreamble = "https://danbourque.github.io/Marie-Antoinette/Photos/";
	private const string GitHubFolderPreamble = "https://api.github.com/repos/DanBourque/Marie-Antoinette/contents/docs/Photos/";
	public GameObject photoPrefab;
	public string roomName;
	public string[] photoAddrs;

	void Start(){
		var toggleGroup = GetComponent< ToggleGroup >();

		StartCoroutine( GetPhotoList( GitHubFolderPreamble+roomName ) );

		foreach( var photoAddr in photoAddrs ){
			var photo = Instantiate( photoPrefab, transform );
			photo.GetComponent< Toggle >().group = toggleGroup;
			StartCoroutine( LoadFromWeb( photo, GitHubPhotoPreamble+photoAddr ) );
		}
	}

	IEnumerator GetPhotoList( string url ){
		var webRequest = new UnityWebRequest( url );
		webRequest.downloadHandler = new DownloadHandlerBuffer();
		webRequest.SetRequestHeader( "Accept", "application/vnd.github.v3+json" );
		yield return webRequest.SendWebRequest();
		if( webRequest.result==Result.Success ){
			Debug.Log( webRequest.downloadHandler.text );
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