/*
This script should be added to the Parent objects,
All objects in the CarouselPicker will be children of this object.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomCarouselPicker : MonoBehaviour {

	/// <summary>
	/// whether or not we should save the carousel's rotation in the PlayerPrefs
	/// </summary>
    public bool saveRotationIndex = true;

    /// <summary>
	/// The key used for saving the rotation in PlayerPrefs
    /// </summary>
    public string saveKey = "carouselCustom";

	//<summary>
	//Radius of the Carousel
	//</summary>
	public float radius = 1f; 

	//<summary>
	//Speed at switch the Carousel will rotate during swiping
	//</summary>
	public float rotateSpeed = 1f;

	//<summary>
	//Speed at switch the Carousel will rotate after swiping
	//</summary>
	public float snapSpeed = 1f;

	//<summary>
	//Weather or not objects will lerp into position or not
	//</summary>
	public bool lerpObjects =  true;

	//<summary>
	//the speed in which objects move (if lerpObjects is true)
	//</summary>
	public float objectSpeed = 5f;

	//<summary>
	// set rotation Index (the rotationIndex once the carousel is set)
	//</summary>
    private int _sri = 0;
	public int sri 
    {
        get{ return _sri; }
        set
        {
            _sri = value;
			rotationIndex = value;

            if (saveRotationIndex)
            {
                PlayerPrefs.SetInt(saveKey,value);
            }

            if (OnSelectedObjectChange != null)
            {
                OnSelectedObjectChange();
            }

        }
    }

	//<summary>
	// the rotation Index
	//</summary>
	private int _rotationIndex = 0;
	public int rotationIndex 
    {
		get{ return _rotationIndex; }
        set
        {
			if (_rotationIndex == value)
			{
				return;
			}

			_rotationIndex = value;

            if (OnSelectedObjectChange != null)
            {
                OnSelectedObjectChange();
            }
        }
    }

    public delegate void genericDelegate();

    /// <summary>
	/// The Delegates that executes when the selected object changes.
    /// </summary>
    public genericDelegate OnSelectedObjectChange;


	//<summary>
	//how much the carousel's rotation show be offset.
	//</summary>
	public float zRotationOffset = 180f;


	/// <summary>
	/// Swipe types.
	/// </summary>
	public enum swipeTypes
	{
		fullScreen,swipeArea
	}

	/// <summary>
	/// The type of the swipe, if swipeArea please make sure you add a SwipeArea component to the UI.
	/// </summary>
	public swipeTypes swipeType = swipeTypes.fullScreen;

	public enum swipeDirections {Vertical, Horizontal};

	//<summary>
	//use to determine what direction of swiping will change the carousel.
	//</summary>
	public swipeDirections swipeDirection = swipeDirections.Horizontal;

	//<summary>
	//this will inverse the swiping direction, if true.
	//</summary>
	public bool invertSwipe = false;

	[HideInInspector]
	//<summary>
	//this is used to determine if the user is swiping, or not.
	//</summary>
	public bool swiping = false;


	[Tooltip ("this converts the swipe distance to the angle the carousel should turn")]
	//<summary>
	//this is a multiplier to change the distance swiped into an angle of how much the carousel should turn.
	//</summary>
	public float swipe2Angle = 1f;


	//private variables
	private float anglePart = 0f; //360 divided by the number of carousel Objects
	private int objectCount; //number of carousel Objects
	private List<GameObject> objectList = new List<GameObject>(); //List of all carousel Objects
	private bool swipeToProcess = false; //used to determine if a swipe has ended and needs to be processed
	private Vector2 swipeStartPos; //the location of where the swipe begain
	private Vector2 swipeEndPos; //the location of where the swipe ended


	// Use this for initialization
	void Awake () 
	{
		//used to set some variables and get a list of all the carousel Objects.
		RefreshList();

		//saves the RotationIndex in the PlayerPrefs
        if(saveRotationIndex)
        {
			sri = (PlayerPrefs.HasKey(saveKey))?PlayerPrefs.GetInt(saveKey):0;
        }
	}


	// Update is called once per frame
	void FixedUpdate () 
	{
		//refresh the list of carousel objects (if needed)
		RefreshList();

		//process swipes if needed
		SwipeManager();

		//rotate the carousel based on the rotationIndex
		Vector3 R = gameObject.transform.localRotation.eulerAngles;
		float swipeOffset = swiping? getSwipeOffset() :0f;
		float speed = swiping? rotateSpeed:snapSpeed;
		gameObject.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(R),Quaternion.Euler(R.x,R.y,zRotationOffset + ( anglePart * sri) + swipeOffset ), speed * Time.deltaTime); 

		//move objects in the carousel to their proper position
		if (lerpObjects)
		{
			int i = 0;
			foreach(GameObject Obj in objectList)
			{
				Vector3 NewPos = new Vector3(Mathf.Sin(anglePart * Mathf.Deg2Rad * i) * radius,Mathf.Cos(anglePart * Mathf.Deg2Rad  * i) * radius,0f);
				Obj.transform.localPosition = Vector3.Lerp (Obj.transform.localPosition, NewPos, objectSpeed * Time.deltaTime);
				i += 1;
			}
		}
		else
		{
			int i = 0;
			foreach(GameObject Obj in objectList)
			{
				Obj.transform.localPosition = new Vector3(Mathf.Sin(anglePart * Mathf.Deg2Rad * i) * radius,Mathf.Cos(anglePart * Mathf.Deg2Rad  * i) * radius,0f);
				i += 1;
			}
		}

		//set the rotationIndex
		rotationIndex = (int)(Mathf.RoundToInt( (transform.localRotation.eulerAngles.z -  zRotationOffset) / anglePart) );
	}

	/// <summary>
	/// Swipes the manager. (only when the swipeTypes is fullScreen)
	/// </summary>
	private void SwipeManager()
	{
		//don't process swipe if swipeType is not fullScreen
		if (swipeType != swipeTypes.fullScreen)
		{
			return;
		}

		//process swipe using mouse, if in OSXEditor or WindowsEditor
		//this can be adjusted if need be
		if (
			Application.platform == RuntimePlatform.OSXEditor
			||
			Application.platform == RuntimePlatform.WindowsEditor
		)
		{
			if (Input.GetMouseButtonDown(0))
			{				
				StartSwipe(Input.mousePosition);
			}

			if (Input.GetMouseButton(0))
			{				
				ContinueSwipe(Input.mousePosition);
//				print("hello");
			}

			if (Input.GetMouseButtonUp(0))
			{
				EndSwipe(Input.mousePosition);
			}
		}
		//if not in Editor process swipes using Touches 
		else 
		{
			Touch[] Touches = Input.touches;

			if (Touches.Length >= 1)
			{
				if (Touches[0].phase == TouchPhase.Began )
				{
					StartSwipe(Touches[0].position);
				}

				if (Touches[0].phase == TouchPhase.Moved 
					|| Touches[0].phase == TouchPhase.Stationary 
				)
				{
					ContinueSwipe(Touches[0].position);
				}

				if (Touches[0].phase == TouchPhase.Ended 
					|| Touches[0].phase == TouchPhase.Canceled 
				)
				{
					EndSwipe(Touches[0].position);
				}
			}

		}

		// Change rotationIndex based on swipe
		if (swipeToProcess)
		{
//			print ("+" + Mathf.RoundToInt( getSwipeOffset() / anglePart).ToString());
			sri += (int)( Mathf.RoundToInt( getSwipeOffset() / anglePart)  );
			swipeToProcess = false;
		}


	}


	/// <summary>
	/// Swipes the manager. (only when the swipeTypes is swipeArea)
	/// </summary>
	public enum SwipeStates {Start = 0,Continue = 1,End = 2}
	public void SwipeManager(SwipeStates swipeState, Vector2 inputPos)
	{

		if (swipeType != swipeTypes.swipeArea)
		{
			return;
		}

		if (swipeState == SwipeStates.Start)
		{
			StartSwipe(inputPos);
		}

		if (swipeState == SwipeStates.Continue)
		{
			ContinueSwipe(inputPos);
		}

		if (swipeState == SwipeStates.End)
		{
			EndSwipe(inputPos);
		}

		if (swipeToProcess)
		{
//			print ("+" + Mathf.RoundToInt( getSwipeOffset() / anglePart).ToString());
			sri += (int)( Mathf.RoundToInt( getSwipeOffset() / anglePart)  );
			swipeToProcess = false;
		}
	}


	/// <summary>
	/// used during the start of a swipe
	/// </summary>
	/// <param name="inputPos">Input position.</param>
	private void StartSwipe(Vector2 inputPos)
	{
		swipeStartPos =  inputPos; 
		swiping = true;

		swipeEndPos =  inputPos; 
	}

	/// <summary>
	/// used during the swipe
	/// </summary>
	/// <param name="inputPos">Input position.</param>
	private void ContinueSwipe(Vector2 inputPos)
	{
		swipeEndPos = inputPos; 
	}

	/// <summary>
	/// used at the End of a swipe
	/// </summary>
	/// <param name="inputPos">Input position.</param>
	private void EndSwipe(Vector2 inputPos)
	{
		swipeEndPos =  inputPos; 

		swipeToProcess = true;
		swiping = false;
	}

	/// <summary>
	/// used to calculate how much the swipe should turn the carousel
	/// </summary>
	/// <returns>The swipe offset.</returns>
	private float getSwipeOffset()
	{
		float invert = invertSwipe ? -1f: 1f;

		if (swipeDirection == swipeDirections.Horizontal)
		{
			return ((swipeEndPos.x - swipeStartPos.x) * swipe2Angle * invert);
		}
		else
		{
			return ((swipeEndPos.y - swipeStartPos.y) * swipe2Angle * invert);
		}
	}

	/// <summary>
	/// Gos to game object.
	/// </summary>
	/// <returns>The to game object.</returns>
	/// <param name="go">Go.</param>
	public bool? goToGameObject(GameObject go)
	{
		int index = 0;
		foreach (GameObject Obj in objectList)
		{
			if (Obj == go) 
			{
				if (sri == index) return true;
				sri = index;
				return false;
			}
			index++;
		}
		return null;
	}


	/// <summary>
	/// Changes the rotationIndex
	/// </summary>
	/// <param name="Delta">Delta.</param>
	public void ChangeRotationIndex(int Delta)
	{
		sri += Delta;
	}

	/// <summary>
	/// Adds to the rotationIndex
	/// </summary>
	public void AddRotationIndex()
	{
		sri += 1;
	}

	/// <summary>
	/// Subtracts to the rotationIndex
	/// </summary>
	public void SubtractRotationIndex()
	{
		sri -= 1;
	}


    /// <summary>
    /// returns the selected index
    /// </summary>
    /// <returns>The selected index.</returns>
    public int getSelectedIndex()
    {

		int result = rotationIndex  % objectCount;

        while (result < 0)
        {
            result += objectCount;
        }

        return result;

    }

    /// <summary>
	/// returns the selected object
    /// </summary>
    /// <returns>The selected object.</returns>
    public GameObject getSelectedObject()
    {
        return objectList[getSelectedIndex()];
    }

	/// <summary>
	/// used to refresh list of carousel Objects and other key variables
	/// </summary>
	public void RefreshList()
	{
		if (objectCount != gameObject.transform.childCount)
		{
			objectCount = gameObject.transform.childCount;

			anglePart = 360f/objectCount;

			objectList.Clear();

			foreach (Transform child  in transform)
			{
				objectList.Add (child.gameObject);
			}
		}
	}
}
