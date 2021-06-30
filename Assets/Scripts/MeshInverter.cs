using UnityEngine;

public class MeshInverter: MonoBehaviour{
	public Transform[] parts;
	public Material invertedMaterial;

	private void Start(){
		foreach( Transform child in parts ){
			var clone = Instantiate( child, child );
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localRotation = Quaternion.identity;
			clone.GetComponent< Renderer >().material = invertedMaterial;
		}
	}
}