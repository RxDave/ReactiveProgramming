# Reactive Programming 101 (RP101)
It's sometimes difficult to convince business owners, project managers and other developers to use the reactive 
programming (RP) approach to software development. There are of course trade-offs to be made, although the benefits 
of reactive programming often outweigh the costs.

Perhaps many developers reject reactive programming because they're so used to object-oriented programming (OOP), 
and they'd rather stick to what they know best. If you don't know how to build a reactive program, or the benefits 
to be claimed by doing so, then choosing the OOP approach to software development makes perfect sense.

But don't be fooled into thinking that using reactive programming means that you won't be using OOP. To the contrary, 
you may find that you're still using more OOP than RP even when you take an RP-first approach. In fact, OOP assumes
a significant supporting role in any modern application that is built with RP at its core.

In this video series, instead of attempting to define "Reactive Programming", or throwing around buzz words, or presenting 
slides that list the benefits and omit the downsides, we're going to use the scientific approach.

## We're Going To Build an Application
Using a reactive-first approach, we'll design and implement a client-server chat program with the following features.

### Chat Application Features
#### Part I
1. Any number of clients may open a persistent connection to the chat server.
2. All messages go through the server, where they are multicast to all connected clients.
3. The service will not implement reliable messaging - it's entirely in-memory, without any fault tolerance.
4. No 3rd-party dependencies. Only the latest version of Rx.NET and any required dependencies of the target platform(s) will be used.
5. All features will be custom-coded, including network communication.

#### Part II
After the primary feature set is complete, we'll add some new features to test the maintainability of the code.

1. Display a list of users currently connected to the chat, including name and status.
2. Add Twitter integration to allow a user to optionally send and receive direct messages through Twitter's 
streaming API:\
   https://dev.twitter.com/webhooks/account-activity

#### Client Features, Part I
1. Choose any user name.
2. Enter a service URL and connect.
3. Disconnect at any time. (Closing the application also disconnects.)
4. Change status (available, busy, away, etc.) without disconnecting. "Busy" status will block all messages.
5. Filter messages by user name or search criteria. (RP201: Improved with [Qactive](https://github.com/RxDave/Qactive).)
6. Optionally have a sound played when a message arrives.
7. Choose a system sound or a custom sound file to play. (Default is C:\Windows\Media\Windows Notify.wav).
8. Change font, size and color at any time. (Only affects subsequent messages.)
9. Option to display date and/or time with each incoming message.
10. View entire chat history. (Stored locally in AppData.)

#### Client Features, Part II
1. Optionally, enter your Twitter handle when signing in.
   1. Receive all of your Twitter DMs as chat messages.
   2. Optionally, send any chat message to a specific user (currently in the chat) as a Twitter DM.

#### Service Features, Part I
1. Optionally whitelist IP addresses.
2. Enable private server mode (requires client authentication).

#### Service Features, Part II
1. Send a snapshot of the current list of users to any user that connects, followed by deltas.
2. Subscribe to Twitter's streaming API for all users that have specified a Twitter ID when signing in:\
   https://dev.twitter.com/webhooks/account-activity

## Chat Application Design

### Cross-cutting Concerns
1. Diagnostics
   1. Logging
   1. Instrumentation
2. Schedulers
   1. Concurrency
   1. UI-thread
3. Connectivity
   1. Service status (connected or disconnected)
   2. Presented status (available, away, busy, etc.)

### Diagnostic Tools
1. User emulator (robot)
2. In-proc service

### Specification: Part I
#### Client Design
##### Startup
1. Start the GUI in the following state: 
   1. Server URL field, user name field and Connect button are enabled.
   2. Chat GUI elements are disabled (including the message list and the Send button).
2. Load the user's settings.
3. If the last user name was saved, prepopulate the user name textbox.
4. If the last service URL was saved, prepopulate the service URL textbox.
5. If enabled, verify that the user's configured sound is available and it can be played.
   1. If it cannot, display a warning to the user.

##### Connecting to the Server
1. The user can set their "user name" before connecting.
   1. The user name from their last session is displayed by default.
2. The user must enter a service URL and click a Connect button.
   1. When the Connect button is pressed, and the user name is valid, and the service URL is a valid URI:
      1. Disable the Connect button.
      1. Display a waiting animation and a Cancel button.
      1. Try connecting to the specified URL.
      1. If the connection cannot be established, or if the service URL is invalid:
         1. Display any network, service or URL-validation errors.
         2. Focus the service URL and select it.
         3. Enable the Connect button. (goto #2)
      1. If the Cancel button is pressed:
         1. Cancel the connection attempt that is in-progress (best effort).
         2. If the cancellation is unsuccessful:
            1. Try disconnecting from the specified URL.
               1. If the program fails to disconnect gracefully, then log any errors and continue.
         3. Enable the Connect button. (goto #2)
      1. If the service requires authentication:
         1. Prompt the user to enter a password.
            1. If the server rejects the password:
               1. Enable the Connect button. (goto #2)
      1. Otherwise, change the Connect button into a Disconnect button and enable it.
         1. Enable the chat GUI so that messages may be sent and received.
         2. Save the user name and service URL in the user's settings.
   1. When the Disconnect button is pressed:
      1. Disable it.
      1. Try disconnecting from the specified URL.
         1. If the program fails to disconnect gracefully, then log any errors and continue.
      1. Change the Disconnect button into a Connect button and enable it. (goto #2)
         1. Disable the chat GUI to prevent messages from being sent or received.

##### Receiving Messages
1. When a message is received:
   1. If the user's status is not "busy" and the message matches the user's current filter criteria (if any):
      1. Append the message to the message list.
         1. Include the date and/or time based on the user's current settings.
         1. Render the message in the font, size and color received with the message. If none received, use defaults.
      1. If the option to play a sound is enabled, and the chat window is not currently in focus:
         1. Play the user's configured sound.

##### Sending Messages
1. When the 'Enter' key is pressed or the 'Send' button is clicked:
   1. If the message is not entirely whitespace:
      1. Send the message to the server.
         1. Include the current font, size and color with the message. Omit them if they are currently the defaults.

#### Server Design
##### Startup and Configuration
1. Load the server settings.
2. If any whitelisted IP addresses are specified, verify they are valid URIs.
   1. If any IP is invalid, stop the service with an unhandled exception.
3. Listen for client connections.

##### Receiving Connections
1. When a client connection is received:
   1. If the client's IP address is not in the whitelist:
      1. Terminate the connection immediately, without any response. (end)
   1. If authentication is enabled:
      1. Send a password request message to the client.
         1. If sending fails, then terminate the connection immediately. (end)
      2. await a password response message.
      3. If the password does not match or a password is not received within 2 minutes:
         1. Terminate the connection immediately, without any response. (end)
   1. Otherwise, add the client to the subscription list.
   1. Multicast a message: "User NAME has entered the chat.".
   1. Listen for messages.

##### Disconnecting (graceful or otherwise)
1. When a client disconnects or is later determined to be in a disconnected state:
   1. Remove the client from the subscription list.

##### Multicasting
1. When a message is received:
   1. Send the message to each client in the subscription list, including the original sender.
      1. If sending fails, then terminate the connection immediately. (end)


### Specification: Part II
#### Client Design
##### Connecting to the Server
##### Receiving Messages
##### Sending Messages

#### Server Design
##### Receiving Connections
##### Disconnecting (graceful or otherwise)
##### Multicasting
