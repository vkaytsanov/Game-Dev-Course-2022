using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeType
{
	Health,
	Keys
}

public class PlayerAttributes : MonoBehaviour
{
	private Dictionary<AttributeType, int> _attributes;

	private Dictionary<AttributeType, Action<int>> _attributeChangedEvent;

	
	PlayerAttributes() 
	{
		_attributes = new Dictionary<AttributeType, int>();
		_attributeChangedEvent = new Dictionary<AttributeType, Action<int>>();
	}

	public void RegisterForEvent(AttributeType type, Action<int> func)
	{
		if (_attributeChangedEvent.ContainsKey(type))
		{
			_attributeChangedEvent[type] += func;
		}
		else
		{
			_attributeChangedEvent[type] = func;
		}
	}

	public void UnregisterForEvent(AttributeType type, Action<int> func)
	{
		_attributeChangedEvent[type] -= func;
	}

	public void SetAttribute(AttributeType type, int value)
	{
		_attributes[type] = value;

		if (_attributeChangedEvent.ContainsKey(type))
		{
			_attributeChangedEvent[type].Invoke(_attributes[type]);
		}
	}

	public void AddAttribute(AttributeType type, int value = 1)
	{
		_attributes.Add(type, value);

		if (_attributeChangedEvent.ContainsKey(type))
		{
			_attributeChangedEvent[type].Invoke(_attributes[type]);
		}
	}

	public int GetHealth()
	{
		return _attributes.GetValueOrDefault(AttributeType.Health, 3);
	}

	public int GetKeys() 
	{
		return _attributes.GetValueOrDefault(AttributeType.Keys, 0);
	}
}
