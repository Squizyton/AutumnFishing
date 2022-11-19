using Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
   public class PlayerMovement : SingletonBehaviour<PlayerMovement>
   {
      [Title("Rigidbody")]
      [SerializeField] private Rigidbody rb;

      [Title("Movement")] 
      [SerializeField,ReadOnly] private float currentSpeed;
      [SerializeField] private float walkSpeed;
      [SerializeField] private float runSpeed;
      public bool isSprinting;
   

      private Vector3 axis;
      private State state;

      private void Start()
      {
         currentSpeed = walkSpeed;
      }


      private void Update()
      {
         axis = state switch
         {
            State.canMove => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized,
            _ => axis
         };
      
         if(Input.GetKeyDown(KeyCode.LeftShift))
            currentSpeed = runSpeed;
         else if(Input.GetKeyUp(KeyCode.LeftShift))
            currentSpeed = walkSpeed;
      }
   
      private void FixedUpdate()
      {
         Movement();
      }



      private void Movement()
      {
         //If the player is actually moving
         if(axis != Vector3.zero)
         {
        
            //Move the player in the direction of the axis and the direction player is looking
            rb.MovePosition(transform.position + (transform.forward * axis.z + transform.right * axis.x) * (currentSpeed * Time.deltaTime));

         }else
         {
            //If the player is not moving
            //Stop the player
            rb.velocity = Vector3.zero;
         }
      }

      public Vector3 GetPosition()
      {
         return transform.position;
      }


      private enum State
      {
         canMove,
         canNotMove
      }
   }
}
