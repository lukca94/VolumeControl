#include <Arduino.h>

#define TOP 3
#define BOT 2

int increment1 = 0;
int increment2 = 0;

volatile bool topint = false;
volatile bool botint = false;

void setup()
{
	Serial.begin(19200);
	pinMode(TOP, INPUT);
	pinMode(BOT, INPUT);
	
	//attachInterrupt(digitalPinToInterrupt(BOT), RisingBot, RISING ); //this is fucked for some reason idk whyyyyyyyyyyyyyyyyyyyyyyyyyyy
	attachInterrupt(digitalPinToInterrupt(TOP), RisingTop, CHANGE );
	attachInterrupt(digitalPinToInterrupt(BOT), RisingBot, CHANGE );
}

char buffer[40];
const long interval = 1000; 
unsigned long previousMillis = 0;
void loop()
{
	if (topint == true)
	{
		increment1++;
		topint = false;
	}
	if (botint == true)
	{
		increment2++;
		botint = false;
	}
	
	unsigned long currentMillis = millis();  
	if (currentMillis - previousMillis >= interval) {
    	previousMillis = currentMillis;
		
		sprintf(buffer, "%d %d", increment1, increment2);
		Serial.println(buffer);	
	}
}

void RisingTop()
{
	topint = true;
}

void RisingBot()
{
	botint = true;
}

