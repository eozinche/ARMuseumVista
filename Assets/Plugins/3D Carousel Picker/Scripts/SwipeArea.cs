/*
This script will allow you to control the carousel via a swipe area on the screen
*/

using UnityEngine;
using System.Collections;

public class SwipeArea : MonoBehaviour {

	/// <summary>
	/// the carousel that will be changed by this script
	/// </summary>
	public CarouselPicker carouselPicker; 

	/// <summary>
	/// weather or not the user's touch left the swipe area, or TouchZone.
	/// </summary>
	public bool LeftSwipeArea = false; 

	private float mHorzStartpos = 0;
	private bool mSlidingStarted = false;

	/// <summary>
	/// The minium distance to swipe before processings
	/// </summary>
	public int mSwipStartDistance = 20;

	// Update is called once per frame
	void Update () 
	{
		//		print (Input.mousePosition.ToString() + ":" + IsOnArea(Input.mousePosition).ToString());

		if (carouselPicker == null)
		{
			Debug.LogError("carouselPicker variable is null");
			return;
		}

		//process swipe using mouse, if in OSXEditor or WindowsEditor
		//this can be adjusted if need be
		if (
			Application.platform == RuntimePlatform.OSXEditor
			||
			Application.platform == RuntimePlatform.WindowsEditor
			||
			Application.platform == RuntimePlatform.WebGLPlayer
		)
		{

			if (IsOnArea(Input.mousePosition) && !LeftSwipeArea)
			{
				//Debug.Log("pos: " + Input.mousePosition);
				if (Input.GetMouseButtonDown(0))
				{
					mHorzStartpos = Input.mousePosition.x;
				}

				if (Input.GetMouseButton(0) && mHorzStartpos != 0 && (Mathf.Abs(mHorzStartpos - Input.mousePosition.x) > mSwipStartDistance || mSlidingStarted))
				{
					if (!mSlidingStarted)
					{
						carouselPicker.SwipeManager(CarouselPicker.SwipeStates.Start, Input.mousePosition);
					}
					mSlidingStarted = true;
					carouselPicker.SwipeManager(CarouselPicker.SwipeStates.Continue,Input.mousePosition);
				}

				if (Input.GetMouseButtonUp(0))
				{
					if (Mathf.Abs(mHorzStartpos - Input.mousePosition.x) < mSwipStartDistance)
					{
						Debug.Log("Click: " + Input.touchCount);
						// check object that is hit
						RaycastHit hit = new RaycastHit();

						// Construct a ray from the current touch coordinates
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						if (Physics.Raycast(ray, out hit))
						{
							GameObject go = hit.transform.gameObject;
							if (carouselPicker.goToGameObject(go) == true)
							{
								// select
								Debug.Log("Select");
							}
							return;
						}

						return;
					}
					mSlidingStarted = false;
					mHorzStartpos=0;
					carouselPicker.SwipeManager(CarouselPicker.SwipeStates.End,Input.mousePosition);
				}
			}
			else
			{
				if (carouselPicker.swiping)
				{
					carouselPicker.SwipeManager(CarouselPicker.SwipeStates.End,Input.mousePosition);
				}
				mSlidingStarted = false;
				mHorzStartpos = 0;
				LeftSwipeArea = true;
			}


			if (!Input.GetMouseButton(0))
			{
				LeftSwipeArea = false;
			}
		}
		//if not in Editor process swipes using Touches 
		else
		{
			Touch[] Touches = Input.touches;

			if (Touches.Length >= 1)
			{
				if (IsOnArea(Touches[0].position) && !LeftSwipeArea)
				{
					if (Touches[0].phase == TouchPhase.Began )
					{
						carouselPicker.SwipeManager(CarouselPicker.SwipeStates.Start,Touches[0].position);
						mHorzStartpos = Input.mousePosition.x;
					}

					if ((Touches[0].phase == TouchPhase.Moved
						|| Touches[0].phase == TouchPhase.Stationary) && mHorzStartpos != 0 && (Mathf.Abs(mHorzStartpos - Input.mousePosition.x) > mSwipStartDistance || mSlidingStarted))
					{
						if (!mSlidingStarted)
						{
							carouselPicker.SwipeManager(CarouselPicker.SwipeStates.Start, Input.mousePosition);
						}
						mSlidingStarted = true;
						carouselPicker.SwipeManager(CarouselPicker.SwipeStates.Continue, Input.mousePosition);
					}

					if (Touches[0].phase == TouchPhase.Ended 
						|| Touches[0].phase == TouchPhase.Canceled 
					)
					{
						if (Mathf.Abs(mHorzStartpos - Input.mousePosition.x) < mSwipStartDistance)
						{
							Debug.Log("Click: " + Input.touchCount);
							// check object that is hit
							RaycastHit hit = new RaycastHit();

							// Construct a ray from the current touch coordinates
							Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
							if (Physics.Raycast(ray, out hit))
							{
								GameObject go = hit.transform.gameObject;
								if (carouselPicker.goToGameObject(go) == true)
								{
									// select
									Debug.Log("Select");
								}
								return;
							}

							return;
						}
						mSlidingStarted = false;
						mHorzStartpos = 0;
						carouselPicker.SwipeManager(CarouselPicker.SwipeStates.End, Input.mousePosition);
					}
				}
				else
				{
					if (carouselPicker.swiping)
					{
						carouselPicker.SwipeManager(CarouselPicker.SwipeStates.End,Touches[0].position);
					}
					mSlidingStarted = false;
					mHorzStartpos = 0;
					LeftSwipeArea = true;
				}

				if (Touches[0].phase == TouchPhase.Ended 
					|| Touches[0].phase == TouchPhase.Canceled 
				)
				{
					mSlidingStarted = false;
					mHorzStartpos = 0;
					LeftSwipeArea = false;
				}
			}
		}
	}


	//used to determine if the Touch is within the swipe area, TouchZone.
	public bool IsOnArea(Vector3 ScreenPoint)
	{
		//this is a little adjustment in case the canvas is scaled
		Vector2 RealSize = new Vector2(gameObject.GetComponent<RectTransform>().rect.width,gameObject.GetComponent<RectTransform>().rect.height);
		RealSize = RealSize * GameObject.Find("Canvas").transform.localScale.x;

		////	Used for Debugging
		//		print ("RightBound: " + RealSize.x/2f.ToString() );
		//		print ("LeftBound: " + RealSize.x/2f.ToString() );
		//		print ("UpperBound: " + RealSize.y/2f.ToString() );
		//		print ("LowerBound: " + RealSize.y/2f.ToString() );

		//if the point is within the bounds of the button
		if (
			gameObject.transform.position.x + (RealSize.x/2f) >= ScreenPoint.x
			&& gameObject.transform.position.x - (RealSize.x/2f)  <= ScreenPoint.x
			&& gameObject.transform.position.y + (RealSize.y/2f)  >= ScreenPoint.y
			&& gameObject.transform.position.y - (RealSize.y/2f)  <= ScreenPoint.y
		)
		{
			return true; //return true
		}
		else
		{
			return false; //if not return false
		}
	}
}
