using UnityEngine;
using UnityEngine.EventSystems;

public class RotationControls: MonoBehaviour{
	private const float ZoomSpeed = .5f, ZoomMin = -14, ZoomMax = 7, LerpDuration = .5f;
	public float dragSpeed = .25f, orbitSpeed = 0.025f;
	private bool isRotating = true;
	public Transform spotlight, camDolly;
	private Camera cam;
	private Vector3 prevMousePos = Vector3.zero, mousePosDelta = Vector3.zero;
	private bool isDragging = false;
	private float timeElapsed = LerpDuration, startCamZ, targetCamZ;

	private void Awake(){
		cam = Camera.main;
		camDolly = cam.transform.parent;
		startCamZ = targetCamZ = Mathf.Clamp( cam.transform.localPosition.z, ZoomMin, ZoomMax );
	}

	public void SetRotating( bool value ) => isRotating = value;

	private void Update(){
		var eventSystem = EventSystem.current;
		if( Input.GetMouseButtonDown( 0 ) && !eventSystem.IsPointerOverGameObject() ){
			isDragging = true;
			prevMousePos = Input.mousePosition;
		}else if( !Input.GetMouseButton( 0 ) )
			isDragging = false;

		if( isDragging ){
			mousePosDelta = ( Input.mousePosition-prevMousePos )*dragSpeed;
			if( Vector3.Dot( camDolly.transform.up, Vector3.up ) > 0 )
				camDolly.transform.RotateAround( spotlight.position, Vector3.up, Vector3.Dot( mousePosDelta, Vector3.right ) );
			else
				camDolly.transform.RotateAround( spotlight.position, Vector3.up, Vector3.Dot( mousePosDelta, Vector3.left ) );
			camDolly.transform.RotateAround( spotlight.position, camDolly.transform.right, Vector3.Dot( mousePosDelta, Vector3.down ) );
			prevMousePos = Input.mousePosition;
		}else if( isRotating )
			camDolly.transform.RotateAround( spotlight.position, Vector3.up, orbitSpeed );

		var scrollwheelDelta = Input.mouseScrollDelta;
		if( scrollwheelDelta.y!=0 && !eventSystem.IsPointerOverGameObject() ){
			startCamZ = cam.transform.localPosition.z;
			targetCamZ = Mathf.Clamp( targetCamZ+scrollwheelDelta.y*ZoomSpeed, ZoomMin, ZoomMax );
			timeElapsed = 0;
		}

		var time = timeElapsed/LerpDuration;
		time = time * time * ( 3f-2f*time );
		if( timeElapsed<LerpDuration ){
			cam.transform.localPosition = new Vector3( 0, 0, Mathf.Lerp( startCamZ, targetCamZ, time ) );
			timeElapsed += Time.deltaTime;
		}else if( cam.transform.localPosition.z!=targetCamZ )
			cam.transform.localPosition = new Vector3( 0, 0, targetCamZ );
	}
}