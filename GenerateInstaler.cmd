mkdir Output
COPY /y imageDeCap\bin\Debug\imageDeCap.exe Output\imageDeCap_v1_26 Portable.exe
tools\ISCC.exe GenerateInstaller.iss
