#ifndef ENCODERSTATE_H
#define ENCODERSTATE_H
enum EncoderState // CLK DT
{
	STATE_0, // Idle 11
	STATE_1, // Backwards 10
	STATE_2, // Backwards 00
	STATE_3, // Backwards 01
	STATE_4, // Forwards 01
	STATE_5, // Forwards 00
	STATE_6	 // Forwards 10
};
#endif //ENCODERSTATE_H
