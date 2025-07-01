using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour, IMovementInput {
  [SerializeField] Rigidbody2D rigidBody;
  [SerializeField] GameObject player;
  [SerializeField] Health playerHealth;
  [SerializeField] ParticleSystem particles;

  [SerializeField] float topSpeed = 3.0f;
  [SerializeField] float friction = 0.9f;

  [SerializeField] int damage = 1;
  [SerializeField] float attackRange = 1f;
  [SerializeField] float attackCooldown = 2f;
  [SerializeField] float cooldownTimer = 0f;

  float Angle = 0f;
  Vector3 playerPosition;
  Vector3 position;

  Vector2 IMovementInput.GetMoveDirection() {
    playerPosition = player.transform.position;
    position = transform.position;
    Vector2 moveDirection = new Vector2(playerPosition.x - position.x, playerPosition.y - position.y);

    if (Vector3.Distance(transform.position, player.transform.position) > attackRange) {
      return moveDirection.normalized;
    }
    else {
      return new Vector2(0, 0);
    }
  }

  float IMovementInput.GetRotation() {
    Vector2 pos = playerPosition - position;
    Angle = 57.3f * (float)Math.Acos(pos.y / Math.Sqrt(Math.Pow(pos.x, 2) + Math.Pow(pos.y, 2)));
    if (pos.x > 0) {
      Angle = -1 * Angle;
    }
    else {
      Angle = -360 + Angle;
    }
    Angle += -270;
    return Angle;
  }

  private void Attack() {
    cooldownTimer = cooldownTimer % attackCooldown;
    playerHealth.AddHealth(damage * -1);
    particles.Play();
  }

  private void Update() {
    playerPosition = player.transform.position;
    position = transform.position;

    cooldownTimer += Time.deltaTime;
    if (cooldownTimer > attackCooldown    &&    Vector3.Distance(transform.position, player.transform.position) < attackRange) {
      Attack();
    }
  }
}
