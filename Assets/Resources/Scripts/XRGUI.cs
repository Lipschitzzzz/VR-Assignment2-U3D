using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class XRGUI : MonoBehaviour
{
    [SerializeField]
    private Button restartGame;
    
    [SerializeField]
    private TextMeshProUGUI timeBoard; // time board

    [SerializeField]
    private TextMeshProUGUI scoreBoard; // score board

    private GameObject whiteBall, stick; // the white ball
    private GameObject[] colorfulBallSet; // the colorful ball object set

    private Vector3 whiteBallPosition; // the white ball position
    private Quaternion whiteBallRotation; // the white ball rotation

    private List<Vector3> colorfulBallPositionList; // the colorful ball position list
    private List<Quaternion> colorfulBallRotationList; // the colorful ball rotation list

    private Vector3 stickPosition; // the stick position
    private Quaternion stickRotation; // the stick roration

    private Vector3 cueStickTipPosition; // the cuesticktip position
    private Quaternion cueStickTipRotation; // the cuesticktip rotation

    // Start is called before the first frame update
    void Start()
    {
        // initialize
        whiteBall = GameObject.Find("whiteBall");
        stick = GameObject.Find("cueStick");
        colorfulBallPositionList = new List<Vector3>();
        colorfulBallRotationList = new List<Quaternion>();
        colorfulBallSet = GameObject.FindGameObjectsWithTag("Balls");


        // record initial white ball position and rotation
        whiteBallPosition = whiteBall.transform.position;
        whiteBallRotation = whiteBall.transform.rotation;
        stickPosition = stick.transform.position;
        stickRotation = stick.transform.rotation;
        cueStickTipPosition = stick.transform.Find("cueStickTip").transform.position;
        cueStickTipRotation = stick.transform.Find("cueStickTip").transform.rotation;

        // record initial colorful ball position and rotation
        for (int i = 0; i < colorfulBallSet.Length; i ++)
        {
            colorfulBallPositionList.Add(colorfulBallSet[i].transform.position);
            colorfulBallRotationList.Add(colorfulBallSet[i].transform.rotation);
        }

        // restart button event
        restartGame.onClick.AddListener(() =>
        {
            // reset the score board and time board
            Hole.score = 0;
            //TimeManager.gameStartMoment = UnityEngine.Time.time;
            TimeManager.timeSpend = 0.0f;

            // recover the white ball, stick position rotation and remove the speed.
            whiteBall.transform.position = whiteBallPosition;
            whiteBall.transform.rotation = whiteBallRotation;
            stick.transform.position = stickPosition;
            stick.transform.rotation = stickRotation;
            stick.transform.Find("cueStickTip").transform.position = cueStickTipPosition;
            stick.transform.Find("cueStickTip").transform.rotation = cueStickTipRotation;
            Collider whiteBallColliderComponent = whiteBall.transform.GetComponent<Collider>();
            if (whiteBallColliderComponent.attachedRigidbody)
            {
                whiteBallColliderComponent.attachedRigidbody.useGravity = false;
                whiteBallColliderComponent.attachedRigidbody.velocity = Vector3.zero;
                whiteBallColliderComponent.attachedRigidbody.angularVelocity = Vector3.zero;
            }

            Rigidbody stickRigidbodyComponent = stick.transform.GetComponent<Rigidbody>();
            if (stickRigidbodyComponent)
            {
                stickRigidbodyComponent.useGravity = false;
                stickRigidbodyComponent.velocity = Vector3.zero;
                stickRigidbodyComponent.angularVelocity = Vector3.zero;
            }

            Rigidbody cueStickTipRigidbodyComponent = stick.transform.Find("cueStickTip").transform.GetComponent<Rigidbody>();
            if (cueStickTipRigidbodyComponent)
            {
                cueStickTipRigidbodyComponent.useGravity = false;
                cueStickTipRigidbodyComponent.velocity = Vector3.zero;
                cueStickTipRigidbodyComponent.angularVelocity = Vector3.zero;
            }


            // recover the colorful ball position rotation and remove the speed.
            for (int i = 0; i < colorfulBallSet.Length; i++)
            {
                colorfulBallSet[i].transform.position = colorfulBallPositionList[i];
                colorfulBallSet[i].transform.rotation = colorfulBallRotationList[i];
                Collider colorfulBallColliderComponent = colorfulBallSet[i].transform.GetComponent<Collider>();
                if (colorfulBallColliderComponent.attachedRigidbody)
                {
                    colorfulBallColliderComponent.attachedRigidbody.useGravity = false;
                    colorfulBallColliderComponent.attachedRigidbody.velocity = Vector3.zero;
                    colorfulBallColliderComponent.attachedRigidbody.angularVelocity = Vector3.zero;
                }

            }
        });

        
    }

    // Update is called once per frame
    void Update()
    {
        scoreBoard.GetComponent<TextMeshProUGUI>().text = "";
        scoreBoard.GetComponent<TextMeshProUGUI>().text = "Score: " + Hole.score.ToString();
    }
}
