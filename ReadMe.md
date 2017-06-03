# Reactive Programming 101 (RP101)
It's sometimes difficult to convince business owners, project managers and other developers to use the reactive 
programming (RP) approach to software development. There are of course trade-offs to be made, although the benefits 
of reactive programming often outweight the costs.

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
1. Any number of clients may open a persistent connection to the chat server.
2. All messages go through the server, where they are multicast to all connected clients.
3. The service will not implement reliable messaging - it's entirely in-memory, without any fault tolerance.
4. No 3rd-party dependencies. Only the latest version of Rx.NET and any required dependencies of the target platform(s). All features 
   will be custom-coded, including network communication.

#### Client Features
1. Choose any user name.
2. Enter a service URL and connect.
3. Disconnect at any time. (Closing the appliction also disconnects.)
4. Change status (available, busy, away, etc.) without disconnecting. "Busy" status will block all messages.
5. Filter messages by user name or search criteria. (RP201: Improved with [Qactive](https://github.com/RxDave/Qactive).)
6. Optionally have a sound played when a message arrives.
7. Choose a system sound or a custom sound file to play. (Default is C:\Windows\Media\Windows Notify.wav).
8. Change font, size and color at any time. (Only affects subsequent messages.)
9. Option to display date and/or time with each incoming message.
10. View entire chat history. (Stored locally in AppData.)

#### Service Features
1. Blacklist IP addresses.
2. Enable private server mode (requires client authentication).

## Chat Application Design

#### Cross-cutting Concerns
1. Diagnostics
   i. Logging
   i. Instrumentation
2. Schedulers
   i. Concurrency
   i. UI-thread
3. Connectivity
   i. Actual status (connected or disconnected)

#### Diagnostic Tools
1. User emulator (robot)
2. In-proc service

#### Client Design
##### Connecting to the Server
1. The user can set their "user name" before connecting.
   i. The user name from their last session is displayed by default.
2. The user must enter a service URL and click a Connect button.
   i. When the Connect button is pressed:
      i. Disable it.
      i. Display a waiting animation and a Cancel button.
      i. Try connecting to the specified URL.
      i. If the connection cannot be established, or if the service URL is invalid:
         1. Display any network, service or URL-validation errors.
         2. Focus the service URL and select it.
         3. Enable the Connect button. (goto #2)
      i. If the Cancel button is pressed:
         1. Cancel the connection attempt that is in-progress (best effort).
         2. If the cancellation is unsuccessful:
            i. Try disconnecting from the specified URL.
               i. If the program fails to disconnect gracefully, then log any errors and continue.
         3. Enable the Connect button. (goto #2)
      i. If the service requires authentication:
         1. Prompt the user to enter a password.
            1. If the server rejects the password:
               i. Enable the Connect button. (goto #2)
      i. Otherwise, change the Connect button into a Disconnect button and enable it.
   i. When the Disconnect button is pressed:
      i. Disable it.
      i. Try disconnecting from the specified URL.
         1. If the program fails to disconnect gracefully, then log any errors and continue.
      i. Change the Disconnect button into a Connect button and enable it. (goto #2)

##### Receiving Messages
1. TODO

##### Sending Messages
1. TODO

#### Server Design

##### Startup and Configuration
1. TODO

##### Receiving Messages
1. TODO

##### Multicasting Messages
1. TODO