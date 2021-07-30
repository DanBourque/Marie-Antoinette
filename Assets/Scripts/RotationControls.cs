using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotationControls: MonoBehaviour{
	private const float ZoomSpeed = .5f, ZoomMin = -14, ZoomMax = 7;
	public float dragSpeed = .25f, orbitSpeed = 0.025f;
	private bool isRotating = true, isTouring = false, isDragging = false;
	public Transform spotlight;
	private Camera cam;
	private Vector3 prevMousePos = Vector3.zero, mousePosDelta = Vector3.zero, preTourCamPosition;
	private Quaternion preTourCamRotation;
	private Transform camDolly, uiControls;
//	private Toggle rotateToggle, tourToggle;

	private void Awake(){
		cam = Camera.main;
		camDolly = cam.transform.parent;
		uiControls = FindObjectOfType< Canvas >( true ).transform;
//		rotateToggle = uiControls.Find( "Top Controls/Rotate Toggle" ).GetComponent< Toggle >();
//		tourToggle = uiControls.Find( "Top Controls/Tour Toggle" ).GetComponent< Toggle >();
	}

	public void SetRotating( bool value ) => isRotating = value;

	public void SetTouring( bool value ){
		isTouring = value;
		if( isTouring ){
			preTourCamPosition = camDolly.transform.localPosition;
			preTourCamRotation = camDolly.transform.localRotation;
		}else{
			camDolly.transform.localPosition = preTourCamPosition;
			camDolly.transform.localRotation = preTourCamRotation;
		}
		ShowUIControls( !isTouring );
		camDolly.GetComponent< Animator >().enabled = isTouring;
	}

	private void ShowUIControls( bool show ){
		uiControls.Find( "Room Selector" ).gameObject.SetActive( show );
		uiControls.Find( "Photos" ).gameObject.SetActive( show );
		uiControls.Find( "X-Ray Controls" ).gameObject.SetActive( show );
	}

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
		if( scrollwheelDelta.y!=0 && !eventSystem.IsPointerOverGameObject() )
			cam.transform.localPosition = new Vector3( 0, 0, Mathf.Clamp( cam.transform.localPosition.z+scrollwheelDelta.y*ZoomSpeed, ZoomMin, ZoomMax ) );
	}
}