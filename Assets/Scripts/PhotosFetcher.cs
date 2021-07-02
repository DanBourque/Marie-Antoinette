using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using static UnityEngine.Networking.UnityWebRequest;
using System.IO;

public class PhotosFetcher: MonoBehaviour{
	private const string GitHubPhotoPreamble = "https://danbourque.github.io/Marie-Antoinette/Photos/";
	private const string GitHubFolderPreamble = "https://api.github.com/repos/DanBourque/Marie-Antoinette/contents/docs/Photos/";
	private const string FilenameLinePreamble = "    \"name\": ";
	public GameObject photoPrefab;
	private float PhotoWidth;

	void Start() => StartCoroutine( GetPhotoList( GetComponent< ToggleGroup >() ) );

	IEnumerator GetPhotoList( ToggleGroup toggleGroup ){
		PhotoWidth = ( photoPrefab.transform as RectTransform ).rect.width;
		var webRequest = new UnityWebRequest( GitHubFolderPreamble+name );
		webRequest.downloadHandler = new DownloadHandlerBuffer();
		webRequest.SetRequestHeader( "Accept", "application/vnd.github.v3+json" );	// Not necessary, but recommended by GitHub.
		yield return webRequest.SendWebRequest();
		if( webRequest.result==Result.Success ){
			using( StringReader reader = new StringReader( webRequest.downloadHandler.text ) ){
				var line = reader.ReadLine();
				while( line!=null ){
					if( line.StartsWith( FilenameLinePreamble ) ){
						var filename = line.Substring( FilenameLinePreamble.Length ).Trim( new []{ '"', ',' } );
						var photo = Instantiate( photoPrefab, transform );
						photo.name = filename;
						photo.GetComponent< Toggle >().group = toggleGroup;
						StartCoroutine( LoadFromWeb( photo, GitHubPhotoPreamble+name+"/"+filename ) );
					}
					line = reader.ReadLine();
				}
			}
		}
	}

	IEnumerator LoadFromWeb( GameObject photo, string url ){
		var webRequest = new UnityWebRequest( url );
		var texDownloadHandler = new DownloadHandlerTexture( true );
		webRequest.downloadHandler = texDownloadHandler;
		yield return webRequest.SendWebRequest();
		if( webRequest.result==Result.Success ){
			Texture2D tex = texDownloadHandler.texture;
			var img = photo.GetComponent< Image >();
			img.type = Image.Type.Simple;
			img.sprite = Sprite.Create( tex, new Rect( 0, 0, tex.width, tex.height ), Vector2.zero, 1f );
			( photo.transform as RectTransform ).SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, tex.height*PhotoWidth/tex.width );
		}
	}
}