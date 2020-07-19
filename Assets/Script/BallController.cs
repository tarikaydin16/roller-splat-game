using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    private bool isTravelling;
    private Vector3 travelDirection;
    private Vector3 nectCollisionPosition;
    public int minSwipeRecoginiton=500;
    private Vector2 swipePosLastFrame;
    private Vector2 currentSwipe;
    private Vector2 swipePosCurrentFrame;
    private Color solveColor;
    private void Start() {
        solveColor = Random.ColorHSV(0.5f,1);
        GetComponent<MeshRenderer>().material.color = solveColor;    
    }
    private void FixedUpdate() {
        if(isTravelling){
            rb.velocity=speed*travelDirection;     
        }  

        Collider[] hitColliders=Physics.OverlapSphere(transform.position-(Vector3.up/2),0.05f);
        int i=0;
        while(i<hitColliders.Length){
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            if(ground && !ground.isColored){
                ground.ChangeColor(solveColor);
            }
            i++;
        }

        if(nectCollisionPosition !=Vector3.zero){
            if(Vector3.Distance(transform.position,nectCollisionPosition)<1){
                isTravelling=false;
                travelDirection=Vector3.zero;
                nectCollisionPosition = Vector3.zero;

            }

        }
         if(isTravelling) return;
         if(Input.GetMouseButton(0)){
             swipePosCurrentFrame=new Vector2(Input.mousePosition.x,Input.mousePosition.y);
             if(swipePosLastFrame !=Vector2.zero){
                 currentSwipe=swipePosCurrentFrame-swipePosLastFrame;
                 if(currentSwipe.sqrMagnitude<minSwipeRecoginiton){
                    return;

                 }
                 currentSwipe.Normalize();
                //Up/Down
                if(currentSwipe.x>-.5f && currentSwipe.x<.5f){
                    //go up/down
                    SetDestionation(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }
                
                if(currentSwipe.y>-.5f && currentSwipe.y<.5f){
                    //go left/right
                    SetDestionation(currentSwipe.x > 0 ? Vector3.right : Vector3.left);

                }


             }
             swipePosLastFrame=swipePosCurrentFrame;

         }
         if (Input.GetMouseButtonUp(0)){

             swipePosLastFrame=Vector2.zero;
             currentSwipe=Vector2.zero;

         }

    }

   private void SetDestionation(Vector3 direction) {
       travelDirection = direction;

        RaycastHit hit;
        if(Physics.Raycast(transform.position,direction,out hit,100f)){
            nectCollisionPosition = hit.point;


        }

        isTravelling=true;
   }

}












