using UnityEngine;
using UnityEngine.UI;

public enum Room{ ChainLocker, EngineRoom, Galley, GuestCabin, Quarters, Sailroom, Salon }

public class RoomSelector: MonoBehaviour{
	private const float SmoothTime = 3, deckExpandedY = 2f, hullExpandedY = -3.1f, expandedScale = 1.5f;
	public Transform hull, deck, spotlight;
	public GameObject[] rooms;
	public RectTransform[] photoStrips;
	private RectTransform defaultPhotoStrip;
	public ScrollRect photosScrollRect;
	private GameObject photosSlideoutViewport;
	private Vector3[] roomAnchors;
	private Vector3[] roomPosTargets;
	private Vector3[] roomScaleTargets;
	private Vector3 hullPosTarget, deckPosTarget;
	public RectTransform roomsSlideoutPanel, photosSlideoutPanel;
	private bool isRoomsSlideoutVisible = true, isPhotosSlideoutVisible = true;

	private void Awake(){
		roomAnchors = new Vector3[ rooms.Length ];
		roomPosTargets = new Vector3[ rooms.Length ];
		roomScaleTargets = new Vector3[ rooms.Length ];
		for( var r=0; r<roomAnchors.Length; r++ ){		// Record the original anchors for animation purposes.
			roomAnchors[ r ] = roomPosTargets[ r ] = rooms[ r ].transform.position;
			roomScaleTargets[ r ] = Vector3.one;
		}
		defaultPhotoStrip = photosScrollRect.content;
		photosSlideoutViewport = photosSlideoutPanel.Find( "Viewport" ).gameObject;
		OnRoomsSlideout();
		OnPhotosSlideout();
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
				roomPosTargets[ r ] = roomAnchors[ r ] + new Vector3( 0, hullExpandedY, 0 );
				roomScaleTargets[ r ] = Vector3.one;
				SetExtended( rooms[ r ].transform, false );
			}
			roomPosTargets[ ( int )room ] = spotlight.position;
			roomScaleTargets[ ( int )room ] = new Vector3( expandedScale, expandedScale, expandedScale );
			deckPosTarget = new Vector3( 0, deckExpandedY, 0 );
			hullPosTarget = new Vector3( 0, hullExpandedY, 0 );
			SetExtended( rooms[ ( int )room ].transform, true );
		}else{
			for( var r=0; r<rooms.Length; r++ ){
				roomPosTargets[ r ] = roomAnchors[ r ];
				roomScaleTargets[ r ] = Vector3.one;
				SetExtended( rooms[ r ].transform, false );
			}
			deckPosTarget = hullPosTarget = Vector3.zero;
		}

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

	private void Update(){
		var time = SmoothTime*Time.deltaTime;

		for( var r=0; r<rooms.Length; r++ ){
			rooms[ r ].transform.position = Vector3.Lerp( rooms[ r ].transform.position, roomPosTargets[ r ], time );
			rooms[ r ].transform.localScale	= Vector3.Lerp( rooms[ r ].transform.localScale, roomScaleTargets[ r ], time );
		}

		deck.transform.localPosition = Vector3.Lerp( deck.transform.localPosition, deckPosTarget, time );
		hull.transform.localPosition = Vector3.Lerp( hull.transform.localPosition, hullPosTarget, time );
	}
}