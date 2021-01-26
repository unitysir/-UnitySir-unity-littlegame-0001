using DSFramework;
using UnityEngine;

public class DSMainRobot : MonoBehaviour
{
    #region definition

    public static DSMainRobot Instance { get; private set; }
    private readonly GameObject[] DS_GameObject_JointArray = new GameObject[6];
    private static int DS_int_Steps = 1000;
    private int DS_int_Count = 0;
    private bool DS_bool_MoveTag = false;
    [HideInInspector] public int[] DS_int_Axis = {2, 0, 0, 1, 0, 1};
    private readonly float[] DS_float_RealTime = new float[6];
    private readonly float[] DS_float_Target = new float[6];
    private readonly float[] DS_float_Delta = new float[6];
    private readonly float[] DS_float_Current = new float[6];
    public bool DS_bool_IsCollider = false;

    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void SetJoint()
    {
        for (int i = 0; i < 6; i++)
        {
            DS_GameObject_JointArray[i] = GameObject.FindWithTag("joint" + i);
        }
    }

    #region User

    private void SetAngle(float[] angle)
    {
        for (int i = 0; i < DS_GameObject_JointArray.Length; i++)
        {
            DS_GameObject_JointArray[i].transform.GetComponent<DSRobotJoint>().AngleMove(angle[i]);
        }

        UpdateJointAngle();
    }

    private void UpdateJointAngle()
    {
        for (int i = 0; i < DS_float_RealTime.Length; i++)
        {
            DS_float_RealTime[i] = DS_GameObject_JointArray[i].GetComponent<DSRobotJoint>().ReadTempangle();
        }
    }

    private void SetTargetAngle(float[] angles)
    {
        for (int i = 0; i < DS_float_Target.Length; i++)
        {
            DS_float_Target[i] = angles[i];
        }
    }

    private void GetDeltaAngle()
    {
        for (int i = 0; i < DS_float_Delta.Length; i++)
        {
            DS_float_Delta[i] = (DS_float_Target[i] - DS_float_RealTime[i]) / DS_int_Steps;
        }
    }

    private float[] GetCurrentAngle()
    {
        for (int i = 0; i < 6; i++)
        {
            DS_float_Current[i] = DS_float_RealTime[i] + DS_float_Delta[i];
        }

        return DS_float_Current;
    }

    #endregion

    private void Start()
    {
        SetJoint();
        SetAngle(new float[] {0, 0, 0, 0, 0, 0});

        DSEntity.Mono.AddFixedUpdate("RobotFixedUpdate", OnFixedUpdate);
    }

    private void OnFixedUpdate()
    {
        if (DS_bool_MoveTag)
        {
            if (DS_int_Count < DS_int_Steps)
            {
                if (!DS_bool_IsCollider)
                {
                    float[] angles = GetCurrentAngle();
                    SetAngle(angles);
                    DSEntity.MsgMechain.Sender("DS_GetCurrentAngle", angles);
                    DS_int_Count++;
                }
            }
            else
            {
                SetAngle(DS_float_Target);
                DS_bool_MoveTag = false;
            }
        }
    }
}