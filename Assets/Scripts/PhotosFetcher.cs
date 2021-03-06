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
	public PhotoLocator photoLocatorPrefab;
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
						var toggle = photo.GetComponent< Toggle >();
						photo.name = filename;
						toggle.group = toggleGroup;
						PhotoLocator photoLocator = null;
						if( filename.Contains( "@" ) ){
							photoLocator = Instantiate( photoLocatorPrefab, photo.transform );
							var coords = filename.Substring( 0, filename.Length-5 ).Substring( filename.IndexOf( "@" )+1 ).Split( new []{ ',' } );
							photoLocator.SetPosRot(
								new Vector3( float.Parse( coords[ 0 ] ), float.Parse( coords[ 1 ] ), float.Parse( coords[ 2 ] ) ),
								new Vector3( float.Parse( coords[ 3 ] ), float.Parse( coords[ 4 ] ), float.Parse( coords[ 5 ] ) )
							);
							toggle.onValueChanged.AddListener( isOn => photoLocator.IsOn = isOn );
						}else
							toggle.onValueChanged.AddListener( isOn => toggle.isOn = false );		// Since the filename didn't contain coords, we have nothing to display.
						StartCoroutine( LoadFromWeb( photo, photoLocator, GitHubPhotoPreamble+name+"/"+filename ) );
					}
					line = reader.ReadLine();
				}
			}
		}
	}

	IEnumerator LoadFromWeb( GameObject photo, PhotoLocator photoLocator, string url ){
		var webRequest = new UnityWebRequest( url );
		var texDownloadHandler = new DownloadHandlerTexture( true );
		webRequest.downloadHandler = texDownloadHandler;
		yield return webRequest.SendWebRequest();
		if( webRequest.result==Result.Success ){
			Texture2D tex = texDownloadHandler.texture;
			var img = photo.transform.Find( "Mask/Image" ).GetComponent< Image >();
			img.type = Image.Type.Simple;
			img.sprite = Sprite.Create( tex, new Rect( 0, 0, tex.width, tex.height ), Vector2.zero, 1f );
			img.color = Color.white;
			( photo.transform as RectTransform ).SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, tex.height*PhotoWidth/tex.width );
			photoLocator?.gameObject.SetActive( true );
		}
	}
}