using System;
using UnityEngine;
using DongerAssetPack.Utility;

namespace DongerAssetPack.IceEngine
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class MovementController : MonoBehaviour
	{
		[Header("Movement Parameters")]
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
		[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.1f;

	#region AnimatorIK

		[Header("Animator IK")]

		[Tooltip("If true, then the character will look in the direction of the mouse pointer, otherwise, it will look straight ahead.")]
		[SerializeField] bool _moveHeadInDirectionOfMouse = false;
		[SerializeField] float _lookAtWeight = 1f;
		private Vector3 _cameraRaycasterHitPoint;

	#endregion
		
		Rigidbody m_Rigidbody;
		Animator m_Animator;
		bool m_IsGrounded;
		bool m_IsShooting;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		float m_StrafeAmount;

		///<summary>The normal Vector of the ground that's hit from the Raycast</summary>
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		CapsuleCollider m_Capsule;
		bool m_Crouching;
		ThirdPersonCharacterLogic _logic;
		CameraRaycaster _cameraRaycaster;

		protected virtual void Start()
		{
			//If animator IK is on, then find the Camera Rayscaster in the game scene. 
			if (_moveHeadInDirectionOfMouse)
			{
				_cameraRaycaster = FindObjectOfType<CameraRaycaster>();
				//TODO: Consider automatically adding CameraRaycaster to the main camera.
				if (!_cameraRaycaster) Debug.LogError("You need to add the CameraRaycaster Component to the MainCamera");

				//Register to cameraRaycaster notifier.
				_cameraRaycaster.OnLayerHit += OnLayerHit;
			}

			_logic = new ThirdPersonCharacterLogic();
			m_Animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;

			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;
		}
		
		//Callback from the Cameraraycaster
        void OnLayerHit(RaycastHit hit) 
        {
            _cameraRaycasterHitPoint = hit.point;
        }
	
		void OnAnimatorIK(int layerIndex)
		{
			//Handle the head movement.
			if(_moveHeadInDirectionOfMouse)
			{
				//Update look direction so that it's looking at the eye level of the player.
				var lookDirection = new Vector3(_cameraRaycasterHitPoint.x, 1f, _cameraRaycasterHitPoint.z);
				m_Animator.SetLookAtPosition(lookDirection);
				m_Animator.SetLookAtWeight(_lookAtWeight);
			}
		}

        ///<summary>Handles the character move abilities</summary>
        public virtual void Move(AbilityArgs args)
		{
			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (args.Move.magnitude > 1f) args.Move.Normalize(); //Normalizes the movement.
			args.Move = transform.InverseTransformDirection(args.Move); //Transforms global to local space.

			//Is the character grounded?  Will impact the abilities.
			CheckGroundStatus();

			args.Move = Vector3.ProjectOnPlane(args.Move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(args.Move.x, args.Move.z);
			m_ForwardAmount = args.Move.z;
			m_IsShooting = args.Shooting; //Sets if the player is shooting.

			//If it's in shooting mode, get the target. 
			if (m_IsShooting == true)
			{
				//Determine the strafe amount for the rotation.
				var target = GetComponent<CharacterHandleWeapon>().Target;

				
				if (Mathf.Abs(args.Move.magnitude) > 0.3)
				{
					var angle = Vector3.SignedAngle(this.transform.forward, args.Move, Vector3.up);
					m_StrafeAmount = Tools.ScaleAngleToOne(angle);
				} 
				
				else {
					m_StrafeAmount = 0;
				}
				
				m_TurnAmount = 0;
			}
			//Otherwise, there will be no strafing allowed.  It'll continue to walk normally.
			else {	
				m_StrafeAmount = 0;
			}
			

			ApplyExtraTurnRotation();

			//If the character is on the ground.
			if (m_IsGrounded)
			{
				HandleGroundedMovement(args);
			}
			//otherwise, if it's in the air.
			else
			{
				HandleAirborneMovement();
			}

			//If the character is crouching make the capsule collidr smaller. 
			ScaleCapsuleForCrouching(args.Crouch);

			PreventStandingInLowHeadroom();

			// send input and other state parameters to the animator
			UpdateAnimator(args.Move);
		}

		void OnGUI()
		{
			GUILayout.Label("Strafe Amount " + m_StrafeAmount);
		}


		void ScaleCapsuleForCrouching(bool crouch)
		{
			if (m_IsGrounded && crouch)
			{
				if (m_Crouching) return;
				m_Capsule.height = m_Capsule.height / 2f;
				m_Capsule.center = m_Capsule.center / 2f;
				m_Crouching = true;
			}
			else
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
					return;
				}
				m_Capsule.height = m_CapsuleHeight;
				m_Capsule.center = m_CapsuleCenter;
				m_Crouching = false;
			}
		}

		void PreventStandingInLowHeadroom()
		{
			// prevent standing up in crouch-only zones
			if (!m_Crouching)
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
				}
			}
		}

		///<summary>Responsible for updating the animator animations</summary>
		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			m_Animator.SetBool("Crouch", m_Crouching);
			m_Animator.SetBool("OnGround", m_IsGrounded);
			m_Animator.SetBool("Shooting", m_IsShooting);
			m_Animator.SetFloat("Strafe", m_StrafeAmount, 0.1f, Time.deltaTime);
			
			if (!m_IsGrounded)
			{
				m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
			}

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
			if (m_IsGrounded)
			{
				m_Animator.SetFloat("JumpLeg", jumpLeg);
			}

			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (m_IsGrounded && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
		}

		///<summary>Applies a gravitational force to the player if it's in the air.  Also updates the ground distance.</summary>
		protected virtual void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);

			// then update the ground distance
			m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}

		///<summary>Will jump if it's not crouched and is grounded.  Jump must be true for it to jump, otherwise, it will not do anything.</summary>
		protected virtual void HandleGroundedMovement(AbilityArgs args)
		{
			//Wrote Unit Tests as other abilities willb e impacted here. 
			if (_logic.CanJump(args.Jump, args.Crouch, m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")))
			{
				m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, args.JumpPower, m_Rigidbody.velocity.z);
				m_IsGrounded = false;
				m_Animator.applyRootMotion = false;
				m_GroundCheckDistance = 0.1f;
			}
		}

		///<summary>Help the character turn faster (this is in addition to root rotation in the animation)</summary>
		void ApplyExtraTurnRotation()
		{
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}


		public void OnAnimatorMove() //Callback method.
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (m_IsGrounded && Time.deltaTime > 0)
			{
				Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = m_Rigidbody.velocity.y;
				m_Rigidbody.velocity = v;
			}
		}

		///<summary>Checks if the character is grounded</summary>
		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				m_Animator.applyRootMotion = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = false;
			}
		}
	}
}
