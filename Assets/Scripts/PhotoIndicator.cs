using UnityEngine;
using UnityEngine.UI;

public class PhotoIndicator: MonoBehaviour{
	private Toggle toggle;

	private void Awake() => toggle = transform.parent.parent.GetComponent< Toggle >();

	public void OnMouseUpAsButton() => toggle.isOn = !toggle.isOn;
}