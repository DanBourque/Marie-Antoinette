using UnityEngine;

public enum Room{ ChainLocker, EngineRoom, Galley, GuestCabin, Quarters, Sailroom, Salon }

public class RoomSelector: MonoBehaviour{
	private const float SmoothTime = 3, deckExpandedY = 1.73f, hullExpandedY = -2.91f, expandedScale = 1.5f;
	public Transform hull, deck, spotlight;
	public GameObject[] rooms;
	private Vector3[] roomAnchors;
	private Vector3[] roomPosTargets;
	private Vector3[] roomScaleTargets;
	private Vector3 hullPosTarget, deckPosTarget;
	public RectTransform slideoutToggle, slideoutPanel;
	private bool isSlideoutVisible = true;

	private void Awake(){
		roomAnchors = new Vector3[ rooms.Length ];
		roomPosTargets = new Vector3[ rooms.Length ];
		roomScaleTargets = new Vector3[ rooms.Length ];
		for( var r=0; r<roomAnchors.Length; r++ ){		// Record the original anchors for animation purposes.
			roomAnchors[ r ] = roomPosTargets[ r ] = rooms[ r ].transform.localPosition;
			roomScaleTargets[ r ] = Vector3.one;
		}
		OnSectionsSlideout();
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
			}
			roomPosTargets[ ( int )room ] = spotlight.position;
			roomScaleTargets[ ( int )room ] = new Vector3( expandedScale, expandedScale, expandedScale );
			deckPosTarget = new Vector3( 0, deckExpandedY, 0 );
			hullPosTarget = new Vector3( 0, hullExpandedY, 0 );
		}else{
			for( var r=0; r<rooms.Length; r++ ){
				roomPosTargets[ r ] = roomAnchors[ r ];
				roomScaleTargets[ r ] = Vector3.one;
			}
			deckPosTarget = hullPosTarget = Vector3.zero;
		}
	}

	public void OnSectionsSlideout(){
		isSlideoutVisible = !isSlideoutVisible;
		slideoutPanel.anchoredPosition = new Vector2( isSlideoutVisible ? 0 : slideoutPanel.rect.width, 0 );
		slideoutToggle.anchoredPosition = new Vector2( isSlideoutVisible ? -slideoutPanel.rect.width : 0, 0 );
	}

	private void Update(){
		var time = SmoothTime*Time.deltaTime;

		for( var r=0; r<rooms.Length; r++ ){
			rooms[ r ].transform.localPosition = Vector3.Lerp( rooms[ r ].transform.localPosition, roomPosTargets[ r ], time );
			rooms[ r ].transform.localScale	= Vector3.Lerp( rooms[ r ].transform.localScale, roomScaleTargets[ r ], time );
		}

		deck.transform.localPosition = Vector3.Lerp( deck.transform.localPosition, deckPosTarget, time );
		hull.transform.localPosition = Vector3.Lerp( hull.transform.localPosition, hullPosTarget, time );
	}
}