using UnityEngine;

public class RoomExtender: MonoBehaviour{
	private const float SmoothTime = 3;
	private bool extended = false;
	private Vector3 initialPos, targetPos;
	public float distance = 2;

	public bool Extended{
		set{
			extended = value;
			targetPos = initialPos + new Vector3( extended ? distance/2 : 0, 0, 0 );
		}
		get{ return extended; }
	}

	private void Start() => initialPos = targetPos = transform.localPosition;

	private void Update() => transform.localPosition = Vector3.Lerp( transform.localPosition, targetPos, SmoothTime*Time.deltaTime );
}