using UnityEngine;

public class Laser : MonoBehaviour
{
     void Start()
    {

    }
    void Update()
    {
        LaserMevement();
    }
    private void LaserMevement()
    {
            transform.Translate(Vector3.up * 5 * Time.deltaTime);
            if (transform.position.y >= 7)
            {
                if (transform.parent != null)
                    Destroy(transform.parent.gameObject);
            Destroy(gameObject);
            }
    }
}