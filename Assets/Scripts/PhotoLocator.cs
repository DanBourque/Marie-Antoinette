using UnityEngine;
using UnityEngine.UI;

public class PhotoLocator: MonoBehaviour{
	private const float AutoScrollPadding = 10f;
	public Material fustrumDefault, fustrumHighlight;
	private Vector3[] worldCorners = new Vector3[ 4 ];
	private Mesh mesh;
	private MeshRenderer MeshRenderer{ get => meshRenderer ? meshRenderer : meshRenderer = GetComponent< MeshRenderer >(); }
	private MeshRenderer FustrumRenderer{ get => fustrumRenderer ? fustrumRenderer : fustrumRenderer = fustrum.GetComponent< MeshRenderer >(); }
	private MeshRenderer meshRenderer, fustrumRenderer;
	private ScrollRect ViewportScrollRect{ get => viewportScrollRect ? viewportScrollRect : viewportScrollRect = GetComponentInParent< ScrollRect >(); }
	private ScrollRect viewportScrollRect;
	private Vector3 position;
	private Quaternion rotation;
	public Transform fustrum;
	private RectTransform rectTransform;
	public bool IsOn{
		set{
			isOn = value;
			MeshRenderer.enabled = isOn;
			FustrumRenderer.material = isOn ? fustrumHighlight : fustrumDefault;

			// Make sure the associated photo is visible in the scrollrect's viewport.
			Canvas.ForceUpdateCanvases();
			var scrollRect = ViewportScrollRect;
			var scrollPosition = scrollRect.content.anchoredPosition;
			float elementTop = rectTransform.anchoredPosition.y, elementBottom = elementTop-rectTransform.rect.height;
			float visibleContentTop = -scrollPosition.y-AutoScrollPadding, visibleContentBottom = -scrollPosition.y-scrollRect.viewport.rect.height+AutoScrollPadding;
			scrollPosition.y += elementTop > visibleContentTop ? visibleContentTop-elementTop : ( elementBottom < visibleContentBottom ? visibleContentBottom-elementBottom : 0f );
			scrollRect.content.anchoredPosition = scrollPosition;
		}
		get => isOn;
	}
	private bool isOn = false;

	private void Awake(){
		rectTransform = transform.parent as RectTransform;
		GetComponent< MeshFilter >().mesh = mesh = BuildMesh();
		MeshRenderer.material.renderQueue = 2999;   // Transparent is 3000, but we set ours to 2999 to allow UI elements to occlude it.
	}

	public void SetPosRot( Vector3 position, Vector3 rotation ){
		this.position = fustrum.position = position;
		this.rotation = fustrum.rotation = Quaternion.Euler( rotation );
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		var rootScale = transform.root.localScale;
		transform.localScale = new Vector3( 1/rootScale.x, 1/rootScale.y, 1/rootScale.z );
	}

	private void Update(){
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		var rootScale = transform.root.localScale;
		transform.localScale = new Vector3( 1/rootScale.x, 1/rootScale.y, 1/rootScale.z );
		fustrum.position = position;
		fustrum.rotation = rotation;

		if( !isOn )
			return;

		Vector3[] vertices = mesh.vertices;

		rectTransform.GetWorldCorners( worldCorners );
		vertices[ 0 ] = worldCorners[ 0 ];
		vertices[ 1 ] = worldCorners[ 3 ];
		vertices[ 2 ] = worldCorners[ 2 ];
		vertices[ 3 ] = worldCorners[ 1 ];
		vertices[ 4 ] =
		vertices[ 5 ] =
		vertices[ 6 ] =
		vertices[ 7 ] = fustrum.position;

		mesh.vertices = vertices;
	}

	private static Mesh BuildMesh(){
		Vector3[] vertices = {
			new Vector3( 0, 0, 0 ),
			new Vector3( 1, 0, 0 ),
			new Vector3( 1, 1, 0 ),
			new Vector3( 0, 1, 0 ),
			new Vector3( 0, 1, 1 ),
			new Vector3( 1, 1, 1 ),
			new Vector3( 1, 0, 1 ),
			new Vector3( 0, 0, 1 ),
		};

		int[] triangles = {
			0, 2, 1, 0, 3, 2,	// front
			2, 3, 4, 2, 4, 5,	// top
			1, 2, 5, 1, 5, 6,	// right
			0, 7, 4, 0, 4, 3,	// left
			5, 4, 7, 5, 7, 6,	// back
			0, 6, 7, 0, 1, 6	// bottom
		};
		
		Mesh mesh = new Mesh();
		mesh.name = "Photo Locator Dynamic Mesh";
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		return mesh;
	}
}