static WiFiClientSecure sslClient; // for ESP8266

const char *onSuccess = "\"Successfully invoke device method\"";
const char *notFound = "\"No method found\"";

/*
 * The new version of AzureIoTHub library change the AzureIoTHubClient signature.
 * As a temporary solution, we will test the definition of AzureIoTHubVersion, which is only defined
 *    in the new AzureIoTHub library version. Once we totally deprecate the last version, we can take
 *    the #ifdef out.
 *
 * AzureIotHub library remove AzureIoTHubClient class in 1.0.34, so we remove the code below to avoid
 *    compile error
*/

/*
 * #ifdef AzureIoTHubVersion
 * static AzureIoTHubClient iotHubClient;
 * void initIoThubClient()
 * {
 *     iotHubClient.begin(sslClient);
 * }
 * #else
 * static AzureIoTHubClient iotHubClient(sslClient);
 * void initIoThubClient()
 * {
 *     iotHubClient.begin();
 * }
 * #endif
 */

int deviceMethodCallback(
    const char *methodName,
    const unsigned char *payload,
    size_t size,
    unsigned char **response,
    size_t *response_size,
    void *userContextCallback)
{
    Serial.printf("Try to invoke method %s.\r\n", methodName);
    const char *responseMessage = onSuccess;
    int result = 200;

    if (strcmp(methodName, "CounterUpdated") == 0)
    {
        blinkLED();
    }
    else
    {
        Serial.printf("No method %s found.\r\n", methodName);
        responseMessage = notFound;
        result = 404;
    }

    *response_size = strlen(responseMessage);
    *response = (unsigned char *)malloc(*response_size);
    strncpy((char *)(*response), responseMessage, *response_size);

    return result;
}
