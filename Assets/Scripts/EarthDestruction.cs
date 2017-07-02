using UnityEngine;


public class EarthDestruction : MonoBehaviour {

    public GameObject brokenEarth;
    private int maxPieces = 15;
    private Health health;

    private void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //int pieces = 1;
        //while (pieces < maxPieces)
        //{
        //    float randRot = Random.Range(0f, 180f);
        //    GameObject brokenPiece = Instantiate(brokenEarth, new Vector3(transform.position.x, transform.position.y, transform.position.z), new Quaternion(randRot, randRot, randRot, randRot));  //new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
        //    Destroy(brokenPiece, Random.Range(1f, 6f)); //pieces go away after 5 seconds
        //    ++pieces;
        //}

        health.TakeDamage(25);

        Destroy(this.gameObject);
    }
}
