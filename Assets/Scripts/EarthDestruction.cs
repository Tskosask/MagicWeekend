using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDestruction : MonoBehaviour {

    public GameObject brokenEarth;
    private int maxPieces = 15;

    private void OnCollisionEnter(Collision collision)
    {
        int pieces = 1;
        while (pieces < maxPieces)
        {
            GameObject brokenPiece = Instantiate(brokenEarth, new Vector3(transform.position.x, transform.position.y, transform.position.z), new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
            Destroy(brokenPiece, Random.Range(1f, 6f); //pieces go away after 5 seconds
            ++pieces;
        }

        Destroy(this.gameObject);
    }
}
