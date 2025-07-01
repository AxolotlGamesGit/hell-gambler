using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {
  [SerializeField] Heart heart;
  [SerializeField] Transform parent;

  [SerializeField] int maxHealth = 10;
  public int CurrentHealth { get; private set; }
  public UnityEvent OnDeath;

  [SerializeField] Vector3 startingLocation = new Vector3(-40, -50, 0);
  [SerializeField] float xOffset = 1;

  private List<Heart> hearts;

  public void AddHealth(int health) {
    CurrentHealth += health;
    if (health < 0) {
      OnDeath.Invoke();
    }
    DisplayHealth();
  }

  void DisplayHealth() {
    for (int i = 0; i < hearts.Count; i++) {
      if (i + 1 > maxHealth) {
        hearts[i].status = Status.empty;
      }
      else if (i + 1 > CurrentHealth) {
        hearts[i].status = Status.dead;
      }
      else {
        hearts[i].status = Status.alive;
      }
    }
  }

  void Awake() {
    if (OnDeath == null) {
      OnDeath = new UnityEvent();
    }

    hearts = new List<Heart>();
    Vector3 currentLocation = startingLocation;
    for (int i = 0; i < maxHealth; i++) {
      hearts.Add(GameObject.Instantiate<Heart>(heart, currentLocation, new Quaternion(), parent));
      currentLocation.x += xOffset;
    }

    CurrentHealth = maxHealth;
  }

  void Start() {
    DisplayHealth();
  }
}