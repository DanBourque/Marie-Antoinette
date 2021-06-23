using UnityEngine;

public class SkyboxSelector: MonoBehaviour{
	public Material skybox1Mat, skybox2Mat;

	public void OnSkyboxSelection1( bool isOn ){
		if( isOn )
			SetSkyboxMat( skybox1Mat );
	}

	public void OnSkyboxSelection2( bool isOn ){
		if( isOn )
			SetSkyboxMat( skybox2Mat );
	}

	private void SetSkyboxMat( Material mat ){
		RenderSettings.skybox = mat;
		DynamicGI.UpdateEnvironment();
	}
}