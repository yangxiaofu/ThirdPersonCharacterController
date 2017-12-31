using System;
using UnityEngine;

namespace DongerAssetPack.MovementEngine
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class DongerController : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        [SerializeField] private bool m_Jump = false;                      // the world-relative desired move direction, calculated from the camForward and user input.
        [SerializeField] private bool m_Crouch = false;

        Ability[] _abilities;
        
        private void Start()
        {

            //Get all of the abilities attached to the gameObject
            _abilities = GetComponents<Ability>();

            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            for(int i = 0; i < _abilities.Length; i++)
            {
                _abilities[i].HandleAbility();
            }

            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            m_Crouch = CrossPlatformInputManager.GetButton("Crouch");
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif
            var abilityArgs = new AbilityArgs();
            abilityArgs.Move = m_Move;
            abilityArgs.Crouch = m_Crouch;
            abilityArgs.Jump = m_Jump;
            // pass all parameters to the character control script
            m_Character.Move(abilityArgs);
            m_Jump = false;
        }
    }
}
