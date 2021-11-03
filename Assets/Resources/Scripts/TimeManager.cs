using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static float gameStartMoment;
    private TextMeshProUGUI timeBoard; // time board

    // time variables
    int hour;
    int minute;
    int second;
    int millsecond;

    public static float timeSpend;


    void TimeSpend()
    {
        timeSpend = UnityEngine.Time.deltaTime + timeSpend;

        hour = (int)timeSpend / 3600;
        minute = ((int)timeSpend - hour * 3600) / 60;
        second = (int)timeSpend - hour * 3600 - minute * 60;
        millsecond = (int)((timeSpend - (int)timeSpend) * 1000);
        this.GetComponent<TextMeshProUGUI>().text = "";
        this.GetComponent<TextMeshProUGUI>().text = "Time: " + string.Format("{0:D2}:{1:D2}:{2:D3}", minute, second, millsecond);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeSpend = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpend();
    }
}
