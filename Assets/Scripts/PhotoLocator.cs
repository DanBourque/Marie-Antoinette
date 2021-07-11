using UnityEngine;

public class PhotoLocator: MonoBehaviour{
	private Vector3[] worldCorners = new Vector3[ 4 ];
	private Mesh mesh;
	private Transform indicator;
	public RectTransform RectTransform{ set; get; }

	private void Awake(){
		GetComponent< MeshFilter >().mesh = mesh = BuildMesh();
		GetComponent< MeshRenderer >().material.renderQueue = 2999;		// Transparent is 3000, but we set ours to 2999 to allow UI elements to occlude it.
		indicator = transform.Find( "Indicator" );
	}

	public void SetPosition( Vector3 position ) => indicator.position = position;

	private void Update(){
		Vector3[] vertices = mesh.vertices;

		RectTransform.GetWorldCorners( worldCorners );
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