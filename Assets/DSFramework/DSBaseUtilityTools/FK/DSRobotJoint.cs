using DSFramework;
using UnityEngine;

public class DSRobotJoint : MonoBehaviour
{
    private DSMainRobot DS_DSMainRobot_MainRobotScript;

    private readonly float[] DS_float_TempAngle = {0, 0, 0};
    public int DS_int_AxisNumber { get; set; }
    public int DS_int_JointNumber { get; set; }

    public float ReadTempangle()
    {
        return DS_float_TempAngle[DS_int_AxisNumber];
    }

    public void AngleMove(float ang)
    {
        #region 将机器人坐标限定在某个范围内

        if (ang > 180)
        {
            while (ang > 180)
            {
                ang -= 360;
            }
        }
        else if (ang < -180)
        {
            while (ang < -180)
            {
                ang += 360;
            }
        }

        #endregion

        DS_float_TempAngle[DS_int_AxisNumber] = ang;
        transform.localEulerAngles = new Vector3(DS_float_TempAngle[0], DS_float_TempAngle[1], DS_float_TempAngle[2]);
    }

    void Start()
    {
        DS_int_JointNumber = int.Parse(gameObject.name.Substring(5, 1));

        DS_DSMainRobot_MainRobotScript = GameObject.FindWithTag("MainRobot").GetComponent<DSMainRobot>();
        DS_int_AxisNumber = DS_DSMainRobot_MainRobotScript.DS_int_Axis[DS_int_JointNumber];
    }
}