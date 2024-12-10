using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultLaunchKinematic : MonoBehaviour
{
    [SerializeField] private Rigidbody _rock;

    [Range(min: 0, max: 90)]
    [SerializeField] private float _angle;
    [SerializeField] private float _power;

    private Vector2 _initialVelocity;
    private Vector2 _initialPosition;


    private float _time;
    private bool _isLaunched;
    private bool _canLaunch = true; 

    public static event Action LaunchVisualEvent;

    void Start()
    {
        _initialPosition = _rock.position;
    }

    private void Launch()
    {
        if (!_canLaunch) return; 

        LaunchVisualEvent?.Invoke();

        _initialVelocity = new Vector2(Mathf.Cos(_angle * Mathf.PI / 180), Mathf.Sin(_angle * Mathf.PI / 180)) * _power;

        _isLaunched = true;
        _canLaunch = false;

        Debug.Log("Launch called!");
    }

    private float KinematicEquation(float acceleration, float velocity, float position, float time)
    {
        return 0.5f * acceleration * time + velocity * time + position;
    }

    private void Update()
    {

        if (Input.GetMouseButton(0) && !_isLaunched && _canLaunch)
        {
            Launch();
        }


        if (_isLaunched)
        {
            _time += Time.deltaTime;

            float newRockX = KinematicEquation(acceleration: 0, _initialVelocity.x, _initialPosition.x, _time);
            float newRockY = KinematicEquation(acceleration: -9.81f, _initialVelocity.y, _initialPosition.y, _time);
            _rock.position = new Vector3(newRockX, newRockY, _rock.position.z);

            if (newRockY <= 0)
            {
                // Distance Calculation
                float distanceTravelled = newRockX - _initialPosition.x;

                Debug.Log($"The projectile hit the ground. Distance traveled: {distanceTravelled:F2} units. Press R to restart.");

                _isLaunched = false;
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCatapult();
        }
    }

    //reset Function
    private void ResetCatapult()
    {
        _rock.position = _initialPosition;
        _time = 0;
        _isLaunched = false;
        _canLaunch = true;

        Debug.Log("Catapult reset. Ready to launch again.");
    }
}
