%~d0
cd %~p0
SetLocal

cd tmp
rd /s /q ER-Tools
md ER-Tools
cd ER-Tools
copy ..\..\*.md 
copy ..\..\ERParamEditor\bin\Release\net6.0-windows\*.* 
del *.txt
del *.pdb
md assets
xcopy /s /y /q ..\..\assets  assets\
md docs 
xcopy /s /y /q ..\..\docs  docs\

md projects


