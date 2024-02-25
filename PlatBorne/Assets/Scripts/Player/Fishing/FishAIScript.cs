using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAIScript : MonoBehaviour
{
    private Rigidbody2D fishRB;

    [SerializeField] private float _nextFishMovementChange;
    [SerializeField] private float _lastFishMovementChange;
    [SerializeField] private bool _isTimeToChangeMovement;

    private float LastFishMovementChange
    {
        get
        {
            return _lastFishMovementChange;
        }
        set
        {
            _lastFishMovementChange = Time.time;
        }
    }
    private float NextFishMovementChange
    {
        get 
        {
            if(_isTimeToChangeMovement)
            {
                float x = 3;
                _nextFishMovementChange = Time.time + Random.Range(0, x);
            }
            return _nextFishMovementChange;
        }
    }
    private bool IsTimeToChangeMovement
    {
        get
        {
            return (Time.time <= NextFishMovementChange);
        }
    }

    private void Awake()
    {
        fishRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(IsTimeToChangeMovement)
        {
            LastFishMovementChange = 1;            
            fishRB.velocity = RandomVector();
        }
    }

    private Vector2 RandomVector()
    {
        float x = 3;
        return new Vector2(Random.Range(-x, x), 0);
    }
}
