/* SainSmart Ultrasonic Ranging Detector Mod HC-SR04 Distance Sensor

This sketch reads a SainSmart ultrasonic rangefinder and returns the
distance to the closest object in range. To do this, it sends a pulse
to the sensor to initiate a reading, then listens for a pulse
to return.  The length of the returning pulse is proportional to
the distance of the object from the sensor.

The circuit:
* +V connection attached to +5V
* GND connection attached to ground
* TRIG connection attached to digital pin 2
* ECHO connection attached to digital pin 3

http://www.arduino.cc/en/Tutorial/Ping

*/

const int triggerPin = 2;
const int echoPin = 3;

void setup() {
	Serial.begin(9600);
	pinMode(triggerPin, OUTPUT);
	pinMode(echoPin, INPUT);
}

void loop()
{
	long duration, cm;

	// The sensor is triggered by a HIGH pulse of 2 or more microseconds.
	// Give a short LOW pulse beforehand to ensure a clean HIGH pulse:
	digitalWrite(triggerPin, LOW);
	delayMicroseconds(2);
	digitalWrite(triggerPin, HIGH);
	delayMicroseconds(5);
	digitalWrite(triggerPin, LOW);

	// The echo pin is used to read the signal:  a HIGH
	// pulse whose duration is the time (in microseconds) from the sending
	// of the ping to the reception of its echo off of an object.
	duration = pulseIn(echoPin, HIGH);

	// convert the time into a distance
	cm = microsecondsToCentimeters(duration);

	Serial.println(cm);

	delay(1 * 1000);
}

long microsecondsToCentimeters(long microseconds)
{
	// The speed of sound is 340 m/s or 29 microseconds per centimeter.
	// The ping travels out and back, so to find the distance of the
	// object we take half of the distance travelled.
	return microseconds / 29 / 2;
}