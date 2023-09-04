# Deputised Object

## Unity and Arduino project

This project uses and esp32 thing plus with a BNo055 accelerometer attached. The Arduino creates a hotspot and broadcasts the rotation data using OSC.

The Unity project listens for this data and uses it to rotate an object on screen with a delay or smoothing

the OSC messages from the arduino use port 7000 so make sure its not blocked by the firewall.


NOTE:

for origianal 3d printed object, change object rotation (GameObject) to x 180, y -90 z-175

