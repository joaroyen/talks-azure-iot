#include <ArduinoJson.h>

#include "IoT_DevKit_HW.h"
#include "AZ3166WiFi.h"
#include "DevKitMQTTClient.h"
#include "RGB_LED.h"

#define RGB_LED_BRIGHTNESS 32
#define SCREEN_COLUMNS 16
static const char ThisDevice[] = "MxChip";
static const char DeviceMethodName[] = "CounterUpdated";
static const int CounterUpdatedEventJsonCapacity = JSON_OBJECT_SIZE(3); // root + 2 properties

RGB_LED rgbLed;

bool isConnected = false;
String device = ThisDevice;
int counter = 0;
int counterUpdate = 0;

bool connectToIotHub()
{
  Screen.print(0, "Wifi...");

  if (!WiFi.begin() == WL_CONNECTED)
  {
    Screen.print(1, "Failed");
    return false;
  }
  Screen.clean();

  Screen.print("IoTHub...");
  // Must enable Device Twin to get direct method working
  if (!DevKitMQTTClient_Init(true))
  {
    Screen.print(1, "Failed");
    return false;
  }
  
  Screen.clean();
  return true;
}

void ScreenPrint(unsigned int line, int number)
{
  char screenBuffer[SCREEN_COLUMNS];
  sprintf_P(screenBuffer, "%d", counter);
  Screen.print(line, screenBuffer);
}

void ScreenPrint(unsigned int line, String string)
{
  char screenBuffer[SCREEN_COLUMNS];
  device.toCharArray(screenBuffer, SCREEN_COLUMNS);
  Screen.print(line, screenBuffer);
}

void UpdateScreen()
{
  Screen.print(0, "Connected");
  ScreenPrint(1, device);
  ScreenPrint(2, counter);
}

void SendCounterUpdatedEvent()
{
  rgbLed.setColor(0, 0, RGB_LED_BRIGHTNESS);

  StaticJsonBuffer<CounterUpdatedEventJsonCapacity> jsonBuffer;

  JsonObject& counterUpdatedEvent = jsonBuffer.createObject();
  counterUpdatedEvent["device"] = device;
  counterUpdatedEvent["counter"] = counter;
  
  char buffer[1024];
  counterUpdatedEvent.printTo(buffer, 1024);

  if (DevKitMQTTClient_SendEvent(buffer))
    rgbLed.setColor(0, RGB_LED_BRIGHTNESS, 0);
  else
    rgbLed.setColor(RGB_LED_BRIGHTNESS, 0, 0);
}

void CounterUpdated()
{
  counter += counterUpdate;
  counterUpdate = 0;
  device = ThisDevice;
  UpdateScreen();
  SendCounterUpdatedEvent();
}

void CheckForInput()
{
  if (getButtonAState())
  {
    counterUpdate = -1;
  }
  else if (getButtonBState())
  {
    counterUpdate = +1;
  }
  else if (counterUpdate != 0)
  {
    CounterUpdated();
  }
}

void CounterUpdatedEventReceived(const unsigned char *payload)
{
  //Did not get StaticJsonBuffer<CounterUpdatedEventJsonCapacity> to work here.
  DynamicJsonBuffer jsonBuffer;
  JsonObject& counterUpdatedEvent = jsonBuffer.parseObject(payload);

  if (!counterUpdatedEvent.success())
  {
    LogError("Unable to de-serialize payload");
    return;
  }

  Serial.println("Received counter updated event:");
  counterUpdatedEvent.prettyPrintTo(Serial);
  
  device = counterUpdatedEvent.get<String>("device");
  counter = counterUpdatedEvent.get<int>("counter");

  UpdateScreen();
}

int  DeviceMethodCallback(const char *methodName, const unsigned char *payload, int size, unsigned char **response, int *response_size)
{
  LogInfo("Try to invoke method %s", methodName);
  const char *responseMessage = "\"Successfully invoke device method\"";
  int result = 200;

  if (strcmp(methodName, DeviceMethodName) == 0)
  {
    CounterUpdatedEventReceived(payload);
  }
  else
  {
    LogInfo("Method %s not found", methodName);
    responseMessage = "\"Method not found\"";
    result = 404;
  }

  *response_size = strlen(responseMessage) + 1;
  *response = (unsigned char *)strdup(responseMessage);

  return result;
}

void CheckForIoTHubMessages()
{
  DevKitMQTTClient_Check();
}

void setup()
{
  Serial.begin(115200);
  LogInfo("Connection to WiFi and IoT Hub...");

  isConnected = connectToIotHub();

  if (isConnected)
  {
    DevKitMQTTClient_SetDeviceMethodCallback(DeviceMethodCallback);
    UpdateScreen();
    rgbLed.setColor(0, RGB_LED_BRIGHTNESS, 0);
  }
  else
  {
    Screen.print(0, "Not connected!");
    rgbLed.setColor(RGB_LED_BRIGHTNESS, 0, 0);
  }
}

void loop()
{
  if (isConnected)
  {
    CheckForIoTHubMessages();
    CheckForInput();
  }
}
