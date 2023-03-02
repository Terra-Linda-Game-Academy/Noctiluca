using System.Collections;
using UnityEngine;

namespace Enemies.ChargeMob
{
    public class ChargeMob : BasicMob
    {
        public float agroRadius = 20f;
        public GameObject player;

        Vector3 _startingLocation;
        Vector3 _chargeLocation;
        bool _isAgro = false;
        float _maxDistanceRun = 0f;

        float _chargeCooldown = 0f;
        float _chargeTime = 0f;


        private void Awake()
        {
            base.Awake();
            updateFunctions += DetectPlayer;
            updateFunctions += ChargeUpdate;
        }

        void DetectPlayer()
        {
            _chargeCooldown -= Time.deltaTime;
            if (_chargeCooldown > 0f || _isAgro) return;
            if (Vector3.Distance(transform.position, player.transform.position) < agroRadius)
            {
                RaycastHit hit;
                Vector3 rayDirection = player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(rayDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
                Debug.DrawRay(transform.position, rayDirection, Color.blue);
                if (Physics.Raycast(transform.position, rayDirection, out hit))
                {
                    if (hit.transform.gameObject == player)
                    {
                        IsWandering = false;
                        _chargeCooldown = 30f;
                        _startingLocation = transform.position;
                        _maxDistanceRun = Vector3.Distance(transform.position, player.transform.position) + 2f;
                        _chargeLocation = player.transform.position;

                        _isAgro = true;


                        StartCoroutine(ChargeAtLocation());
                    }
                }
            }
            else { IsWandering = true; }
        }

        IEnumerator ChargeAtLocation()
        {
            // chargin period (stands still while getting ready to charge at you)
            yield return new WaitForSeconds(0.3f);
            Agent.SetDestination(_chargeLocation);
            Agent.speed = 40f;
            Agent.angularSpeed = 5;
        }

        void ChargeUpdate()
        {
            if (_isAgro)
            {
                _chargeTime += Time.deltaTime;
                if (Vector3.Distance(transform.position, _startingLocation) > _maxDistanceRun
                 || Vector3.Distance(transform.position, _chargeLocation) < 1f
                 || _chargeTime > 10f)
                {
                    _chargeTime = 0f;
                    _isAgro = false;
                    IsWandering = true;
                    Agent.speed = 4.67f;
                    Agent.angularSpeed = 120f;
                }
            }
        }
    }
}