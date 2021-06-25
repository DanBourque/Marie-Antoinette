using UnityEngine;

public class PropellerSpinner: MonoBehaviour{
	public Transform propeller;
	public float speed = 0.5f;

	private void Update() => propeller.Rotate( Vector3.up, speed );
}