#include <Arduino.h>
#include <Encoder.h>

#define TOP 3
#define BOT 2

// volatile int stateTop = HIGH;
// volatile int stateBot = HIGH;

void setup()
{
	Serial.begin(250000);
	pinMode(TOP, INPUT_PULLUP);
	pinMode(BOT, INPUT_PULLUP);
	// attachInterrupt(digitalPinToInterrupt(TOP), ChangeTop, CHANGE ); 
	attachInterrupt(digitalPinToInterrupt(BOT), RisingBot, RISING ); //this is fucked for some reason idk whyyyyyyyyyyyyyyyyyyyyyyyyyyy
	// attachInterrupt(digitalPinToInterrupt(BOT), FallingBot, FALLING );

}

void loop()
{
	Serial.println(".");
	delay(2000);
	// if( digitalRead(TOP)==HIGH){
	// 	Serial.println("TOP HIGH");
	// }
	// else{
	// 	Serial.println("TOP LOW");
	// }
	// if( digitalRead(BOT)==HIGH){
	// 	Serial.println("BOT HIGH");
	// }
	// else{
	// 	Serial.println("BOT LOW");
	// }

}

void ChangeTop()
{
	Serial.println("ChangeTop");
}

void RisingBot()
{
	Serial.println("RisingBot");
}
void FallingBot()
{
	Serial.println("FallingBot");
}