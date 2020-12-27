using UnityEngine;

public static class ApplicationManager
{
	public static void Quit() 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
