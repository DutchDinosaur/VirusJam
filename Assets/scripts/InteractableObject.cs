using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour {

    public GameObject canvas;

    private void OnMouseOver() {
        this.gameObject.layer = 3;
    }

    private void OnMouseDown() {
        canvas.SetActive(true);
    }

    private void OnMouseExit() {
        this.gameObject.layer = 0;
    }

    public void closeMenu() {
        canvas.SetActive(false);
    }
}