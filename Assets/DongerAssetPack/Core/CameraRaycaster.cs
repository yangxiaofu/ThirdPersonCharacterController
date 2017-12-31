using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongerAssetPack.MovementEngine
{
	//Attach to the camera.
	public class CameraRaycaster : MonoBehaviour {

		protected Camera _cam;
		[SerializeField] LayerMask _layerMaskToHit;

		public delegate void LayerHit(RaycastHit hit);
		public event LayerHit OnLayerHit;

		// Use this for initialization
		protected virtual void Start () 
		{
			_cam = GetComponent<Camera>();
			if (!_cam) Debug.LogError("There is no camera attached to " + this.gameObject.name);
		}
		
		// Update is called once per frame
		void Update () {
			//Shoot Raycast
			RaycastForLayerMask();
		}

        private void RaycastForLayerMask()
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100f, _layerMaskToHit))
			{
				print("Hit the ground");
				//If a layer was hit.
				if (OnLayerHit != null) OnLayerHit(hit);
			}
        }
    }

}
