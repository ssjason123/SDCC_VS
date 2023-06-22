#include <stdio.h>

void test(int arg)
{
    //int arg = 890;
    arg = 123;
}

void main(void)
{
    int a = 123;
    test(a);
    //invalid line

    return;
    a = 234;
}