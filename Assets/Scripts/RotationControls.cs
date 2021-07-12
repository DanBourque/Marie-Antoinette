using UnityEngine;
using UnityEngine.EventSystems;

public class RotationControls: MonoBehaviour{
	public float dragSpeed = .25f, orbitSpeed = 0.025f;
	private bool isRotating = true;
	public Transform spotlight;
	private Camera cam;
	private Vector3 prevMousePos = Vector3.zero, mousePosDelta = Vector3.zero;
	private bool isDragging = false;

	private void Awake() => cam = Camera.main;

	public void SetRotating( bool value ) => isRotating = value;

	private void Update(){
		if( Input.GetMouseButtonDown( 0 ) && !EventSystem.current.IsPointerOverGameObject() ){
			isDragging = true;
			prevMousePos = Input.mousePosition;
		}else if( !Input.GetMouseButton( 0 ) )
			isDragging = false;

		if( isDragging ){
			mousePosDelta = ( Input.mousePosition-prevMousePos )*dragSpeed;
			if( Vector3.Dot( cam.transform.up, Vector3.up ) > 0 )
				cam.transform.RotateAround( spotlight.position, Vector3.up, Vector3.Dot( mousePosDelta, Vector3.right ) );
			else
				cam.transform.RotateAround( spotlight.position, Vector3.up, Vector3.Dot( mousePosDelta, Vector3.left ) );
			cam.transform.RotateAround( spotlight.position, cam.transform.right, Vector3.Dot( mousePosDelta, Vector3.down ) );
			prevMousePos = Input.mousePosition;
		}else if( isRotating )
			cam.transform.RotateAround( spotlight.position, Vector3.up, orbitSpeed );
	}
}