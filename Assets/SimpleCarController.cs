using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SimpleCarController : NetworkBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    [SyncVar (hook = "OnMotorTorqueChanged")]
    public float motorTorque = 0.0f;
    [SyncVar (hook = "OnWheelsSteeringChanged")]
    public float wheelsSteering = 0.0f;

    public void OnMotorTorqueChanged(float value)
    {
        motorTorque = value;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motorTorque;
                axleInfo.rightWheel.motorTorque = motorTorque;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void OnWheelsSteeringChanged(float value)
    {
        wheelsSteering = value;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = wheelsSteering;
                axleInfo.rightWheel.steerAngle = wheelsSteering;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    [Command]
    public void CmdChangeMotorTorque(float value)
    {
        motorTorque = value;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motorTorque;
                axleInfo.rightWheel.motorTorque = motorTorque;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    [Command]
    public void CmdChangeWheelsSteering(float value)
    {
        wheelsSteering = value;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = wheelsSteering;
                axleInfo.rightWheel.steerAngle = wheelsSteering;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }



    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    //private void Awake()
    void Start()
    {
        if (!isLocalPlayer)
            return;

        CustomButton driveButton = GameObject.Find("DriveButton").GetComponent<CustomButton>();
        driveButton.onCustomButton += OnDriveClicked;

        CustomButton reverseButton = GameObject.Find("ReverseButton").GetComponent<CustomButton>();
        reverseButton.onCustomButton += OnReverseClicked;

        CustomButton rightButton = GameObject.Find("RightButton").GetComponent<CustomButton>();
        rightButton.onCustomButton += OnRightClicked;

        CustomButton leftButton = GameObject.Find("LeftButton").GetComponent<CustomButton>();
        leftButton.onCustomButton += OnLeftClicked;
    }


    public void OnDriveClicked(bool value)
    {
        float motor = value ? maxMotorTorque : 0.0f;
        CmdChangeMotorTorque(motor);
    }

    public void OnReverseClicked(bool value)
    {
        float motor = value ? -maxMotorTorque : 0.0f;
        CmdChangeMotorTorque(motor);
    }

    public void OnRightClicked(bool value)
    {
        float steering = value ? maxSteeringAngle : 0.0f;
        CmdChangeWheelsSteering(steering);
    }

    public void OnLeftClicked(bool value)
    {
        float steering = value ? -maxSteeringAngle : 0.0f;
        CmdChangeWheelsSteering(steering);
    }

    public void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        /*float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        CmdChangeMotorTorque(motor);
        CmdChangeWheelsSteering(steering);*/

        /*foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }*/
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}