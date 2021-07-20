using UnityEngine;

public class RoomExtender: MonoBehaviour{
	private const float LerpDuration = 2;		// How long, in seconds, should the transition take.
	private bool extended = false;
	private Vector3 initialPos, startPos, targetPos;
	public float distance = 2;
	private float timeElapsed;

	public bool Extended{
		set{
			extended = value;
			startPos = transform.localPosition;
			targetPos = initialPos + new Vector3( extended ? distance/2 : 0, 0, 0 );
			timeElapsed = 0;
		}
		get{ return extended; }
	}

	private void Start() => initialPos = targetPos = transform.localPosition;

	private void Update(){
		var time = timeElapsed/LerpDuration;
		time = time * time * ( 3f-2f*time );

		if( timeElapsed<LerpDuration ){
			transform.localPosition = Vector3.Lerp( startPos, targetPos, time );
			timeElapsed += Time.deltaTime;
		}else
			transform.localPosition = targetPos;
	}
}