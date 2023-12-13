using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ImpactParams : EventArgs{
    public ImpactParams(Vector3 point)
    {
        ImpactPoint = point;
    }
    public Vector3 ImpactPoint { get; private set; }
}

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] Vector3 Direction;
    [SerializeField] private bool AffectedByGravity;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private GameObject ImpactEffects;

    #region Event
    public static event EventHandler<ImpactParams> ProjectileHit;
    #endregion

    public float _speed = 50;

    Vector3 _velocity;
    Vector3 _previousPos;
    float _yVel = 0;
    float _damage;
    RaycastHit _hit;

    private void Start()
    {
        _previousPos = transform.position;
        _velocity = Direction.normalized * _speed;
    }

    void Update()
    {
        if (AffectedByGravity) _yVel += Physics.gravity.y * Time.deltaTime;
        transform.Translate(_velocity * Time.deltaTime);
        transform.Translate(Vector3.up * _yVel * Time.deltaTime, Space.World);

        if (Physics.Linecast(_previousPos, transform.position, out _hit, collisionMask, QueryTriggerInteraction.Collide))
        {
            if (_hit.collider.tag == "AI")
            {
                EnemyBaseAI enemy;
                if (_hit.transform.root.TryGetComponent(out enemy))
                {
                    enemy.TakeDamage();
                }
                if (ImpactEffects) Instantiate(ImpactEffects, _hit.point, Quaternion.LookRotation(_hit.normal));
                Destroy(gameObject);
            }
	        if (_hit.collider.tag == "Target")
	        {
	        	_hit.collider.gameObject.GetComponent<Shatter>().oneShot(0);
	        }
	        if (_hit.collider.tag == "Breakable")
	        {
	        	_hit.collider.gameObject.GetComponent<Shatter>().oneShot(0);
	        }
            else
            {
                if (ProjectileHit != null) ProjectileHit(this, new ImpactParams(_hit.point));
                Destroy(gameObject);
            }
        }
    }
}