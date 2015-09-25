
using UnityEditor;
using UnityEngine;
using System;

[CanEditMultipleObjects, CustomEditor(typeof(InertiaCalculator))]
public class InertiaCalculatorEditor : Editor
{
	void OnEnable()
	{
	}
	
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		if(GUILayout.Button("Calculate Inertia", GUILayout.MaxWidth(200)))
		{
			for(int i = 0;i < targets.Length;++i)
			{
				InertiaCalculator calc = targets[i] as InertiaCalculator;
				if(calc != null  && calc.rigidbody2D != null)
				{
					calc.inertia = InertiaCalculator.ComputeInertia(calc.rigidbody2D);
				}
			}
		}
	}
}
