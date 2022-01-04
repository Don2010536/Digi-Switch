<h1 align="center">
  DigiSwitch: Home
</h1>

<p align="center">
  <img width="640" height="480" src="https://imgur.com/fbPiekp.png">
</p>

</br>
</br>

<p align="center">
  Here you can add new websites and applications to swap between at set intravals. I use this to primarily swap between chat and monitoring applications.
</p>

<h1 align="center">
  Adding an Application
</h1>

1. Click the "+" button
   - A new line will be added to the list and the start button will disappear replaced with a Save and Cancel button
2. Click on "Click Here..." and input the process name or url
3. Set your intraval (in seconds) that you want the app to remain on the screen
4. Check off fullscreen to force an app fullscreen if possible

</br>
</br>

<p align="center">
  <img width="640" height="119" src="https://i.imgur.com/2YDKfVL.png">
</p>

<h1 align="center">
  Using cookies
</h1>

<p align="center">
  This application posses the ability to submit a supplied cookie to a given website to attempt to bypass logins
</p>

1. Click the cookie checkbox
   - A new line will appear below the item checked
2. In the new line paste the cookie

</br>
</br>

<p align="center">
  <img width="640" height="73" src="https://i.imgur.com/b0tCSCS.png">
</p>

<h1 align="center">
  Getting process names and IDs
</h1>

1. Open Task Manager
2. Fine the application you want to use under the processes tab
3. Click the arrow to the app name
4. Find child you want to use
   - This can take some trial and error
5. Right Click the child and select go to details
   - If the go to details option is not available then go to the details tab and hunt for the procces there
6. That will highlight an item in the details tab
   - The name ending in exe is the item to put in the URL section
   - The PID highlighted will be the item you select in the combobox that appears
     - If you reboot or close and reopen the app the the PID will change but the combobox will refresh periodically with any PID associated with the processes name

<h1 align="center">Things to note</h1>

- Intravals are the ammount of time the application is on screen
  - The intraval is set in seconds
- If you input a URL a dedicated browser will open to that page
- If  the text input for an item contains ".exe" then it is assumed this is an application and not a website
- A PID must be selected for the application if one is input. See "Adding an Application"
- If a website doesn't open please verify the URL is fully typed with not mistakes

</br>
</br>
</br>

<h1 align="center">
  DigiSwitch: Settings
</h1>

<p align="center">
  <img width="640" height="480" src="https://i.imgur.com/R5J9lT9.png">
</p>

1. Colors
   - Colors can be set using hex values
   - Once set click Save and the colors will be permanant
   - If you mess up the reset button will set all colors back to default
2. Settings
   - Always on top forces the app to always appear on top
   - Intended use was to monitor as app switch using a secondary display
3. Log
   - Populates as windows switch but history never goes back further than 50 lines to keep things simple and decrease memory usage

<br>
<br>
<br>

<h1 align="center">
  DigiSwitch: About
</h1>

<p align="center">
  <img width="640" height="480" src="https://i.imgur.com/JzzgxbG.png">
</p>

<p align="center">
  This section provides some ideas for use case and one minor troubleshooting point that some windows require the app to run as administrator to manipulate
</p>
