
# Xylophone Song App 

This app was originally made for a company's coding challenge interview in ~4 hours, then another ~1 hour of clean-up afterwards. The mockup is included in the ui sketch.jpg within the ASSIGNMENT folder and the given requirements are below.


## Requirements



This is a relatively small programming assignment for Unity;
 please don't spend more than four hours on it, it's fine if there are rough edges or incomplete parts.


 The task is to write a Unity app which allows the user to compose a simple five-second song.

The deliverable should be a Unity Project which loads and runs in the most recent stable version of Unity IDE.




### Main application

The app UI should have three major parts:
 (See 'ui sketch.jpg')


- a musical keyboard with eight keys.


- a "staff" or "notes" area, where recorded notes are displayed & played back.


- a "filters" area with some options for applying playback filters/effects.





### The UX should be:



- The musical keyboard represents eight xylophone notes.

- As soon as the user taps a key in the musical keyboard,
  the app begins recording the notes they tap,
  and the notes appear in the "Notes" area of the app.

- Notes should be positioned horizontally according to their time in the song,
  and vertically according to their pitch.
  (eg, the lowest-pitched musical note is also lowest on the screen).

- Notes should audibly play during recording.

- When the user taps the Play button,
  the notes they recorded are played back.


- There should be a visual indication of the current playhead position.

- By default, notes are played back w/ the exact timing of when they were recorded,
  but see the extra-credit "Quatization" filter below.

- The song length should be limited to five seconds.


- There should be a button to erase the current song.


- There should be a section for Filters:
  
- Echo:
    Please include a toggle button for "Echo".
    when Echo is on, when a note is played it should be followed by two or more echoes.
    Each echo should play with 250ms of delay, and attenuated by 25% volume.
   PLEASE DO NOT USE any Unity Audio filters to implement this effect.

  
- Reverse:
    Please include a toggle button for "Reverse".

    when Reverse is on the notes should play in backwards order.

  

### Extra-Credit: 
* Quantization:
    Please include a set of radio buttons for "Quantization" with these options:
    "1000ms", "250ms", "125ms", "None".
    
"None" is the default, and with that selected the notes play back with the exact timing they were recorded with.
 	For the numeric values, each note should be played back "snapped" to the nearest increment of that many milliseconds.
    So for "125", each note should be played back at the nearest 1/8th of a second. 


### Art / Assets




To let you focus on coding and not on assets, we've included a folder of assets. 
If you find that you need an asset we haven't provided, 
please spend as little time as possible producing the asset.
- "programmer art" is fine. 

The included files are:
assets/images/keyboard     
- 9 buttons for the musical keyboard
assets/images/notes       
- 9 different musical notes.
assets/images/misc buttons - buttons for play, stop, and erase.
assets/sounds/xylo         
- 9 samples for the xylophone.
ui sketch.jpg              
- a rough sketch of how the UI might look
assignment.txt             
- this file.


