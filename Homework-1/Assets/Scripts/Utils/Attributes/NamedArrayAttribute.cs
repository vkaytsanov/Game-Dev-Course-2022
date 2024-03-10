using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Attributes
{
	[Serializable]
	public class NamedArrayAttribute : PropertyAttribute
	{
		public readonly string[] names;

		public NamedArrayAttribute(string[] names)
		{
			this.names = names;
		}

		public NamedArrayAttribute(Type type)
		{
			names = Enum.GetNames(type);
		}
	}
}
