using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    private ContactFilter2D castFilter;
    CapsuleCollider2D touchingCol;
    Animator animator;
    //? what is RaycastHit2D
    //? why using value 5 for RaycastHit2D array
    RaycastHit2D[] groundHit = new RaycastHit2D[5];
    RaycastHit2D[] wallHit = new RaycastHit2D[5];
    RaycastHit2D[] cellingHit = new RaycastHit2D[5];

    //? how to calculate the number
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float gcellingDistance = 0.05f;

    [SerializeField]
    private bool _isOnWall;
    [SerializeField]
    private bool _isGrounded = true;
    [SerializeField]
    private bool _isOnCelling;
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsGrounded { 
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    public bool IsOnCelling
    {
        get
        {
            return _isOnCelling;
        }
        private set
        {
            _isOnCelling = value;
            animator.SetBool(AnimationStrings.isOnCelling, value);
        }
    }

    // Start is called before the first frame update

    private void Awake()
    {
        animator = GetComponent<Animator>();
        touchingCol = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        //?  what is cast function 
       IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHit, groundDistance) > 0;
       IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHit, wallDistance) > 0;
       IsOnCelling = touchingCol.Cast(Vector2.up, castFilter, cellingHit, gcellingDistance) > 0;
    }
}
