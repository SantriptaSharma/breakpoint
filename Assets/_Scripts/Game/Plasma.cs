using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plasma : MonoBehaviour
{
    public float timeScale;

    private SpriteRenderer rendererer;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        rendererer = GetComponent<SpriteRenderer>();
        startTime = rendererer.material.GetFloat("time");
    }

    // Update is called once per frame
    void Update()
    {
        float currentValue = startTime + Time.time * timeScale;
        rendererer.material.SetFloat("time", currentValue);
    }
}
