using UnityEngine;

public class PhotoLocator: MonoBehaviour{
	private Vector3[] worldCorners = new Vector3[ 4 ];
	private Mesh mesh;
	private MeshRenderer meshRenderer;
	public Vector3 GPosition{ set; get; }
	private bool isOn = false;
	public bool IsOn{
		set{
			isOn = value;
			meshRenderer.enabled = isOn;
		}
		get => isOn;
	}
	public RectTransform RectTransform{ set; get; }

	private void Start(){
		GetComponent< MeshFilter >().mesh = mesh = BuildMesh();
		meshRenderer = GetComponent< MeshRenderer >();
	}

	private void Update(){
		if( IsOn ){
			RectTransform.GetWorldCorners( worldCorners );
			Vector3[] vertices = mesh.vertices;

			vertices[ 0 ] = worldCorners[ 0 ];
			vertices[ 1 ] = worldCorners[ 3 ];
			vertices[ 2 ] = worldCorners[ 2 ];
			vertices[ 3 ] = worldCorners[ 1 ];
			vertices[ 4 ] =
			vertices[ 5 ] =
			vertices[ 6 ] =
			vertices[ 7 ] = GPosition;

			mesh.vertices = vertices;
		}
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
		mesh.name = "Dynamic Mesh";
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		return mesh;
	}
}