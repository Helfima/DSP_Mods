using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    public Rect windowRect0 = new Rect(20, 20, 120, 50);
    public Rect windowRect1 = new Rect(20, 100, 120, 50);

    public void OnGUI()
    {
        // Here we make 2 windows. We set the GUI.color value to something before each.
        GUI.color = Color.red;
        windowRect0 = GUI.Window(0, windowRect0, DoMyWindow, "Red Window");

        GUI.color = Color.green;
        windowRect1 = GUI.Window(1, windowRect1, DoMyWindow, "Green Window");
    }

    // Make the contents of the window.
    // The value of GUI.color is set to what it was when the window
    // was created in the code above.
    void DoMyWindow(int windowID)
    {
        if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
        {
            print("Got a click in window with color " + GUI.color);
        }

        // Make the windows be draggable.
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
}