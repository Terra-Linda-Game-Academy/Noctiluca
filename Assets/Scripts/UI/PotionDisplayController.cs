using JetBrains.Annotations;
using Player;
using Potions;
using Potions.Fluids;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI {
	public class PotionDisplayController : MonoBehaviour {
		public ScriptableVar<MonoBehaviour> player;

		public Image           potionImage;
		public TextMeshProUGUI potionLabel;

		public int imageSize       = 16;
		public int potionRadius    = 6;
		public int neckWidth       = 4;
		public int neckHeight      = 6;
		public int maxPotionHeight = 14;

		public FluidAsset testFluidAsset;

		private BetterPlayerController _player;

		private void Start() {
			_player = (BetterPlayerController) player.Value;

			_player.inventory.OnSelectionChange += () => {
				                                       SetPotion(
					                                       !_player.inventory.IsEmpty
						                                       ? _player.inventory.Current
						                                       : null);
			                                       };
		}

		public void Test() => SetPotion(new Potion(testFluidAsset.GetFluid(), 1f, 1f));

		public void Empty() => SetPotion(null);

		private void SetPotion([CanBeNull] Potion potion) {
			Sprite newSprite = GeneratePotionImage(potion);

			potionImage.sprite = newSprite;

			potionLabel.text = potion != null ? potion.Name() : "Empty";
		}

		private Sprite GeneratePotionImage([CanBeNull] Potion potion) {
			const float edgeEpsilon = .5f;
			const int   edgePadding = 1;

			Texture2D tex = new Texture2D(imageSize, imageSize) {filterMode = FilterMode.Point};

			int xCen = tex.width / 2;
			int yCen = potionRadius + edgePadding;

			for (int x = 0; x < tex.width; x++) {
				for (int y = 0; y < tex.height; y++) {
					int   dx   = x - xCen;
					int   dy   = y - yCen;
					float dist = Mathf.Sqrt(dx * dx + dy * dy);

					float edge = dist - potionRadius;
					float val  = (y - edgePadding) / (float) maxPotionHeight;

					Color col = Color.clear;

					bool potionInBottle = edge < -edgeEpsilon;

					bool circleEdge = edge < edgeEpsilon && edge > -edgeEpsilon;

					bool neckEdge = Mathf.Abs(dx) == neckWidth / 2
					             && edge          > edgeEpsilon
					             && dy            > 0
					             && dy            < potionRadius + neckHeight;

					if (circleEdge || neckEdge) col = Color.black;

					bool potionInNeck = dx < neckWidth  / 2
					                 && dx > -neckWidth / 2
					                 && dy > 0;

					Color potColor = potion != null ? potion.Fluid.PrimaryColor.Evaluate(val) : Color.clear;

					float top = potion?.NormalizedRemaining ?? 1;

					if (potionInBottle || potionInNeck) {
						if (y < maxPotionHeight * top + edgePadding) { col = potColor; } else { col = Color.clear; }
					}

					//draw ellipse for top of potion

					float a = neckWidth / 2f + 0.5f;
					float b = 2f;

					int exCen = xCen;
					int eyCen = yCen + potionRadius + neckHeight;

					float eVal = (x - exCen) * (x - exCen) / (a * a)
					           + (y - eyCen) * (y - eyCen) / (b * b)
					           - 1;

					if (eVal < edgeEpsilon && eVal > -edgeEpsilon) col = Color.black;

					tex.SetPixel(x, y, col);
				}
			}

			tex.Apply();

			return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
		}
	}
}