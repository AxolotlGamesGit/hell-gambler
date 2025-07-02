using System;
using UnityEngine;

public class StayUpright : MonoBehaviour {
  [SerializeField] Transform parent;
  [SerializeField] Vector3 targetPosition;

  private void Awake() {
    if (parent == null) {
      parent = transform.parent;
      if (parent == null) {
        Debug.LogError("Didn't find a parent");
      }
    }

    if (targetPosition == Vector3.zero) {
      targetPosition = transform.position - parent.position;
    }
  }

  void Update() {
    transform.position = targetPosition + parent.position;
    float rotation = (float)(parent.eulerAngles.z / -57.3);
    transform.rotation = Quaternion.Euler(0, 0, rotation);
  }
}
