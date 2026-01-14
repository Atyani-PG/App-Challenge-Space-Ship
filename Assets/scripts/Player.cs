using System.Collections;
using UnityEngine;
/*
 * Player
 * This script controls the player's movement, shooting, and reactions to collisions.
 */
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab; // Prefab for the player's laser
    [SerializeField] private GameManager _gameManager;     // Reference to the GameManager

    private Animator _animator;           // Animator controlling the player animations
    private Collider2D _collider2D;       // Collider used to detect collisions

    private float _speed = 5.2f;         // Movement speed of the player
    private float _fireAfter = 0f;        // Time until the next laser can be fired

    void Start()
    {
        // Place the player at the starting position near the bottom of the screen
        transform.position = new Vector3(0, -3, 0);

        if (_gameManager == null)
        {
            Debug.LogError("Player: GameManager not found in the scene.");
        }

        // Get the Animator component for controlling animations
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Player: Animator component is missing.");
        }

        // Get the Collider2D component for collision detection
        _collider2D = GetComponent<Collider2D>();
        if (_collider2D == null)
        {
            Debug.LogError("Player: Collider2D component is missing.");
        }
    }
    void Update()
    {
        playerMovements();
        shooting();
    }
    void playerMovements()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontal_input, vertical_input, 0) * _speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.65f, 4.68f), 0);
        if (transform.position.x >= 11.3f)
            transform.position = new Vector3(-9.2f, transform.position.y, 0);

        else if (transform.position.x <= -11.3f)
            transform.position = new Vector3(9.2f, transform.position.y, 0);

        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetBool("turnRight", false);
            _animator.SetBool("turnLeft", true);
            Debug.Log("A key pressed");
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            _animator.SetBool("turnLeft", false);
            _animator.SetBool("turnRight", true);
            Debug.Log("D key pressed");
        }
        else if (!Input.anyKey)
        {
            _animator.SetBool("turnLeft", false);
            _animator.SetBool("turnRight", false);
        }


    }
    void shooting()
    {
        // Check if the space key has been pressed and the cooldown has expired
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fireAfter)
        {
            // Create a new laser slightly above the player
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            // Set the next time the player can fire
            _fireAfter = Time.time + 0.25f;
            // Ask the GameManager to play the laser sound
            if (_gameManager != null)
            {
                _gameManager.PlayLaser();
            }
        }
    }

    /// <summary>
    /// Increases the player's score by the specified amount.  Delegates the work to the GameManager.
    /// </summary>
    /// <param name="x">The number of points to add (default is 10).</param>
    public void scoreIncreasing(int x = 10)
    {
        if (_gameManager != null)
        {
            _gameManager.AddScore(x);
        }
    }

    /// <summary>
    /// Called when the player takes fatal damage.  Triggers the explosion animation,
    /// tells the GameManager the player has died, and destroys the player object
    /// after a delay.
    /// </summary>
    public void getDamaged()
    {
        // Play explosion sound through the GameManager
        if (_gameManager != null)
        {
            _gameManager.PlayExplosion();
        }
        // Play the explosion animation on the player
        if (_animator != null)
        {
            _animator.SetBool("explosion", true);
        }
        // Turn off the player engine's fire (child index 1) if it exists
        if (transform.childCount > 1)
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
        // Notify the GameManager that the player has died
        if (_gameManager != null)
        {
            _gameManager.PlayerDied();
        }
        // Disable the collider so no further collisions are detected
        if (_collider2D != null)
        {
            _collider2D.enabled = false;
        }
        // Destroy the player after a short delay to allow animations to play
        Destroy(this.gameObject, 1.5f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            getDamaged();
        }
        if (other.tag == "moveEnemy")
        {
            getDamaged();
        }
    }
}