using UnityEngine;
using UnityEngine.UI;

public enum Room{ ChainLocker, EngineRoom, Galley, GuestCabin, Quarters, Sailroom, Salon }

public class RoomSelector: MonoBehaviour{
	private const float LerpDuration = 2, DeckExpandedY = 2f, HullExpandedY = -3.1f, ExpandedScale = 1.5f;
	public Transform hull, deck, spotlight;
	public Transform[] rooms;
	public RectTransform[] photoStrips;
	private RectTransform defaultPhotoStrip;
	public ScrollRect photosScrollRect;
	private GameObject photosSlideoutViewport;
	private Vector3[] roomAnchors;
	private Vector3[] roomPosStarts, roomPosTargets;
	private Vector3[] roomScaleStarts, roomScaleTargets;
	private Vector3 hullPosStart, hullPosTarget, deckPosStart, deckPosTarget;
	private XRayControls xrayControls;
	public RectTransform roomsSlideoutPanel, photosSlideoutPanel, xraySlideoutPanel;
	private bool isRoomsSlideoutVisible = true, isPhotosSlideoutVisible = true, isXRaySlideoutVisible = true;
	private float timeElapsed = LerpDuration;

	private void Awake(){
		roomAnchors = new Vector3[ rooms.Length ];
		roomPosStarts = new Vector3[ rooms.Length ];
		roomPosTargets = new Vector3[ rooms.Length ];
		roomScaleStarts = new Vector3[ rooms.Length ];
		roomScaleTargets = new Vector3[ rooms.Length ];
		for( var r=0; r<roomAnchors.Length; r++ ){		// Record the original anchors for animation purposes.
			roomAnchors[ r ] = roomPosTargets[ r ] = rooms[ r ].position;
			roomScaleTargets[ r ] = Vector3.one;
		}
		defaultPhotoStrip = photosScrollRect.content;
		photosSlideoutViewport = photosSlideoutPanel.Find( "Viewport" ).gameObject;
		xrayControls = FindObjectOfType< XRayControls >( true );
		OnRoomsSlideout();
		OnPhotosSlideout();
		OnXRaySlideout();
	}

	public void OnChainLocker( bool isOn ) => SelectRoom( Room.ChainLocker, isOn );
	public void OnEngineRoom( bool isOn ) => SelectRoom( Room.EngineRoom, isOn );
	public void OnGalley( bool isOn ) => SelectRoom( Room.Galley, isOn );
	public void OnGuestCabin( bool isOn ) => SelectRoom( Room.GuestCabin, isOn );
	public void OnQuarters( bool isOn ) => SelectRoom( Room.Quarters, isOn );
	public void OnSailroom( bool isOn ) => SelectRoom( Room.Sailroom, isOn );
	public void OnSalon( bool isOn ) => SelectRoom( Room.Salon, isOn );

	private void SelectRoom( Room room, bool isOn ){
		if( isOn ){
			for( var r=0; r<rooms.Length; r++ ){
				roomPosTargets[ r ] = roomAnchors[ r ] + new Vector3( 0, HullExpandedY, 0 );
				roomScaleTargets[ r ] = Vector3.one;
				SetExtended( rooms[ r ], false );
			}
			roomPosTargets[ ( int )room ] = spotlight.position;
			roomScaleTargets[ ( int )room ] = new Vector3( ExpandedScale, ExpandedScale, ExpandedScale );
			deckPosTarget = new Vector3( 0, DeckExpandedY, 0 );
			hullPosTarget = new Vector3( 0, HullExpandedY, 0 );
			SetExtended( rooms[ ( int )room ], true );
			if( isXRaySlideoutVisible )
				OnXRaySlideout();		// Close the X-Ray panel when we expand a room.
		}else{
			for( var r=0; r<rooms.Length; r++ ){
				roomPosTargets[ r ] = roomAnchors[ r ];
				roomScaleTargets[ r ] = Vector3.one;
				SetExtended( rooms[ r ], false );
			}
			deckPosTarget = hullPosTarget = Vector3.zero;
		}

		for( var r=0; r<rooms.Length; r++ ){
			roomPosStarts[ r ] = rooms[ r ].position;
			roomScaleStarts[ r ] = rooms[ r ].localScale;
		}
		deckPosStart = deck.localPosition;
		hullPosStart = hull.localPosition;
		timeElapsed = 0;

		for( var r=0; r<photoStrips.Length; r++ )
			photoStrips[ r ].gameObject.SetActive( isOn ? r==( int )room : false );
		defaultPhotoStrip.gameObject.SetActive( !isOn );
		photosScrollRect.content = isOn ? photoStrips[ ( int )room ] : defaultPhotoStrip;
	}

	private void SetExtended( Transform room, bool extended ){
		foreach( Transform child in room ){
			var roomExtender = child.GetComponent< RoomExtender >();
			if( roomExtender!=null )
				roomExtender.Extended = extended;
		}
	}

	public void OnRoomsSlideout(){
		isRoomsSlideoutVisible = !isRoomsSlideoutVisible;
		roomsSlideoutPanel.anchoredPosition = new Vector2( isRoomsSlideoutVisible ? 0 : -roomsSlideoutPanel.rect.width, 0 );
	}

	public void OnPhotosSlideout(){
		isPhotosSlideoutVisible = !isPhotosSlideoutVisible;
		photosSlideoutPanel.anchoredPosition = new Vector2( isPhotosSlideoutVisible ? 0 : photosSlideoutPanel.rect.width, 0 );
		photosSlideoutViewport.SetActive( isPhotosSlideoutVisible );
	}

	public void OnXRaySlideout(){
		isXRaySlideoutVisible = !isXRaySlideoutVisible;
		xraySlideoutPanel.anchoredPosition = new Vector2( xraySlideoutPanel.anchoredPosition.x, isXRaySlideoutVisible ? 0 : -xraySlideoutPanel.rect.height );
		xrayControls.ApplyXRayShader( isXRaySlideoutVisible );
	}

	private void Update(){
		if( timeElapsed<LerpDuration ){
			var time = timeElapsed/LerpDuration;
			time = time * time * ( 3f-2f*time );

			for( var r=0; r<rooms.Length; r++ ){
				rooms[ r ].position = Vector3.Lerp( roomPosStarts[ r ], roomPosTargets[ r ], time );
				rooms[ r ].localScale	= Vector3.Lerp( roomScaleStarts[ r ], roomScaleTargets[ r ], time );
			}
			deck.localPosition = Vector3.Lerp( deckPosStart, deckPosTarget, time );
			hull.localPosition = Vector3.Lerp( hullPosStart, hullPosTarget, time );

			timeElapsed += Time.deltaTime;
		}else{
			for( var r=0; r<rooms.Length; r++ ){
				rooms[ r ].position = roomPosTargets[ r ];
				rooms[ r ].localScale	= roomScaleTargets[ r ];
			}
			deck.localPosition = deckPosTarget;
			hull.localPosition = hullPosTarget;
		}
	}
}