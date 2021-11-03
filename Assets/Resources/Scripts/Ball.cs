using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    private GameObject whiteBall;
    private GameObject ballsManager;

    private void GenerateWhiteBallRandomPosition()
    {
        whiteBall = GameObject.Find("whiteBall");
        ballsManager = GameObject.Find("BallsManager");

        float randomX = UnityEngine.Random.Range(-0.21f, 0.21f);
        float randomY = UnityEngine.Random.Range(-0.21f, 0.21f);
        float randomZ = UnityEngine.Random.Range(-0.21f, 0.21f);

        // ensure the white ball will not be generated inside/close to the pyramid colorful balls

        // Center point (0,0)

        // Wall length 0.5 half is 0.25
        // ball diameter 0.05 radius 0.025
        // Range < 0.25 - 0.025 = 0.225 we use 0.21

        // suppose the white ball will be far away at least 3 ball radius length from the center
        // in fact it is totally enough
        // Range > 0.05 * 3 = 0.15

        // [-0.21, -0.15] + [0.15, 0.21]

        // Remove the point inside [-0.15, 0.15]
        while (Math.Abs(randomX) > 0.15f && Math.Abs(randomY) > 0.15f && Math.Abs(randomZ) > 0.15f)
        {
            randomX = UnityEngine.Random.Range(-0.21f, 0.21f);
            randomY = UnityEngine.Random.Range(-0.21f, 0.21f);
            randomZ = UnityEngine.Random.Range(-0.21f, 0.21f);
        }
        whiteBall.transform.position = ballsManager.transform.position + new Vector3(randomX, randomY + 0.1f, randomZ);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        GenerateWhiteBallRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
