using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Axis{ X, Y, Z }

public class XRayControls: MonoBehaviour{
	private const string PlanePosition = "_PlanePosition", PlaneNormal = "_PlaneNormal";
	private const float ScanCoverage = 1.8f;
	private Dictionary< Axis, Slider > sliders = new Dictionary< Axis, Slider >();
	private Dictionary< Axis, float > extents = new Dictionary< Axis, float >{ { Axis.X, 2.25f }, { Axis.Y, 2.17f }, { Axis.Z, 6.7f } };
	private Shader defaultShader;
	public Shader xrayShader;
	private HashSet< Material > materials = new HashSet< Material >();
	public Transform patient, plane;
	private bool materialsInitialized = false;

	private void Awake(){
		defaultShader = Shader.Find( "Universal Render Pipeline/Lit" );
		sliders[ Axis.X ] = transform.Find( "X Axis" ).GetComponent< Slider >();
		sliders[ Axis.Y ] = transform.Find( "Y Axis" ).GetComponent< Slider >();
		sliders[ Axis.Z ] = transform.Find( "Z Axis" ).GetComponent< Slider >();
		foreach( Axis axis in Enum.GetValues( typeof( Axis ) ) )
			sliders[ axis ].onValueChanged.AddListener( v => OnValueChanged( axis, v ) );

		PrepPatient( patient );
	}

	private void PrepPatient( Transform patient ){
		if( patient.name=="Mast" )
			return;

		foreach( Transform child in patient )
			PrepPatient( child );

		var renderer = patient.GetComponent< Renderer >();
		if( renderer!=null )
			foreach( var material in renderer.materials )
				materials.Add( material );
	}

	private void OnValueChanged( Axis axis, float value ){
		foreach( Axis nextAxis in Enum.GetValues( typeof( Axis ) ) )
			if( nextAxis!=axis )
				sliders[ nextAxis ].SetValueWithoutNotify( 0 );

		var sign = Math.Sign( value );
		switch( axis ){
			case Axis.X:
				plane.localRotation = Quaternion.LookRotation( value>=0 ? Vector3.back : Vector3.forward, Vector3.up );
				plane.localPosition = new Vector3( 0, 0, -sign*extents[ axis ]+ScanCoverage*extents[ axis ]*value );
			break;
			case Axis.Y:
				plane.localRotation = Quaternion.LookRotation( value>=0 ? Vector3.up : Vector3.down, Vector3.forward );
				plane.localPosition = new Vector3( 0, sign*extents[ axis ]-ScanCoverage*extents[ axis ]*value, 0 );
			break;
			case Axis.Z:
				plane.localRotation = Quaternion.LookRotation( value>=0 ? Vector3.right : Vector3.left, Vector3.up );
				plane.localPosition = new Vector3( sign*extents[ axis ]-ScanCoverage*extents[ axis ]*value, 0, 0 );
			break;
		}

		foreach( var material in materials ){
			material.SetVector( PlanePosition, plane.position );
			material.SetVector( PlaneNormal, plane.forward );
		}
	}

	public void ApplyXRayShader( bool apply ){
		var shader = apply ? xrayShader : defaultShader;
		foreach( var material in materials )
			material.shader = shader;

		if( !materialsInitialized && apply ){
			foreach( var material in materials ){
				material.SetVector( PlanePosition, Vector3.down*10 );
				material.SetVector( PlaneNormal, Vector3.down );
			}
			materialsInitialized = true;
		}
	}
}