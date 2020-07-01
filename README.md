# SwitchboardClient

A Demonstration of the SwitchboardClient Class through a small program called the Switchboard Console. It lets you connect to a Switchboard server, and execute commands on it.

Not mentioned in the help is the option to read the stream if there is any data by using the command `READ`.
Also, if the server connection ever hangs, you can break out of a read attempt by using the <kbd>Esc</kbd> key.

See the Server Here: https://github.com/igtampe/SwitchboardServer

Below is a little bit more about the SwitchboardClient itself:

## Still simple
Switchboard aims to provide an improved connection response-time, and expanded capabilities for sending/receiving data, with the same level of simplicity to SmokeSignal.
While we can no longer offer the `ServerCommand()` function, the process of connecting to the server is still relatively simple. It is as folows:

1. Create a SwitchboardClient object with IP and Port
2. Connect to the server using `Connect()`.
3. Use `SendReceive(Data)` as a ServerCommand Equivalent.

Once you're done, simply close the connection using `close()`

## Flexible
Along with usign `SendReceive()`, Switchboard also lets you only `send()` or `receive()` data using the appropriate functions. This opens the door to other possibilities, since using this, a client can act as a pseudo-server, allowing true two way communication.

## Faster
Compared to SmokeSignal, a Switchboard Client should be a lot faster when dealing with repeated requests. Instead of having to open a connection for each request, a single connection can be maintained for the duration of the burst of requests. 
