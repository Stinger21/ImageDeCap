mkdir Output
COPY /y "imageDeCap\bin\Debug\imageDeCap.exe" "Output\imageDeCap_v1_27 Preview 3 Portable.exe"
tools\ISCC.exe GenerateInstaller.iss
