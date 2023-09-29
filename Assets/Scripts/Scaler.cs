using UnityEngine;

public class Scaler: MonoBehaviour{
  public void Scale(float scale) =>
    transform.localScale = new Vector3(scale, scale, scale);
}