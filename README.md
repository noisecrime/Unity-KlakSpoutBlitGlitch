# Klak Spout Blit Bug
[Keijiro's **Klak.Spout** package](https://github.com/keijiro/KlakSpout) for **Unity** has an odd glitch that will place the active renderTexture into the Spout output when using the 'GameView' CaptureMethod. This glitch appears to be susceptible to either;
 - the state of renderTexture.active ( i.e not being null )
 - instantiating the SproutSender component into a scene at runtime.

This issue has been reported [here](https://github.com/keijiro/KlakSpout/issues/93)


![Project Splash Image](/Media/GlitchKlakSpoutBlit.gif)

## The Problem ('s)
If you always embed the SpoutSender component in a scene ( i.e on a gameObject ) then you'll unlikely to ever experience the glitch, regardless of what state renderTexture.active is when Klak.Spout comes to capture the game view. 

However should you instantiate the SpoutSender component into a scene at runtime, for example by having it within a prefab that you instantiate based on a setting/flag in the project, then you will see this glitch, but  only if you have left renderTexture.active in a none 'null' state ( e.g. by doing any Graphics.Blit functions ) by the time  Klak.Spout comes to capture the game view.

At this point in time I'm unable to answer why this should be the case. I need to look into it further. 

## The Real Issue ( Maybe )
The issue appears to be related to Klak.Spout using Unity's ScreenCapture.CaptureScreenshotIntoRenderTexture method, which in turn appears to assume nothing about the active renderTexture and thus when is not null will capture whatever the active renderTexture contents are!
Its probably not a terrible implementation, as being able to capture the active renderTexture could have its uses, but it is badly denoted as  'Captures a screenshot of the game view into a RenderTexture object'.
However as shown its not purely just this method at fault, as you have to also instantiate the SpoutSender at runtime to cause the problem, so something else is going on here too.

## Fix the Glitch
Thankfully its relatively simple to fix this glitch in your project, though deciding which method to use is a bit tricky.

**1. Always embed the SpoutSender component.**
The simplest and quickest solution, though it may just be hiding the problem as we aren't directly addressing the issue with renderTexture.active not being null. 
The bigger issue with this solution is you may really want to instantiate Spout at runtime based on user perference of the project. For example your project may not need to output to Spout, but you want the option to do so.

**2. Wrap all your calls to Graphics.Blit to restore previous active renderTexture.**
Maybe the worse solution as its quite painful to do if you have many of these calls in your project and you're limited to your own project code. third party code will still be a problem! 
Worse its technically not solving the issue as its falling into the same trap that appears to cause the problem in the first place, namely that if rendereTexture.active is not null when Klak.Spout captures the game view it will capture whatever the active renderTexture is at that time! Alternatively you could set the active renderTexture to null after using Graphics.Blit, but that also seems counter-intuitive and error prone.

**3. Apply a fix to Klak.Spout**
Probably the cleanest solution at this time is to simply add some code to Klak.Spout to force the active renderTexture to be null prior to the call to Unity's method for Capturing the screen. 
Alas this means embedding Klak.Spout package into your project and losing the ability to update it from the Package Manager. Though there is a chance this fix will be implemented in Klak.Spout at some point, but that assumes such a fix does not cause any other problems.
This fix can be found in the source here by searching for '**KLAK_SPOUT_RT_FIX**'. By adding this define to your project you'll enforce use of the fix.


## Purpose of this Project
Purpose of this project is as a simple demonstration of the issue and the potential fixes.

To test
-   Open Unity project.
-   Open the scene.
-   Open a Spout viewer ( e.g. Resolume or OBS ) assign 'Sprout Sender' as the spout input name.
-   Run Unity - Observe flashing glitch.

GameObjects

-   Spout Launcher will instantiate the Spout prefab if none is found in the scene.
-   Blitter - will randomly perform a blit.  
    ------ Frequency of blits is controlled by the component via slider.  
    ------ Toggle button allows you to enable/disable wrapping the blit call with renderTexture.active.

Default is for Sprout Launcher to add the SpoutSender to the scene via instantiate, which is the main cause of the glitch.
To test with a SpoutSender embedded in the scene, just drag the Spout prefab into the scene and save.

Results  
SpoutSender - SCENE | Wrap Blit calls - DISABLED | Klak Spout Fix - DISABLED = No Glitch  
SpoutSender - SCENE | Wrap Blit calls - ENABLED | Klak Spout Fix - DISABLED = No Glitch  
SpoutSender - SCENE | Wrap Blit calls - DISABLED | Klak Spout Fix - ENABLED = No Glitch

SpoutSender - PREFAB | Wrap Blit calls - DISABLED | Klak Spout Fix - DISABLED = Glitch  
SpoutSender - PREFAB | Wrap Blit calls - ENABLED | Klak Spout Fix - DISABLED = No Glitch  
SpoutSender - PREFAB | Wrap Blit calls - DISABLED | Klak Spout Fix - ENABLED = No Glitch



## System Requirements
Unity 2021.2.8f1

## Acknowledgements
Keijiro for so many amazing and useful packages for Unity.

## License
The Unlicense