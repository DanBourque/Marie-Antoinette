using System.Collections.Generic;
using UnityEngine;

public class XRayPrep: MonoBehaviour{
	public Transform plane;
	public Shader textureShader;
	private HashSet< Material > materials = new HashSet< Material >();

	private void Start() => PrepPatient( transform );

	private void PrepPatient( Transform patient ){
		if( patient.name=="Mast" )
			return;

		foreach( Transform child in patient )
			PrepPatient( child );

		var renderer = patient.GetComponent< Renderer >();
		if( renderer!=null ){
			foreach( var material in renderer.materials ){
				materials.Add( material );
				material.shader = textureShader;
			}
		}
	}

	private void Update(){
		foreach( var material in materials ){
			material.SetVector( "_PlanePosition", plane.transform.position );
			material.SetVector( "_PlaneNormal", plane.transform.forward );
		}
	}
}