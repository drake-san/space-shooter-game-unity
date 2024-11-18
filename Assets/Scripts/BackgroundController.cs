using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{

    private Vector2 position;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * 0.5f * Time.deltaTime);

        if (transform.position.y < -26.0f)
            transform.position = position;
    }
}
