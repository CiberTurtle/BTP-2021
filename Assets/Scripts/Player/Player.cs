using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Mover2D))]
public class Player : MonoBehaviour
{
	// Singleton
	public static Player current;

	// Perams
	[SerializeField] float interactionSize;
	[SerializeField] LayerMask interactableLayer;
	[Space]
	[SerializeField] GameObject pfInteractEffect;
	[SerializeField] Transform interatableUI;
	[SerializeField] TMP_Text interactableText;

	// Data
	[Space]
	public List<SOItem> inv = new List<SOItem>();
	Interactable interactableObj;

	// Cache
	Mover2D mover;
	Inputs inputs;

	void Awake()
	{
		current = this;

		mover = GetComponent<Mover2D>();

		inputs = new Inputs();
		inputs.Player.Move.performed += (x) => mover.v2MoveInput.x = x.ReadValue<float>();
		inputs.Player.Jump.performed += (x) => mover.JumpDown();
		inputs.Player.Jump.canceled += (x) => mover.JumpUp();
		inputs.Player.Interact.performed += (x) => { if (!Game.current.PAUSED) Interact(); };
	}

	void Start()
	{
		Game.current.UpdateHUD();
	}

	void Update()
	{
		if (Game.current.PAUSED) return;

		Collider2D closestItem = null;
		var closestDist = float.PositiveInfinity;

		foreach (var item in Physics2D.OverlapCircleAll(transform.position, interactionSize, interactableLayer))
		{
			var dist = Vector2.SqrMagnitude(transform.position + item.transform.position);
			if (dist < closestDist)
			{
				closestItem = item;
				closestDist = dist;
			}
		}

		interactableObj = closestItem?.GetComponent<Interactable>();

		if (!mover.bIsGrounded)
			interactableObj = null;

		if (interactableObj)
		{
			interatableUI.gameObject.SetActive(true);
			interatableUI.position = (Vector2)interactableObj.transform.position + new Vector2(0, interactableObj.transform.localScale.y);
			interactableText.text = interactableObj.pickupName;
		}
		else
			interatableUI.gameObject.SetActive(false);
	}

	void Interact()
	{
		if (interactableObj)
		{
			if (pfInteractEffect) Instantiate(pfInteractEffect, interactableObj.transform.position, Quaternion.identity);
			interactableObj.Interact();
		}
	}

	public bool UseItem(SOItem item)
	{
		if (inv.Contains(item))
		{
			inv.Remove(item);
			Game.current.UpdateHUD();
			return true;
		}

		return false;
	}

	public void AddItem(SOItem item)
	{
		inv.Add(item);
		Game.current.UpdateHUD();
	}

	void OnEnable()
	{
		inputs.Enable();
	}

	void OnDisable()
	{
		inputs.Disable();
	}
}