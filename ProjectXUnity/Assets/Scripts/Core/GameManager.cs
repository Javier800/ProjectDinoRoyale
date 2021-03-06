﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Enemy;
using System.Linq;

namespace Runner.Core {
    public class GameManager : MonoBehaviour {

		[SerializeField] private float globalMultiplier;

		[SerializeField] bool isPlayerAlive = true;
		[SerializeField] DinosScriptableObject dinosScriptableObject;
		[SerializeField] Canvas gameOverCanvas;

		EnemyGenerator[] enemiesGenerators;

		public static GameManager instance;

		private void Awake(){
			instance = this;
			
			dinosScriptableObject.Reset();
		}

        private void Start() {			
			globalMultiplier = 1f;
			if (PlayerPrefs.GetInt("score") > 0)
			{
				ServerManager.instance.SaveTempScore(PlayerPrefs.GetInt("score"));
			}
            if(IsPlayerAlive){
				enemiesGenerators = FindObjectsOfType<EnemyGenerator>();
				Time.timeScale = 1;
				gameOverCanvas.gameObject.SetActive(false);
				StartCoroutine(GenerateEnemy());
            }
		}


        public bool IsPlayerAlive{
			get{
                return isPlayerAlive;
			}
			set{
				isPlayerAlive = value;
			}
        }

		private IEnumerator GenerateEnemy() {
			if (IsPlayerAlive) {
				int randomGenerator = Random.Range(0, enemiesGenerators.Length);
				yield return enemiesGenerators[randomGenerator].Spawn();				
				StartCoroutine(GenerateEnemy());
			}			
        }


		public void GameOver() {
			ScoreManager.instance.StopScoring();
			ScoreManager.instance.GameOver();
			if (!string.IsNullOrEmpty(ServerManager.instance.user?.nickname))
			{
				ServerManager.instance.NewScore();
			}
			gameOverCanvas.gameObject.SetActive(true);
			DesactivateParticles();
		}

		public float GlobalMultiplier() {
			return globalMultiplier;
        }

		public void DesactivateParticles() {
			FindObjectsOfType<ParticleSystem>().Where(p => p.gameObject.name == "WhiteSmoke").First().Pause();
		}

		//private void Update() {

		//	RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		//	if (hit.collider != null) {

		//	}
		//}
	}

}