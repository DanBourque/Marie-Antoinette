using System.Linq;
using UnityEngine;

public class MeshInverter: MonoBehaviour{
	public Transform[] shellParts;
	public Material invertedMaterial, shellInsideMaterial;

	private void Start(){

		foreach( Transform child in transform ){
			var clone = Instantiate( child, child );
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localRotation = Quaternion.identity;

			if( shellParts.Contains( child ) )
				clone.GetComponent< Renderer >().material = shellInsideMaterial;
			else
				clone.GetComponent< Renderer >().material = invertedMaterial;
		}
	}
}