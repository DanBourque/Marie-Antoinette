using UnityEngine;
using UnityEngine.EventSystems;

public class ResolutionChangeListener: UIBehaviour{
	protected override void OnRectTransformDimensionsChange(){
		Debug.Log( name+"'s new size: "+( transform as RectTransform ).rect.size );
		foreach( var photoLocator in FindObjectsOfType< PhotoLocator >() )
			photoLocator.UpdateScale();
	}
}