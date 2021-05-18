#pragma once

extern "C"
{
    int __declspec(dllexport) Function1(int a)
    {
        return a + 5;
    }
    void __declspec(dllexport) Function2(int tt[2][2])
    {
        tt[0][0] = tt[0][0] + 10;
    }
}