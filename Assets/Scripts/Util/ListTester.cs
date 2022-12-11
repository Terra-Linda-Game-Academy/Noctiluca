using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util {
	[CreateAssetMenu]
	public class ListTester : ScriptableObject {
		public string testString;

		[SerializeReference] public List<ListType> things;
	}

	[Serializable]
	public class ListType {
		public int num;
	}
}