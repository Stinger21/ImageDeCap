mkdir Output
COPY /y imageDeCap\bin\Debug\imageDeCap.exe Output\imageDeCap_v1_25.exe
tools\ISCC.exe GenerateInstaller.iss
