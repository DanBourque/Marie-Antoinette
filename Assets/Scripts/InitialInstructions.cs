using UnityEngine;

public class InitialInstructions: MonoBehaviour{
	private void Update(){		// All we do is wait for an initial mouse click, and then we deactivate ourself.
		if( Input.GetMouseButtonDown( 0 ) )
			gameObject.SetActive( false );
	}
}