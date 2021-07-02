using UnityEngine;
using UnityEngine.EventSystems;

public class RotationControls: MonoBehaviour{
	public float dragSpeed = .1f, orbitSpeed = 0.025f;
	public Transform spotlight;
	private Camera cam;
	private Vector3 prevMousePos = Vector3.zero, mousePosDelta = Vector3.zero;

	private void Awake() => cam = Camera.main;

	private void Update(){
		if( Input.GetMouseButton( 0 ) && !EventSystem.current.IsPointerOverGameObject() ){
			mousePosDelta = ( Input.mousePosition-prevMousePos )*dragSpeed;
			if( Vector3.Dot( cam.transform.up, Vector3.up ) > 0 )
				cam.transform.RotateAround( spotlight.position, Vector3.up, Vector3.Dot( mousePosDelta, Vector3.right ) );
			else
				cam.transform.RotateAround( spotlight.position, Vector3.up, Vector3.Dot( mousePosDelta, Vector3.left ) );
			cam.transform.RotateAround( spotlight.position, cam.transform.right, Vector3.Dot( mousePosDelta, Vector3.down ) );
		}else
			cam.transform.RotateAround( spotlight.position, Vector3.up, orbitSpeed );
		prevMousePos = Input.mousePosition;
	}
}