using UnityEngine;

public class RotationControls: MonoBehaviour{
	private float Speed = .1f;
	private Vector3 prevMousePos = Vector3.zero, mousePosDelta = Vector3.zero;

	private void Update(){
		if( Input.GetMouseButton( 0 ) ){
			mousePosDelta = ( Input.mousePosition-prevMousePos )*Speed;
			if( Vector3.Dot( transform.up, Vector3.up ) > 0 )
				transform.Rotate( transform.up, -Vector3.Dot( mousePosDelta, Camera.main.transform.right ), Space.World );
			else
				transform.Rotate( transform.up, Vector3.Dot( mousePosDelta, Camera.main.transform.right ), Space.World );
			transform.Rotate( Camera.main.transform.right, Vector3.Dot( mousePosDelta, Camera.main.transform.up ), Space.World );
		}
		prevMousePos = Input.mousePosition;
	}
}