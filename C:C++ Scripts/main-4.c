//Final Exam
//Sonny Sparks
//August 4, 2021
//F_1

#include <stdio.h>
#include <stdlib.h>                                               //Call to Libraries
#include <time.h>
#define MAXGUESS 10
////////////////////////////////////////////////////
int get_number();                                                 //Function Prototype
/////////////////////////////////////////////////////
int g, n, i=0;                                                    //Global Variables
/////////////////////////////////////////////////////
int main()
{
    srand(time(NULL));
    n=get_number();                                               //Function Call
    printf("%d\n",n);
    while(i<=MAXGUESS)                                            //Guess game Loop
    {
        printf("Guess a number from 1 to 30: ");
        scanf("%d",&g);
        if(g<n)
            printf("Too Low\n");
        if(g>n)
            printf("Too High\n");
        if(g==n)
        {
            printf("You Win!\n");
            break;
        }
    i++;
    }
    if(g!=n)
    {
        printf("Sadly you used all your trys :(");
    }
    return 0;
}
//////////////////////////////////////////////////////
int get_number()
{
    n=rand()%100;                                                 //Infinite function loop until
    if(n>30)                                                      // n<=30 :)
        get_number();
    if(n<30)
        return(n);
}
///////////////////////////////////////////////////////
