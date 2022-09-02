#include<Wire.h>
#include <SPI.h>
#include <WiFi.h>
#include <ArduinoOSCWiFi.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>



/* Set the delay between fresh samples */
#define BNO055_SAMPLERATE_DELAY_MS (100)

// Check I2C device address and correct line below (by default address is 0x29 or 0x28)
//                                   id, address
Adafruit_BNO055 bno = Adafruit_BNO055(55, 0x28);




const char* ssid     = "Deputised Object";
const char* password = "";

const int   rxPort   = 9000;
const int   txPort   = 7000;
int uptime = 0;

void displaySensorDetails(void);
void displaySensorStatus(void);


void setup(){
  delay(500);

  Serial.begin(9600);

  Serial.println("setup");
   
  Serial.println("Orientation Sensor Test");
  
  Serial.println("");

  /* Initialise the sensor */
  if(!bno.begin())
  {
    /* There was a problem detecting the BNO055 ... check your connections */
    Serial.print("Ooops, no BNO055 detected ... Check your wiring or I2C ADDR!");
    while(1);
  }

  delay(1000);

  /* Display some basic information on this sensor */
  displaySensorDetails();

  /* Optional: Display current status */
  displaySensorStatus();

  bno.setExtCrystalUse(true);

 
  Serial.println("sensor setup complete!\n");

  setupWIFI();
}

void loop(){
 
  sensors_event_t event;
  bno.getEvent(&event);

  imu::Quaternion quat = bno.getQuat();
      

  OscWiFi.send("255.255.255.255", txPort, "/pos/", quat.x(),quat.y(),quat.z(),quat.w());

  OscWiFi.update();  // must be called to receive + send osc
   
  delay(BNO055_SAMPLERATE_DELAY_MS);
  
}


void setupWIFI(){
     // create and broadcast access point ssid
    WiFi.mode(WIFI_AP);
    WiFi.softAP(ssid, NULL);

    int tick = 0;
    while (++tick < 5) {
        delay(500);
        Serial.print(".");
    }

}


/**************************************************************************/
/*
    Displays some basic information on this sensor from the unified
    sensor API sensor_t type (see Adafruit_Sensor for more information)
*/
/**************************************************************************/
void displaySensorDetails(void)
{
  sensor_t sensor;
  bno.getSensor(&sensor);
  Serial.println("------------------------------------");
  Serial.print  ("Sensor:       "); Serial.println(sensor.name);
  Serial.print  ("Driver Ver:   "); Serial.println(sensor.version);
  Serial.print  ("Unique ID:    "); Serial.println(sensor.sensor_id);
  Serial.print  ("Max Value:    "); Serial.print(sensor.max_value); Serial.println(" xxx");
  Serial.print  ("Min Value:    "); Serial.print(sensor.min_value); Serial.println(" xxx");
  Serial.print  ("Resolution:   "); Serial.print(sensor.resolution); Serial.println(" xxx");
  Serial.println("------------------------------------");
  Serial.println("");
  delay(500);
}


/**************************************************************************/
/*
    Display some basic info about the sensor status
*/
/**************************************************************************/
void displaySensorStatus(void)
{
  /* Get the system status values (mostly for debugging purposes) */
  uint8_t system_status, self_test_results, system_error;
  system_status = self_test_results = system_error = 0;
  bno.getSystemStatus(&system_status, &self_test_results, &system_error);

  /* Display the results in the Serial Monitor */
  Serial.println("");
  Serial.print("System Status: 0x");
  Serial.println(system_status, HEX);
  Serial.print("Self Test:     0x");
  Serial.println(self_test_results, HEX);
  Serial.print("System Error:  0x");
  Serial.println(system_error, HEX);
  Serial.println("");
  delay(500);
}



/**************************************************************************/
/*
    Display sensor calibration status
*/
/**************************************************************************/
void displayCalStatus(void)
{
  /* Get the four calibration values (0..3) */
  /* Any sensor data reporting 0 should be ignored, */
  /* 3 means 'fully calibrated" */
  uint8_t system, gyro, accel, mag;
  system = gyro = accel = mag = 0;
  bno.getCalibration(&system, &gyro, &accel, &mag);

  /* The data should be ignored until the system calibration is > 0 */
  Serial.print("\t");
  if (!system)
  {
    Serial.print("! ");
  }

  /* Display the individual values */
  Serial.print("Sys:");
  Serial.print(system, DEC);
  Serial.print(" G:");
  Serial.print(gyro, DEC);
  Serial.print(" A:");
  Serial.print(accel, DEC);
  Serial.print(" M:");
  Serial.print(mag, DEC);
}
