using System;
using UnityEngine;
using UnityEngine.UI;

public enum Status {
  alive,
  dead,
  empty
}

public class Heart : MonoBehaviour {
  [SerializeField] Image image;
  [SerializeField] Sprite alive;
  [SerializeField] Sprite dead;
  [SerializeField] Sprite empty;

  public Status status;

  private void Update() {
    switch (status) {
      case Status.alive:
        image.sprite = alive;
        break;
      case Status.dead:
        image.sprite = dead;
        break;
      case Status.empty:
        image.sprite = empty;
        break;
    }
  }
}
