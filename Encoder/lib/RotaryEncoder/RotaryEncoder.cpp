#include <HardwareSerial.h>

#include "RotaryEncoder.h"

RotaryEncoder::RotaryEncoder(uint8_t index)
{
	sprintf(upMessage ,"%d 1", index);
	sprintf(downMessage ,"%d 0", index);
}

void RotaryEncoder::rotaryState0(EncoderInput input) // Idle 11
{
	if (input == INPUT_CLK)
		currentState = STATE_4;
	else if (input == INPUT_DT)
		currentState = STATE_1;
}
void RotaryEncoder::rotaryState1(EncoderInput input) // Backwards 10
{
	if (input == INPUT_CLK)
		currentState = STATE_2;
	else if (input == INPUT_DT)
		currentState = STATE_0;
}
void RotaryEncoder::rotaryState2(EncoderInput input) // Backwards 00
{
	if (input == INPUT_CLK)
		currentState = STATE_1;
	else if (input == INPUT_DT)
		currentState = STATE_3;
}
void RotaryEncoder::rotaryState3(EncoderInput input) // Backwards 01
{
	if (input == INPUT_CLK)
	{
		currentState = STATE_0;
		Serial.println(downMessage);
	}
	else if (input == INPUT_DT)
		currentState = STATE_2;
}
void RotaryEncoder::rotaryState4(EncoderInput input) // Forwards 01
{
	if (input == INPUT_CLK)
		currentState = STATE_0;
	else if (input == INPUT_DT)
		currentState = STATE_5;
}
void RotaryEncoder::rotaryState5(EncoderInput input) // Forwards 00
{
	if (input == INPUT_CLK)
		currentState = STATE_6;
	else if (input == INPUT_DT)
		currentState = STATE_4;
}
void RotaryEncoder::rotaryState6(EncoderInput input) // Forwards 10
{
	if (input == INPUT_CLK)
		currentState = STATE_5;
	else if (input == INPUT_DT)
	{
		currentState = STATE_0;
		Serial.println(upMessage);
	}
}

void RotaryEncoder::rotaryChange(EncoderInput input)
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