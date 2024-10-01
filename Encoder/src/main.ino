#include <Arduino.h>

#define CLK 3
#define DT 2

volatile bool CLKChanged = false;
volatile bool DTChanged = false;

void setup()
{
	Serial.begin(250000);
	pinMode(CLK, INPUT_PULLUP);
	pinMode(DT, INPUT_PULLUP);
	
	//attachInterrupt(digitalPinToInterrupt(DT), RisingDT, RISING ); //this is fucked for some reason idk whyyyyyyyyyyyyyyyyyyyyyyyyyyy
	attachInterrupt(digitalPinToInterrupt(CLK), ChangeCLK, CHANGE ); 
	attachInterrupt(digitalPinToInterrupt(DT), ChangeDT, CHANGE );

}
typedef enum State	//CLK DT
{
	STATE_0,	//Idle 11
	STATE_1,	//Backwards 10
	STATE_2,	//Backwards 00
	STATE_3,	//Backwards 01
	STATE_4,	//Forwards 01
	STATE_5,	//Forwards 00
	STATE_6		//Forwards 10
};

typedef enum Input
{
	NOTHING,
	INPUT_CLK,
	INPUT_DT
};

class RotaryEncoder
{
	private:
		State currentState = STATE_0; //State::STATE_0; je v tom rozdíl?

		void rotaryState0(Input input)	//Idle 11
		{
			if (input == INPUT_CLK)
				currentState = STATE_4;
			else if(input == INPUT_DT)
				currentState = STATE_1;
		}
		void rotaryState1(Input input)	//Backwards 10
		{
			if (input == INPUT_CLK)
				currentState = STATE_2;
			else if(input == INPUT_DT)
				currentState = STATE_0;
		}
		void rotaryState2(Input input)	//Backwards 00
		{
			if (input == INPUT_CLK)
				currentState = STATE_1;
			else if(input == INPUT_DT)
				currentState = STATE_3;
		}
		void rotaryState3(Input input)	//Backwards 01
		{
			if (input == INPUT_CLK)
			{
				currentState = STATE_0;
				Serial.println("downies");
			}
			else if(input == INPUT_DT)
				currentState = STATE_2;
		}
		void rotaryState4(Input input)	//Forwards 01
		{
			if (input == INPUT_CLK)
				currentState = STATE_0;
			else if(input == INPUT_DT)
				currentState = STATE_5;
		}
		void rotaryState5(Input input)	//Forwards 00
		{
			if (input == INPUT_CLK)
				currentState = STATE_6;
			else if(input == INPUT_DT)
				currentState = STATE_4;
		}
		void rotaryState6(Input input)	//Forwards 10
		{
			if (input == INPUT_CLK)
				currentState = STATE_5;
			else if(input == INPUT_DT)
			{
				currentState = STATE_0;
				Serial.println("uppies");
			}
		}

	public:
		void rotaryChange(Input input)
		{
			switch (currentState)
			{
			case STATE_0:
				rotaryState0(input);
				break;
			case STATE_1:
				rotaryState1(input);
				break;
			case STATE_2:
				rotaryState2(input);
				break;
			case STATE_3:
				rotaryState3(input);
				break;
			case STATE_4:
				rotaryState4(input);
				break;
			case STATE_5:
				rotaryState5(input);
				break;
			case STATE_6:
				rotaryState6(input);
				break;
			}
		}
};

Input input;
RotaryEncoder encoder1;

void loop()
{
	noInterrupts();

	//translate interrupts
	if (CLKChanged == true)
	{
		input = INPUT_CLK;
		CLKChanged = false;
	}
	if (DTChanged == true)
	{
		input = INPUT_DT;
		DTChanged = false;
	}
	
	if (input != NOTHING)
	{
		encoder1.rotaryChange(input);
		input = NOTHING;
	}
	
	
	
	interrupts();
}

void ChangeCLK()
{
	CLKChanged = true;
}

void ChangeDT()
{
	DTChanged = true;
}

