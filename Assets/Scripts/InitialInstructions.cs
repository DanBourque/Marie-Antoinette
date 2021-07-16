using UnityEngine;

public class InitialInstructions: MonoBehaviour{
	public GameObject[] delayedActivations;

	private void Awake(){
		foreach( var delayedActivation in delayedActivations )
			delayedActivation.SetActive( false );
	}

	private void Update(){		// All we do is wait for an initial mouse click, and then we deactivate ourself.
		if( Input.GetMouseButtonDown( 0 ) ){
			foreach( var delayedActivation in delayedActivations )
				delayedActivation.SetActive( true );
			gameObject.SetActive( false );
		}
	}
}