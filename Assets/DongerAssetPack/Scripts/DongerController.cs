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
        Ability[] _abilities;
        AbilityArgs _args = new AbilityArgs();

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

        public string HelpBox(){
            return "The Donger Controller is the core of the MovementEngine. The DongerController handles the abilities and move Parameters and passes it to the Third Person Character Controller.  If you want to have movement abilities added to this, you can add these abilities as a component to this gameObject.";
        }

        private void Update()
        {
            HandleAttachedAbilities();
        }

        ///<summary>Handles all of the abilities that are attached to this gameObject</summary>
        private void HandleAttachedAbilities()
        {
            //Handle all abilities attached to this gameobject.
            for (int i = 0; i < _abilities.Length; i++)
            {
                _args = _abilities[i].HandleAbility(_args);
            }
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            HandleCharacterMovement();

            m_Character.Move(_args);

            _args.Jump = false;
        }

        ///<summary>Updates the character Ability Args movement Vector per the input of the CrossPlatformInputManager</summary>
        private void HandleCharacterMovement()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            _args.Move = m_Move;
        }
    }
}
