using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Player {
	public class PlayerHealthController : MonoBehaviour {
		[SerializeField] private RuntimeVar<int> healthVar;
		private Sprite[] heartSprites = new Sprite[3];
		[SerializeField] private GameObject heartPrefab;
		private List<Image> hearts = new List<Image>();

		private GameObject healthBar;
		private RectTransform healthBarTransform;

		public bool healthBarHidden = false;

		public int Health {
			get => healthVar.Value;
			private set {
				healthVar.Value = value;

				if (value == 0) { OnZero?.Invoke(); } else { OnChange?.Invoke(value); }
			}
		}
		public int maxHealth;
		public Action<int> OnChange;
		public Action      OnZero; //death?

        private void Start()
        {
			//loading the heart sprites
			heartSprites[0] = Resources.Load<Sprite>("Heart_Full"); //Full heart
			heartSprites[1] = Resources.Load<Sprite>("Heart_Half"); //Half heart
			heartSprites[2] = Resources.Load<Sprite>("Heart_Empty"); //Empty heart

			healthBar = GameObject.Find("Healthbar");
			healthBarTransform = healthBar.GetComponent<RectTransform>();

			//Instantiating Hearts as children of a pre-existing healthbar GameObject
			GameObject heart;
			for (int i = 0; i < maxHealth/2; i++)
            {
				heart = Instantiate(heartPrefab, healthBarTransform);
				RectTransform rt = heart.GetComponent<RectTransform>();
				heart.name = "Heart_" + i;
				rt.anchoredPosition = new Vector2(i * 65, 0);
				hearts.Add(heart.GetComponent<Image>()); //Storing the heart's image component
			}
			StartCoroutine(HiderCoroutine());
        }

        public void Damage(int d)
        {
			if (Health - d < 0) {Health = 0; return;}
			Health -= d; //damage the player by d
			UpdateHealthBar();
		}

		
		public void Heal(int h)
        {
			if (Health + h > maxHealth) {Health = maxHealth; return;}
			Health += h; //heal the player by h
			UpdateHealthBar();
		} 

		private void UpdateHealthBar()
		{
			//Update each heart's sprite based on the player's current health
			for (int i = 0; i < hearts.Count; i++)
			{
				if (Health - (i * 2) >= 2)
				{
					hearts[i].sprite = heartSprites[0];
				}
				else if (Health - (i * 2) == 1)
				{
					hearts[i].sprite = heartSprites[1];
				}
				else
				{
					hearts[i].sprite = heartSprites[2];
				}

			}
		}

		//methods for hiding and unhiding the healthbar

		IEnumerator HiderCoroutine()
        {
			while(true)
            {
				Vector2 target = healthBarHidden ? new Vector2(55, 50) : new Vector2(55, -50);
				if (Vector2.Distance(healthBar.transform.position, target) > 0.01)
				{
					healthBarTransform.anchoredPosition = Vector2.MoveTowards(healthBarTransform.anchoredPosition, target, 400 * Time.deltaTime);
				}
				yield return null;
			}
		}
		
	}
}