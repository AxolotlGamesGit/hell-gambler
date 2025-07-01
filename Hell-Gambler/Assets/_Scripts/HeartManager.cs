using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class HeartManager : MonoBehaviour {
  [SerializeField] Heart heart;

  [SerializeField] int maxHealth = 10;
  public int Health = 10;
  public UnityEvent GameOver;

  [SerializeField] Vector3 startingLocation = new Vector3(-40, -50, 0);
  [SerializeField] float xOffset = 1;

  private List<Heart> hearts;

  public void AddHealth(int health) {
    Health += health;
    if (health < 0) {
      GameOver.Invoke();
    }
    DisplayHealth();
  }

  void DisplayHealth() {
    for (int i = 0; i < hearts.Count; i++) {
      if (i + 1 > maxHealth) {
        hearts[i].status = Status.empty;
      }
      else if (i + 1 > Health) {
        hearts[i].status = Status.dead;
      }
      else {
        hearts[i].status = Status.alive;
      }
    }
  }

  void Awake() {
    if (GameOver == null) {
      GameOver = new UnityEvent();
    }

    hearts = new List<Heart>();
    Vector3 currentLocation = startingLocation;
    for (int i = 0; i < maxHealth; i++) {
      hearts.Add(GameObject.Instantiate<Heart>(heart, currentLocation, new Quaternion(), transform));
      currentLocation.x += xOffset;
    }
  }

  void Start() {
    DisplayHealth();
  }
}