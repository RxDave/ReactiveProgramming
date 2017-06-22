## Chat Application Technical Specification

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
4. User preferences
5. User dialogs and input prompts

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
3. If the last user name was saved, pre-populate the user name textbox.
4. If the last service URL was saved, pre-populate the service URL textbox.
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
2. If any white-listed IP addresses are specified, verify they are valid URIs.
   1. If any IP is invalid, stop the service with an unhandled exception.
3. Listen for client connections.

##### Receiving Connections
1. When a client connection is received:
   1. If the client's IP address is not in the white-list:
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
