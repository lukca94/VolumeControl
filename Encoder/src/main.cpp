#include <avr/io.h>
#include <avr/interrupt.h>
#include <HardwareSerial.h>

#include "EncoderState.h"
#include "EncoderInput.h"
#include "RotaryEncoder.h"

// Encoder 1
#define CLK_1 PD2
#define DT_1 PB2
#define BTN_1 PC0

// Encoder 2
#define CLK_2 PD3
#define DT_2 PB3
#define BTN_2 PC1

// Encoder 3
#define CLK_3 PD4
#define DT_3 PB4
#define BTN_3 PC2

// button ISR flags
volatile bool pressedBTN_1 = false;
volatile bool pressedBTN_2 = false;
volatile bool pressedBTN_3 = false;

volatile EncoderInput input1;
volatile EncoderInput input2;
volatile EncoderInput input3;

int main(void)
{
	// Set as input
	DDRD &= ~((1 << CLK_1) | (1 << CLK_2) | (1 << CLK_3));
	DDRB &= ~((1 << DT_1) | (1 << DT_2) | (1 << DT_3));
	DDRC &= ~((1 << BTN_1) | (1 << BTN_2) | (1 << BTN_3));

	PCICR |= ((1 << PCIE2) | (1 << PCIE1) | (1 << PCIE0)); // 0b00000111; //PD PC PB

	PCMSK2 |= ((1 << PCINT18) | (1 << PCINT19) | (1 << PCINT20));
	PCMSK0 |= ((1 << PCINT2) | (1 << PCINT3) | (1 << PCINT4));
	PCMSK1 |= ((1 << PCINT8) | (1 << PCINT9) | (1 << PCINT10));

	Serial.begin(250000);

	RotaryEncoder encoder1(0);
	RotaryEncoder encoder2(1);
	RotaryEncoder encoder3(2);

	sei();
	while (true)
	{
		// translate interrupts

		// encoder 1
		if (input1 != NOTHING)
		{
			encoder1.rotaryChange(input1);
			input1 = NOTHING;
		}
		if (pressedBTN_1 == true)
		{
			Serial.println("0 2");
			pressedBTN_1 = false;
		}

		// encoder 2
		if (input2 != NOTHING)
		{
			encoder2.rotaryChange(input2);
			input2 = NOTHING;
		}
		if (pressedBTN_2 == true)
		{
			Serial.println("1 2");
			pressedBTN_2 = false;
		}

		// encoder 3
		if (input3 != NOTHING)
		{
			encoder3.rotaryChange(input3);
			input3 = NOTHING;
		}
		if (pressedBTN_3 == true)
		{
			Serial.println("2 2");
			pressedBTN_3 = false;
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

ISR(PCINT2_vect) // CLK  pins
{
	static bool currentStateCLK_1 = true;
	static bool currentStateCLK_2 = true;
	static bool currentStateCLK_3 = true;

	if (read(&PIND, CLK_1) != currentStateCLK_1)
	{
		currentStateCLK_1 = !currentStateCLK_1;
		input1 = INPUT_CLK;
	}
	if (read(&PIND, CLK_2) != currentStateCLK_2)
	{
		currentStateCLK_2 = !currentStateCLK_2;
		input2 = INPUT_CLK;
	}
	if (read(&PIND, CLK_3) != currentStateCLK_3)
	{
		currentStateCLK_3 = !currentStateCLK_3;
		input3 = INPUT_CLK;
	}
}
ISR(PCINT0_vect) // DT pins
{
	static bool currentStateDT_1 = true;
	static bool currentStateDT_2 = true;
	static bool currentStateDT_3 = true;

	if (read(&PINB, DT_1) != currentStateDT_1)
	{
		currentStateDT_1 = !currentStateDT_1;
		input1 = INPUT_DT;
	}
	if (read(&PINB, DT_2) != currentStateDT_2)
	{
		currentStateDT_2 = !currentStateDT_2;
		input2 = INPUT_DT;
	}
	if (read(&PINB, DT_3) != currentStateDT_3)
	{
		currentStateDT_3 = !currentStateDT_3;
		input3 = INPUT_DT;
	}
}
ISR(PCINT1_vect) // BTN pins
{
	if (read(&PINC, BTN_1) == 0)
	{
		pressedBTN_1 = true;
	}
	if (read(&PINC, BTN_2) == 0)
	{
		pressedBTN_2 = true;
	}
	if (read(&PINC, BTN_3) == 0)
	{
		pressedBTN_3 = true;
	}
}