using UnityEngine;

public class HatchOpener: MonoBehaviour{
	private const float TableTravel = 0.6f;
	public float speed = 0.4f;
	public Transform table;
	private Vector3 initialTablePos;
	private float angle = 0;

	private void Awake() => initialTablePos = table.localPosition;

	private void Update(){
		angle += speed;
		if( angle>360 )
			angle -= 360;

		var sin = Mathf.Sin( Mathf.Deg2Rad*angle );
		transform.localEulerAngles = new Vector3( Mathf.Min( sin*-90, 0 ), 0, 0 );
		table.transform.localPosition = initialTablePos + new Vector3( 0, Mathf.Min( sin*TableTravel, 0 ), 0 );
	}
}