using UnityEngine;

/*
 * Enemy
 *
 * Handles enemy movement and collisions.  When the enemy collides with the player
 * or the player's laser, it triggers appropriate effects like playing an explosion
 * sound and awarding points via the GameManager.
 */
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed;    // Speed at which the enemy moves downward
    [SerializeField] private Animator _animator;                    // Animator for enemy destruction
    private Player _player;                        // Reference to the player for damaging them
    [SerializeField] private Collider2D _collider;                  // Collider used for enabling/disabling collisions
    private GameManager _gameManager;              // Reference to the GameManager for scoring and sound

    private void Start()
    {
        // Set a default movement speed
        _enemySpeed = 3f;

        // Find the Player in the scene by type.  This assumes there is only one Player active.
        _player = FindFirstObjectByType<Player>();

        // Get the Animator component for playing the destruction animation
        _animator = GetComponent<Animator>();


        // Get the Collider2D component for detecting collisions
        _collider = GetComponent<Collider2D>();


        // Grab a reference to the GameManager for scoring and sound effects
        _gameManager = FindFirstObjectByType<GameManager>();

    }

    private void Update()
    {
        // Continuously move the enemy downward every frame
        MoveEnemy();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the enemy collides with the player, damage the player
        if (other.CompareTag("Player"))
        {
            // Trigger the enemy's destruction animation
            if (_animator != null)
            {
                _animator.SetTrigger("OnDestroyEnemy");
            }
            // Slow down movement so the enemy stops moving during the explosion
            _enemySpeed = 0.2f;
            // Disable this enemy's collider to prevent further hits
            if (_collider != null)
            {
                _collider.enabled = false;
            }
            // Play the explosion sound via the GameManager
            if (_gameManager != null)
            {
                _gameManager.PlayExplosion();
            }
            // Tell the player they have been damaged
            if (_player != null)
            {
                _player.getDamaged();
            }
            // Destroy this enemy after a delay to let the animation and sound play
            Destroy(this.gameObject, 2.5f);
        }
        // If the enemy collides with a laser, destroy both and award points
        else if (other.CompareTag("Laser"))
        {
            // Destroy the laser GameObject
            Destroy(other.gameObject);
            // Increase the player's score using the GameManager
            if (_gameManager != null)
            {
                _gameManager.AddScore(10);
                _gameManager.PlayExplosion();
            }
            // Play the enemy's destruction animation
            if (_animator != null)
            {
                _animator.SetTrigger("OnDestroyEnemy");
                Destroy(this.gameObject, 2f);
            }
            else
            {
                Destroy(gameObject);
            }
            // Slow down the enemy's movement so it looks like it is destroyed
            _enemySpeed = 0.2f;
            // Disable the collider so it can't be hit again
            if (_collider != null)
            {
                _collider.enabled = false;
            }
            // Finally destroy the enemy after a short delay
            
        }
    }

    /// <summary>
    /// Handles moving the enemy downward and respawning it at the top when it goes off screen.
    /// </summary>
    private void MoveEnemy()
    {
        // Move downward at a constant speed
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        // If the enemy has moved past the bottom of the screen, wrap it back to the top
        if (transform.position.y <= -6.5f)
        {
            transform.position = new Vector3(Random.Range(-9.1f, 9.1f), 6.5f, 0);
        }
    }
}