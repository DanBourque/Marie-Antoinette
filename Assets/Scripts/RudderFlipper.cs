using UnityEngine;

public class RudderFlipper: MonoBehaviour{
	public const float Speed = 10;
	private float angle = 0;

	private void Update(){
		angle += Time.deltaTime*Speed;
		if( angle>360 )
			angle -= 360;

		transform.localEulerAngles = new Vector3( 0, 0, Mathf.Sin( Mathf.Deg2Rad*angle )*45 );
	}
}