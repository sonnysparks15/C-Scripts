//Final Exam
//Sonny Sparks
//August 4, 2021
//F_2

#include <stdio.h>
#include <stdlib.h>                                   //Call the Libraries
#include <time.h>
////////////////////////////////////////////////
int i=0;                                              //Global Variables
///////////////////////////////////////////////
int main()
{
    srand(time(NULL));                    //Randomizer for Real and Imaginary numbers
    struct complex_number
            {
        int  real;
        int  imaginary;
            }C1,C2,C3;
    ///////////////////////
    C1.real=rand();
    C1.imaginary=rand();                              //Random C1 Real and Imaginary + add of the two
    ///////////////////////
    C2.real=rand();
    C2.imaginary=rand();                              //Random C1 Real and Imaginary + add of the two
    ///////////////////////
    printf("Please choose addition or subtraction (1 for Add. / 2 for Sub.): \n");
    scanf("%d", &i);
    if(i==1)                                           //Choice and print
    {
        C3.real=(C1.real)+(C2.real);
        C3.imaginary=(C1.imaginary)+(C2.imaginary);
        printf("The Sum of the two Complex Structures is:  %d + i(%d)", C3.real, C3.imaginary);
    }
    if(i==2)
    {
        C3.real=(C1.real)-(C2.real);
        C3.imaginary=(C1.imaginary)-(C2.imaginary);
        printf("The Subtraction of the two Complex Structures is:  %d + i(%d)", C3.real, C3.imaginary);
    }
    return 0;
}
