using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyController : MonoBehaviour {
  [SerializeField] Rigidbody2D rigidBody;
  [SerializeField] GameObject player;
  [SerializeField] HeartManager heartManager;
  [SerializeField] ParticleSystem particles;

  [SerializeField] float topSpeed = 3.0f;
  [SerializeField] float friction = 0.9f;
  [SerializeField] int damage = 1;
  [SerializeField] float attackRange = 1f;
  [SerializeField] float attackCooldown = 2f;
  [SerializeField] float cooldownTimer = 0f;

  [SerializeField] float Angle = 0f;

  private void Move() {
    Vector3 playerPosition = player.transform.position;
    Vector3 position = transform.position;
    Vector2 moveDirection = new Vector2(playerPosition.x - position.x, playerPosition.y - position.y).normalized;
    float acceleration = (1 / friction - 1) * topSpeed; // Magic formula found at https://www.desmos.com/calculator/qkzwobcwyk

    // Keeps the enemy from going too close to the player.
    if (Vector3.Distance(transform.position, player.transform.position) > attackRange) {
      rigidBody.linearVelocity += moveDirection * acceleration;
    }
    rigidBody.linearVelocity *= friction;

    //Angle = Vector3.Angle(position, playerPosition);
    //Angle = Angle * Math.Sign(playerPosition.y - position.y);\
    Vector2 pos = playerPosition - position;
    Angle = 57.3f * (float)Math.Acos(pos.y / Math.Sqrt(Math.Pow(pos.x, 2) + Math.Pow(pos.y, 2)));
    if (pos.x > 0) {
      Angle = -1 * Angle;
    }
    else {
      Angle = -360 + Angle;
    }
    Angle += -270;
    transform.Rotate(new Vector3(0, 0, Angle - transform.eulerAngles.z));
    Debug.Log(Angle);
  }

  private void Attack() {
    cooldownTimer = cooldownTimer % attackCooldown;
    heartManager.AddHealth(damage * -1);
    particles.Play();
  }

  private void Update() {
    Move();

    cooldownTimer += Time.deltaTime;
    if (cooldownTimer > attackCooldown    &&    Vector3.Distance(transform.position, player.transform.position) < attackRange) {
      Attack();
    }
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.GetComponent<PlayerController>() != null) {
      Attack();
    }
  }
}
