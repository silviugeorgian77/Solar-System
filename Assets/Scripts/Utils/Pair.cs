using System;
using UnityEngine;

[Serializable]
public class Pair<T, U>
{
	[SerializeField]
	private T element1;
	[SerializeField]
	private U element2;

	public Pair()
	{
	}

	public Pair(T element1, U element2)
	{
		this.Element1 = element1;
		this.Element2 = element2;
	}

	public T Element1 { get { return element1; } set { element1 = value; } }
	public U Element2 { get { return element2; } set { element2 = value; } }

}
