using UnityEngine;

public static class ScriptChecker 
{
	public static bool IsBehaviourExists<T>(T monoBehaviourScript) where T : MonoBehaviour
	{
		return monoBehaviourScript != null || !monoBehaviourScript.Equals(null);
	}
}
