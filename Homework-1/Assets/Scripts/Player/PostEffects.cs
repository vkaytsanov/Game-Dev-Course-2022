using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffects : MonoBehaviour
{
	[SerializeField]
	private Material _vignetteMaterial;

	[SerializeField]
	[Range(0.0f, 1.0f)]
	private float _vignetteRadius = 0.5f;

	[SerializeField]
	[Range(0.0f, 1.0f)]
	private float _vignetteSmoothEdge = 0.0f;

	[SerializeField]
	private Color _vignetteTintColor = Color.black;

	private PlayerAttributes _attributes;

	private bool _renderVignette = false;


	private void OnEnable()
	{
		Game.Instance.RegisterForPlayerCreated(OnNewPlayer);
	}

	private void OnDisable()
	{
		if (_attributes)
		{
			_attributes.UnregisterForEvent(AttributeType.Health, OnPlayerHealthChanged);
		}
		Game.Instance.UnregisterForPlayerCreated(OnNewPlayer);
	}

	private void OnNewPlayer(GameObject gameObject)
	{
		_attributes = gameObject.GetComponent<PlayerAttributes>();
		_attributes.RegisterForEvent(AttributeType.Health, OnPlayerHealthChanged);
	}

	private void OnPlayerHealthChanged(int health)
	{
		_renderVignette = (health == 1);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_renderVignette)
		{
			_vignetteMaterial.SetFloat("_Radius", _vignetteRadius);
			_vignetteMaterial.SetFloat("_Smoothing", _vignetteSmoothEdge);
			_vignetteMaterial.SetColor("_TintColor", _vignetteTintColor);
			Graphics.Blit(source, destination, _vignetteMaterial);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}
}
