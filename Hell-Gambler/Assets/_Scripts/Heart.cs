using System;
using UnityEngine;
using UnityEngine.UI;

public enum Status {
  alive,
  dead,
  empty
}

public class Heart : MonoBehaviour {
  [SerializeField] SpriteRenderer spriteRenderer;
  [SerializeField] Sprite alive;
  [SerializeField] Sprite dead;
  [SerializeField] Sprite empty;

  public Status status;

  private void Update() {
    switch (status) {
      case Status.alive:
        spriteRenderer.sprite = alive;
        break;
      case Status.dead:
        spriteRenderer.sprite = dead;
        break;
      case Status.empty:
        spriteRenderer.sprite = empty;
        break;
    }
  }
}