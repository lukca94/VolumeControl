#include <avr/io.h>
#include <avr/interrupt.h>
#include <HardwareSerial.h>

#define CLK_1 PD3
#define DT_1 PD2
// #define CLK_2 PD4
// #define DT_2 PD5

volatile bool CLKChanged_1 = false;
volatile bool DTChanged_1 = false;
// volatile bool CLKChanged_2 = false;
// volatile bool DTChanged_2 = false;

enum State // CLK DT
{
	STATE_0, // Idle 11
	STATE_1, // Backwards 10
	STATE_2, // Backwards 00
	STATE_3, // Backwards 01
	STATE_4, // Forwards 01
	STATE_5, // Forwards 00
	STATE_6	 // Forwards 10
};
enum Input
{
	NOTHING,
	INPUT_CLK,
	INPUT_DT
};
class RotaryEncoder
{
private:
	State currentState = STATE_0; // State::STATE_0; je v tom rozdíl?

	void rotaryState0(Input input) // Idle 11
	{
		if (input == INPUT_CLK)
			currentState = STATE_4;
		else if (input == INPUT_DT)
			currentState = STATE_1;
	}
	void rotaryState1(Input input) // Backwards 10
	{
		if (input == INPUT_CLK)
			currentState = STATE_2;
		else if (input == INPUT_DT)
			currentState = STATE_0;
	}
	void rotaryState2(Input input) // Backwards 00
	{
		if (input == INPUT_CLK)
			currentState = STATE_1;
		else if (input == INPUT_DT)
			currentState = STATE_3;
	}
	void rotaryState3(Input input) // Backwards 01
	{
		if (input == INPUT_CLK)
		{
			currentState = STATE_0;
			Serial.println("0");
		}
		else if (input == INPUT_DT)
			currentState = STATE_2;
	}
	void rotaryState4(Input input) // Forwards 01
	{
		if (input == INPUT_CLK)
			currentState = STATE_0;
		else if (input == INPUT_DT)
			currentState = STATE_5;
	}
	void rotaryState5(Input input) // Forwards 00
	{
		if (input == INPUT_CLK)
			currentState = STATE_6;
		else if (input == INPUT_DT)
			currentState = STATE_4;
	}
	void rotaryState6(Input input) // Forwards 10
	{
		if (input == INPUT_CLK)
			currentState = STATE_5;
		else if (input == INPUT_DT)
		{
			currentState = STATE_0;
			Serial.println("1");
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

int main(void)
{
	DDRD &= ~(1 << CLK_1);
	DDRD &= ~(1 << DT_1);

	PCICR |= (1 << PCIE2); // 0b00000100; //PD
	PCMSK2 |= 0b00001100;  // D2 D3 D4 D5
	sei();

	Serial.begin(250000);

	Input input;
	RotaryEncoder encoder1;

	// interrupt
	// DT
	// DT_
	// interrupt
	// interrupt
	// CLK
	// CLK_
	// interrupt
	// CLK
	// DT
	// CLK_
	// DT_

	while (true)
	{
		// translate interrupts
		if (CLKChanged_1 == true)
		{
			Serial.println("CLK_");
			input = INPUT_CLK;
			CLKChanged_1 = false;
		}
		if (DTChanged_1 == true)
		{
			Serial.println("DT_");
			input = INPUT_DT;
			DTChanged_1 = false;
		}

		if (input != NOTHING)
		{
			encoder1.rotaryChange(input);
			input = NOTHING;
		}
	}
}

uint8_t read(volatile uint8_t *address, uint8_t pin)
{
	uint8_t temp;

	temp = *address & (1 << pin);

	if (temp != 0)
	{
		return 1;
	}
	else
	{
		return 0;
	}
}

ISR(PCINT2_vect)
{
	Serial.println("____");

	if (read(&PIND, CLK_1))
	{
		Serial.println("CLK");
		CLKChanged_1 = true;
	}
	if (read(&PIND, DT_1))
	{
		Serial.println("DT");
		DTChanged_1 = true;
	}
}