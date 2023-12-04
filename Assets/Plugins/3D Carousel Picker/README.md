3D Carousel Picker
-------------------------------------
[Asset Store Link](http://u3d.as/m57)  
© 2017 Justin Garza

PLEASE LEAVE A REVIEW OR RATE THE PACKAGE IF YOU FIND IT USEFUL!
Enjoy! :)

Contact  
-------------------------------------
Questions, suggestions, help needed?  
Contact me at:  
Email: jgarza9788@gmail.com  
Cell: 1-818-251-0647  
Contact Info: [justingarza.net/contact](http://justingarza.net/contact/)
  
Description/Features
-------------------------------------
Arranges objects is an carousel so the user can choose between them.* Add and remove objects to carousel, easily.
* Use Swipe,Buttons, or a part of the screen to rotate carousel.
Terms of Use
-------------------------------------
You are free to add this asset to any game you’d like
However:  
please put my name in the credits, or in the special thanks section. :)  
please do not re-distribute.  

Table of Contents 
-------------------------------------
1. CarouselPicker.cs
2. swipeArea.cs
3. RotateCarouselPicker.cs
3. getSelected.cs
  
CarouselPicker.cs
-------------------------------------
This carousel is just one GameObject with 
the CarouselPicker.cs script
Children GameObjects (this will be the objects that make up the carousel)
Note: depending on how you use this the Children might need components of their own.

![Imgur](http://i.imgur.com/FdDiVKu.png)

###Variables###

**saveRotationIndex:**  
whether or not we should save the carousel's rotation in the PlayerPrefs

**saveKey:**   
The key used for saving the rotation in PlayerPrefs

**radius:**   
Radius of the Carousel

**rotateSpeed:**   
Speed at switch the Carousel will rotate during swiping

**snapSpeed:**   
Speed at switch the Carousel will rotate after swiping

**lerpObjects:**   
Weather or not objects will lerp into position or not

**objectSpeed:**
the speed in which objects move (if lerpObjects is true)

**rotationIndex:**
this number is used to how much the carousel should rotate 

**zRotationOffset:**
how much the carousel's rotation show be offset.

**swipeType:**
fullScreen would allow the user to swipe anywhere on the screen, swipeArea would mean a swipeArea is needed to swipe this carousel.

**swipeDirection:**
use to determine what direction of swiping will change the carousel.

**invertSwipe:**
this will inverse the swiping direction, if true.

**swiping:**
this is used to determine if the user is swiping, or not.

**swipe2Angle:**
this is a multiplier to change the distance swiped into an angle of how much the carousel should turn.


swipeArea.cs
-------------------------------------

This script can be attached to a canvas object to create a Swipe Area, or TouchZone, that can override the carousel.

Make sure to a carousel to the CarouselPicker variable.
The LeftSwipeArea is used to determine of the user’s touch went outside of the area.

Note: please make sure the swipeType is set to swipeArea, in the CarouselPicker.cs

![Imgur](http://i.imgur.com/rkmCjqs.png)

RotateCarouselPicker.cs 
-------------------------------------

This is a pretty simple script that’s just used to rotate the Carousel.
In the Demo i have it attached to a button, but it can be assigned to anything.

![Imgur](http://i.imgur.com/jj4XukC.png)

getSelected.cs 
-------------------------------------
The "SelectSphere.cs" is no longer needed.
The selected object can now be obtained by using the following code:

~~~cs
GameObject selectedGameObject = carouselPicker.getSelectedObject();
~~~

Or you can also set up a delegate to execute everytime the selected object changes. (see script for more information).

![Imgur](http://i.imgur.com/zJW4u1i.png)