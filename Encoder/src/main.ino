#include <Arduino.h>

#define TOP 3
#define BOT 2

volatile bool passedTop = false;
volatile bool passedBot = false;
volatile int current = 0;
int previous = 0;
int increment = 0;

void setup()
{
	Serial.begin(250000);
	pinMode(TOP, INPUT_PULLUP);
	pinMode(BOT, INPUT_PULLUP);
	
	//attachInterrupt(digitalPinToInterrupt(BOT), RisingBot, RISING ); //this is fucked for some reason idk whyyyyyyyyyyyyyyyyyyyyyyyyyyy
	attachInterrupt(digitalPinToInterrupt(TOP), FallingTop, FALLING ); 
	attachInterrupt(digitalPinToInterrupt(BOT), FallingBot, FALLING );

}

void loop()
{
	if (previous != current)
	{
		increment = current - previous;
		if (increment > 0)
		{
			for (int i = 0; i < increment; i++)
			{
				Serial.println("OneUp");
			}
		}
		else
		{
			increment = abs(increment);
			for (int i = 0; i < increment; i++)
			{
				Serial.println("OneDown");
			}
		}
	}
}

void FallingTop()
{
	if (passedBot == true)
	{
		current--;
		passedBot == false;
	}
	else if (passedBot == false)
	{
		passedTop == true;
	}
}

void FallingBot()
{
	if (passedTop == true)
	{
		current++;
		passedTop == false;
	}
	else if (passedTop == false)
	{
		passedBot == true;
	}
}

void RisingBot()
{
	Serial.println("RisingBot");
}