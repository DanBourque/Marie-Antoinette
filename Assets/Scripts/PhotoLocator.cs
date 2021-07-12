using UnityEngine;
using UnityEngine.UI;

public class PhotoLocator: MonoBehaviour{
	private const float AutoScrollPadding = 10f;
	private Vector3[] worldCorners = new Vector3[ 4 ];
	private Mesh mesh;
	private MeshRenderer meshRenderer;
	private MeshRenderer MeshRenderer{
		get{
			if( !meshRenderer )
				meshRenderer = GetComponent< MeshRenderer >();
			return meshRenderer;
		}
	}
	private ScrollRect viewportScrollRect;
	private ScrollRect ViewportScrollRect{
		get{
			if( !viewportScrollRect )
				viewportScrollRect = GetComponentInParent< ScrollRect >();
			return viewportScrollRect;
		}
	}
	private Vector3 position;
	public Transform indicator;
	private RectTransform rectTransform;
	private bool isOn = false;
	public bool IsOn{
		set{
			isOn = value;
			MeshRenderer.enabled = isOn;
			
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

	private void Awake(){
		rectTransform = transform.parent as RectTransform;
		GetComponent< MeshFilter >().mesh = mesh = BuildMesh();
		MeshRenderer.material.renderQueue = 2999;		// Transparent is 3000, but we set ours to 2999 to allow UI elements to occlude it.
		var rootScale = transform.root.localScale;
		transform.localScale = new Vector3( 1/rootScale.x, 1/rootScale.y, 1/rootScale.z );
	}

	public void SetPosition( Vector3 position ){
		transform.rotation = Quaternion.identity;
		transform.position = Vector3.zero;
		this.position = position;
	}

	private void Update(){
		transform.rotation = Quaternion.identity;
		transform.position = Vector3.zero;
		indicator.position = position;

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
		vertices[ 7 ] = indicator.position;

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