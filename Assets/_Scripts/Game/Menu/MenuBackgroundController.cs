using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundController : MonoBehaviour
{
    public GameObject[] polygons;
    public Transform[] positions;
    public Transform target;
    public Vector2 forceRange;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            for(int i = 0; i < positions.Length; i++)
            {
                int j = Mathf.FloorToInt(Random.Range(0, polygons.Length-0.0001f));
                Rigidbody2D rb = Instantiate(polygons[j], positions[i]).GetComponent<Rigidbody2D>();
                Vector2 dir = (target.position - rb.transform.position).normalized;
                yield return new WaitForSeconds(0.8f);
                rb.AddForce(dir * Random.Range(forceRange.x, forceRange.y));
                yield return new WaitForSeconds(0.6f);
            }
            yield return new WaitForSeconds(15);
        }
    }
}
