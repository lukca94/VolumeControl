#include "../../include/EncoderInput.h"
#include "../../include/EncoderState.h"

#ifndef ROTARYENCODER_H
#define ROTARYENCODER_H

class RotaryEncoder
{
private:
	char downMessage[5];
	char upMessage[5];

	EncoderState currentState = STATE_0; // State::STATE_0; je v tom rozdíl?

	void rotaryState0(EncoderInput input); // Idle 11
	void rotaryState1(EncoderInput input); // Backwards 10
	void rotaryState2(EncoderInput input); // Backwards 00
	void rotaryState3(EncoderInput input); // Backwards 01
	void rotaryState4(EncoderInput input); // Forwards 01
	void rotaryState5(EncoderInput input); // Forwards 00
	void rotaryState6(EncoderInput input); // Forwards 10
public:
	RotaryEncoder(uint8_t index);
	void rotaryChange(EncoderInput input);
};

#endif //ROTARYENCODE_H

