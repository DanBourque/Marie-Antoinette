using UnityEngine;

public enum Room{ ChainLocker, EngineRoom, Galley, GuestCabin, Quarters, Sailroom, Salon }

public class RoomSelector: MonoBehaviour{
	private const float SmoothTime = .01f, deckExpandedY = 1.73f, hullExpandedY = -2.91f, expandedScale = 1.5f;
	public Transform ship, hull, deck, spotlight;
	public GameObject[] rooms;
	private Vector3[] roomAnchors;
	private Vector3[] roomPosTargets;
	private Vector3[] roomScaleTargets;
	private Vector3 hullPosTarget, deckPosTarget;

	private void Awake(){
		roomAnchors = new Vector3[ rooms.Length ];
		roomPosTargets = new Vector3[ rooms.Length ];
		roomScaleTargets = new Vector3[ rooms.Length ];
		for( var r=0; r<roomAnchors.Length; r++ ){		// Record the original anchors for animation purposes.
			roomAnchors[ r ] = rooms[ r ].transform.Find( "in" ).localPosition;
			roomPosTargets[ r ] = Vector3.zero;
			roomScaleTargets[ r ] = Vector3.one;
		}
	}

	public void OnChainLocker( bool isOn ) => SelectRoom( Room.ChainLocker, isOn );
	public void OnEngineRoom( bool isOn ) => SelectRoom( Room.EngineRoom, isOn );
	public void OnGalley( bool isOn ) => SelectRoom( Room.Galley, isOn );
	public void OnGuestCabin( bool isOn ) => SelectRoom( Room.GuestCabin, isOn );
	public void OnQuarters( bool isOn ) => SelectRoom( Room.Quarters, isOn );
	public void OnSailroom( bool isOn ) => SelectRoom( Room.Sailroom, isOn );
	public void OnSalon( bool isOn ) => SelectRoom( Room.Salon, isOn );

	private void SelectRoom( Room room, bool isOn ){
//		rooms[ ( int )room ].SetActive( isOn );
		if( isOn ){
			for( var r=0; r<rooms.Length; r++ ){
				roomPosTargets[ r ] = new Vector3( 0, hullExpandedY, 0 );
				roomScaleTargets[ r ] = Vector3.one;
			}
			roomPosTargets[ ( int )room ] = spotlight.position-roomAnchors[ ( int )room ];
			roomScaleTargets[ ( int )room ] = new Vector3( expandedScale, expandedScale, expandedScale );
			deckPosTarget = new Vector3( 0, deckExpandedY, 0 );
			hullPosTarget = new Vector3( 0, hullExpandedY, 0 );
		}else{
			for( var r=0; r<rooms.Length; r++ ){
				roomPosTargets[ r ] = Vector3.zero;
				roomScaleTargets[ r ] = Vector3.one;
			}
			deckPosTarget = hullPosTarget = Vector3.zero;
		}
	}

	private void Update(){
		for( var r=0; r<rooms.Length; r++ ){
			rooms[ r ].transform.localPosition = Vector3.Lerp( rooms[ r ].transform.localPosition, roomPosTargets[ r ], SmoothTime );
			rooms[ r ].transform.localScale	= Vector3.Lerp( rooms[ r ].transform.localScale, roomScaleTargets[ r ], SmoothTime );
		}

		deck.transform.localPosition = Vector3.Lerp( deck.transform.localPosition, deckPosTarget, SmoothTime );
		hull.transform.localPosition = Vector3.Lerp( hull.transform.localPosition, hullPosTarget, SmoothTime );
	}
}