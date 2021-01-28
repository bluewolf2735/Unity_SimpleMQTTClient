using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MQTTClientConfig
{
    /// <summary>
    /// Broker address
    /// </summary>
    public static string ServerUrl = "test.mosquitto.org";
    
    /// <summary>
    /// Broker connect Port
    /// </summary>
    public static int Port = 1883;

    /// <summary>
    /// user name
    /// </summary>
    public static string UserName = "Zach";

    /// <summary>
    /// password
    /// </summary>
    public static string Password = "testmqtt";

    /// <summary>
    /// subscribing topic
    /// </summary>
    public static string SubscribeTopic = "MQTTNet_Unity/test";

    /// <summary>
    /// publishing topic
    /// </summary>
    public static string PublishTopic = "MQTTNet_Unity/test";

    /// <summary>
    /// need retained
    /// </summary>
    public static bool Retained = false;

    /// <summary>
    /// Quality of service
    /// </summary>
    public static int QoS = 0;
}
