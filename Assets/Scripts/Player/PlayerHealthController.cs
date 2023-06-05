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

				if (value == 0) { return; } else { OnChange?.Invoke(value); }
			}
		}
		public int maxHealth;
		public Action<int> OnChange;
		public GameEvent OnZero;  //death?


		private void Start()
        {
			//loading the heart sprites
			heartSprites[0] = Resources.Load<Sprite>("Heart_Full"); //Full heart
			heartSprites[1] = Resources.Load<Sprite>("Heart_Half"); //Half heart
			heartSprites[2] = Resources.Load<Sprite>("Heart_Empty"); //Empty heart

			healthBar = GameObject.Find("Healthbar");
			healthBarTransform = healthBar.GetComponent<RectTransform>();

			Health = maxHealth;

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
        }

        public void Damage(int d)
        {
	        if (Health - d < 0) {
		        Health = 0;
		        Debug.Log("die");
		        Application.Quit();
		        return;
	        }
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

		public void HideHealthBar()
        {
			StopCoroutine(HiderCoroutine(1));
			StartCoroutine(HiderCoroutine(0));
        }

		public void UnhideHealthBar()
        {
			StopCoroutine(HiderCoroutine(0));
			StartCoroutine(HiderCoroutine(1));
		}
		IEnumerator HiderCoroutine(float target)
        {
			while(hearts[0].color.a != target)
            {
				foreach(Image h in hearts)
                {
					h.color = new Color(255, 255, 255, Mathf.MoveTowards(h.color.a, target, 2 * Time.deltaTime));
                }
				yield return null;
			}
			if(target == 0)
            {
				healthBarHidden = true;
            } else
            {
				healthBarHidden = false;
			}
		}
		
	}
}