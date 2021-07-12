using UnityEngine.EventSystems;

public class ResolutionChangedListener: UIBehaviour{
	protected override void OnRectTransformDimensionsChange(){
		foreach( var photoLocator in FindObjectsOfType< PhotoLocator >() )
			photoLocator.UpdateScale();
	}
}