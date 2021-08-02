using System;
using System.Collections.Generic;
using UnityEngine;

public enum Slideout{ Rooms, Photos, XRay }

public class Slideouts: MonoBehaviour{
	private const float LerpDuration = .5f;
	private GameObject photosSlideoutViewport;
	private XRayControls xrayControls;
	private Dictionary< Slideout, RectTransform > panels = new Dictionary< Slideout, RectTransform >();
	private Dictionary< Slideout, bool > isPanelVisible = new Dictionary< Slideout, bool >{ { Slideout.Rooms, true }, { Slideout.Photos, true }, { Slideout.XRay, true } };
	private Dictionary< Slideout, ( Vector2, Vector2 ) > panelPositions = new Dictionary< Slideout, ( Vector2, Vector2 ) >();
	private Dictionary< Slideout, LerpData > panelLerps = new Dictionary< Slideout, LerpData >();

	private struct LerpData{
		public float timeElapsed;
		public Vector2 startPos, endPos;
		public Action postLerpAction;
		public bool completed;
	}

	private void Awake(){
		panels[ Slideout.Rooms ] = transform.Find( "Room Selector" ).GetComponent< RectTransform >();
		panels[ Slideout.Photos ] = transform.Find( "Photos" ).GetComponent< RectTransform >();
		panels[ Slideout.XRay ] = transform.Find( "X-Ray Controls" ).GetComponent< RectTransform >();

		panelPositions[ Slideout.Rooms ] = ( new Vector2( 0, 0 ), new Vector2( -panels[ Slideout.Rooms ].rect.width, 0 ) );
		panelPositions[ Slideout.Photos ] = ( new Vector2( 0, 0 ), new Vector2( panels[ Slideout.Photos ].rect.width, 0 ) );
		var xPos = panels[ Slideout.XRay ].anchoredPosition.x;
		panelPositions[ Slideout.XRay ] = ( new Vector2( xPos, 0 ), new Vector2( xPos, -panels[ Slideout.XRay ].rect.height ) );

		foreach( Slideout slideout in Enum.GetValues( typeof( Slideout ) ) )
			panelLerps[ slideout ] = new LerpData{ timeElapsed = LerpDuration };

		photosSlideoutViewport = panels[ Slideout.Photos ].Find( "Viewport" ).gameObject;
		xrayControls = FindObjectOfType< XRayControls >( true );
		OnRoomsSlideout();
		OnPhotosSlideout();
		OnXRaySlideout();
	}

	public void OnRoomsSlideout() => OnSlideout( Slideout.Rooms, null );

	public void OnPhotosSlideout() => OnSlideout( Slideout.Photos, () => photosSlideoutViewport.SetActive( isPanelVisible[ Slideout.Photos ] ) );

	public void OnXRaySlideout() => OnSlideout( Slideout.XRay, () => xrayControls.ApplyXRayShader( isPanelVisible[ Slideout.XRay ] ) );

	private void OnSlideout( Slideout slideout, Action postLerpAction ){
		var isVisible = isPanelVisible[ slideout ] = !isPanelVisible[ slideout ];
		var lerpData = panelLerps[ slideout ];
		lerpData.startPos = panels[ slideout ].anchoredPosition;
		lerpData.endPos = isVisible ? panelPositions[ slideout ].Item1 : panelPositions[ slideout ].Item2;
		lerpData.postLerpAction = postLerpAction;
		lerpData.timeElapsed = 0;
		lerpData.completed = false;
		panelLerps[ slideout ] = lerpData;
	}

	public void CloseXRaySlideoutIfVisible(){
		if( isPanelVisible[ Slideout.XRay ] )
			OnXRaySlideout();
	}

	private void Update(){
		foreach( Slideout slideout in Enum.GetValues( typeof( Slideout ) ) ){
			var lerpData = panelLerps[ slideout ];
			if( lerpData.timeElapsed<LerpDuration ){
				var time = lerpData.timeElapsed/LerpDuration;
				time = time * time * ( 3f-2f*time );
				panels[ slideout ].anchoredPosition = Vector2.Lerp( lerpData.startPos, lerpData.endPos, time );
				lerpData.timeElapsed += Time.deltaTime;
				panelLerps[ slideout ] = lerpData;
			}else if( !lerpData.completed ){
				panels[ slideout ].anchoredPosition = lerpData.endPos;
				if( lerpData.postLerpAction!=null )
					lerpData.postLerpAction.Invoke();
				lerpData.completed = true;
				panelLerps[ slideout ] = lerpData;
			}
		}
	}
}