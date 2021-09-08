// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "Windows.h"
#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <Shlwapi.h>
#include <strsafe.h>
#pragma comment (lib, "Shlwapi.lib")

HANDLE hThread;
DWORD dwThread;


__declspec(dllexport) DWORD  WINAPI WriteLog() {
    
    // Create a Folder in C: to write logs!



    BOOL createfolder = CreateDirectory(L"C:\\DLLLogs", NULL);


    wchar_t path[MAX_PATH];
    wchar_t processpath[MAX_PATH];

    HMODULE hm = NULL;
    if (GetModuleHandleEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS |
        GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
        (LPCWSTR)&WriteLog, &hm) == 0)
    {
        return 0;
    }
    if (GetModuleFileName(hm, path, sizeof(path) / sizeof(wchar_t)) == 0)
    {
        return 0;
    }
    if (GetModuleFileName(NULL, processpath, sizeof(processpath) / sizeof(wchar_t)) == 0)
    {
        return 0;
    }
  
    // Get DLLName from Path :-

    LPCWSTR outputFile = L"C:\\DLLLogs\\";
    LPCWSTR  filepart = PathFindFileNameW(path);

    // Get ProcesName from Path

    LPCWSTR  processnamepath = PathFindFileNameW(processpath);
    PathRemoveExtensionW(processpath);

    //Underscore

    LPCWSTR underscore = L"_";


    // Concat Strings Format --> ProcessName_dllname.dll

    wchar_t destination1[MAX_PATH];
    wchar_t source1[MAX_PATH];
    wchar_t underscore1[2];
    wchar_t source2[MAX_PATH];
    wcscpy(destination1, outputFile);
    wcscpy(source1, processnamepath);
    wcscpy(source2, filepart);
    wcscpy(underscore1, underscore);
    wcsncat(destination1, source1, wcslen(processnamepath));
    wcsncat(destination1, underscore1, wcslen(underscore));
    wcsncat(destination1, source2, wcslen(filepart));

    // Create File with DLL Filename --> format: ProcessName_dllname.dll :-

    HANDLE hCreateFile, hAppendFile;
    DWORD dwBytesWritten, dwBytesToWrite;

    hCreateFile = CreateFileW(destination1, GENERIC_WRITE, NULL, NULL, CREATE_NEW, FILE_ATTRIBUTE_NORMAL, NULL);
    CloseHandle(hCreateFile);

    hAppendFile = CreateFileW((LPCWSTR)destination1, FILE_APPEND_DATA, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

    if (hAppendFile == INVALID_HANDLE_VALUE)
    {
        return 0;
    }

    dwBytesToWrite = sizeof(outputFile) / sizeof(wchar_t);
    WriteFile(hAppendFile, (LPVOID)outputFile, dwBytesToWrite, &dwBytesWritten, NULL);

    CloseHandle(hAppendFile);

}


BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        WriteLog();
        break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

