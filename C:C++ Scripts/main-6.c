//Final Exam
//Sonny Sparks
//August 4, 2021
//F_3

#include <stdio.h>
#include <stdlib.h>                                   //Call the Libraries
#include <time.h>
#define MAXSIZE 10
///////////////////////////////////////////
void validate(int);
void choice_1(int, int, int[]);                       //User defined Functions
void choice_2(int, int, int[]);
void choice_3();
//////////////////////////////////////////
int main()
{
    int n, i, I, k, c, R=5;                               //Local Variables
    int a[MAXSIZE];
    //Array Sizer////////////////
        printf("Please enter number of desire Elements in Array: \n");
        scanf("%d", &n);                      //Gets array Size and Validation
    validate(n);
    //Randomize Elements/////////
        srand(time(NULL));
        for(i=0;i<n;i++)                             //Randomizer
        {
            a[i]=rand()%1000;
        }
        printf("The Elements are: \n");       //Elements Print
        for(I=0;I<n;I++)
        {
            printf("%d, ",a[I]);
        }
        printf("\n\nPlease pick a key element to be searched: \n");
        scanf("%d", &k);                      //Key Element Pick
    while(R>0)
    {
    //Choice//////////////////////
    printf("1. To find the key element searching from the first to the last element\n");
    printf("2. To print all the duplicates positions of the key element\n");
    printf("3. Exit\n");                     //Choice List
    scanf("%d", &c);
    /////////////////////////////
        if(c==1)
            choice_1(n,k, a);                         //Choice Call to functions
        if(c==2)
            choice_2(n, k, a);
        if(c==3)
            choice_3();
    R++;
    }
return 0;
}
///////////////////////////////////////////
void validate(n)
{
    if(n>MAXSIZE)                                  //Validation Function
    {
        printf("Your input exceeds the limit of the array, Please try again\n");
        main();
    }
    if(n<=MAXSIZE)
        printf("\n");
}
///////////////////////////////////////////
void choice_1(int N, int K, int A[])
{
    int x;
    for(x=0;x<N;x++)
    {                                         //Choice 1 function
        if(A[x]==K)
        {
            printf("Position of Key Element is: %d\n", x);
            break;
        }
    }
}
////////////////////////////////////////////
void choice_2(int N, int K, int A[])
{
    int x, y;
    y=N;
    for(x=0;x<N;x++)                              //Choice 2 function
        if(A[x]==K)
        {
            printf("Position of Key Element is: %d\n", x);
        }
}
/////////////////////////////////////////////
void choice_3()                                   //Choice 3 Exit
{
    printf("Thank you Goodbye!");
    exit(0);
}