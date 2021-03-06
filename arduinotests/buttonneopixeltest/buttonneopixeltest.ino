#include <Adafruit_NeoPixel.h>

#define PIN 5

// Parameter 1 = number of pixels in strip
// Parameter 2 = Arduino pin number (most are valid)
// Parameter 3 = pixel type flags, add together as needed:
//   NEO_KHZ800  800 KHz bitstream (most NeoPixel products w/WS2812 LEDs)
//   NEO_KHZ400  400 KHz (classic 'v1' (not v2) FLORA pixels, WS2811 drivers)
//   NEO_GRB     Pixels are wired for GRB bitstream (most NeoPixel products)
//   NEO_RGB     Pixels are wired for RGB bitstream (v1 FLORA pixels, not v2)
Adafruit_NeoPixel strip = Adafruit_NeoPixel(60, PIN, NEO_GRB + NEO_KHZ800);

// IMPORTANT: To reduce NeoPixel burnout risk, add 1000 uF capacitor across
// pixel power leads, add 300 - 500 Ohm resistor on first pixel's data input
// and minimize distance between Arduino and first pixel.  Avoid connecting
// on a live circuit...if you must, connect GND first.

const int buttonPin = 2;     // the number of the pushbutton pin
const int ledPin =  13;      // the number of the LED pin
const int buzzpin = 3;

int buttonState = HIGH;  // current reading from the button
int activityLevel = 6;

int previous = HIGH;    // the previous reading from the input pin
int state = HIGH;

// the follow variables are long's because the time, measured in miliseconds,
// will quickly become a bigger number than can be stored in an int.
long time = 0;         // the last time the output pin was toggled
long debounce = 200;   // the debounce time, increase if the output flickers

void setup() 
{
  strip.begin();
  strip.show(); // Initialize all pixels to 'off'
  
  // initialize the LED pin as an output:
  pinMode(ledPin, OUTPUT);      
  // initialize the pushbutton pin as an input:
  pinMode(buttonPin, INPUT);    
  // pull up resistor 
  digitalWrite(buttonPin, HIGH);
  pinMode(buzzpin, OUTPUT);
  
}

void loop() 
{
  buttonState = digitalRead(buttonPin);

  // debounce + toggle
  if (buttonState == HIGH && previous == LOW && millis() - time > debounce) 
  {
    if (state == HIGH)
    {
          // turn LED on:    
      digitalWrite(ledPin, HIGH);  
      digitalWrite(buzzpin, HIGH);  

      for (int i = 0 ; i < activityLevel; i++)
      {
        strip.setPixelColor(i, strip.Color(0,127,0));
      }  
      for (int i = activityLevel ; i < strip.numPixels(); i++)
      {
        strip.setPixelColor(i, strip.Color(0,0,0));  
      }
    
      strip.show();
      
      state = LOW; // toggle
    }
    else
    {
      // turn LED off:
      digitalWrite(ledPin, LOW); 
      digitalWrite(buzzpin, LOW); 
    
      for (int i = 0 ; i < strip.numPixels(); i++)
      {
         strip.setPixelColor(i, strip.Color(0,0,0));
       }
        strip.show();
    
      state = HIGH; // toggle
    }

    time = millis();    
  }
  
  previous = buttonState;
}
