#include <Arduino.h>

#define TOP 3
#define BOT 2

volatile bool passedTop = false;
volatile bool passedBot = false;
volatile int current = 0;
int previous = 0;
int increment = 0;

volatile bool topint = false;
volatile bool botint = false;

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
	noInterrupts();
	if (topint == true)
	{
		if (passedBot == true)
		{
			current--;
			passedBot = false;
		}
		else if (passedBot == false)
		{
			passedTop = true;
		}
		topint = false;
	}
	if(botint == true)
	{
		if (passedTop == true)
		{
			current++;
			passedTop = false;
		}
		else if (passedTop == false)
		{
			passedBot = true;
		}
		botint = false;
	}
	
	if (previous != current)
	{
		increment = current - previous;
		Serial.println(increment);
		if (increment > 0)
		{
			Serial.print("OneUp ");
			Serial.println(current);
		}
		else
		{
			Serial.print("OneDown ");
			Serial.println(current);
		}
		previous = current;
	}
	
	interrupts();
	//if (previous != current)
	// {
	// 	increment = current - previous;
	// 	Serial.println(increment);
	// 	if (increment > 0)
	// 	{
	// 		for (int i = 0; i < increment; i++)
	// 		{
	// 			Serial.print("OneUp ");
	// 			Serial.println(current);
	// 		}
	// 	}
	// 	else
	// 	{
	// 		increment = abs(increment);
	// 		for (int i = 0; i < increment; i++)
	// 		{
	// 			Serial.print("OneDown ");
	// 			Serial.println(current);
	// 		}
	// 	}
	// 	previous = current;
	// }

}

void FallingTop()
{
	topint = true;
	// if (passedBot == true)
	// {
	// 	current--;
	// 	passedBot = false;
	// }
	// else if (passedBot == false)
	// {
	// 	passedTop = true;
	// }
}

void FallingBot()
{
	botint = true;
	// if (passedTop == true)
	// {
	// 	current++;
	// 	passedTop = false;
	// }
	// else if (passedTop == false)
	// {
	// 	passedBot = true;
	// }
}

