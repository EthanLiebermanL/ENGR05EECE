#include <msp430.h> 


/**
 * main.c
 */
int main(void)
{
	WDTCTL = WDTPW | WDTHOLD;	// stop watchdog timer
	
	int a = 32;
	int b;

	a += 1;
	b = 17/2;

	while (1);
}
