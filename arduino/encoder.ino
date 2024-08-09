#define inputbutton 10
#define inputRot1 2
#define inputRot2 3
#define output1 0
#define output2 1
#include <Encoder.h>
#include <stdint.h>

Encoder knob(inputRot1, inputRot2);

void setup() {
	pinMode(inputbutton, INPUT);
	Serial.begin(9600);
	Serial.println("Encoder Test:");
	pinMode(output1, OUTPUT);
	pinMode(output2, OUTPUT);
	pinMode(inputRot1, INPUT);
	pinMode(inputRot2, INPUT);
  
}
//uint16_t pepik; unsigned int 16bit type cool uint8_t
long position = -999;
long update;
void loop() 
{
	update = knob.read();
	if (update != position) {
		
		Serial.print("Left = ");
		Serial.print(update);
		Serial.println();
		position = update;
	}
}

  
