using UnityEngine;

public class PropellerSpinner: MonoBehaviour{
	public float speed = 0.5f;

	private void Update() => transform.Rotate( Vector3.up, speed );
}