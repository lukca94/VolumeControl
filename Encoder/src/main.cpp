#include <Arduino.h>
#include <Encoder.h>

#define TOP 3
#define BOT 2

int stateTop = HIGH;
int stateBot = HIGH;

void setup()
{
	Serial.begin(115200);
	pinMode(TOP, INPUT_PULLUP);
	pinMode(BOT, INPUT_PULLUP);
	attachInterrupt(digitalPinToInterrupt(TOP), ChangeTop, CHANGE); //falling mb
	attachInterrupt(digitalPinToInterrupt(BOT), ChangeBot, CHANGE);
}

void loop()
{
	Serial.println("_______");
	/* if( digitalRead(TOP)==HIGH){
		Serial.println("TOP HIGH");
	}
	else{
		Serial.println("TOP LOW");
	}
	if( digitalRead(BOT)==HIGH){
		Serial.println("BOT HIGH");
	}
	else{
		Serial.println("BOT LOW");
	}	 */
}

void ChangeTop()
{
	stateTop = !stateTop;
}
void ChangeBot()
{
	stateBot = !stateBot;
}