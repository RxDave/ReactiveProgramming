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
   
#### Part III
Unit testing reactive code is often easier when using a virtual time scheduler and/or reified notifications, which I'll introduce in this part of the series. By now we will probably have created some custom operators, so we'll test those; otherwise, I'll probably choose a small reactive query from the business layer to test.

#### Part IV
Finally, we'll measure performance and memory usage, and adjust our code to meet our goals. The app may already perform acceptably well by this point; in that case, I'll just have to pick some arbitrary goals and attempt to satisfy them, for didactic purposes only.

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
1. Optionally white-list IP addresses.
2. Enable private server mode (requires client authentication).

#### Service Features, Part II
1. Send a snapshot of the current list of users to any user that connects, followed by deltas.
2. Subscribe to Twitter's streaming API for all users that have specified a Twitter ID when signing in:\
   https://dev.twitter.com/webhooks/account-activity