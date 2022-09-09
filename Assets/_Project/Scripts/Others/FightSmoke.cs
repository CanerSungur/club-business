using UnityEngine;
using System.Collections;
using ZestGames;

namespace ClubBusiness
{
    public class FightSmoke : MonoBehaviour
    {
        private Ai _ai;

        [Header("-- SETUP --")]
        [SerializeField] private GameObject armObj;
        [SerializeField] private GameObject legObj;

        private IEnumerator _spawnArmEnum, _spawnLegEnum;
        private WaitForSeconds _waitForArmSpawnDelay, _waitForLegSpawnDelay;

        public void Init(Ai ai)
        {
            _ai = ai;
            _waitForArmSpawnDelay = new WaitForSeconds(0.5f);
            _waitForLegSpawnDelay = new WaitForSeconds(0.75f);

            _spawnArmEnum = SpawnArm();
            _spawnLegEnum = SpawnLeg();

            _ai.OnStartFighting += StartSpawning;
            _ai.OnStopFighting += StopSpawning;
        }

        private void OnDisable()
        {
            if (_ai == null) return;

            _ai.OnStartFighting -= StartSpawning;
            _ai.OnStopFighting -= StopSpawning;
        }

        private void StartSpawning()
        {
            StartCoroutine(_spawnArmEnum);
            StartCoroutine(_spawnLegEnum);
        }
        private void StopSpawning()
        {
            StopCoroutine(_spawnArmEnum);
            StopCoroutine(_spawnLegEnum);

            _spawnArmEnum = SpawnArm();
            _spawnLegEnum = SpawnLeg();
        }

        #region SPAWNING FUNCTIONS
        private IEnumerator SpawnArm()
        {
            while (true)
            {
                var randomRot = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), 0f);
                Instantiate(armObj, transform.position, randomRot);
                yield return _waitForArmSpawnDelay;
            }
        }
        private IEnumerator SpawnLeg()
        {
            while (true)
            {
                var randomRot = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), 0f);
                Instantiate(legObj, transform.position, randomRot);
                yield return _waitForLegSpawnDelay;
            }
        }
        #endregion
    }
}
