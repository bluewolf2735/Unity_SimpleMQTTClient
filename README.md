# Unity_SimpleMQTTClient
A simple MQTT client that include basic Subscribe, Publish and Reconnect functions. And can also deploy to HoloLens2.

![image](https://github.com/bluewolf2735/Hololens2_GetHandJointPositions/blob/master/GetHandJointPositions.gif)

## Requirements

- Visual Studio 2019
- Unity 2019.3.5f1
- Windows 10.0.18362.0 SDK
- [MQTTnet v3.0.13](https://github.com/chkr1011/MQTTnet/releases)


## Usage

Client will try to connect to [Eclipse Mosquitto MQTT server/broker](https://test.mosquitto.org/) automatically when app is launched and publish test payload to topic "MQTTNet_Unity/test" . 

## License

[MIT © Richard McRichface.](../LICENSE)
