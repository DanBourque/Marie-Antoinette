using UnityEngine;
using UnityEngine.UI;

public class RotationControls: MonoBehaviour{
	public Slider rotationSlider;
	public Toggle autoRotateToggle;
	public float autoRotationSpeed = .001f;

	private void Awake(){
		rotationSlider.onValueChanged.AddListener( value => {
			if( autoRotateToggle.isOn )
				autoRotateToggle.isOn = false;		// Stop auto-rotating.
		} );
	}

	private void Update(){
		if( autoRotateToggle.isOn ){
			rotationSlider.SetValueWithoutNotify( rotationSlider.value+autoRotationSpeed );
			if( rotationSlider.value >= rotationSlider.maxValue )
				rotationSlider.SetValueWithoutNotify( 0 );
		}

		transform.localRotation = Quaternion.Euler( 0, rotationSlider.value*360, 0 );
	}
}