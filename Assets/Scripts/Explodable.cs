using UnityEngine;

public class Explodable: MonoBehaviour{
  private const float k_Duration = 2f;
  [SerializeField] private Transform m_RestingPlace;
  [SerializeField] private float ratio;
  private float m_ExplodeTime;

  public void Explode() =>
    m_ExplodeTime = Time.time;

  public void Place() =>
    transform.SetPositionAndRotation(m_RestingPlace.localPosition, m_RestingPlace.localRotation);

  private void Update(){
    var time = Time.time;
    if (time - m_ExplodeTime > k_Duration)
      return;
    
    ratio = (time - m_ExplodeTime) / k_Duration;
    var t = transform;
    t.localPosition = Vector3.Lerp(t.localPosition, m_RestingPlace.localPosition, ratio);
    t.localRotation = Quaternion.Lerp(t.localRotation, m_RestingPlace.localRotation, ratio);
  }
}