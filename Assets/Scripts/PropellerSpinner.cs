using UnityEngine;

public class PropellerSpinner: MonoBehaviour{
	public const float Speed = 100f;

	private void Update() => transform.Rotate( Vector3.up, Time.deltaTime*Speed );
}