using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Threading.Tasks;
using System;
using System.Threading;

public class MQTTNetClient : MonoBehaviour
{
    private IMqttClient mqttClient;
    private MqttFactory mqttFactory;

    public bool isAutoReconnect = true;
    public double reconnectPeriod = 5;

    // Start is called before the first frame update
    void Start()
    {
        InitClient();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Initialize mqtt client
    /// </summary>
    private async void InitClient()
    {
        //create client instance
        mqttFactory = new MqttFactory();
        mqttClient = mqttFactory.CreateMqttClient();

        //set client options
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(MQTTClientConfig.ServerUrl, MQTTClientConfig.Port)
            .WithCredentials(MQTTClientConfig.UserName, MQTTClientConfig.Password)
            .Build();

        Debug.Log("CONNECTING SERVER");

        //connected processing
        mqttClient.UseConnectedHandler(async e =>
        {
            Debug.Log("CONNECTED SERVER");
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic(MQTTClientConfig.SubscribeTopic)
                .Build());
            Debug.Log($"SUBSCRIBED: {MQTTClientConfig.SubscribeTopic}");

            Publish(MQTTClientConfig.PublishTopic, "Publishing from MQTTNetClient after subscribed");
        });

        //disconnected processing
        mqttClient.UseDisconnectedHandler(async e =>
        {
            Debug.Log($"DISCONNECT FROM SERVER: {e.Exception.Message.ToString()}, {e.ReasonCode}");

            if (isAutoReconnect)
            {
                //
                await Task.Delay(TimeSpan.FromSeconds(reconnectPeriod));
                if (null != mqttClient)
                {
                    try
                    {
                        Debug.Log($"AUTO RECONNECT: {e.Exception.Message.ToString()}, {e.ReasonCode}");
                        await mqttClient.ConnectAsync(options, CancellationToken.None);
                    }
                    catch
                    {
                        Debug.Log($"RECONNECT FAILED: {e.Exception.Message.ToString()}, {e.ReasonCode}");
                    }
                }
            }
        });

        //received msg processing
        mqttClient.UseApplicationMessageReceivedHandler(e =>
        {
            Debug.Log($"RECEIVE MSG:\n " +
                $"Topic:{e.ApplicationMessage.Topic}, " +
                $"Payload: {System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}, " +
                $"Qos: {e.ApplicationMessage.QualityOfServiceLevel}, " +
                $"Retain: {e.ApplicationMessage.Retain}");
        });

        //start to connect mqtt broker
        if (null != mqttClient)
        {
            try
            {
                await mqttClient.ConnectAsync(options, CancellationToken.None);
            }
            catch (Exception e)
            {
                Debug.Log("Init ConnectServer Fail: " + e.Message);
            }
        }
    }

    /// <summary>
    /// subscripts topic
    /// </summary>
    /// <param name="topic"></param>
    private async void SubScribe(string topic)
    {
        var options = new MqttTopicFilterBuilder().WithTopic(topic).Build();
        await mqttClient.SubscribeAsync(options);
    }

    /// <summary>
    /// publishs topic
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="payload"></param>
    private async void Publish(string topic, string payload)
    {
        Debug.Log($"Publish Payload: {payload} to Topic: {topic}");
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithExactlyOnceQoS()
            .WithRetainFlag(MQTTClientConfig.Retained)
            .Build();

        await mqttClient.PublishAsync(message, CancellationToken.None);
    }

    private void OnDestroy()
    {
        isAutoReconnect = false;
        DisconnectToServer();
    }

    public void DisconnectToServer()
    {
        if (null != mqttClient)
        {
            mqttClient.DisconnectAsync();
            Debug.Log("Force Disconnect");
            mqttClient = null;
        }
    }
}
