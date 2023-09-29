using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Exploder: MonoBehaviour{
  [SerializeField] private List<Explodable> m_Pieces;

  private void Start(){
    foreach (var piece in m_Pieces){
      piece.Place();  // Set it's position to it's exploded resting place before turning on the XRGrabInteractable.
      piece.GetComponent<XRGrabInteractable>().enabled = true;
    }
  }

  public void Explode(){
    foreach (var piece in m_Pieces)
      piece.Explode();
  }
}