using UnityEngine;

[ExecuteAlways]
public class GenerateFilenameCoordinates: MonoBehaviour{
	[TextArea( 1, 1 )][SerializeField]
	private string generatedFilename;

	private void Update(){
		Vector3 pos = transform.position, rot = transform.eulerAngles;
		if( rot.x>180 )
			rot.x -= 360;
		if( rot.y>180 )
			rot.y -= 360;
		if( rot.z>180 )
			rot.z -= 360;
		generatedFilename =  $"{transform.parent.name} @ {pos.x:0.000}, {pos.y:0.000}, {pos.z:0.000}, {rot.x:0.000}, {rot.y:0.000}, {rot.z:0.000}.jpg";
	}
}