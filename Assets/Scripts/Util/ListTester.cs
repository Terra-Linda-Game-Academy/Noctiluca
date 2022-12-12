using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util {
	[CreateAssetMenu]
	public class ListTester : ScriptableObject {
		[SerializeReference] public List<ListType> things;
	}

	[Serializable]
	public class ListType {
		public int num;
	}
}