#include <avr/io.h>
#include <avr/interrupt.h>
#include <HardwareSerial.h>

#include "EncoderState.h"
#include "EncoderInput.h"
#include "RotaryEncoder.h"

// Encoder 1
#define CLK_1 PD2
volatile bool currentStateCLK_1 = true;
volatile bool changedCLK_1 = false;
#define DT_1 PB2
volatile bool currentStateDT_1 = true;
volatile bool changedDT_1 = false;

// Encoder 2
#define CLK_2 PD3
volatile bool currentStateCLK_2 = true;
volatile bool changedCLK_2 = false;
#define DT_2 PB3
volatile bool currentStateDT_2 = true;
volatile bool changedDT_2 = false;

// Encoder 3
#define CLK_3 PD4
volatile bool currentStateCLK_3 = true;
volatile bool changedCLK_3 = false;
#define DT_3 PB4
volatile bool currentStateDT_3 = true;
volatile bool changedDT_3 = false;


int main(void)
{
	// Set as input
	DDRD &= ~((1 << CLK_1) | (1 << CLK_2) | (1 << CLK_3));

	DDRB &= ~((1 << DT_1) | (1 << DT_2) | (1 << DT_3));

	PCICR |= ((1 << PCIE2) | (1 << PCIE0));						  	// 0b00000101; //PD PB

	PCMSK2 |= ((1 << PCINT18) | (1 << PCINT19) | (1 << PCINT20)); 	// 0b00011100;  // D2 D3 D4
	PCMSK0 |= ((1 << PCINT2) | (1 << PCINT3) | (1 << PCINT4)); 		// 0b00011100;  // B2 B3 B4

	Serial.begin(250000);

	EncoderInput input1;
	EncoderInput input2;
	EncoderInput input3;

	RotaryEncoder encoder1(0);
	RotaryEncoder encoder2(1);
	RotaryEncoder encoder3(2);

	sei();
	while (true)
	{
		// translate interrupts
		if (changedCLK_1 == true)
		{
			input1 = INPUT_CLK;
			changedCLK_1 = false;
		}
		if (changedDT_1 == true)
		{
			input1 = INPUT_DT;
			changedDT_1 = false;
		}

		if (input1 != NOTHING)
		{
			encoder1.rotaryChange(input1);
			input1 = NOTHING;
		}

		if (changedCLK_2 == true)
		{
			input2 = INPUT_CLK;
			changedCLK_2 = false;
		}
		if (changedDT_2 == true)
		{
			input2 = INPUT_DT;
			changedDT_2 = false;
		}

		if (input2 != NOTHING)
		{
			encoder2.rotaryChange(input2);
			input2 = NOTHING;
		}

		if (changedCLK_3 == true)
		{
			input3 = INPUT_CLK;
			changedCLK_3 = false;
		}
		if (changedDT_3 == true)
		{
			input3 = INPUT_DT;
			changedDT_3 = false;
		}

		if (input3 != NOTHING)
		{
			encoder3.rotaryChange(input3);
			input3= NOTHING;
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
	if (read(&PIND, CLK_1) != currentStateCLK_1)
	{
		currentStateCLK_1 = !currentStateCLK_1;
		changedCLK_1 = true;
	}
	if (read(&PIND, CLK_2) != currentStateCLK_2)
	{
		currentStateCLK_2 = !currentStateCLK_2;
		changedCLK_2 = true;
	}
	if (read(&PIND, CLK_3) != currentStateCLK_3)
	{
		currentStateCLK_3 = !currentStateCLK_3;
		changedCLK_3 = true;
	}
}
ISR(PCINT0_vect) // DT pins
{
	if (read(&PINB, DT_1) != currentStateDT_1)
	{
		currentStateDT_1 = !currentStateDT_1;
		changedDT_1 = true;
	}
	if (read(&PINB, DT_2) != currentStateDT_2)
	{
		currentStateDT_2 = !currentStateDT_2;
		changedDT_2 = true;
	}
	if (read(&PINB, DT_3) != currentStateDT_3)
	{
		currentStateDT_3 = !currentStateDT_3;
		changedDT_3 = true;
	}
}