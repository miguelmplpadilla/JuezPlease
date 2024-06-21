using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class PruebaNode : Node {

	[Input] public float a;
	[Output] public float b;

	public GameObject objectPrueba;

	public override object GetValue(NodePort port) {
		if (port.fieldName == "b") return GetInputValue<float>("a", a);
		else return null;
	}
}